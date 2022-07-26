#include "StdAfx.h"
#include "SaveImage.h"
#include "AsyncSaveImage.h"

struct SaveItem
{
	int hCamera;
	CString strFileName;
	void* Image;
	UINT byFileType;
	BYTE byQuality;
	CAsyncSaveImage::DrawTextItemArray DrawTexts;

	~SaveItem()
	{
		CameraImage_Destroy(Image);
	}
};

static CAsyncSaveImage* g_pInstance;

CAsyncSaveImage* CAsyncSaveImage::GetInstance()
{
	if (g_pInstance == NULL)
		g_pInstance = new CAsyncSaveImage();
	return g_pInstance;
}

void CAsyncSaveImage::PurgeInstance()
{
	delete g_pInstance;
	g_pInstance = NULL;
}

CAsyncSaveImage::CAsyncSaveImage(void) : 
	m_QueueNotify(FALSE, TRUE, NULL, NULL),
	m_QuitWorkerNotify(FALSE, TRUE, NULL, NULL)
{
}

CAsyncSaveImage::~CAsyncSaveImage(void)
{
	Stop();

	for (int i = 0; i < m_Queue.GetSize(); ++i)
	{
		delete (SaveItem*)m_Queue[i];
	}
	m_Queue.RemoveAll();
}

BOOL CAsyncSaveImage::Init(int nWorkerNum, int MaxQueueNum)
{
	ASSERT(nWorkerNum > 0);
	ASSERT(MaxQueueNum > 0);
	m_MaxQueueNum = MaxQueueNum;
	m_nWorkerNum = nWorkerNum;
	return TRUE;
}

void CAsyncSaveImage::Start()
{
	if (!m_WorkerHandles.IsEmpty())
		return;

	m_QuitWorkerNotify.ResetEvent();
	for (int i = 0; i < m_nWorkerNum; ++i)
	{
		HANDLE hThread = (HANDLE)_beginthreadex(NULL, 0, WorkerEntry, this, 0, NULL);
		m_WorkerHandles.Add(hThread);
	}
}

void CAsyncSaveImage::Stop()
{
	m_QuitWorkerNotify.SetEvent();
	for (int i = 0; i < m_WorkerHandles.GetSize(); ++i)
	{
		WaitForSingleObject(m_WorkerHandles[i], INFINITE);
		CloseHandle(m_WorkerHandles[i]);
	}
	m_WorkerHandles.RemoveAll();
}

BOOL CAsyncSaveImage::SaveImage(int hCamera, char const* lpszFileName, BYTE* pRaw, tSdkFrameHead* FrameHead, 
	UINT byFileType, BYTE byQuality, DrawTextItemArray const& DrawTexts)
{
	void* Image = NULL;
	if (CameraImage_Create(&Image, pRaw, FrameHead, TRUE) != CAMERA_STATUS_SUCCESS)
		return FALSE;

	SaveItem* item = new SaveItem;
	item->hCamera = hCamera;
	item->strFileName = lpszFileName;
	item->Image = Image;
	item->byFileType = byFileType;
	item->byQuality = byQuality;
	item->DrawTexts = DrawTexts;

	if (!Enqueue(item) )
	{
		delete item;
		return FALSE;
	}
	return TRUE;
}

void CAsyncSaveImage::Worker()
{
	HANDLE Handles[] = { m_QuitWorkerNotify, m_QueueNotify };
	for (;;)
	{
		DWORD e = WaitForMultipleObjects(ARRAYSIZE(Handles), Handles, FALSE, INFINITE);
		if (e == WAIT_OBJECT_0)
			break;

		SaveItem* item = (SaveItem*)Dequeue();
		if (item != NULL)
		{
			BYTE* pRaw;
			tSdkFrameHead* pFrameHead;
			CameraImage_GetData(item->Image, &pRaw, &pFrameHead);

			UINT uIspOutFmt;
			CameraGetIspOutFormat(item->hCamera, &uIspOutFmt);

			int w = pFrameHead->iWidth, h = pFrameHead->iHeight;
			if (pFrameHead->iWidthZoomSw > 0 && pFrameHead->iHeightZoomSw > 0)
			{
				w = pFrameHead->iWidthZoomSw;
				h = pFrameHead->iHeightZoomSw;
			}

			UINT RequireBufferSize = w * h * CAMERA_MEDIA_TYPE_PIXEL_SIZE(uIspOutFmt) / 8;
			BYTE* pFrameBuffer = (BYTE*)CameraAlignMalloc(RequireBufferSize, 16);
			if (pFrameBuffer != NULL)
			{
				CameraImageProcess(item->hCamera, pRaw, pFrameBuffer, pFrameHead);

				for (DrawTextItemArray::iterator it = item->DrawTexts.begin();
					it != item->DrawTexts.end(); ++it)
				{
					CameraDrawText(pFrameBuffer, pFrameHead, 
						it->strFont.c_str(), it->FontWidth, it->FontHeight,
						it->strText.c_str(), it->x, it->y, 
						pFrameHead->iWidth, pFrameHead->iHeight,
						it->TextColor, 0);
				}

				CameraDisplayRGB24(item->hCamera, pFrameBuffer, pFrameHead);
				
				CameraSaveImage(item->hCamera, item->strFileName.GetBuffer(), 
					pFrameBuffer, pFrameHead, item->byFileType, item->byQuality);
				CameraAlignFree(pFrameBuffer);
			}

			delete item;
		}
	}
}

void* CAsyncSaveImage::Dequeue()
{
	CSingleLock Lock(&m_csLock, TRUE);
	if (m_Queue.IsEmpty() )
	{
		m_QueueNotify.ResetEvent();
		return NULL;
	}
	void* item = m_Queue.GetAt(0);
	m_Queue.RemoveAt(0);
	return item;
}

BOOL CAsyncSaveImage::Enqueue(void* item)
{
	CSingleLock Lock(&m_csLock, TRUE);
	if (m_Queue.GetSize() >= m_MaxQueueNum)
		return FALSE;
	m_Queue.Add(item);
	m_QueueNotify.SetEvent();
	return TRUE;
}

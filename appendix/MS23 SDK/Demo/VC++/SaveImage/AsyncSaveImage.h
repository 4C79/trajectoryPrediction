#pragma once

#include <string>
#include <vector>

class CAsyncSaveImage
{
public:

	~CAsyncSaveImage(void);

	static CAsyncSaveImage* GetInstance();
	static void PurgeInstance();

	BOOL Init(int nWorkerNum, int MaxQueueNum);

	void Start();
	void Stop();

	struct DrawTextItem
	{
		std::string strFont;
		UINT	FontWidth;
		UINT	FontHeight;

		std::string strText;
		INT		x;
		INT		y;
		UINT	TextColor;
	};
	typedef std::vector<DrawTextItem> DrawTextItemArray;

	BOOL SaveImage(int hCamera, char const* lpszFileName, BYTE* pRaw, tSdkFrameHead* FrameHead, 
		UINT byFileType, BYTE byQuality, DrawTextItemArray const& DrawTexts);

private:

	CAsyncSaveImage(void);

	static unsigned WINAPI WorkerEntry(void* arg) { ((CAsyncSaveImage*)arg)->Worker(); return 0; }
	void Worker();

	void* Dequeue();
	BOOL Enqueue(void* item);

private:

	int m_nWorkerNum;
	int m_MaxQueueNum;

	CCriticalSection m_csLock;
	CArray<void*> m_Queue;
	CEvent m_QueueNotify;

	CEvent m_QuitWorkerNotify;
	CArray<HANDLE> m_WorkerHandles;

};

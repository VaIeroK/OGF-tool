
BearMutex::BearMutex()
{
	InitializeCriticalSection(&m_mutex);
}

BearMutex::~BearMutex()
{
	DeleteCriticalSection(&m_mutex);
}

void BearMutex::Lock()
{
	EnterCriticalSection(&m_mutex);
}

void BearMutex::Unlock()
{
	LeaveCriticalSection(&m_mutex);
}

void BearMutex::Swap(BearMutex & r)
{
	bear_swap(r.m_mutex, m_mutex);
}
bool BearMutex::TryLock()
{
	return TryEnterCriticalSection(&m_mutex);
}

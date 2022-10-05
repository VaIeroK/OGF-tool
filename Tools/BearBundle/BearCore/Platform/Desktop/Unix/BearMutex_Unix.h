
BearCore::BearMutex::BearMutex()
{
    pthread_mutexattr_t a;
	pthread_mutexattr_init(&a);
    pthread_mutexattr_settype(&a, PTHREAD_MUTEX_RECURSIVE);
    pthread_mutex_init(&m_mutex, &a);
}

BearCore::BearMutex::~BearMutex()
{
    pthread_mutex_destroy(&m_mutex);
}

void BearCore::BearMutex::Lock()
{
	  pthread_mutex_lock(&m_mutex);
}

void BearCore::BearMutex::Unlock()
{
	pthread_mutex_unlock(&m_mutex);
}

void BearCore::BearMutex::Swap(BearMutex & r)
{
	bear_swap(r.m_mutex, m_mutex);
}

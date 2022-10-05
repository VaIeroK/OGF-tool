static int32 FunctionTread(void*arg_)
{
	void**args = reinterpret_cast<void**>(arg_);
	auto*function = reinterpret_cast<BearFunctionRef*>(args[0]);
	auto*args_list = reinterpret_cast<BearArgumentsList*>(args[1]);
	int32 i = function->CallWithArgumentsList<int32>(*args_list);
	bear_free(arg_);
	return i;

}

void BearCore::BearThread::Join(const char*name)
{
	if (!m_active) {
		void**arg = bear_alloc<void*>(sizeof(void*) * 2);
		arg[0] = reinterpret_cast<void*>(m_fun);
		arg[1] = reinterpret_cast<void*>(&m_arg);
		RunThread(name,(void*)FunctionTread, arg);
	}
}
void BearCore::BearThread::Wait()
{
	if (m_active)
	{
		BEAR_FATALERROR(pthread_equal(pthread_self(), m_thread) == 0, TEXT("Паток не может ждать себя"));
        pthread_join(m_thread, NULL);
        m_active=false;
	}
}
void BearCore::BearThread::Terminate()
{
	if (m_active)
    {
        pthread_cancel(m_thread);
        m_active=false;
    }
}
BearCore::BearThread::~BearThread()
{
    Terminate();
}
void BearCore::BearThread::Swap(BearThread & sw)
{
	m_arg.swap(sw.m_arg);
	bear_swap(m_fun,sw.m_fun);
	bear_swap(m_active, sw.m_active);
	bear_swap(m_thread, sw.m_thread);
}

typedef void*(*FUN)(void*);
void BearCore::BearThread::RunThread(const char*name,void * fun, void * arg)
{
    m_active =  pthread_create(&m_thread, NULL,(FUN)fun, arg) == 0;
}

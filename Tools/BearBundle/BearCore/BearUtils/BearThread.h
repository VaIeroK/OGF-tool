#pragma once
class BEARTOOL_API BearThread
{
public:
	BEAR_CLASS_WITHOUT_COPY(BearThread);
	// ����������� ������ (������� � �� ���������);
#ifdef WINDOWS
	template<class F,class ...A>
	BearThread(F function,A&&...args):m_id(0),m_thread(0)
	{
		bear_make_arguments(m_arg, args...);
		m_fun = bear_create_function_ref(function);

	}
	template<class F>
	BearThread(F function) :m_id(0), m_thread(0)
	{
		m_fun = bear_create_function_ref(function);

	}
#else
	template<class F,class ...A>
	BearThread(F function,A&&...args):m_active(false),m_thread(0)
	{
		bear_make_arguments(m_arg, args...);
		m_fun = bear_create_function_ref(function);

	}
	template<class F>
	BearThread(F function) :m_active(false), m_thread(0)
	{
		m_fun = bear_create_function_ref(function);

	}
#endif
	//���������
	void Join(const char*name);
	//������� ���� ���������� �����. ������ � ����� ������ ��������� �������
	void Wait();
	//������� ������������ ������ ������
	void Terminate();

	void Swap(BearThread&Thread);
	void SetAffinity(uint64 mask);
	~BearThread();
private:
	void RunThread(const char*name, void * fun, void * arg);

#ifdef WINDOWS
	mutable void* m_id;
	mutable	uint32 m_thread;
#elif UNIX
	bool m_active;
 	pthread_t m_thread; 
#endif
	mutable	BearFunctionRef* m_fun;
	mutable	BearArgumentsList m_arg;
};

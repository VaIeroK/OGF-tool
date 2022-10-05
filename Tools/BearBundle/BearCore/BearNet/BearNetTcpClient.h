#pragma once
class BEARTOOL_API BearNetTcpClient
{
	BEAR_CLASS_WITHOUT_COPY(BearNetTcpClient);
public:
	friend class BearNetTcpServer;
	BearNetTcpClient();
	BearNetTcpClient(BearIP ip, uint16 port = 25565);
	inline ~BearNetTcpClient() { Close(); }

	bool Contect(BearIP ip, uint16 port = 25565);
	void SetBlocking(bool is_block);
	inline bool IsBlocking()const { return m_IsBlock; }
	void Close();

	bool Send(const void* data, bsize size) const;
	bool Send(BearBufferedReader& data)const;
	bool Send(const bchar8* data)const;
	bool Send(const bchar16* data)const;


	int32 Receive(void* data, bsize size)const;
	bool Receive(BearOutputStream& data)const;
	bool Receive(BearStringAnsi& data)const;
	bool Receive(BearStringUnicode& data)const;
public:
	template<class C>
	inline bool Send(const C& data) const
	{
		return Send(&data, sizeof(C));
	}
	template<bsize SizeBuffer>
	inline bool Send(const bchar8(&dst)[SizeBuffer]) const
	{
		return Send((const void*)dst, SizeBuffer);
	}
	template<bsize SizeBuffer>
	inline bool Send(const bchar16(&dst)[SizeBuffer]) const
	{
		return Send((const void*)dst, SizeBuffer * 2);
	}
	template<class C>
	inline bool Receive(C& data) const
	{
		return Receive(&data, sizeof(C));
	}
	template<bsize SizeBuffer>
	inline bool Receive( bchar8(&dst)[SizeBuffer]) 
	{
		return Receive((void*)dst, SizeBuffer);
	}
	template<bsize SizeBuffer>
	inline bool Receive( bchar16(&dst)[SizeBuffer]) 
	{
		return Receive((void*)dst, SizeBuffer * 2);
	}

private:
	void Create();
	void Create(bsize server);
	bsize m_Socket;
	bool m_IsBlock;
};
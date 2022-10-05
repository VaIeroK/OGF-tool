#pragma once
class BEARTOOL_API BearNetTcpServer
{
public:
	BearNetTcpServer();
	BearNetTcpServer(uint16 port);
	inline ~BearNetTcpServer() { Close(); }
	bool Create(uint16 port=25565);
	void Close();
	void SetBlocking(bool is_block);
	inline bool IsBlocking() const { return m_IsBlock; }
	bool GetClient(BearNetTcpClient& client) const;
	bool GetClient(BearNetTcpClient& client,BearIP& ip) const;
private:
	bsize m_Socket;
	bool m_IsBlock;
};

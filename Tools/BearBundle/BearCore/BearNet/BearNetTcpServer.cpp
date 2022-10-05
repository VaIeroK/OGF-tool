#include "BearCore.hpp"
#ifdef _WINDOWS
#include <windows.h>
#ifdef _WIN32_WINDOWS
#undef _WIN32_WINDOWS
#endif
#ifdef _WIN32_WINNT
#undef _WIN32_WINNT
#endif
#include <basetsd.h>
#define _WIN32_WINDOWS 0x0501
#define _WIN32_WINNT   0x0501
#include <winsock.h>
#else
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netinet/tcp.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <unistd.h>
#include <errno.h>
#include <fcntl.h>
#include <cstring>
#define INVALID_SOCKET -1
#endif
void BearNetTcpServer::Close()
{
	if (m_Socket)
	#ifdef _WINDOWS
		closesocket(m_Socket);
    #else
        ::close(m_Socket);
    #endif
	m_Socket = 0;
}
BearNetTcpServer::BearNetTcpServer() :m_Socket(0), m_IsBlock(true)
{

}
BearNetTcpServer::BearNetTcpServer(uint16 port) :m_Socket(0), m_IsBlock(true)
{
	Create(port);
}
bool BearNetTcpServer::Create(uint16 port)
{
	if (m_Socket)
		Close();
	m_Socket = socket(PF_INET, SOCK_STREAM, 0);
	sockaddr_in Addr = {};
	Addr.sin_addr.s_addr = htonl(INADDR_ANY);
	Addr.sin_family = AF_INET;
	Addr.sin_port = htons(port);
	if (m_Socket == INVALID_SOCKET)
		return false;
	if (::bind(m_Socket, reinterpret_cast<sockaddr*>(&Addr), sizeof(Addr)) == -1)
		return false;

	if (::listen(m_Socket, 5) == -1)
		return false;
	if (m_Socket == INVALID_SOCKET)
		return false;
	return true;
}
void BearNetTcpServer::SetBlocking(bool is_block)
{
	if (m_Socket)
	{
#ifdef _WINDOWS
		m_IsBlock = is_block;
		u_long blocking = is_block ? 0 : 1;
		ioctlsocket(m_Socket, FIONBIO, &blocking);
#else
		int status = fcntl(m_Socket, F_GETFL);
		if (is_block)
			fcntl(m_Socket, F_SETFL, status & ~O_NONBLOCK);
		else
			fcntl(m_Socket, F_SETFL, status | O_NONBLOCK);
#endif

	}
}

bool BearNetTcpServer::GetClient(BearNetTcpClient& client) const
{
	BearIP ip;
	return GetClient(client, ip);
}
bool BearNetTcpServer::GetClient(BearNetTcpClient& client, BearIP& ip) const
{
	sockaddr_in Addr = {};
	int32 Length = sizeof(Addr);
	bsize ClientID = ::accept(m_Socket, (sockaddr*)&Addr,&Length);
	if (ClientID == INVALID_SOCKET)
		return false;
	client.Close();
	client.Create(ClientID);
	client.SetBlocking(IsBlocking());
	ip = BearIP(ntohl(Addr.sin_addr.s_addr));
	return true;
}
#ifdef WINDOWS
struct SocketInitializer
{
	SocketInitializer()
	{
		WSADATA init;
		WSAStartup(MAKEWORD(2, 2), &init);
	}

	~SocketInitializer()
	{
		WSACleanup();
	}
};

SocketInitializer globalInitializer;
#endif

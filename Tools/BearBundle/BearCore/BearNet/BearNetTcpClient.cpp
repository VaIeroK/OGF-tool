#include "BearCore.hpp"
#ifdef _WINDOWS
#ifdef __WINDOWS32__WINDOWSDOWS
#undef __WINDOWS32__WINDOWSDOWS
#endif
#ifdef __WINDOWS32__WINDOWSNT
#undef __WINDOWS32__WINDOWSNT
#endif
#include <basetsd.h>
#define __WINDOWS32__WINDOWSDOWS 0x0501
#define __WINDOWS32__WINDOWSNT   0x0501
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
void BearNetTcpClient::Close()
{
	if (m_Socket)
#ifdef _WINDOWS
		closesocket(m_Socket);
#else
   ::close(m_Socket);
#endif
	m_Socket = 0;
}
BearNetTcpClient::BearNetTcpClient() :m_Socket(0), m_IsBlock(true)
{

}
BearNetTcpClient::BearNetTcpClient(BearIP ip, uint16 port) : m_Socket(0), m_IsBlock(true)
{
	Contect(ip, port);
}



bool  BearNetTcpClient::Contect(BearIP ip, uint16 port)
{
	if (!m_Socket)
	{
		Create();
		Create(m_Socket);
	}

	sockaddr_in Addr = {};
	Addr.sin_addr.s_addr = htonl(ip.ToUint());
	Addr.sin_family = AF_INET;
	Addr.sin_port = htons(port);
	if (::connect(m_Socket, reinterpret_cast<sockaddr*>(&Addr), sizeof(Addr)) == -1)
		return false;
	SetBlocking(true);
	return true;
}
void BearNetTcpClient::SetBlocking(bool is_block)
{
	if (m_Socket)
	{
#ifdef _WINDOWS
		m_IsBlock = is_block;
		u_long blocking = m_IsBlock;
		ioctlsocket(m_Socket, FIONBIO, &blocking);
#else
        int status = fcntl(m_Socket, F_GETFL);
        if (bl)
            fcntl(m_Socket, F_SETFL, status & ~O_NONBLOCK);
        else
            fcntl(m_Socket, F_SETFL, status | O_NONBLOCK);
#endif
	}
}
bool BearNetTcpClient::Send(const void* data, bsize size) const
{
	if (data == 0)
		return false;
	if (size==0)
		return false;
	bsize Result = 0;
	bsize Sent;
	for (Sent = 0; Sent < size; Sent +=Result)
	{
		Result = static_cast<bsize>(::send(m_Socket, static_cast<const char*>(data)+Sent, static_cast<int>(size - Sent), 0));

		if (Result < 0)
		{
			return false;
		}
	}

	return true;
}
int32 BearNetTcpClient::Receive(void* data, bsize size) const
{
	if (data == 0)
		return false;
	if (size == 0)
		return false;

	int32 Result = recv(m_Socket, static_cast<char*>(data), static_cast<int>(size), 0);

	if (Result <= 0)
	{
		return false;
	}

	return Result;
}

void BearNetTcpClient::Create()
{
	m_Socket = socket(PF_INET, SOCK_STREAM, 0);
	BEAR_CHECK(m_Socket != INVALID_SOCKET);
}
void BearNetTcpClient::Create(bsize socket)
{
	int yes = 1;
	BEAR_CHECK(m_Socket != INVALID_SOCKET);
	BEAR_CHECK(setsockopt(socket, IPPROTO_TCP, TCP_NODELAY, reinterpret_cast<char*>(&yes), sizeof(yes)) != -1);
	m_Socket = socket;
}
bool BearNetTcpClient::Send(BearBufferedReader& data) const
{
	uint32 Size = static_cast<uint32>(data.Size() - data.Tell());
	if (Size == 0)
		return false;
	if (!Send(Size))
		return false;
	return Send(data.Begin(), Size);
}
bool BearNetTcpClient::Send(const bchar8* data) const
{
	uint32 Size = static_cast<uint32>(BearString::GetSize(data));
	if (!Send(Size))
		return false;
	if(Size)
		return Send((void*)data, Size );
	return true;
}
bool BearNetTcpClient::Send(const bchar16* data) const
{
	uint32 Size = static_cast<uint32>(BearString::GetSize(data));
	if (!Send(Size))
		return false;
	if (Size)
		return Send((void*)data, (Size )*2);
	return true;
}

bool BearNetTcpClient::Receive(BearOutputStream& data) const
{
	uint32 Size = 0;
	if (!Receive(Size))
		return false;
	if (!Size)
		return false;
	
	return true;
}
bool BearNetTcpClient::Receive(BearStringAnsi& data) const
{
	uint32 Size = 0;
	if (!Receive(Size))
		return false;
	if (!Size)
		return false;
	data.assign(Size, ' ');
	Receive(&data[0], Size);
	return true;
}
bool BearNetTcpClient::Receive(BearStringUnicode& data) const
{
	uint32 Size = 0;
	if (!Receive(Size))
		return false;
	if (!Size)

		return false;
	data.assign(Size, ' ');
	Receive(&data[0], Size*2);
	return true;
}
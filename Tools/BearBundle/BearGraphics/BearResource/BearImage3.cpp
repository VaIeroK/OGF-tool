#include "BearGraphics.hpp"
#include "dds.h"
#include "BearRHI/BearTextureUtils.h"
#pragma warning(disable:4005)
#include "DXGIFormat.h"
inline void MaskShiftAndSize(bsize mask, uint32* shift, uint32* size)
{
	if (!mask)
	{
		*shift = 0;
		*size = 0;
		return;
	}

	*shift = 0;
	while ((mask & 1) == 0) {
		++(*shift);
		mask >>= 1;
	}

	*size = 0;
	while ((mask & 1) == 1) {
		++(*size);
		mask >>= 1;
	}
}

static uint32 ConvertColor(uint32 c, uint32 inbits, uint32 outbits)
{
	if (inbits == 0)
	{
		return 0;
	}
	else if (inbits >= outbits)
	{
		return c >> (inbits - outbits);
	}
	else
	{
		return (c << (outbits - inbits)) | ConvertColor(c, inbits, outbits - inbits);
	}
}

bool BearImage::LoadDDSFromStream(const BearInputStream & stream)
{
	Clear();

	if (stream.ReadUint32() == MAKEFOURCC('D', 'D', 'S', ' '))
	{
		bsize size = stream.ReadUint32();
		DDS_HEADER Header;
		bear_fill(Header);
		stream.Seek(4);
		if (size > sizeof(DDS_HEADER) || size < 4)
			return false;

		stream.ReadBuffer(&Header, size);
		if (!(Header.dwHeaderFlags&DDSD_WIDTH) || !(Header.dwHeaderFlags&DDSD_HEIGHT) || !(Header.dwHeaderFlags&DDSD_PIXELFORMAT))
			return false;
		m_Depth = Header.dwHeaderFlags&DDSD_DEPTH ? Header.dwDepth : 1;
		m_Mips = Header.dwHeaderFlags&DDSD_MIPMAPCOUNT ? Header.dwMipMapCount : 1;
		m_bCube = !!(Header.dwCubemapFlags & DDS_CUBEMAP_ALLFACES);
		m_Height = Header.dwHeight;
		m_Width = Header.dwWidth;
		m_sHeight = Header.dwHeight;
		m_sWidth = Header.dwWidth;

		if (m_Mips==0)
		{
			Clear();
			return false;
		}
		if (Header.ddspf.dwFlags != DDS_FOURCC)
		{
			m_bHasAlpha = Header.ddspf.dwFlags&(DDS_RGBA&(~DDS_RGB));
			if (Header.ddspf.dwRGBBitCount % 8 || Header.ddspf.dwRGBBitCount > 64)
			{
				Clear();
				return false;
			}
			bsize ByteSizePixel = Header.ddspf.dwRGBBitCount / 8;
			uint32 SizeBit[4], ShiftBit[4];
			for (bsize x = 0; x < 4; x++)
				MaskShiftAndSize(Header.ddspf.dwBitsMask[x], ShiftBit + x, SizeBit + x);
			if (Header.ddspf.dwRBitMask&&Header.ddspf.dwGBitMask&&Header.ddspf.dwBBitMask)
				m_PixelFotmat = m_bHasAlpha ? BearTexturePixelFormat::R8G8B8A8 : BearTexturePixelFormat::R8G8B8;
			else	if (Header.ddspf.dwRBitMask&&Header.ddspf.dwGBitMask)
				m_PixelFotmat = BearTexturePixelFormat::R8G8;
			else
				m_PixelFotmat = BearTexturePixelFormat::R8;
			uint8 coutComp =BearTextureUtils::GetCountComp(m_PixelFotmat);
			if (coutComp == 1 && Header.ddspf.dwBitsMask[3])
			{
				bear_swap(SizeBit[0], SizeBit[3]);

				bear_swap(ShiftBit[0], ShiftBit[3]);
				bear_swap(Header.ddspf.dwBitsMask[0], Header.ddspf.dwBitsMask[3]);
			}

			uint32 pixel = 0;
			Create(m_Width, m_Height, m_Mips, m_Depth, m_PixelFotmat);
			for (bsize d = 0; d < m_Depth; d++)
			{
				for (bsize m = 0; m < m_Mips; m++)
				{
					bsize h =BearTextureUtils::GetMip(m_Height, m);
					bsize w =BearTextureUtils::GetMip(m_Width, m);
					uint8*data =BearTextureUtils::GetImage(m_ImageBuffer, m_Width, m_Height, m_Mips, d, m, m_PixelFotmat);
					for (bsize x = 0; x < w*h; x++)
					{
						stream.ReadBuffer(&pixel, ByteSizePixel);
						for (bsize a = 0; a < coutComp; a++)
						{
							*BearTextureUtils::GetPixelUint8(x, 0, 0, coutComp, a, data)= static_cast<uint8>(ConvertColor((pixel & Header.ddspf.dwBitsMask[a]) >> ShiftBit[a], SizeBit[a], 8));;
						}
					}
				}
			}
			return true;
		}
		else
		{

			
			if (Header.ddspf.dwFourCC == MAKEFOURCC('D', 'X', '1', '0'))
			{
				if (size + sizeof(DDSHeader10) == sizeof(DDS_HEADER))
				{
					stream.Seek(4);
					stream.ReadBuffer(&Header, size += sizeof(DDSHeader10));
				}
				switch (Header.Header10.dxgiFormat)
				{
				case DXGI_FORMAT_R32G32B32A32_FLOAT:
					m_PixelFotmat = BearTexturePixelFormat::R32G32B32A32F;
					break;
				case DXGI_FORMAT_R32G32B32_FLOAT:
					m_PixelFotmat = BearTexturePixelFormat::R32G32B32F;
					break;
				case DXGI_FORMAT_R32G32_FLOAT:
					m_PixelFotmat = BearTexturePixelFormat::R32G32F;
					break;
				case DXGI_FORMAT_R32_FLOAT:
					m_PixelFotmat = BearTexturePixelFormat::R32F;
					break;
				case DXGI_FORMAT_R8G8B8A8_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::R8G8B8A8;
					break;
				case DXGI_FORMAT_R8G8_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::R8G8;
					break;
				case DXGI_FORMAT_R8_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::R8;
					break;
				case DXGI_FORMAT_BC1_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::BC1;
					break;
				case DXGI_FORMAT_BC2_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::BC2;
					break;
				case DXGI_FORMAT_BC3_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::BC3;
					break;
				case DXGI_FORMAT_BC4_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::BC4;
					break;
				case DXGI_FORMAT_BC5_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::BC5;
					break;
				case DXGI_FORMAT_BC6H_SF16:
					m_PixelFotmat = BearTexturePixelFormat::BC6;
					break;
				case DXGI_FORMAT_BC7_UNORM:
					m_PixelFotmat = BearTexturePixelFormat::BC7;
					break;
				default:
					Clear();
					return false;
				}
			}
			else
			{
				switch (Header.ddspf.dwFourCC)
				{
				case  MAKEFOURCC('D', 'X', 'T', '1'):
					m_PixelFotmat = BearTexturePixelFormat::BC1;
					break;
				case  MAKEFOURCC('D', 'X', 'T', '2'):
				case  MAKEFOURCC('D', 'X', 'T', '3'):
					m_PixelFotmat = BearTexturePixelFormat::BC2;
					break;
				case  MAKEFOURCC('D', 'X', 'T', '4'):
				case  MAKEFOURCC('D', 'X', 'T', '5'):
					m_PixelFotmat = BearTexturePixelFormat::BC3;
					break;
				case  MAKEFOURCC('A', 'T', 'I', '1'):
					m_PixelFotmat = BearTexturePixelFormat::BC4;
					break;
				case  MAKEFOURCC('A', 'T', 'I', '2'):
					m_PixelFotmat = BearTexturePixelFormat::BC5;
					break;
				default:
					Clear();
					return false;
				}
			}

			Create(m_Width, m_Height, m_Mips , m_Depth, m_PixelFotmat, m_bCube);
			stream.ReadBuffer(m_ImageBuffer, GetSizeInMemory());
			return true;
		}
		
	}
	return false;
}
bool BearImage::LoadDDSFromFile(const bchar * str)
{
	BearFileStream stream;
	if (!stream.Open(str))
		return false;
	return LoadDDSFromStream(stream);
}
bool BearImage::LoadDDSFromBuffer(const BearBufferedReader & stream)
{
	return LoadDDSFromStream(stream);
}


bool BearImage::SaveToDds(const bchar* name)
{
	BearFileStream dds;
	if (!dds.Open(name, BearFileStream::M_Write))
		return false;
	dds.Seek(0);

	DDS_HEADER Header = {  };

	Header.dwSize = sizeof(Header) - sizeof(Header.Header10);
	Header.dwHeaderFlags = DDSD_CAPS|DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT;// DDSD_MIPMAPCOUNT | DDSD_DEPTH;
	Header.dwWidth = static_cast<DWORD>(m_Width);
	Header.dwHeight = static_cast<DWORD>(m_Height);
	Header.dwSurfaceFlags = DDS_SURFACE_FLAGS_TEXTURE;
	if (m_Depth>1)
	{
		Header.dwDepth = static_cast<DWORD>(m_Depth);
		Header.dwHeaderFlags |= DDSD_DEPTH;
		
	}
	if (m_Mips>1)
	{
		Header.dwMipMapCount = static_cast<DWORD>(m_Mips);
		Header.dwHeaderFlags |= DDSD_MIPMAPCOUNT;
	}
	switch (m_PixelFotmat)
	{
	case BearTexturePixelFormat::R8:
	case BearTexturePixelFormat::R8G8:
	case BearTexturePixelFormat::R8G8B8:
	case BearTexturePixelFormat::R8G8B8A8:
	case BearTexturePixelFormat::R32F:
	case BearTexturePixelFormat::R32G32F:
	case BearTexturePixelFormat::R32G32B32F:
	case BearTexturePixelFormat::R32G32B32A32F:
		Header.dwHeaderFlags |= DDSD_LINEARSIZE;
		Header.dwPitchOrLinearSize = static_cast<DWORD>(BearTextureUtils::GetSizePixel(m_PixelFotmat) * m_Width);
		break;
	default:
		break;
	}
	char ddst[] = {'D','D','S',' '};
	dds.WriteBuffer(ddst, 4);
	switch (m_PixelFotmat)
	{
	case BearTexturePixelFormat::R8:
		Header.ddspf = DDSPF_R8G8B8;
		Header.ddspf.dwFlags = 0x2;
		Header.ddspf.dwRBitMask = 0;
		Header.ddspf.dwBBitMask = 0;
		Header.ddspf.dwGBitMask = 0;
		Header.ddspf.dwABitMask = 255;
		Header.ddspf.dwRGBBitCount = 8;
		break;
	case BearTexturePixelFormat::R8G8:
		Header.ddspf = DDSPF_R8G8B8;
		Header.ddspf.dwBBitMask = 0;
		Header.ddspf.dwGBitMask = 0xFF<<8;
		Header.ddspf.dwRBitMask = 0xFF;
		Header.ddspf.dwRGBBitCount =16;
		break;
	case BearTexturePixelFormat::R8G8B8:
		Header.ddspf = DDSPF_R8G8B8;
		Header.ddspf.dwBBitMask = 0xFF << 16;
		Header.ddspf.dwGBitMask = 0xFF << 8;
		Header.ddspf.dwRBitMask = 0xFF;
		Header.ddspf.dwRGBBitCount = 24;
		break;
	case BearTexturePixelFormat::R8G8B8A8:
		Header.ddspf = DDSPF_A8R8G8B8;
		Header.ddspf.dwBBitMask = 0xFF << 16;
		Header.ddspf.dwGBitMask = 0xFF << 8;
		Header.ddspf.dwRBitMask = 0xFF;
		Header.ddspf.dwABitMask = DWORD(0xFF << 24);
		Header.ddspf.dwRGBBitCount = 32;
		break;
	case BearTexturePixelFormat::R32F:
		Header.ddspf = DDSPF_DX10;
		Header.Header10.dxgiFormat = DXGI_FORMAT_R32_FLOAT;
		Header.Header10.arraySize = static_cast<DWORD>(m_Depth);
		Header.Header10.resourceDimension = 3;
		break;
	case BearTexturePixelFormat::R32G32F:
		Header.ddspf = DDSPF_DX10;
		Header.Header10.dxgiFormat = DXGI_FORMAT_R32G32_FLOAT;
		Header.Header10.arraySize = static_cast<DWORD>(m_Depth);
		Header.Header10.resourceDimension = 3;
		break;
	case BearTexturePixelFormat::R32G32B32F:
		Header.ddspf = DDSPF_DX10;
		Header.Header10.dxgiFormat = DXGI_FORMAT_R32G32B32_FLOAT;
		Header.Header10.arraySize = static_cast<DWORD>(m_Depth);
		Header.Header10.resourceDimension = 3;
		break;
	case BearTexturePixelFormat::R32G32B32A32F:
		Header.ddspf = DDSPF_DX10;
		Header.Header10.dxgiFormat = DXGI_FORMAT_R32G32B32A32_FLOAT;
		Header.Header10.arraySize = static_cast<DWORD>(m_Depth);
		Header.Header10.resourceDimension = 3;
		break;
	case BearTexturePixelFormat::DXT_1:
	case BearTexturePixelFormat::DXT_1_Alpha:
		Header.ddspf = DDSPF_DXT1;
		break;
	case BearTexturePixelFormat::DXT_3:
		Header.ddspf = DDSPF_DXT3;
		break;
	case BearTexturePixelFormat::DXT_5:
		Header.ddspf = DDSPF_DXT5;
		break;
	case BearTexturePixelFormat::BC4:
		Header.ddspf = DDSPF_ATI1;
		break;
	case BearTexturePixelFormat::BC5:
		Header.ddspf = DDSPF_ATI2;
		break;
	case BearTexturePixelFormat::BC6:
		Header.ddspf = DDSPF_DX10;			
		Header.Header10.dxgiFormat = DXGI_FORMAT_BC6H_UF16;
		Header.Header10.arraySize = static_cast<DWORD>(m_Depth);
		Header.Header10.resourceDimension = 3;
		break;
	case BearTexturePixelFormat::BC7:
		Header.ddspf = DDSPF_DX10;
		Header.Header10.dxgiFormat = DXGI_FORMAT_BC7_UNORM;
		Header.Header10.arraySize = static_cast<DWORD>(m_Depth);
		Header.Header10.resourceDimension = 3;
		break;
	default:
		return false;
	}
	
	dds.WriteBuffer(&Header, Header.dwSize + (Header.ddspf.dwFourCC == MAKEFOURCC('D', 'X', '1', '0') ? sizeof(Header.Header10) : 0));
	dds.WriteBuffer(m_ImageBuffer, BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat)*m_Depth);
	dds.Close();
	return true;
}
#include "BearGraphics.hpp"
#include "BearRHI/BearTextureUtils.h"

BearImage::BearImage() :m_PixelFotmat(BearTexturePixelFormat::R8), m_Width(0), m_Height(0), m_Mips(0), m_Depth(0), m_ImageBuffer(0), m_bCube(false), m_bHasAlpha(false), m_sWidth(0), m_sHeight(0)
{
	
}

BearImage::BearImage(bsize w, bsize h, bsize mip, bsize depth, BearTexturePixelFormat px, bool cube ) : m_PixelFotmat(BearTexturePixelFormat::R8), m_Width(0), m_Height(0), m_Mips(0), m_Depth(0), m_ImageBuffer(0)
{
	Create(w, h, mip, depth, px, cube);
}

void BearImage::Fill(const BearColor & color)
{
	if (Empty())return;
	bsize Size = BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat);
	for (bsize i = 0; i < m_Depth; i++)
	{
		BearTextureUtils::Fill(m_ImageBuffer + (Size*i), m_Width, m_Height, m_Mips, color, m_PixelFotmat);
	}
}

void BearImage::Create(bsize w, bsize h, bsize mip, bsize depth, BearTexturePixelFormat px, bool cube )
{
	Clear();
	m_bCube = cube;
	m_Depth = depth;
	if (cube)m_Depth *= 6;
	if (BearTextureUtils::isCompressor(px))
	{
		BEAR_CHECK(m_Width%4==0&&m_Height%4==0);
	}
	m_Width = w;
	m_Height = h;
	m_PixelFotmat = px;
	BEAR_CHECK(m_Depth);
	if(mip)
		m_Mips = mip;
	else
		m_Mips = 1;
	m_ImageBuffer = bear_alloc<uint8>(GetSizeInMemory());
}

void BearImage::Append(bsize x, bsize y, const BearImage & Image, bsize x_src, bsize y_src, bsize w_src, bsize h_src, bsize dst_depth, bsize src_depth)
{
	if (Empty() || Image.Empty())
		return;
	BEAR_ASSERT(m_Width >= x + w_src && m_Height >= y + h_src);
	BEAR_ASSERT(Image.m_Width >= x_src + w_src && Image.m_Height >= y_src + h_src);
	BEAR_ASSERT(m_Depth > dst_depth);
	BEAR_ASSERT(m_Depth > src_depth);
	bsize DestSize = BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat)*dst_depth;
	bsize SourceSize = BearTextureUtils::GetSizeInMemory(Image.m_Width, Image.m_Height, Image.m_Mips, Image.m_PixelFotmat)*src_depth;
	uint8*DestData = m_ImageBuffer + DestSize;
	uint8*src_data = Image.m_ImageBuffer + SourceSize;
	BearTextureUtils::Append(DestData, m_Width, m_Height, x, y, src_data, Image.m_Width, Image.m_Height, BearVector4<bsize>(x_src, y_src, w_src, h_src), m_PixelFotmat, Image.m_PixelFotmat);
}

void BearImage::Append(bsize x, bsize y, const BearImage & Image, bsize dst_depth, bsize src_depth)
{
	if (Empty() || Image.Empty())
		return;
	Append(x, y, Image, 0, 0, Image.m_Width, Image.m_Height, dst_depth, src_depth);
}

void BearImage::Scale(bsize w, bsize h, BearResizeFilter filter)
{
	if (Empty())
		return;
	BearImage Image;
	bsize Mips = m_Mips;
	ClearMipLevels();
	Image.Create(w, h, 1 , m_Depth, m_PixelFotmat);
	bsize SourceSize = BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat);
	bsize DestSize = BearTextureUtils::GetSizeInMemory(Image.m_Width, Image.m_Height, Image.m_Mips, Image.m_PixelFotmat);
	for (bsize i = 0; i < m_Depth; i++)
	{

		uint8*DestData = Image.m_ImageBuffer + DestSize * i;
		uint8*src_data = m_ImageBuffer + SourceSize * i;
		BearTextureUtils::Scale(DestData, Image.m_Width, Image.m_Height, src_data, m_Width, m_Height, m_PixelFotmat, filter);
	}

	Swap(Image);
	if (Mips > 1) GenerateMipmap(filter);
}

void BearImage::ScaleCanvas(bsize w, bsize h)
{
	if (Empty())
		return;
	bsize Mips = m_Mips;
	BearImage Image;
	ClearMipLevels();
	Image.Create(w, h, m_Mips, m_Depth, m_PixelFotmat);
	Image.Fill(BearColor::Black);
	for (bsize i = 0; i < m_Depth; i++)
	{
		Image.Append(0, 0, *this, 0, 0, BearMath::min(m_Width, w), BearMath::min(m_Height, h), i, i);
	}
	Swap(Image);
	if (Mips > 1) GenerateMipmap();

}


void BearImage::GenerateMipmap(bsize depth, BearResizeFilter filter)
{
	if (Empty())
		return;
	if (m_Mips == 1)
	{
		GenerateMipmap(filter);
	}
	else
	{
	
		for (bsize i = 0 ; i < m_Mips-1; i++)
		{
			uint8*cur_mip = BearTextureUtils::GetImage(m_ImageBuffer, m_Width, m_Height, m_Mips, depth, i, m_PixelFotmat);
			uint8*new_mip= BearTextureUtils::GetImage(m_ImageBuffer, m_Width, m_Height, m_Mips, depth, i+1, m_PixelFotmat);
			BearTextureUtils::GenerateMip(new_mip, cur_mip, BearTextureUtils::GetMip(m_Width, i), BearTextureUtils::GetMip(m_Height, i), m_PixelFotmat, filter);
		}
		
	}
}

void BearImage::GenerateMipmap(BearResizeFilter filter)
{
	if (Empty())
		return;
	m_Mips = BearTextureUtils::GetCountMips(m_Width, m_Height);
	m_ImageBuffer = bear_realloc(m_ImageBuffer, BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat)*m_Depth);
	bsize size = BearTextureUtils::GetSizeInMemory(m_Width, m_Height, 1, m_PixelFotmat);
	for (bsize i = m_Depth; i > 1; i--)
	{
		uint8*images = BearTextureUtils::GetImage(m_ImageBuffer, m_Width, m_Height, 1, i-1, 0, m_PixelFotmat);
		uint8*images_new = BearTextureUtils::GetImage(m_ImageBuffer, m_Width, m_Height, m_Mips, i - 1, 0, m_PixelFotmat);
		bear_move(images, images_new- images, size);
	}
	for (bsize i = 0; m_Depth>i; i++)
	{
		GenerateMipmap(i, filter);
	}
}


void BearImage::ClearMipLevels()
{
	if (Empty() || m_Mips == 1)return;
	bsize size = BearTextureUtils::GetSizeInMemory(m_Width, m_Height, 1, m_PixelFotmat);
	for (bsize i = m_Depth; i > 1; i--)
	{
		uint8*images_new = BearTextureUtils::GetImage(m_ImageBuffer, m_Width, m_Height, 1, i - 1, 0, m_PixelFotmat);
		uint8*images = BearTextureUtils::GetImage(m_ImageBuffer, m_Width, m_Height, m_Mips, i - 1, 0, m_PixelFotmat);
		bear_move(images, images_new - images, size);
	}
	m_Mips = 1;
	m_ImageBuffer = bear_realloc(m_ImageBuffer, BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat)*m_Depth);
}

void BearImage::NormalizedSizeNotScale()
{
	bsize size = BearMath::max(bear_recommended_size(m_Width), bear_recommended_size(m_Height));
	ScaleCanvas(size, size);
}

void BearImage::NormalizedSize(BearResizeFilter filter)
{
	bsize size = BearMath::max(bear_recommended_size(m_Width), bear_recommended_size(m_Height));
	Scale(size, size);
}

BearColor BearImage::GetPixel(bsize x, bsize y, bsize d) const
{
	if (Empty())
		return BearColor();
	
	BearColor color;
	BearTextureUtils::GetPixel(color,m_ImageBuffer,x,y,d,m_Width,m_Height,m_Mips,m_PixelFotmat);
	return color;

}

void BearImage::SetPixel(const BearColor & color, bsize x, bsize y, bsize d)
{
	if (Empty())
		return;
	BearTextureUtils::SetPixel(color, m_ImageBuffer, x, y, d, m_Width, m_Height, m_Mips, m_PixelFotmat);
}

void BearImage::SwapRB()
{
	if (Empty())return;
	BEAR_CHECK(m_Mips == 1);
	if (m_Mips != 1)return;
	if (m_PixelFotmat != BearTexturePixelFormat::R8G8B8A8)
	{
		for (bsize i = 0; i < m_Depth; i++)
		{
			for (bsize x = 0; x < m_Width * m_Height; x++)
			{
				for (bsize y = 0; y < m_Width * m_Height; y++)
				{
					BearColor Color = GetPixel(x, y, i);
					bear_swap(Color.R8U, Color.B8U);
					bear_swap(Color.R32F, Color.B32F);
					SetPixel(Color, x, y, i);
				}
			}
		}
	}
	else
	{
		for (bsize i = 0; i < m_Depth; i++)
		{
			for (bsize a = 0; a < m_Width*m_Height; a++)
			{
				uint8*Pixel = m_ImageBuffer + i * (m_Width * m_Height) + a * 4;
				bear_swap(Pixel[0], Pixel[2]);

			}
		}
	}
}

void BearImage::Clear()
{
	if (m_ImageBuffer)
		bear_free(m_ImageBuffer); 
	m_ImageBuffer = 0;
	m_Height = 0;
	m_Width = 0;
	m_Mips = 0;
	m_Depth = 0;
	m_PixelFotmat = BearTexturePixelFormat::R8;
}

BearImage::~BearImage()
{
	Clear();
}

bool BearImage::Empty() const
{
	return m_Depth==0||m_Mips==0||m_Width==0||m_Height==0;
}

BearImage::BearImage(BearImage&& Image) :m_PixelFotmat(BearTexturePixelFormat::R8), m_Width(0), m_Height(0), m_Mips(0), m_Depth(0), m_ImageBuffer(0)
{
	Swap(Image);
}

BearImage::BearImage(const BearImage & Image) :m_PixelFotmat(BearTexturePixelFormat::R8), m_Width(0), m_Height(0), m_Mips(0), m_Depth(0), m_ImageBuffer(0)
{
	Copy(Image);
}

void BearImage::Copy(const BearImage & Image)
{
	Clear();
	if (Image.Empty())
	{
		return;
	}
	m_bCube = Image.m_bCube;
	m_Height = Image.m_Height;
	m_Width = Image.m_Width;
	m_Mips = Image.m_Mips;
	m_Depth = Image.m_Depth;
	m_PixelFotmat = Image.m_PixelFotmat;
	m_ImageBuffer = bear_alloc<uint8>(GetSizeInMemory());
	bear_copy(m_ImageBuffer, Image.m_ImageBuffer, GetSizeInMemory());
}

void BearImage::Swap(BearImage & Image)
{
	bear_swap(m_Height , Image.m_Height);
	bear_swap(m_Width , Image.m_Width);
	bear_swap(m_Mips , Image.m_Mips);
	bear_swap(m_Depth , Image.m_Depth);
	bear_swap(m_PixelFotmat , Image.m_PixelFotmat);
	bear_swap(m_ImageBuffer, Image.m_ImageBuffer);
}

BearImage & BearImage::operator=(const BearImage & Image)
{
	Copy(Image);
	return*this;
}

BearImage& BearImage::operator=(BearImage&& Image)
{
	Swap(Image);
	return*this;
}

void BearImage::Convert(BearTexturePixelFormat format)
{
	if (Empty())return;
	BearImage Image(m_Width, m_Height, m_Mips , m_Depth, format);
	uint8*src= (uint8*)**this;
	uint8*dst = (uint8*)*Image;
	for (bsize i = 0; i < m_Depth; i++)
	{
		
		for (bsize a = 0; a < m_Mips; a++)
		{
			bsize w = BearTextureUtils::GetMip(m_Width,a);
			bsize h = BearTextureUtils::GetMip(m_Height, a);
			BearTextureUtils::Convert(format, m_PixelFotmat, dst, src, w, h);
			src +=BearTextureUtils::GetSizeDepth(w, h,m_PixelFotmat);
			dst += BearTextureUtils::GetSizeDepth(w, h, format);
		}
	}
	Swap(Image);
}

BearTexturePixelFormat BearImage::GetFormat() const
{
	return m_PixelFotmat;
}

bsize BearImage::GetDepth() const
{
	return m_Depth;
}

bsize BearImage::GetMips()const
{
	return m_Mips;
}

void * BearImage::operator*()
{
	return m_ImageBuffer;
}

const void * BearImage::operator*() const
{
	return m_ImageBuffer;
}

bsize BearImage::GetSizeInMemory() const
{
	return BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat)*m_Depth;
}

BearVector2<bsize> BearImage::GetSize() const
{
	return BearVector2<bsize>(m_Width,m_Height);
}

bool BearImage::HasAlpha() const
{
	return m_bHasAlpha;
}

size_t BearImage::GetWidth() const
{
	return m_sWidth;
}

size_t BearImage::GetHeight() const
{
	return m_sHeight;
}


void BearImage::Resize(bsize w, bsize h,bsize depth,BearTexturePixelFormat px)
{
	if (m_Width != w || m_Height != h || m_PixelFotmat != px)
	{
		Create(w, h, m_Mips , depth + 1, px);
	}
	else
	{
		if(m_Depth< depth + 1)
			m_ImageBuffer = bear_realloc(m_ImageBuffer, BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat)*(depth + 1));
		m_Depth = depth + 1;
	}
}


#include "BearGraphics.hpp"
#include "BearRHI/BearTextureUtils.h"
#define STB_IMAGE_IMPLEMENTATION
#define STB_IMAGE_WRITE_IMPLEMENTATION

extern "C"
{
	void* Malloc(bsize a)
	{
		return bear_alloc<uint8>(a);
	}
	void Free(void* a)
	{
		if (!a)return;
		bear_free(a);

	}
	void* Realloc(void* x, bsize n)
	{
		return  bear_realloc<uint8>((uint8*)x, n);
	}
}
#include "StbImage/stb_image.h"
#include "StbImage/stb_image_write.h"
bool BearImage::LoadFromFile(const bchar * str)
{
	BearFileStream stream;
	if (!stream.Open(str))
		return false;
	return LoadFromStream(stream);
}

bool BearImage::LoadFromStream(const BearInputStream & stream)
{
	BearMemoryStream memory(stream);
	return LoadFromBuffer(memory);
}

bool BearImage::LoadFromBuffer(const BearBufferedReader & stream)
{
	if (LoadDDSFromBuffer(stream))
		return true;
	Clear();
	int w, h, comp;
	stbi_uc*data1 = stbi_load_from_memory((const stbi_uc*)stream.Begin(), (int)((uint8*)stream.End() - (uint8*)stream.Begin()), &w, &h, &comp, STBI_rgb_alpha);
	if (!data1) {
		return false;
	}
	Create(w, h);
	bear_copy(m_ImageBuffer, data1, BearTextureUtils::GetSizeInMemory(w, h, 1, m_PixelFotmat));
	bear_free(data1);
	return true;
}

bool BearImage::LoadFromFile(bsize depth, const bchar * str)
{
	if (!Empty())
		return false;
	BearFileStream stream;
	if (!stream.Open(str))
		return false;
	return LoadFromStream(depth, stream);
}

bool BearImage::LoadFromStream(bsize depth, const BearInputStream & stream)
{
	if (!Empty())
		return false;
	BearMemoryStream memory(stream);
	return LoadFromBuffer(depth, memory);
}

bool BearImage::LoadFromBuffer(bsize depth, const BearBufferedReader & stream)
{
	if (!Empty())
		return false;
	int w, h, comp;
	stbi_uc*data = stbi_load_from_memory((const stbi_uc*)stream.Begin(), (int)((uint8*)stream.End() - (uint8*)stream.Begin()), &w, &h, &comp, STBI_rgb_alpha);
	if (!data)
	{
		return false;
	}
	Resize(w, h, depth, BearTexturePixelFormat::R8G8B8A8);
	bsize size = BearTextureUtils::GetSizeInMemory(w, h, m_Mips, m_PixelFotmat);
	bear_copy(m_ImageBuffer + (depth*size), data, BearTextureUtils::GetSizeInMemory(w, h, 1, m_PixelFotmat));
	bear_free(data);
	return true;
}
bool BearImage::SaveToJpg(const bchar * name, bsize depth)
{
	BEAR_ASSERT(m_Depth > depth);
	BEAR_ASSERT(!BearTextureUtils::isCompressor(m_PixelFotmat));
	BEAR_ASSERT(!BearTextureUtils::isFloatPixel(m_PixelFotmat));
#ifdef UNICODE
	return stbi_write_jpg(*BearEncoding::ToAnsi(name), static_cast<int>(m_Width), static_cast<int>(m_Height), static_cast<int>(BearTextureUtils::GetCountComp(m_PixelFotmat)), m_ImageBuffer + depth * BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat), 100);

#else
	return stbi_write_jpg(name, static_cast<int>(m_Width), static_cast<int>(m_Height), static_cast<int>(BearTextureUtils::GetCountComp(m_PixelFotmat)), m_ImageBuffer + depth * BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat), 100);

#endif
}

bool BearImage::SaveToPng(const bchar * name, bsize depth)
{
	BEAR_ASSERT(m_Depth > depth);
	BEAR_ASSERT(!BearTextureUtils::isCompressor(m_PixelFotmat));
	BEAR_ASSERT(!BearTextureUtils::isFloatPixel(m_PixelFotmat));
#ifdef UNICODE
	return stbi_write_png(*BearEncoding::ToAnsi(name), static_cast<int>(m_Width), static_cast<int>(m_Height), static_cast<int>(BearTextureUtils::GetCountComp(m_PixelFotmat)), m_ImageBuffer + depth * BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat), 0);
#else
	return stbi_write_png(name, static_cast<int>(m_Width), static_cast<int>(m_Height), static_cast<int>(BearTextureUtils::GetCountComp(m_PixelFotmat)), m_ImageBuffer + depth * BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat), 0);

#endif
}

bool BearImage::SaveToBmp(const bchar * name, bsize depth)
{
	BEAR_ASSERT(m_Depth > depth);
	BEAR_ASSERT(!BearTextureUtils::isCompressor(m_PixelFotmat));
	BEAR_ASSERT(!BearTextureUtils::isFloatPixel(m_PixelFotmat));
#ifdef UNICODE
	return stbi_write_bmp(*BearEncoding::ToAnsi(name), static_cast<int>(m_Width), static_cast<int>(m_Height), static_cast<int>(BearTextureUtils::GetCountComp(m_PixelFotmat)), m_ImageBuffer + depth * BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat));
#else
	return stbi_write_bmp(name, static_cast<int>(m_Width), static_cast<int>(m_Height), static_cast<int>(BearTextureUtils::GetCountComp(m_PixelFotmat)), m_ImageBuffer + depth * BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat));
#endif
}

bool BearImage::SaveToTga(const bchar * name, bsize depth)
{
	BEAR_ASSERT(m_Depth > depth);
	BEAR_ASSERT(!BearTextureUtils::isCompressor(m_PixelFotmat));
	BEAR_ASSERT(!BearTextureUtils::isFloatPixel(m_PixelFotmat));
#ifdef UNICODE
	return stbi_write_tga(*BearEncoding::ToAnsi(name), static_cast<int>(m_Width), static_cast<int>(m_Height), static_cast<int>(BearTextureUtils::GetCountComp(m_PixelFotmat)), m_ImageBuffer + depth * BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat));
#else
	return stbi_write_tga(name, static_cast<int>(m_Width), static_cast<int>(m_Height), static_cast<int>(BearTextureUtils::GetCountComp(m_PixelFotmat)), m_ImageBuffer + depth * BearTextureUtils::GetSizeInMemory(m_Width, m_Height, m_Mips, m_PixelFotmat));

#endif
}
#include "BearGraphics.hpp"
#include "BearRHI/BearTextureUtils.h"
#ifdef DEVELOPER_VERSION
#include "IntelCompressor/ispc_texcomp.h"
#include "Nvtt/nvimage\ColorBlock.h"
#include "Nvtt/nvimage\BlockDXT.h"
#include "Nvtt/nvtt/CompressorDX9.h"
#include "Nvtt/nvtt/QuickCompressDXT.h"
#endif
#define STB_IMAGE_RESIZE_IMPLEMENTATION
#include "StbImage/stb_image_resize.h"
bsize BearTextureUtils::GetSizeWidth(bsize w, BearTexturePixelFormat format)
{
	if (isCompressor(format))
	{
		return GetSizeBlock(format)*GetCountBlock(w);
	}
	else
	{
		return GetSizePixel(format)*w;
	}
}

bsize BearTextureUtils::GetSizeDepth(bsize w, bsize h, BearTexturePixelFormat format)
{
	if (isCompressor(format))
	{
		return GetSizeBlock(format)*GetCountBlock(w)*GetCountBlock(h);
	}
	else
	{
		return GetSizePixel(format)*w*h;
	}
}

bsize BearTextureUtils::GetSizePixel(BearTexturePixelFormat format)
{
	switch (format)
	{
	case BearTexturePixelFormat::R8:
		return 1;
		break;
	case BearTexturePixelFormat::R8G8:
		return 2;
		break;
	case BearTexturePixelFormat::R8G8B8:
		return 3;
		break;
	case BearTexturePixelFormat::R8G8B8A8:
		return 4;
		break;
	case BearTexturePixelFormat::R32F:
		return 4;
		break;
	case BearTexturePixelFormat::R32G32F:
		return 8;
		break;
	case BearTexturePixelFormat::R32G32B32F:
		return 12;
		break;
	case BearTexturePixelFormat::R32G32B32A32F:
		return 16;
		break;
	default:
		BEAR_ASSERT(false);
		break;

	}
	return 0;
}

void BearTextureUtils::Append(uint8*dst, bsize w_dst, bsize h_dst, bsize x_dst, bsize y_dst, uint8*src, bsize w_src, bsize h_src, BearVector4<bsize> squard_src, BearTexturePixelFormat dst_format, BearTexturePixelFormat src_format)
{
	if (isCompressor(dst_format))
	{
		BearTexturePixelFormat PixelFormatOut;
		uint8*DestTemp = TempUncompressor(dst, w_dst, h_dst, dst_format, PixelFormatOut);
		Append(DestTemp, w_dst, h_dst, x_dst, y_dst, src, w_src, h_src, squard_src, PixelFormatOut, src_format);
		TempCompress(DestTemp, dst, w_dst, h_dst, dst_format);
		return;
	}
	if (isCompressor(src_format))
	{
		BearTexturePixelFormat PixelFormatOut;
		uint8*SourceTemp = TempUncompressor(src, w_src, h_src, src_format, PixelFormatOut);
		Append(dst, w_dst, h_dst, x_dst, y_dst, SourceTemp, w_src, h_src, squard_src, dst_format, PixelFormatOut );
		TempCompress(SourceTemp, 0, w_src, h_src, src_format);
		return;
	}
	BEAR_ASSERT(w_dst >= squard_src.x1+x_dst);
	BEAR_ASSERT(h_dst >=  squard_src.y1+ y_dst);
	BEAR_ASSERT(w_src >= squard_src.x	+ squard_src.x1);
	BEAR_ASSERT(h_src >= squard_src.y + squard_src.y1);
	uint8 SourceComp = GetCountComp(src_format);
	uint8 DestComp = GetCountComp(dst_format);
	if (isFloatPixel(dst_format) && isFloatPixel(src_format))
	{
		for (bsize iy = 0; iy<squard_src.y1; iy++)
			for (bsize ix = 0; ix < squard_src.x1; ix++)
			{
				FloatPixelToFloat(GetPixelFloat(ix + x_dst, iy + y_dst, w_dst, DestComp, 0, dst), GetPixelFloat(ix+squard_src.x , iy + squard_src.y, w_src, SourceComp, 0, src), DestComp, SourceComp);
			}
	}
	else if (isFloatPixel(dst_format) )
	{
		for (bsize iy = 0; iy<squard_src.y1; iy++)
			for (bsize ix = 0; ix < squard_src.x1; ix++)
			{
				Uint8PixelToFloat(GetPixelFloat(ix + x_dst, iy + y_dst, w_dst, DestComp, 0, dst), GetPixelUint8(ix + squard_src.x, iy + squard_src.y, w_src, SourceComp, 0, src), DestComp, SourceComp);
			}
	}
	else if (isFloatPixel(src_format))
	{
		for (bsize iy = 0; iy<squard_src.y1; iy++)
			for (bsize ix = 0; ix < squard_src.x1; ix++)
			{
				FloatPixelToUint8(GetPixelUint8(ix + x_dst, iy + y_dst, w_dst, DestComp, 0, dst), GetPixelFloat(ix + squard_src.x, iy + squard_src.y, w_src, SourceComp, 0, src), DestComp, SourceComp);
			}
	}
	else
	{
		for (bsize iy = 0; iy<squard_src.y1; iy++)
			for (bsize ix = 0; ix < squard_src.x1; ix++)
			{
				Uint8PixelToUint8(GetPixelUint8(ix + x_dst, iy + y_dst, w_dst, DestComp, 0, dst), GetPixelUint8(ix + squard_src.x, iy + squard_src.y, w_src, SourceComp, 0, src), DestComp, SourceComp);
			}
	}
}


bsize BearTextureUtils::GetCountMips(bsize w, bsize h)
{
	bsize MaxSize = BearMath::max(w, h);
	return static_cast<bsize>(log2f(static_cast<float>(MaxSize))+1);
}

bsize BearTextureUtils::GetMip(bsize w, bsize level)
{
	bsize Mip = static_cast<bsize>((w ) / pow(2, static_cast<double>(level)));
	return BearMath::max(Mip, bsize(1));
}

uint8 * BearTextureUtils::GetPixelUint8(bsize x, bsize y, bsize w, bsize comps, bsize comp_id, uint8 * data)
{
	return  &data[(x + (y * w))*comps+comp_id];
}

float * BearTextureUtils::GetPixelFloat(bsize x, bsize y, bsize w, bsize comps, bsize comp_id, uint8 * data)
{
	return (float*)&data[y*w*comps*4+x*4*comps+comp_id*4];
}

bool BearTextureUtils::isFloatPixel(BearTexturePixelFormat format)
{
	switch (format)
	{
	case BearTexturePixelFormat::R8:
	case BearTexturePixelFormat::R8G8:
	case BearTexturePixelFormat::R8G8B8:
	case BearTexturePixelFormat::R8G8B8A8:
		return false;;
		break;
	case BearTexturePixelFormat::R32F:
	case BearTexturePixelFormat::R32G32F:
	case BearTexturePixelFormat::R32G32B32F:
	case BearTexturePixelFormat::R32G32B32A32F:
		return true;
	case BearTexturePixelFormat::DXT_1:
	case BearTexturePixelFormat::DXT_1_Alpha:
	case BearTexturePixelFormat::DXT_3:
	case BearTexturePixelFormat::DXT_5:
	case BearTexturePixelFormat::BC4:
	case BearTexturePixelFormat::BC5:
	case BearTexturePixelFormat::BC6:
	case BearTexturePixelFormat::BC7:
		return false;

		break;
	default:
		BEAR_ASSERT(false);
	}
	return false;
}

bool BearTextureUtils::isCompressor(BearTexturePixelFormat format)
{
	switch (format)
	{
	case BearTexturePixelFormat::R8:
	case BearTexturePixelFormat::R8G8:
	case BearTexturePixelFormat::R8G8B8:
	case BearTexturePixelFormat::R8G8B8A8:
	case BearTexturePixelFormat::R32F:
	case BearTexturePixelFormat::R32G32F:
	case BearTexturePixelFormat::R32G32B32F:
	case BearTexturePixelFormat::R32G32B32A32F:
		return false;
		break;
	case BearTexturePixelFormat::DXT_1:
	case BearTexturePixelFormat::DXT_1_Alpha:
	case BearTexturePixelFormat::DXT_3:
	case BearTexturePixelFormat::DXT_5:
	case BearTexturePixelFormat::BC4:
	case BearTexturePixelFormat::BC5:
	case BearTexturePixelFormat::BC6:
	case BearTexturePixelFormat::BC7:
		return true;
	default:
		break;
	}
	return false;
}

void BearTextureUtils::Fill(uint8 * data, bsize w, bsize h, bsize mip, const BearColor & color, BearTexturePixelFormat format)
{	
	if (isCompressor(format))
	{
		BearTexturePixelFormat  PixelFormatOut;
		bsize PixelSize = GetSizeBlock(format);
		for (bsize i = 0; i < mip; i++) 
		{

			bsize MipWidth = GetMip(w, i);
			bsize MipHeight = GetMip(h, i);
			uint8*Temp = TempUncompressor(0, MipWidth, MipHeight, format, PixelFormatOut);

			if (isFloatPixel(PixelFormatOut))
			{
				FillFloat(Temp, MipWidth, MipHeight, color, PixelFormatOut);
			}
			else
			{
				FillUint8(Temp, MipWidth, MipHeight, color, PixelFormatOut);
			}
		
			TempCompress(Temp, data, MipWidth, MipHeight, format);
			data += PixelSize * GetCountBlock(MipWidth)*GetCountBlock(MipHeight);
		}

	}
	
	else if (isFloatPixel(format))
	{
		bsize PixelSize = GetSizePixel(format);
		for (bsize i = 0; i < mip; i++) {
			bsize MipWidth = GetMip(w, i);
			bsize MipHeight = GetMip(h, i);
			FillFloat(data, MipWidth, MipHeight, color, format);
			data += PixelSize * MipWidth*MipHeight;
		}
	}
	else
	{
		bsize PixelSize = GetSizePixel(format);
		for (bsize i = 0; i < mip; i++) {
			bsize MipWidth = GetMip(w, i);
			bsize MipHeight = GetMip(h, i);
			FillUint8(data, MipWidth, MipHeight, color, format);
			data += PixelSize * MipWidth*MipHeight;
		}
	}
}

bsize BearTextureUtils::GetSizeInMemory(bsize w, bsize h, bsize mips, BearTexturePixelFormat format)
{
	bsize Result = 0;
	if (!isCompressor(format))
	{
		bsize PixelSize = GetSizePixel(format);
		//Result += w * h * PixelSize;
		for (bsize i = 0; i < mips; i++)
		{
			bsize MipWidth = GetMip(w, i);
			bsize MipHeight = GetMip(h, i);
			Result += MipWidth * MipHeight * PixelSize;
		}
	}
	else
	{
		bsize size_block = GetSizeBlock(format);
	//	Result += GetCountBlock(w)*GetCountBlock(h)*size_block;
		for (bsize i = 0; i < mips; i++)
		{
			bsize MipWidth = GetMip(w, i);
			bsize MipHeight = GetMip(h, i);
			Result += GetCountBlock(MipWidth)*GetCountBlock(MipHeight)* size_block;
		}
	}
	return Result;
}

void BearTextureUtils::Convert(BearTexturePixelFormat dst_format, BearTexturePixelFormat src_format, uint8 * dst, uint8 * src, bsize w, bsize h)
{
	if (src_format == dst_format) { bear_copy(dst, src, GetSizeInMemory(w, h, 1, dst_format)); return; }
	if (isFloatPixel(dst_format)&& isCompressor(src_format))
	{
		CompressorToFloat(dst, src, w, h, GetCountComp(dst_format), src_format);
	}
	else if (isFloatPixel(dst_format) && isFloatPixel(src_format))
	{
		FloatToFloat(dst, src, w, h, GetCountComp(dst_format), GetCountComp(src_format));
	}
	else if (isFloatPixel(dst_format))
	{
		Uint8ToFloat(dst, src, w, h, GetCountComp(dst_format), GetCountComp(src_format));
	}
	else if (isCompressor(dst_format) && isCompressor(src_format))
	{
		CompressorToCompressor(dst, src, w, h, dst_format, src_format);
	}
	else if (isCompressor(dst_format) && isFloatPixel(src_format))
	{
		FloatToCompressor(dst, src, w, h, dst_format, GetCountComp(src_format));
	}
	else  if (isCompressor(dst_format))
	{
		Uint8ToCompressor(dst, src, w, h, dst_format, GetCountComp(src_format));
	}
	else if (isCompressor(src_format))
	{
		CompressorToUint8(dst, src, w, h, GetCountComp(dst_format), src_format);
	}
	else if (isFloatPixel(src_format))
	{
		FloatToUint8(dst, src, w, h, GetCountComp(dst_format), GetCountComp(src_format));
	}
	else
	{
		Uint8ToUint8(dst, src, w, h, GetCountComp(dst_format), GetCountComp(src_format));
	}
}

bsize BearTextureUtils::GetCountBlock(bsize w)
{
	return (w + 3) / 4;
}

bsize BearTextureUtils::GetCountBlock(bsize w, BearTexturePixelFormat format)
{
	if (isCompressor(format))
	{
		return GetCountBlock(w);
	}
	else
	{
		return w;
	}
}

bsize BearTextureUtils::GetSizeBlock(BearTexturePixelFormat format)
{
	switch (format)
	{
	case BearTexturePixelFormat::DXT_1:
		return 8;
	case BearTexturePixelFormat::DXT_1_Alpha:
		return 8;
	case BearTexturePixelFormat::DXT_3:
		return 16;
	case BearTexturePixelFormat::DXT_5:
		return 16;
	case BearTexturePixelFormat::BC4:
		return 8;
		break;
	case BearTexturePixelFormat::BC5:
		return 16;
		break;
	case BearTexturePixelFormat::BC6:
		return 16;
		break;
	case BearTexturePixelFormat::BC7:
		return 16;
		break;
	default:
		BEAR_ASSERT(false);
	}
	return 0;
}	

uint8 BearTextureUtils::GetCountComp(BearTexturePixelFormat format)
{
	switch (format)
	{
	case BearTexturePixelFormat::R8:
		return 1;
		break;
	case BearTexturePixelFormat::R8G8:
		return 2;
		break;
	case BearTexturePixelFormat::R8G8B8:
		return 3;
		break;
	case BearTexturePixelFormat::R8G8B8A8:
		return 4;
		break;
	case BearTexturePixelFormat::R32F:
		return 1;
		break;
	case BearTexturePixelFormat::R32G32F:
		return 2;
		break;
	case BearTexturePixelFormat::R32G32B32F:
		return 3;
		break;
	case BearTexturePixelFormat::R32G32B32A32F:
		return 4;
	default:
		BEAR_ASSERT(false);
	}
	return 0;
}

void BearTextureUtils::GenerateMip(uint8 * dst, uint8 * src, bsize w_src, bsize h_src, BearTexturePixelFormat format, BearResizeFilter filter)
{
	if (isCompressor(format))
	{
		BearTexturePixelFormat PixelFormatOut;
		uint8*SourceTemp = TempUncompressor(src, w_src, h_src, format, PixelFormatOut);
		uint8*DestTemp = TempUncompressor(0, w_src/2, h_src/2, format, PixelFormatOut);
		GenerateMip(DestTemp, SourceTemp, w_src, h_src, PixelFormatOut, filter);
		TempCompress(SourceTemp, 0, w_src, h_src, format);
		TempCompress(DestTemp, dst, w_src / 2, h_src / 2, format);
	}
	else if (isFloatPixel(format))
	{
		GenerateMipFloat(dst, src, w_src, h_src,GetCountComp(format), filter);
	}
	else
	{
		GenerateMipUint8(dst, src, w_src, h_src, GetCountComp(format), filter);
	}
}

void BearTextureUtils::Scale(uint8 * dst, bsize w_dst, bsize h_dst, uint8 * src, bsize w_src, bsize h_src, BearTexturePixelFormat format, BearResizeFilter filter)
{
	if (isCompressor(format))
	{
		BearTexturePixelFormat PixelFormatOut;
		uint8*SourceTemp = TempUncompressor(src, w_src, h_src, format, PixelFormatOut);
		uint8*DestTemp = TempUncompressor(0, w_dst, h_dst, format, PixelFormatOut);
		Scale(DestTemp, w_dst, h_dst, SourceTemp, w_src, h_src, PixelFormatOut,filter);
		TempCompress(SourceTemp, 0, w_src, h_src, format);
		TempCompress(DestTemp, dst, w_dst, h_dst, format);
	}
	else if (isFloatPixel(format))
	{
		ScaleFloat(dst, w_dst, h_dst, src, w_src, h_src, GetCountComp(format), filter);
	}
	else
	{
		ScaleUint8(dst, w_dst, h_dst, src, w_src, h_src, GetCountComp(format), filter);
	}
}

uint8 * BearTextureUtils::GetImage(uint8 * data, bsize w, bsize h, bsize mips, bsize depth, bsize mip, BearTexturePixelFormat format)
{
	return data + BearTextureUtils::GetSizeInMemory(w, h, mips, format)*depth + BearTextureUtils::GetSizeInMemory(w, h, mip, format);
}

void BearTextureUtils::GetPixel(BearColor & color,  uint8*data, bsize x, bsize y, bsize depth, bsize w, bsize h, bsize mips, BearTexturePixelFormat format)
{
	color = BearColor::Black;
	uint8*Image = data + BearTextureUtils::GetSizeInMemory(w, h, mips, format)*depth;
	if (isCompressor(format))
	{
		BearColor Colors[16];
		GetBlock(Colors, Image, w, h, x, y, format);
		color = Colors[x % 4 +( 4 * (y % 4))];
	}
	else if (isFloatPixel(format))
	{
		float* pixels = GetPixelFloat(x, y, w, GetCountComp(format), 0, data);
		switch ( GetCountComp(format))
		{
		case 1:
			color.SetAsFloat(  pixels[0],0,0);
			break;
		case 2:
			color.SetAsFloat(pixels[0], pixels[1], 0);
			break;
		case 3:
			color.SetAsFloat(pixels[0], pixels[1], pixels[2]);
			break;
		case 4:
			color.SetAsFloat(pixels[0], pixels[1], pixels[2], pixels[3]);
			break;
		default:
			BEAR_CHECK(false);
			break;
		}
	}
	else 
	{
		uint8* pixels = GetPixelUint8(x, y, w, GetCountComp(format), 0, data);
		switch (GetCountComp(format))
		{
		case 1:
			color.SetAsUint8(pixels[0], 0, 0);
			break;
		case 2:
			color.SetAsUint8(pixels[0], pixels[1], 0);
			break;
		case 3:
			color.SetAsUint8(pixels[0], pixels[1], pixels[2]);
			break;
		case 4:
			color.SetAsUint8(pixels[0], pixels[1], pixels[2], pixels[3]);
			break;
		default:
			BEAR_CHECK(false);
			break;
		}
	}

	return;
}

void BearTextureUtils::SetPixel(const BearColor & color, uint8 * data, bsize x, bsize y,bsize depth, bsize w, bsize h, bsize mips, BearTexturePixelFormat format)
{
	uint8*Image = data + BearTextureUtils::GetSizeInMemory(w, h, mips, format)*depth;
	if (isCompressor(format))
	{
		BearColor Colors[16];
		GetBlock(Colors, Image, w, h, x, y, format);
		Colors[x % 4 + (4 * (y % 4))] = color;
		SetBlock(Colors, Image, w, h, x, y, format);
	}
	else if (isFloatPixel(format))
	{
		float* pixels = GetPixelFloat(x, y, w, GetCountComp(format), 0, data);
		switch (GetCountComp(format))
		{
		case 1:
			bear_copy(pixels, color.R32G32B32A32, 1);
			break;
		case 2:
			bear_copy(pixels, color.R32G32B32A32,2);
			break;
		case 3:
			bear_copy(pixels, color.R32G32B32A32, 3);
			break;
		case 4:
			bear_copy(pixels, color.R32G32B32A32, 4);
			break;
		default:
			BEAR_CHECK(false);
			break;
		}
	}
	else
	{
		uint8* pixels = GetPixelUint8(x, y, w, GetCountComp(format), 0, data);
		switch (GetCountComp(format))
		{
		case 1:
			bear_copy(pixels, &color.R8G8B8A8, 1);
			break;
		case 2:
			bear_copy(pixels, &color.R8G8B8A8, 2);
			break;
		case 3:
			bear_copy(pixels, &color.R8G8B8A8,3);
			break;
		case 4:
			bear_copy(pixels, &color.R8G8B8A8, 4);
			break;
		default:
			BEAR_CHECK(false);
			break;
		}
	}

	return;
}

void BearTextureUtils::ScaleFloat(uint8 * dst, bsize w_dst, bsize h_dst, uint8 * src, bsize w_src, bsize h_src, uint8 comp, BearResizeFilter filter)
{
	stbir_filter Filter = STBIR_FILTER_DEFAULT;
	switch (filter)
	{
	case BearResizeFilter::Box:
		Filter = STBIR_FILTER_BOX;
		break;
	case BearResizeFilter::Triangle:
		Filter = STBIR_FILTER_TRIANGLE;
		break;
	case BearResizeFilter::Cubicbspline:
		Filter = STBIR_FILTER_CUBICBSPLINE;
		break;
	case BearResizeFilter::Catmullrom:
		Filter = STBIR_FILTER_CATMULLROM;
		break;
	case BearResizeFilter::Mitchell:
		Filter = STBIR_FILTER_MITCHELL;
		break;
	}
	stbir_resize(reinterpret_cast<void*>(src), static_cast<int>(w_src), static_cast<int>(h_src), 0, reinterpret_cast<void*>(dst), static_cast<int>(w_dst), static_cast<int>(h_dst), 0, STBIR_TYPE_FLOAT, comp, -1, 0, STBIR_EDGE_CLAMP, STBIR_EDGE_CLAMP, Filter, Filter, STBIR_COLORSPACE_LINEAR, NULL);
}

void BearTextureUtils::ScaleUint8(uint8 * dst, bsize w_dst, bsize h_dst, uint8 * src, bsize w_src, bsize h_src, uint8 comp, BearResizeFilter filter)
{
	stbir_filter Filter = STBIR_FILTER_DEFAULT;
	switch (filter)
	{
	case BearResizeFilter::Box:
		Filter = STBIR_FILTER_BOX;
		break;
	case BearResizeFilter::Triangle:
		Filter = STBIR_FILTER_TRIANGLE;
		break;
	case BearResizeFilter::Cubicbspline:
		Filter = STBIR_FILTER_CUBICBSPLINE;
		break;
	case BearResizeFilter::Catmullrom:
		Filter = STBIR_FILTER_CATMULLROM;
		break;
	case BearResizeFilter::Mitchell:
		Filter = STBIR_FILTER_MITCHELL;
		break;
	}
	stbir_resize(reinterpret_cast<void*>(src), static_cast<int>(w_src), static_cast<int>(h_src), 0, reinterpret_cast<void*>(dst), static_cast<int>(w_dst), static_cast<int>(h_dst), 0, STBIR_TYPE_UINT8, comp, -1, 0, STBIR_EDGE_CLAMP, STBIR_EDGE_CLAMP, Filter, Filter, STBIR_COLORSPACE_LINEAR, NULL);

}



void BearTextureUtils::GenerateMipFloat(uint8 * dst, uint8 * src, bsize w_src, bsize h_src, uint8 comps, BearResizeFilter filter)
{
	ScaleFloat(dst, w_src / 2, h_src / 2, src, w_src, h_src, comps, filter);
}

void BearTextureUtils::GenerateMipUint8(uint8 * dst, uint8 * src, bsize w_src, bsize h_src, uint8 comps, BearResizeFilter filter)
{
	ScaleUint8(dst, w_src / 2, h_src / 2, src, w_src, h_src, comps, filter);
}

void BearTextureUtils::FloatPixelToUint8(uint8 * dst, float * src, uint8 comp_dst, uint8 comp_src)
{
	for (uint8 i = 0; i < comp_dst; i++)
	{
		if (i < comp_src)
		{
			* dst = static_cast<uint8>(BearMath::clamp(*src, 0.f, 1.f) *255.f);
			src++;
		}
		else if(i==3)
		{
			*dst = 255;
		}
		else
		{
			*dst = 0;
		}
		dst++;
	}
}

void BearTextureUtils::FloatPixelToFloat(float * dst, float * src, uint8 comp_dst, uint8 comp_src)
{
	for (uint8 i = 0; i < comp_dst; i++)
	{
		if (i < comp_src)
		{
			*dst = *src;
			src++;
		}
		else if (i == 3)
		{
			*dst = 1.f;
		}
		else
		{
			*dst = 0.f;
		}
		dst++;
	}
}

void BearTextureUtils::Uint8PixelToFloat(float * dst, uint8 * src, uint8 comp_dst, uint8 comp_src)
{
	for (uint8 i = 0; i < comp_dst; i++)
	{
		if (i < comp_src)
		{
			*dst = static_cast<float>(*src/255.f);
			src++;
		}
		else if (i == 3)
		{
			*dst = 1.f;
		}
		else
		{
			*dst = 0.f;
		}
		dst++;
	}
}
void BearTextureUtils::Uint8PixelToUint8(uint8 * dst, uint8 * src, uint8 comp_dst, uint8 comp_src)
{
	for (uint8 i = 0; i < comp_dst; i++)
	{
		if (i < comp_src)
		{
			*dst = *src;
			src++;
		}
		else if (i == 3)
		{
			*dst = 255;
		}
		else
		{
			*dst = 255;
		}
		dst++;
	}
}
uint16 floatToHalf(float f)
{
	uint32 fuint32 = *((uint32*)&f);
	uint16 a = ((fuint32 >> 16) & 0b1000000000000000) | ((((fuint32 & 0b1111111100000000000000000000000) - 0b111000000000000000000000000000) >> 13) & 0b111110000000000) | ((fuint32 >> 13) & 0b1111111111);
	if (f == 0)
		return 0;
	return a;
}

void BearTextureUtils::FloatPixelToHalf4(uint16 * dst, float * src, uint8 comp_src)
{
	for (uint8 i = 0; i < 4; i++)
	{
		if (i < comp_src)
		{
			if(i==3)
				*dst = floatToHalf(1.f);
			else
				*dst = floatToHalf(*src);
			src++;
		}
		else if (i == 3)
		{
			*dst = floatToHalf(1.f);
		}
		else
		{
			*dst = floatToHalf(0.f);
		}
		dst++;
	}
}

void BearTextureUtils::Uint8PixelToHalf4(uint16 * dst, uint8 * src, uint8 comp_src)
{
	for (uint8 i = 0; i < 4; i++)
	{
		if (i < comp_src)
		{
			if (i == 3)
				*dst = floatToHalf(1.f);
			else
			*dst = floatToHalf(static_cast<float>(*src / 255.f));
			src++;
		}
		else if (i == 3)
		{
			*dst = floatToHalf(1.f);
		}
		else
		{
			*dst = floatToHalf(0.f);
		}
		dst++;
	}
}

void BearTextureUtils::SwapRB(uint32 & color)
{
		uint32 R = color & 0x000000FF;

		uint32 B = color & 0x00FF0000;
		uint32 G = color & 0x0000FF00;
		uint32 A = color & 0xFF000000;
		color = (B >> 16) | G | (R << 16) | A;
}

void BearTextureUtils::FillUint8(uint8 * data, bsize x, bsize y, const BearColor & color, BearTexturePixelFormat format)
{
	bsize SizePixel = GetSizePixel(format);
	for (bsize i = 0; i < x*y; i++)
	{
		bear_copy(data + (SizePixel*i), &color.R8G8B8A8, SizePixel);
	}
}

void BearTextureUtils::FillFloat(uint8 * data, bsize x, bsize y, const BearColor & color, BearTexturePixelFormat format)
{
	bsize SizePixel = GetSizePixel(format);
	for (bsize i = 0; i < x*y; i++)
	{
		bear_copy(data + (SizePixel*i), color.R32G32B32A32, SizePixel);
	}
}

void BearTextureUtils::Uint8ToHalf4(uint8 * dst, uint8 * src, bsize w, bsize h, uint8 comp_src)
{
	for (bsize i = 0; i < w*h; i++)
	{
		Uint8PixelToHalf4((uint16*)dst, src, comp_src);
		dst += 4*2;
		src += comp_src;
	}
}

void BearTextureUtils::Uint8ToFloat(uint8 * dst, uint8 * src, bsize w, bsize h, uint8 comp_dst, uint8 comp_src)
{
	for (bsize i = 0; i < w*h; i++)
	{
		Uint8PixelToFloat((float*)dst, src, comp_dst, comp_src);
		dst += comp_dst * 4;
		src += comp_src;
	}
}

void BearTextureUtils::Uint8ToUint8(uint8 * dst, uint8 * src, bsize w, bsize h, uint8 comp_dst, uint8 comp_src)
{
	for (bsize i = 0; i < w*h; i++)
	{
		Uint8PixelToUint8(dst, src, comp_dst, comp_src);
		dst += comp_dst ;
		src += comp_src;
	}
}

void BearTextureUtils::Uint8ToCompressor(uint8 * dst, uint8 * src, bsize w, bsize h, BearTexturePixelFormat compressor, uint8 comp_src)
{
	uint8*In=(uint8*)StartCompressor(compressor,w,h);
	switch (compressor)
	{
	case BearTexturePixelFormat::DXT_1:
		Uint8ToFloat(In, src, w, h, 3, comp_src);
		break;
	case BearTexturePixelFormat::DXT_1_Alpha:
		Uint8ToUint8(In, src, w, h, 4, comp_src);
		break;
	case BearTexturePixelFormat::DXT_3:
		Uint8ToUint8(In, src, w, h, 4, comp_src);
		break;
	case BearTexturePixelFormat::DXT_5:
		Uint8ToUint8(In, src, w, h, 4, comp_src);
		break;
	case BearTexturePixelFormat::BC4:
		Uint8ToUint8(In, src, w, h, 1, comp_src);
		break;
	case BearTexturePixelFormat::BC5:
		Uint8ToUint8(In, src, w, h, 2, comp_src);
		break;
	case BearTexturePixelFormat::BC6:
		Uint8ToHalf4(In, src, w, h,  comp_src);
		break;
	case BearTexturePixelFormat::BC7:
		Uint8ToUint8(In, src, w, h, 4, comp_src);
		break;
	default:
		BEAR_ASSERT(false)
	}

	EndCompressor(compressor, w, h, In, dst);
}

void BearTextureUtils::FloatToHalf4(uint8 * dst, uint8 * src, bsize w, bsize h, uint8 comp_src)
{
	for (bsize i = 0; i < w*h; i++)
	{
		Uint8PixelToHalf4((uint16*)dst, src, comp_src);
		dst += 4 * 2;
		src += comp_src*4;
	}
}

void BearTextureUtils::FloatToUint8(uint8 * dst, uint8 * src, bsize w, bsize h, uint8 comp_dst, uint8 comp_src)
{
	for (bsize i = 0; i < w*h; i++)
	{
		FloatPixelToUint8(dst, (float*)src, comp_dst, comp_src);
		dst += comp_dst ;
		src += comp_src * 4;
	}
}

void BearTextureUtils::FloatToFloat(uint8 * dst, uint8 * src, bsize w, bsize h, uint8 comp_dst, uint8 comp_src)
{
	for (bsize i = 0; i < w*h; i++)
	{
		FloatPixelToFloat((float*)dst, (float*)src, comp_dst, comp_src);
		dst += comp_dst * 4;
		src += comp_src * 4;
	}
}

void BearTextureUtils::FloatToCompressor(uint8 * dst, uint8 * src, bsize w, bsize h, BearTexturePixelFormat compressor, uint8 comp_src)
{
	uint8*in = (uint8*)StartCompressor(compressor, w, h);
	switch (compressor)
	{
	case BearTexturePixelFormat::DXT_1:
		FloatToFloat(in, src, w, h, 3, comp_src);
		break;
	case BearTexturePixelFormat::DXT_1_Alpha:
		FloatToUint8(in, src, w, h, 4, comp_src);
		break;
	case BearTexturePixelFormat::DXT_3:
		FloatToUint8(in, src, w, h, 4, comp_src);
		break;
	case BearTexturePixelFormat::DXT_5:
		FloatToUint8(in, src, w, h, 4, comp_src);
		break;
	case BearTexturePixelFormat::BC4:
		FloatToUint8(in, src, w, h, 1, comp_src);
		break;
	case BearTexturePixelFormat::BC5:
		FloatToUint8(in, src, w, h, 2, comp_src);
		break;
	case BearTexturePixelFormat::BC6:
		FloatToHalf4(in, src, w, h, comp_src);
		break;
	case BearTexturePixelFormat::BC7:
		FloatToUint8(in, src, w, h, 4, comp_src);
		break;
	default:
		BEAR_ASSERT(false)
	}

	EndCompressor(compressor, w, h, in, dst);
}

void BearTextureUtils::CompressorToCompressor(uint8 * dst, uint8 * src, bsize w, bsize h, BearTexturePixelFormat compressor_dst, BearTexturePixelFormat compressor_src)
{

	uint8*in_dst = (uint8*)StartCompressor(compressor_dst, w, h);
	switch (compressor_dst)
	{
	case BearTexturePixelFormat::DXT_1:
		CompressorToFloat(in_dst, src, w, h, 3, compressor_src);
		break;
	case BearTexturePixelFormat::DXT_1_Alpha:
		CompressorToUint8(in_dst, src, w, h, 4, compressor_src);
		break;
	case BearTexturePixelFormat::DXT_3:
		CompressorToUint8(in_dst, src, w, h, 4, compressor_src);
		break;
	case BearTexturePixelFormat::DXT_5:
		CompressorToUint8(in_dst, src, w, h, 4, compressor_src);
		break;
	case BearTexturePixelFormat::BC4:
		CompressorToUint8(in_dst, src, w, h, 1, compressor_src);
		break;
	case BearTexturePixelFormat::BC5:
		CompressorToUint8(in_dst, src, w, h, 2, compressor_src);
		break;
	case BearTexturePixelFormat::BC6:
	{		
	float*temp = bear_alloc<float>(w*h * 3);
	CompressorToFloat((uint8*)temp, src, w, h, 3, compressor_src);
	FloatToHalf4(dst, (uint8*)temp, w, h, 3);
	bear_free(temp);
	}
		break;
	case BearTexturePixelFormat::BC7:
		CompressorToUint8(in_dst, src, w, h, 4, compressor_src);
		break;
	default:
		BEAR_ASSERT(false)
	}
	EndCompressor(compressor_dst, w, h, in_dst, dst);
}

void BearTextureUtils::CompressorToUint8(uint8 * dst, uint8 * src, bsize w, bsize h, uint8 comp_dst, BearTexturePixelFormat compressor)
{
	uint8*in=(uint8*)StartDecompressor(compressor, w, h, src);
	switch (compressor)
	{
	case BearTexturePixelFormat::DXT_1:
	case BearTexturePixelFormat::DXT_1_Alpha:
		Uint8ToUint8(dst, in, w, h, comp_dst, 4);
		break;
	case BearTexturePixelFormat::DXT_3:
		Uint8ToUint8(dst, in, w, h, comp_dst, 4);
		break;
	case BearTexturePixelFormat::DXT_5:
		Uint8ToUint8(dst, in, w, h, comp_dst, 4);
		break;
	case BearTexturePixelFormat::BC4:
		Uint8ToUint8(dst, in, w, h, comp_dst, 1);
		break;
	case BearTexturePixelFormat::BC5:
		Uint8ToUint8(dst, in, w, h, comp_dst, 2);
		break;
	case BearTexturePixelFormat::BC6:
		FloatToUint8(dst, in, w, h, comp_dst, 3);
		break;
	case BearTexturePixelFormat::BC7:
		Uint8ToUint8(dst, in, w, h, comp_dst, 4);
		break;
	default:
		BEAR_ASSERT(false)
	}
	EndDecompressor(in);
}

void BearTextureUtils::CompressorToFloat(uint8 * dst, uint8 * src, bsize w, bsize h, uint8 comp_dst, BearTexturePixelFormat compressor)
{
	uint8*in = (uint8*)StartDecompressor(compressor, w, h, src);
	switch (compressor)
	{
	case BearTexturePixelFormat::DXT_1:
	case BearTexturePixelFormat::DXT_1_Alpha:
		Uint8ToFloat(dst, in, w, h, comp_dst, 4);
		break;
	case BearTexturePixelFormat::DXT_3:
		Uint8ToFloat(dst, in, w, h, comp_dst, 4);
		break;
	case BearTexturePixelFormat::DXT_5:
		Uint8ToFloat(dst, in, w, h, comp_dst, 4);
		break;
	case BearTexturePixelFormat::BC4:
		Uint8ToFloat(dst, in, w, h, comp_dst, 1);
		break;
	case BearTexturePixelFormat::BC5:
		Uint8ToFloat(dst, in, w, h, comp_dst, 2);
		break;
	case BearTexturePixelFormat::BC6:
		FloatToFloat(dst, in, w, h, comp_dst, 3);
		break;
	case BearTexturePixelFormat::BC7:
		Uint8ToFloat(dst, in, w, h, comp_dst, 4);
		break;
	default:
		BEAR_ASSERT(false)
	}
	EndDecompressor(in);
}

void * BearTextureUtils::StartCompressor(BearTexturePixelFormat compressor, bsize w, bsize h)
{
	switch (compressor)
	{
	case BearTexturePixelFormat::DXT_1:
		return bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R32G32B32F)*h);
	case BearTexturePixelFormat::DXT_1_Alpha:
		return bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8B8A8)*h);
	case BearTexturePixelFormat::DXT_3:
		return bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8B8A8)*h);
	case BearTexturePixelFormat::DXT_5:
		return bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8B8A8)*h);
	case BearTexturePixelFormat::BC4:
		return bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8)*h);
	case BearTexturePixelFormat::BC5:
		return bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8)*h);
	case BearTexturePixelFormat::BC6:
		return bear_alloc<uint8>(w*2*4*h);
	case BearTexturePixelFormat::BC7:
		return bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8B8A8)*h);
	default:
		BEAR_ASSERT(false);
	}
	return 0;
}

void BearTextureUtils::EndCompressor(BearTexturePixelFormat compressor, bsize w, bsize h, void*in, void*out)
{

#ifdef DEVELOPER_VERSION
	switch (compressor)
	{
	case BearTexturePixelFormat::DXT_1:
	{
		nvtt::CompressionOptions CompOpt;
		CompOpt.setQuality(nvtt::Quality_Highest);
		CompOpt.setQuantization(false, false, true);

		CompOpt.setFormat(nvtt::Format_DXT1);

		const uint32 BlockWidth =static_cast<uint32>( (w + 3) / 4);
		const uint32 BlockHeight = static_cast<uint32>((h + 3) / 4);
		nv::CompressorDXT1 Compressor;
		for (uint32 by = 0; by < BlockHeight; by++)
		{
			for (uint32 bx = 0; bx < BlockWidth; bx++)
			{
				float wa[16];
				nv::Vector4 Block[16];
				for (bsize y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (bsize x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						wa[x + y * 4] = 1.f;

						Block[x + y * 4].component[0] = *GetPixelFloat( bx * 4+x,  by * 4+y, w, 3,0,(uint8*) in);
						Block[x + y * 4].component[1] = *(GetPixelFloat( bx * 4+x,  by * 4+y, w, 3, 1,(uint8*)in));
						Block[x + y * 4].component[2] = *(GetPixelFloat( bx * 4+x,  by * 4+y, w, 3, 2,(uint8*)in));
						Block[x + y * 4].component[3] = 1;


					}
				}
				Compressor.compressBlock(Block, wa, CompOpt.m, (uint8*)out + 8 * (bx + (by * BlockWidth)));
			}
		}
	}
	break;
	case BearTexturePixelFormat::DXT_1_Alpha:
	{
		nvtt::CompressionOptions CompOpt;
		CompOpt.setQuality(nvtt::Quality_Highest);
		CompOpt.setQuantization(false, false, true);

		CompOpt.setFormat(nvtt::Format_DXT1a);

		const bsize BlockWidth = static_cast<bsize>((w + 3) / 4);
		const bsize BlockHeight = static_cast<bsize>((h + 3) / 4);
		nv::CompressorDXT1a Compressor;
		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::ColorBlock Block;
				for (bsize y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (bsize x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						auto&color = Block.color(static_cast<uint>(x), static_cast<uint>(y));
						color.r = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 0, (uint8*)in);
						color.g = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 1, (uint8*)in);
						color.b = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 2, (uint8*)in);
						color.a = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 3, (uint8*)in);
						color.a = color.a > 127 ? 255 : 0;
					}
				}
				Compressor.compressBlock(Block, nvtt::AlphaMode_Transparency, CompOpt.m, (uint8*)out + 8 * (bx + (by * BlockWidth)));
			}
		}
	}
	break;
	case BearTexturePixelFormat::DXT_3:
	{
		nvtt::CompressionOptions CompOpt;
		CompOpt.setQuality(nvtt::Quality_Highest);
		CompOpt.setQuantization(false, false, true);

		CompOpt.setFormat(nvtt::Format_DXT3);
		const bsize BlockWidth = static_cast<bsize>((w + 3) / 4);
		const bsize BlockHeight = static_cast<bsize>((h + 3) / 4);
		nv::CompressorDXT3 Compressor;
		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::ColorBlock Block;
				for (bsize y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (bsize x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						auto& color = Block.color(static_cast<uint>(x), static_cast<uint>(y));
						color.r = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 0, (uint8*)in);
						color.g = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 1, (uint8*)in);
						color.b = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 2, (uint8*)in);
						color.a = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 3, (uint8*)in);

					}
				}
				Compressor.compressBlock(Block, nvtt::AlphaMode_Transparency, CompOpt.m, (uint8*)out + 16 * (bx + by * BlockWidth));
			}
		}
	}
	break;
	case BearTexturePixelFormat::DXT_5:
	{
		nvtt::CompressionOptions CompOpt;
		CompOpt.setQuality(nvtt::Quality_Highest);
		CompOpt.setQuantization(false, false, true);

		CompOpt.setFormat(nvtt::Format_DXT5);

		const bsize BlockWidth = static_cast<bsize>((w + 3) / 4);
		const bsize BlockHeight = static_cast<bsize>((h + 3) / 4);
		nv::CompressorDXT5 Compressor;
		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::ColorBlock Block;
				for (bsize y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (bsize x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						auto& color = Block.color(static_cast<uint>(x), static_cast<uint>(y));
						color.r = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 0, (uint8*)in);
						color.g = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 1, (uint8*)in);
						color.b = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 2, (uint8*)in);
						color.a = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 4, 3, (uint8*)in);

					}
				}
				Compressor.compressBlock(Block, nvtt::AlphaMode_Transparency, CompOpt.m, (uint8*)out + 16 * (bx + by * BlockWidth));
			}
		}
	}
	break;
	case BearTexturePixelFormat::BC4:
	{
		const bsize BlockWidth = static_cast<bsize>((w + 3) / 4);
		const bsize BlockHeight = static_cast<bsize>((h + 3) / 4);

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::ColorBlock Block;
				nv::AlphaBlock4x4 Alpha1;
				nv::AlphaBlockDXT5 AlphaBlock1;
				for (bsize y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (bsize x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						auto& color = Block.color(static_cast<uint>(x), static_cast<uint>(y));
						color.b = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 1, 0, (uint8*)in);
					}
				}
				Alpha1.init(Block, 2);
				nv::QuickCompress::compressDXT5A(Alpha1, &AlphaBlock1, 8);
				bear_copy((uint8*)out + 8 * (bx + by * BlockWidth), &AlphaBlock1.u, 8);
			}
		}
		break;
	}
	case BearTexturePixelFormat::BC5:
	{
		const bsize BlockWidth = static_cast<bsize>((w + 3) / 4);
		const bsize BlockHeight = static_cast<bsize>((h + 3) / 4);

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::ColorBlock Block;
				nv::AlphaBlock4x4 Alpha1, Alpha2;
				nv::AlphaBlockDXT5 AlphaBlock1, AlphaBlock2;
				for (bsize y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (bsize x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						auto& color = Block.color(static_cast<uint>(x), static_cast<uint>(y));
						color.b = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 2, 0, (uint8*)in);
						color.g = *GetPixelUint8(bx * 4 + x, by * 4 + y, w, 2,1, (uint8*)in);
						//in = (uint8*)in + 2;
					}
				}
				Alpha1.init(Block, 2);
				Alpha2.init(Block, 1);
				nv::QuickCompress::compressDXT5A(Alpha1, &AlphaBlock1, 8);
				nv::QuickCompress::compressDXT5A(Alpha2, &AlphaBlock2, 8);
				bear_copy((uint8*)out + 16 * (bx + by * BlockWidth), &AlphaBlock1.u, 8);
				bear_copy((uint8*)out + 16 * (bx + by * BlockWidth) + 8, &AlphaBlock2.u, 8);
			}
		}
	}
	break;
	case BearTexturePixelFormat::BC6:
	{
		rgba_surface Surface;
		Surface.height = static_cast<uint32>(h);
		Surface.width = static_cast<uint32>(w);
		Surface.ptr = (uint8*)in;
		Surface.stride = static_cast<uint32>(w * 2 * 4);
		bc6h_enc_settings str;
		GetProfile_bc6h_veryslow(&str);
		CompressBlocksBC6H(&Surface, (uint8*)out, &str);
	}
	break;
	case BearTexturePixelFormat::BC7:
	{
		rgba_surface Surface;
		Surface.height = static_cast<uint32>(h);
		Surface.width = static_cast<uint32>(w);
		Surface.ptr = (uint8*)in;
		Surface.stride = static_cast<uint32>(w * 4);
		bc7_enc_settings str;
		GetProfile_alpha_slow(&str);
		CompressBlocksBC7(&Surface, (uint8*)out, &str);
		break;
	}
	default:
		BEAR_ASSERT(false);
	}
	bear_free(in);
#else
BEAR_ASSERT(false);
#endif
}

void * BearTextureUtils::StartDecompressor(BearTexturePixelFormat Compressor, bsize w, bsize h, void * in)
{

#ifdef DEVELOPER_VERSION
	switch (Compressor)
	{
	case BearTexturePixelFormat::DXT_1_Alpha:
	case BearTexturePixelFormat::DXT_1:
	{

		uint8*new_img = bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8B8A8)*h);
		const bsize BlockWidth = (w + 3) / 4;
		const bsize BlockHeight = (h + 3) / 4;

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::BlockDXT1 BlockDXT;
				nv::ColorBlock ColorBlock;
				bear_copy(&BlockDXT.col0.u, (uint8*)in + 8 * (bx + (by*BlockWidth)), 2);
				bear_copy(&BlockDXT.col1.u, (uint8*)in + 8 * (bx + (by*BlockWidth)) + 2, 2);
				bear_copy(&BlockDXT.indices, (uint8*)in + 8 * (bx + (by*BlockWidth)) + 4, 4);
				BlockDXT.decodeBlock(&ColorBlock);

				for (uint32 y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (uint32 x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 0, new_img) = ColorBlock.color(x, y).r;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 1, new_img) = ColorBlock.color(x, y).g;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 2, new_img) = ColorBlock.color(x, y).b;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 3, new_img) = ColorBlock.color(x, y).a;

					}
				}
			}
		}
		return new_img;
	}
	break;
	case BearTexturePixelFormat::DXT_3:
	{
		uint8*new_img = bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8B8A8)*h);
		const bsize BlockWidth = (w + 3) / 4;
		const bsize BlockHeight = (h + 3) / 4;

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::BlockDXT3 BlockDXT;
				nv::ColorBlock ColorBlock;
				bear_copy(&BlockDXT.color.col0.u, (uint8*)in + 16 * (bx + (by*BlockWidth)) + 8, 2);
				bear_copy(&BlockDXT.color.col1.u, (uint8*)in + 16 * (bx + (by*BlockWidth)) + 2 + 8, 2);
				bear_copy(&BlockDXT.color.indices, (uint8*)in + 16 * (bx + (by*BlockWidth)) + 4 + 8, 4);
				bear_copy(&BlockDXT.alpha.row, (uint8*)in + 16 * (bx + (by*BlockWidth)) , 8);
				BlockDXT.decodeBlock(&ColorBlock);

				for (uint32 y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (uint32 x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 0, new_img) = ColorBlock.color(x, y).r;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 1, new_img) = ColorBlock.color(x, y).g;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 2, new_img) = ColorBlock.color(x, y).b;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 3, new_img) = ColorBlock.color(x, y).a;

					}
				}
			}
		}
		return new_img;
	}
	break;
	case BearTexturePixelFormat::DXT_5:
	{
		uint8*new_img = bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8B8A8)*h);
		const bsize BlockWidth = (w + 3) / 4;
		const bsize BlockHeight = (h + 3) / 4;

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::BlockDXT5 BlockDXT;
				nv::ColorBlock ColorBlock;
				bear_copy(&BlockDXT.color.col0.u, (uint8*)in + 16 * (bx + (by*BlockWidth)) + 8, 2);
				bear_copy(&BlockDXT.color.col1.u, (uint8*)in + 16 * (bx + (by*BlockWidth)) + 2 + 8, 2);
				bear_copy(&BlockDXT.color.indices, (uint8*)in + 16 * (bx + (by*BlockWidth)) + 4+ 8, 4);
				bear_copy(&BlockDXT.alpha.u, (uint8*)in + 16 * (bx + (by*BlockWidth)) , 8);
				BlockDXT.decodeBlock(&ColorBlock);

				for (uint32 y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (uint32 x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 0, new_img) = ColorBlock.color(x, y).r;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 1, new_img) = ColorBlock.color(x, y).g;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 2, new_img) = ColorBlock.color(x, y).b;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 3, new_img) = ColorBlock.color(x, y).a;

					}
				}
			}
		}
		return new_img;
	}
	break;
	case BearTexturePixelFormat::BC4:
	{
		uint8*new_img = bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8)*h);
		const bsize BlockWidth = (w + 3) / 4;
		const bsize BlockHeight = (h + 3) / 4;

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::BlockATI1 ATI1;
				nv::ColorBlock ColorBlock;
				bear_copy(&ATI1.alpha, (uint8*)in + 8 * (bx + (by*BlockWidth)), 8);
				ATI1.decodeBlock(&ColorBlock);

				for (uint32 y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (uint32 x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 1, 0, new_img) = ColorBlock.color(x, y).r;

					}
				}
			}
		}
		return new_img;
	}
	break;
	case BearTexturePixelFormat::BC5:
	{
		uint8*new_img = bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8)*h);
		const bsize BlockWidth = (w + 3) / 4;
		const bsize BlockHeight = (h + 3) / 4;

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::BlockATI2 ATI2;
				nv::ColorBlock ColorBlock;
				bear_copy(&ATI2.x, (uint8*)in + 16 * (bx + (by*BlockWidth)), 8);
				bear_copy(&ATI2.y, (uint8*)in + 16 * (bx + (by*BlockWidth)) + 8, 8);
				ATI2.decodeBlock(&ColorBlock);

				for (uint32 y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (uint32 x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 2, 0, new_img) = ColorBlock.color(x, y).r;
						*GetPixelUint8(x + bx * 4, y + by * 4, w, 2, 1, new_img) = ColorBlock.color(x, y).g;
					}
				}
			}
		}
		return new_img;
	}
	break;
	case BearTexturePixelFormat::BC6:
	{
		uint8*new_img = bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R32G32B32F)*h);
		const bsize BlockWidth = (w + 3) / 4;
		const bsize BlockHeight = (h + 3) / 4;

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::BlockBC6 BlockBC6;
				nv::Vector3 ColorBlock[16];
				bear_copy(BlockBC6.data, (uint8*)in + 16 * (bx + (by*BlockWidth)), 16);
				BlockBC6.decodeBlock(ColorBlock);

				for (bsize y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (bsize x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						bear_copy(GetPixelFloat(x + bx * 4, y + by * 4, w, 3, 0, new_img), ColorBlock[x + y * 4].component, 3);
						//swapRG(*reinterpret_cast<uint32*>(getPixelComp((uint8*)out, w, x + bx * 4, y + by * 4, out_comp, 0));
					}
				}
			}
		}
		return new_img;
	}
	break;
	case BearTexturePixelFormat::BC7:
	{
		uint8*new_img = bear_alloc<uint8>(GetSizeWidth(w, BearTexturePixelFormat::R8G8B8A8)*h);
		const bsize BlockWidth = (w + 3) / 4;
		const bsize BlockHeight = (h + 3) / 4;

		for (bsize by = 0; by < BlockHeight; by++)
		{
			for (bsize bx = 0; bx < BlockWidth; bx++)
			{
				nv::BlockBC7 BlockBC7;
				nv::ColorBlock ColorBlock;
				bear_copy(BlockBC7.data, (uint8*)in + 16 * (bx + (by*BlockWidth)), 16);
				BlockBC7.decodeBlock(&ColorBlock);

				for (uint32 y = 0; y < BearMath::min(bsize(4), h - 4 * by); y++)
				{
					for (uint32 x = 0; x < BearMath::min(bsize(4), w - 4 * bx); x++)
					{
						SwapRB(*reinterpret_cast<uint32*>((uint8*)ColorBlock.color(x, y).component));
						bear_copy(GetPixelUint8(x + bx * 4, y + by * 4, w, 4, 0, new_img), ColorBlock.color(x, y).component, 4);
					}
				}
			}
		}
		return new_img;
	}
	break;
	default:
		BEAR_ASSERT(false);
	}
	return 0;
#else
BEAR_ASSERT(false);
return 0;
#endif
}

void BearTextureUtils::EndDecompressor(void * in)
{
	bear_free(in);
}

void BearTextureUtils::GetBlock(BearColor(&color)[16], uint8 * data, bsize w, bsize h, bsize x_, bsize y, BearTexturePixelFormat px)
{
#ifdef DEVELOPER_VERSION

	uint8*Block = data + ((x_ / 4) + ((w + 3) / 4)*(y / 4))*(px == BearTexturePixelFormat::BC1 || px == BearTexturePixelFormat::BC1a || px == BearTexturePixelFormat::BC4 ? 8 : 16);
	nv::ColorBlock ColorBlock;
	switch (px)
	{

	case BearTexturePixelFormat::BC1:
	{
		nv::BlockDXT1 BlockDXT;

		bear_copy(&BlockDXT.col0, Block, 8);
		BlockDXT.decodeBlock(&ColorBlock);

		for (uint x = 0; x < 16; x++)
		{
			color[x].SetAsUint8(ColorBlock.color(x).r, ColorBlock.color(x).g, ColorBlock.color(x).b, ColorBlock.color(x).a);
		}
	}
	break;
	case BearTexturePixelFormat::BC1a:
	{
		nv::BlockDXT1 BlockDXT;

		bear_copy(&BlockDXT.col0, Block, 8);
		BlockDXT.decodeBlock(&ColorBlock);
		for (uint x = 0; x < 16; x++)
		{
			color[x].SetAsUint8(ColorBlock.color(x).r, ColorBlock.color(x).g, ColorBlock.color(x).b, ColorBlock.color(x).a);
		}
	}
	break;
	case BearTexturePixelFormat::BC2:
	{
		nv::BlockDXT3 BlockDXT;

		bear_copy(&BlockDXT.alpha, Block, 8);
		bear_copy(&BlockDXT.color, Block + 8, 8);
		BlockDXT.decodeBlock(&ColorBlock);
		for (uint x = 0; x < 16; x++)
		{
			color[x].SetAsUint8(ColorBlock.color(x).r, ColorBlock.color(x).g, ColorBlock.color(x).b, ColorBlock.color(x).a);
		}
	}
	break;
	case BearTexturePixelFormat::BC3:
	{
		nv::BlockDXT5 BlockDXT;

		bear_copy(&BlockDXT.alpha, Block, 8);
		bear_copy(&BlockDXT.color, Block+8, 8);
		BlockDXT.decodeBlock(&ColorBlock);
		for (uint x = 0; x < 16; x++)
		{
			color[x].SetAsUint8(ColorBlock.color(x).r, ColorBlock.color(x).g, ColorBlock.color(x).b, ColorBlock.color(x).a);
		}
	}
	break;
	case BearTexturePixelFormat::BC4:
	{
		nv::BlockATI1 BlockATI;

		bear_copy(&BlockATI.alpha, Block, 8);
		BlockATI.decodeBlock(&ColorBlock);
		for (uint x = 0; x < 16; x++)
		{
			color[x].SetAsUint8(ColorBlock.color(x).r, ColorBlock.color(x).g, ColorBlock.color(x).b, ColorBlock.color(x).a);
		}
	}
	break;
	case BearTexturePixelFormat::BC5:
	{
		nv::BlockATI2 BlockATI;

		bear_copy(&BlockATI.x, Block, 8);
		bear_copy(&BlockATI.y, Block+8, 8);
		BlockATI.decodeBlock(&ColorBlock);
		for (uint x = 0; x < 16; x++)
		{
			color[x].SetAsUint8(ColorBlock.color(x).r, ColorBlock.color(x).g, ColorBlock.color(x).b, ColorBlock.color(x).a);
		}
	}
	break;
	case BearTexturePixelFormat::BC6:
	{
		nv::BlockBC6 BlockBC6;
		nv::Vector3 ColorFloat[16];

		bear_copy(&BlockBC6.data, Block, 16);
		BlockBC6.decodeBlock(ColorFloat);
		for (uint x = 0; x < 16; x++)
		{
			color[x].SetAsFloat(ColorFloat[x].x, ColorFloat[x].y, ColorFloat[x].z, 1.f);
		}
		return;
	}
	break;
	case BearTexturePixelFormat::BC7:
	{
		nv::BlockBC7 BlockBC7;

		bear_copy(&BlockBC7.data, Block, 16);
		BlockBC7.decodeBlock(&ColorBlock);

		for (uint x = 0; x < 16; x++)
		{
			color[x].SetAsUint8(ColorBlock.color(x).r, ColorBlock.color(x).g, ColorBlock.color(x).b, ColorBlock.color(x).a);
		}
	}
	break;
	default:
		break;
	}
#else
BEAR_ASSERT(false);
#endif
	
}

void BearTextureUtils::SetBlock(BearColor(&color)[16], uint8 * data, bsize w, bsize h, bsize x_, bsize y, BearTexturePixelFormat px)
{
#ifdef DEVELOPER_VERSION
	uint8*Block = data + ((x_ / 4) + ((w + 3) / 4)*(y / 4))*(px == BearTexturePixelFormat::BC1 || px == BearTexturePixelFormat::BC1a || px == BearTexturePixelFormat::BC4 ? 8 : 16);
	uint8 ImagePixel16UInt8[16 * 4];
//	float imagePixel16Float[16 * 4];
	uint16 ImagePixel16UInt16[16 * 4];
	switch (px)
	{
	case BearTexturePixelFormat::BC1:
	{
		nv::CompressorDXT1 Compressor;
		nvtt::CompressionOptions CompOpt;
		CompOpt.setQuality(nvtt::Quality_Highest);
		CompOpt.setQuantization(false, false, false);

		CompOpt.setFormat(nvtt::Format_DXT1);

		float wa[16];
		nv::Vector4 blockColor[16];
		for (bsize x = 0; x < 16; x++)
		{
			wa[x] = 1.f;
			blockColor[x].x = color[x].R32F;
			blockColor[x].y = color[x].B32F;
			blockColor[x].z = color[x].G32F;
			blockColor[x].w = color[x].A32F;
		}
		Compressor.compressBlock(blockColor, wa, CompOpt.m, Block);
	}

	break;
	case BearTexturePixelFormat::BC1a:
	{
		nv::CompressorDXT1a Compressor;
		nvtt::CompressionOptions CompOpt;
		CompOpt.setQuality(nvtt::Quality_Highest);
		CompOpt.setQuantization(false, false, false);

		CompOpt.setFormat(nvtt::Format_DXT1a);

		nv::ColorBlock blockColor;
		for (uint x = 0; x < 16; x++)
		{
			blockColor.color(x).setRGBA(color[x].R8U, color[x].G8U, color[x].B8U, color[x].A8U );
		}
		Compressor.compressBlock(blockColor, nvtt::AlphaMode_Transparency, CompOpt.m, Block);
	}
	break;
	case BearTexturePixelFormat::BC2:
	{
		nv::CompressorDXT3 Compressor;
		nvtt::CompressionOptions CompOpt;
		CompOpt.setQuality(nvtt::Quality_Highest);
		CompOpt.setQuantization(false, false, false);

		CompOpt.setFormat(nvtt::Format_DXT1a);

		nv::ColorBlock blockColor;
		for (uint x = 0; x < 16; x++)
		{
			blockColor.color(x).setRGBA(color[x].R8U, color[x].G8U, color[x].B8U, color[x].A8U);
		}
		Compressor.compressBlock(blockColor, nvtt::AlphaMode_Transparency, CompOpt.m, Block);
	}
	break;
	case BearTexturePixelFormat::BC3:
	{
		nv::CompressorDXT5 Compressor;
		nvtt::CompressionOptions CompOpt;
		CompOpt.setQuality(nvtt::Quality_Highest);
		CompOpt.setQuantization(false, false, false);

		CompOpt.setFormat(nvtt::Format_DXT1a);

		nv::ColorBlock blockColor;
		for (uint x = 0; x < 16; x++)
		{
			blockColor.color(x).setRGBA(color[x].R8U, color[x].G8U, color[x].B8U, color[x].A8U);
		}
		Compressor.compressBlock(blockColor, nvtt::AlphaMode_Transparency, CompOpt.m, Block);
	}
	break;
	case BearTexturePixelFormat::BC4:
	{
		nv::ColorBlock blockColor;
		nv::AlphaBlock4x4 Alpha1;
		nv::AlphaBlockDXT5 AlphaBlock1;
		for (uint x = 0; x < 16; x++)
		{
			blockColor.color(x).setRGBA(color[x].R8U, color[x].G8U, color[x].B8U, color[x].A8U);
		}
		Alpha1.init(blockColor, 2);
		nv::QuickCompress::compressDXT5A(Alpha1, &AlphaBlock1, 8);
		bear_copy(Block, &AlphaBlock1.u, 8);

	}
	break;
	case BearTexturePixelFormat::BC5:
	{
		nv::ColorBlock blockColor;
		nv::AlphaBlock4x4 Alpha1, Alpha2;
		nv::AlphaBlockDXT5 AlphaBlock1, AlphaBlock2;
		for (uint x = 0; x < 16; x++)
		{
			blockColor.color(x).setRGBA(color[x].R8U, color[x].G8U, color[x].B8U, color[x].A8U);
		}
		Alpha1.init(blockColor, 2);
		Alpha2.init(blockColor, 1);
		nv::QuickCompress::compressDXT5A(Alpha1, &AlphaBlock1, 8);
		nv::QuickCompress::compressDXT5A(Alpha2, &AlphaBlock2, 8);
		bear_copy(Block, &AlphaBlock1.u, 8);
		bear_copy(Block + 8, &AlphaBlock2.u, 8);
	}
	break;
	case BearTexturePixelFormat::BC6:
		for (uint x = 0; x < 16; x++)
		{
			ImagePixel16UInt16[x * 4] = floatToHalf(color[x].R32F);
			ImagePixel16UInt16[x * 4 + 1] = floatToHalf(color[x].G32F);
			ImagePixel16UInt16[x * 4 + 2] = floatToHalf(color[x].B32F);
			ImagePixel16UInt16[x * 4 + 3] = floatToHalf(color[x].A32F);

		}
		{

			rgba_surface Surface;
			Surface.height = static_cast<int32>(h);
			Surface.width = static_cast<int32>(w);
			Surface.ptr = (uint8*)ImagePixel16UInt16;
			/*for (bsize i = 0; i < w*h; i++)
			*(Image + i * 4) = 0;*/
			Surface.stride = static_cast<int32>(w) * 2 * 4;
			bc6h_enc_settings str;
			GetProfile_bc6h_veryslow(&str);
			CompressBlocksBC6H(&Surface, (uint8*)Block, &str);
		}
		break;
	case BearTexturePixelFormat::BC7:
		for (bsize x = 0; x < 16; x++)
		{
			ImagePixel16UInt8[x * 4] = color[x].R8U;
			ImagePixel16UInt8[x * 4 + 1] = color[x].G8U;
			ImagePixel16UInt8[x * 4 + 2] = color[x].B8U;
			ImagePixel16UInt8[x * 4 + 3] = color[x].A8U;

		}
		{
			rgba_surface Surface;
			Surface.height = static_cast<int32>(h);
			Surface.width = static_cast<int32>(w);
			Surface.ptr = ImagePixel16UInt8;
			Surface.stride = static_cast<int32>(w) * 4;
			bc7_enc_settings str;
			if (GetCountComp(px) == 4)
				GetProfile_alpha_slow(&str);
			else
				GetProfile_slow(&str);
			CompressBlocksBC7(&Surface, (uint8*)Block, &str);
		}
		break;
	default:
		break;
	}
#else
BEAR_ASSERT(false);
#endif
}

bool BearTextureUtils::CompressorAsFloat(BearTexturePixelFormat px)
{
	switch (px)
	{
	case BearTexturePixelFormat::BC6:
		return true;
		break;
	}
	return false;
}

uint8 * BearTextureUtils::TempUncompressor(uint8 * data, bsize w, bsize h, BearTexturePixelFormat px, BearTexturePixelFormat & out_px)
{
	
	switch (px)
	{
	case BearTexturePixelFormat::BC1:
		out_px = BearTexturePixelFormat::R32G32B32F;
		break;
	case BearTexturePixelFormat::BC1a:
	case BearTexturePixelFormat::BC2:
	case BearTexturePixelFormat::BC3:
		out_px = BearTexturePixelFormat::R8G8B8A8;
		break;
	case BearTexturePixelFormat::BC4:
		out_px = BearTexturePixelFormat::R8;
		break;
	case BearTexturePixelFormat::BC5:
		out_px = BearTexturePixelFormat::R8G8;
		break;
	case BearTexturePixelFormat::BC6:
		out_px = BearTexturePixelFormat::R32G32B32F;
		break;
	case BearTexturePixelFormat::BC7:
		out_px = BearTexturePixelFormat::R8G8B8A8;
		break;
	default:
		break;
	}

	uint8*data_out = bear_alloc<uint8>(GetSizeWidth(w, out_px)*h);
	if(data!=0)
	Convert(out_px, px, data_out,data , w, h);
	return data_out;
}

void BearTextureUtils::TempCompress(uint8 * in, uint8 * out, bsize w, bsize h, BearTexturePixelFormat px)
{
	BearTexturePixelFormat InPixelFormat;
	switch (px)
	{
	case BearTexturePixelFormat::BC1:
		InPixelFormat = BearTexturePixelFormat::R32G32B32F;
		break;
	case BearTexturePixelFormat::BC1a:
	case BearTexturePixelFormat::BC2:
	case BearTexturePixelFormat::BC3:
		InPixelFormat = BearTexturePixelFormat::R8G8B8A8;
		break;
	case BearTexturePixelFormat::BC4:
		InPixelFormat = BearTexturePixelFormat::R8;
		break;
	case BearTexturePixelFormat::BC5:
		InPixelFormat = BearTexturePixelFormat::R8G8;
		break;
	case BearTexturePixelFormat::BC6:
		InPixelFormat = BearTexturePixelFormat::R32G32B32F;
		break;
	case BearTexturePixelFormat::BC7:
		InPixelFormat = BearTexturePixelFormat::R8G8B8A8;
		break;
	default:
		InPixelFormat = BearTexturePixelFormat::R8;
		break;
	}
	if(out!=0)
		Convert(px, InPixelFormat,out, in, w, h);
	bear_free(in);
}


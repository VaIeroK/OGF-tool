#pragma once
class BEARGRAPHICS_API BearTextureUtils
{
public:
	static void Fill(uint8* data, bsize x, bsize y, bsize mip, const BearColor& color, BearTexturePixelFormat format);
	static void GenerateMip(uint8* dst, uint8* src, bsize w_src, bsize h_src, BearTexturePixelFormat format, BearResizeFilter filter);
	static void Scale(uint8* dst, bsize w_dst, bsize h_dst, uint8* src, bsize w_src, bsize h_src, BearTexturePixelFormat format, BearResizeFilter filter);
	static void Append(uint8* dst, bsize w_dst, bsize h_dst, bsize x_dst, bsize y_dst, uint8* src, bsize w_src, bsize h_src, BearVector4<bsize> squard_src, BearTexturePixelFormat dst_format, BearTexturePixelFormat src_format);


	static bsize GetSizeWidth(bsize w, BearTexturePixelFormat format);
	static bsize GetSizeDepth(bsize w, bsize h, BearTexturePixelFormat format);
	static bsize GetSizePixel(BearTexturePixelFormat format);
	static bsize GetCountMips(bsize w, bsize h);
	static bsize GetMip(bsize w, bsize level);
	static uint8* GetPixelUint8(bsize x, bsize y, bsize w, bsize comps, bsize comp_id, uint8* data);
	static float* GetPixelFloat(bsize x, bsize y, bsize w, bsize comps, bsize comp_id, uint8* data);
	static bool isFloatPixel(BearTexturePixelFormat format);
	static bool isCompressor(BearTexturePixelFormat format);
	static bsize GetSizeInMemory(bsize w, bsize h, bsize mips, BearTexturePixelFormat format);
	static void Convert(BearTexturePixelFormat dst_format, BearTexturePixelFormat src_format, uint8* dst, uint8* src, bsize w, bsize h);
	static bsize GetCountBlock(bsize w);
	static bsize GetCountBlock(bsize w, BearTexturePixelFormat format);
	static bsize GetSizeBlock(BearTexturePixelFormat format);
	static uint8 GetCountComp(BearTexturePixelFormat format);
	static uint8* GetImage(uint8* data, bsize w, bsize h, bsize mips, bsize depth, bsize mip, BearTexturePixelFormat format);

	static void  GetPixel(BearColor& color, uint8* data, bsize x, bsize y, bsize depth, bsize w, bsize h, bsize mips, BearTexturePixelFormat format);
	static void  SetPixel(const BearColor& color, uint8* data, bsize x, bsize y, bsize depth, bsize w, bsize h, bsize mips, BearTexturePixelFormat format);
private:
	static void ScaleFloat(uint8* dst, bsize w_dst, bsize h_dst, uint8* src, bsize w_src, bsize h_src, uint8 comp, BearResizeFilter filter);
	static void ScaleUint8(uint8* dst, bsize w_dst, bsize h_dst, uint8* src, bsize w_src, bsize h_src, uint8 comp, BearResizeFilter filter);


	static void GenerateMipFloat(uint8* dst, uint8* src, bsize w_src, bsize h_src, uint8 comps, BearResizeFilter filter);
	static void GenerateMipUint8(uint8* dst, uint8* src, bsize w_src, bsize h_src, uint8 comps, BearResizeFilter filter);

	static void FloatPixelToUint8(uint8* dst, float* src, uint8 comp_dst, uint8 comp_src);
	static void FloatPixelToFloat(float* dst, float* src, uint8 comp_dst, uint8 comp_src);
	static void Uint8PixelToFloat(float* dst, uint8* src, uint8 comp_dst, uint8 comp_src);
	static void Uint8PixelToUint8(uint8* dst, uint8* src, uint8 comp_dst, uint8 comp_src);

	static void FloatPixelToHalf4(uint16* dst, float* src, uint8 comp_src);
	static void Uint8PixelToHalf4(uint16* dst, uint8* src, uint8 comp_src);

	static void SwapRB(uint32& a);

	static void FillUint8(uint8* data, bsize x, bsize y, const BearColor& color, BearTexturePixelFormat format);
	static void FillFloat(uint8* data, bsize x, bsize y, const BearColor& color, BearTexturePixelFormat format);

	static void Uint8ToHalf4(uint8* dst, uint8* src, bsize w, bsize h, uint8 comp_src);
	static void Uint8ToFloat(uint8* dst, uint8* src, bsize w, bsize h, uint8 comp_dst, uint8 comp_src);
	static void Uint8ToUint8(uint8* dst, uint8* src, bsize w, bsize h, uint8 comp_dst, uint8 comp_src);
	static void Uint8ToCompressor(uint8* dst, uint8* src, bsize w, bsize h, BearTexturePixelFormat compressor, uint8 comp_src);

	static void FloatToHalf4(uint8* dst, uint8* src, bsize w, bsize h, uint8 comp_src);
	static void FloatToUint8(uint8* dst, uint8* src, bsize w, bsize h, uint8 comp_dst, uint8 comp_src);
	static void FloatToFloat(uint8* dst, uint8* src, bsize w, bsize h, uint8 comp_dst, uint8 comp_src);
	static void FloatToCompressor(uint8* dst, uint8* src, bsize w, bsize h, BearTexturePixelFormat compressor, uint8 comp_src);

	static void CompressorToCompressor(uint8* dst, uint8* src, bsize w, bsize h, BearTexturePixelFormat compressor_dst, BearTexturePixelFormat compressor_src);
	static void CompressorToUint8(uint8* dst, uint8* src, bsize w, bsize h, uint8 comp_dst, BearTexturePixelFormat compressor);
	static void CompressorToFloat(uint8* dst, uint8* src, bsize w, bsize h, uint8 comp_dst, BearTexturePixelFormat compressor);

	static void* StartCompressor(BearTexturePixelFormat compressor, bsize w, bsize h);
	static void EndCompressor(BearTexturePixelFormat compressor, bsize w, bsize h, void* in, void* out);

	static void* StartDecompressor(BearTexturePixelFormat compressor, bsize w, bsize h, void* in);
	static void EndDecompressor(void* in);

	static void GetBlock(BearColor(&color)[16], uint8* data, bsize w, bsize h, bsize x, bsize y, BearTexturePixelFormat px);
	static void SetBlock(BearColor(&color)[16], uint8* data, bsize w, bsize h, bsize x, bsize y, BearTexturePixelFormat px);
	static bool CompressorAsFloat(BearTexturePixelFormat px);
	static uint8* TempUncompressor(uint8* data, bsize w, bsize h, BearTexturePixelFormat px, BearTexturePixelFormat& out_px);
	static void TempCompress(uint8* in, uint8* out, bsize w, bsize h, BearTexturePixelFormat px);
};;
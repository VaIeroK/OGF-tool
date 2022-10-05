#pragma once 

class BEARGRAPHICS_API BearImage
{
public:
	BearImage();
	BearImage(bsize w, bsize h, bsize mip = 1, bsize depth = 1, BearTexturePixelFormat px = BearTexturePixelFormat::R8G8B8A8, bool cube = false);
	void Fill(const BearColor& color = BearColor::Brown);
	void Create(bsize w, bsize h, bsize mip = 1, bsize depth = 1, BearTexturePixelFormat px = BearTexturePixelFormat::R8G8B8A8, bool cube = false);
	void Append(bsize x, bsize y, const BearImage& Image, bsize x_src, bsize y_src, bsize w_src, bsize h_src, bsize dst_depth, bsize src_depth);
	void Append(bsize x, bsize y, const BearImage& Image, bsize dst_depth, bsize src_depth);

	void Scale(bsize w, bsize h,  BearResizeFilter filter = BearResizeFilter::Default);

	void ScaleCanvas(bsize w, bsize h);


	void GenerateMipmap(bsize depth, BearResizeFilter filter = BearResizeFilter::Default);
	void GenerateMipmap(BearResizeFilter filter = BearResizeFilter::Default);
	void ClearMipLevels();

	void NormalizedSizeNotScale();
	void NormalizedSize(BearResizeFilter filter = BearResizeFilter::Default);

	BearColor GetPixel(bsize x, bsize y, bsize d = 0)const;
	void SetPixel(const BearColor& color, bsize x, bsize y, bsize d = 0);
	void SwapRB();
	
	bool LoadFromFile(const bchar* str);
	bool LoadFromStream(const BearInputStream& stream);
	bool LoadFromBuffer(const BearBufferedReader& stream);

	bool LoadFromFile(bsize depth, const bchar* str);
	bool LoadFromStream(bsize depth, const BearInputStream& stream);
	bool LoadFromBuffer(bsize depth, const BearBufferedReader& stream);

	bool SaveToJpg(const bchar* name, bsize depth = 0);
	bool SaveToPng(const bchar* name, bsize depth = 0);
	bool SaveToBmp(const bchar* name, bsize depth = 0);
	bool SaveToTga(const bchar* name, bsize depth = 0);
	bool SaveToDds(const bchar* name);
	void* operator*();
	const void* operator*()const;

	void Clear();
	~BearImage();

	bool Empty() const;

	BearImage(BearImage&& Image);
	BearImage(const BearImage& Image);
	void Copy(const BearImage& Image);
	void Swap(BearImage& Image);
	BearImage& operator=(const BearImage& Image);
	BearImage& operator=(BearImage&& Image);
	void Convert(BearTexturePixelFormat format);

	BearTexturePixelFormat GetFormat()const;
	bsize GetDepth()const;
	bsize GetMips()const;
	bsize GetSizeInMemory()const;
	BearVector2<bsize>GetSize()const;
	bool HasAlpha()const;
	size_t GetWidth()const;
	size_t GetHeight()const;
	inline bool SetCubeMap() { m_bCube = m_Width == m_Height && (m_Depth % 6 == 0) && m_Depth; if (m_bCube)m_Depth = m_Depth / 6; return m_bCube; }
	inline bool IsCubeMap()const { return m_bCube; }
private:
	void Resize(bsize w, bsize h, bsize depth, BearTexturePixelFormat px);
	BearTexturePixelFormat m_PixelFotmat;
	bsize m_Width, m_Height, m_Mips, m_Depth;
	bool m_bCube;
	bool m_bHasAlpha;
	uint8* m_ImageBuffer;
	size_t m_sWidth, m_sHeight;
private:
	bool LoadDDSFromFile(const bchar* str);
	bool LoadDDSFromStream(const BearInputStream& stream);
	bool LoadDDSFromBuffer(const BearBufferedReader& stream);
};

#pragma once 
class  BearColor
{
public:
	BearColor() { SetAsUint32(0); }
	inline ~BearColor() { }

	BearColor(uint32 color)
	{
		SetAsUint32(color);
	}
	BearColor(float R, float G, float B, float A = 1) 
	{
		SetAsFloat(R, G, B, A);
	}

	BearColor(const BearColor&color)  { Copy(color); }
	inline void Copy(const BearColor&color) { R8G8B8A8 = color.R8G8B8A8; bear_copy(R32G32B32A32, color.R32G32B32A32, 4); }
	inline void Swap(BearColor&color) { bear_swap(&R8G8B8A8, &color.R8G8B8A8,1); bear_swap(R32G32B32A32, color.R32G32B32A32,4); }
	inline BearColor&operator=(const BearColor&color)
	{
		Copy(color);
		return *this;
	}

	inline void SetAsUint32(uint32 color)
	{
		R8G8B8A8 = color;
		R32F = static_cast<float>(R8U) / 255.f;
		G32F = static_cast<float>(G8U) / 255.f;
		B32F = static_cast<float>(B8U) / 255.f;
		A32F = static_cast<float>(A8U) / 255.f;
	}
	void SetAsUint8(uint8 R, uint8 G, uint8 B, uint8 A = 255)
	{
		R8U = R;
		G8U = G;
		B8U = B;
		A8U = A;
		R32F = static_cast<float>(R8U) / 255.f;
		G32F = static_cast<float>(G8U) / 255.f;
		B32F = static_cast<float>(B8U) / 255.f;
		A32F = static_cast<float>(A8U) / 255.f;
	}
	void SetAsFloat(float R, float G, float B, float A = 1.f)
	{
		R32F = R;
		G32F = G;
		B32F = B;
		A32F = A;
		R8U = static_cast<uint8>(R32F*255.f);
		G8U = static_cast<uint8>(G32F*255.f);
		B8U = static_cast<uint8>(B32F*255.f);
		A8U = static_cast<uint8>(A32F*255.f);
	}

	union
	{
		struct
		{
			float R32F;
			float G32F;
			float B32F;
			float A32F;
		};
		struct
		{
			float R32G32B32A32[4];
		};
	};
	union
	{
		struct
		{
			uint8 R8U;
			uint8 G8U;
			uint8 B8U;
			uint8 A8U;
		};
		struct
		{
			uint32 R8G8B8A8;
		};
	};
	BEARTOOL_API static const BearColor Black;
	BEARTOOL_API static const BearColor White;
	BEARTOOL_API static const BearColor Red;
	BEARTOOL_API static const BearColor Green;
	BEARTOOL_API static const BearColor Blue;
	BEARTOOL_API static const BearColor Yellow;
	BEARTOOL_API static const BearColor Magenta;
	BEARTOOL_API static const BearColor Cyan;
	BEARTOOL_API static const BearColor Transparent;
	BEARTOOL_API static const BearColor Brown;
private:

}BEAR_PACKED;
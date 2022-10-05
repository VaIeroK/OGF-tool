#pragma once
struct BearSamplerDescription
{
	BearSamplerDescription(
		BearSamplerAddressMode addressU = BearSamplerAddressMode::Wrap,
		BearSamplerAddressMode addressV = BearSamplerAddressMode::Wrap,
		BearSamplerAddressMode addressW = BearSamplerAddressMode::Wrap,
		bsize mipBias = 0,
		bsize maxAnisotropy = 1,
		BearSamplerFilter filter = BearSamplerFilter::MinMagMipLinear,
		BearColor borderColor = BearColor::Transparent
	)
		:AddressU(addressU),
		AddressV(addressV),
		AddressW(addressW),
		MipBias(mipBias),
		MaxAnisotropy(maxAnisotropy),
		Filter(filter),
		BorderColor(borderColor)
	{}

	BearSamplerAddressMode AddressU;
	BearSamplerAddressMode AddressV;
	BearSamplerAddressMode AddressW;
	bsize MipBias;
	bsize MaxAnisotropy;
	BearSamplerFilter Filter;
	BearColor BorderColor;
};
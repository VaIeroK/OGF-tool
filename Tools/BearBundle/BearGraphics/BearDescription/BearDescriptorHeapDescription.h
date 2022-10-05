#pragma once

class  BEARGRAPHICS_API  BearDescriptorHeapDescription
{
public:

	BearDescriptorHeapDescription() {}
	inline ~BearDescriptorHeapDescription(){}
	BearDescriptorHeapDescription(const BearDescriptorHeapDescription& Right) { Copy(Right); }
	BearDescriptorHeapDescription(BearDescriptorHeapDescription&& Right) { Swap(Right); }
	void Copy(const BearDescriptorHeapDescription& Right);
	void Swap(BearDescriptorHeapDescription& Right);
	inline BearDescriptorHeapDescription& operator=(const BearDescriptorHeapDescription& Right) { Copy(Right); return*this; }
	inline BearDescriptorHeapDescription& operator=(BearDescriptorHeapDescription&& Right) { Swap(Right); return*this; }

	BearFactoryPointer<BearRHI::BearRHIRootSignature> RootSignature;

};
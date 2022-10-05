#include "BearGraphics.hpp"

void BearDescriptorHeapDescription::Copy(const BearDescriptorHeapDescription& Right)
{
	RootSignature = Right.RootSignature;
}

void BearDescriptorHeapDescription::Swap(BearDescriptorHeapDescription& Right)
{
	RootSignature.swap(Right.RootSignature);
}

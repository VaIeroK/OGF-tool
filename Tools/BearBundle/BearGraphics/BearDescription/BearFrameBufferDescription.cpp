#include "BearGraphics.hpp"

void BearFrameBufferDescription::Copy(const BearFrameBufferDescription& Right)
{
	for (bsize i = 0; i < 8; i++)
	{
		RenderTargets[i] = Right.RenderTargets[i];
		
	}
	DepthStencil = Right.DepthStencil;
	RenderPass = Right.RenderPass;
}

void BearFrameBufferDescription::Swap(BearFrameBufferDescription& Right)
{
	for (bsize i = 0; i < 8; i++)
	{
		RenderTargets[i] .swap( Right.RenderTargets[i]);

	}
	DepthStencil.swap( Right.DepthStencil);
	RenderPass.swap(Right.RenderPass);
}

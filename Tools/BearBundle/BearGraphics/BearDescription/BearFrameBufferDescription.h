#pragma once

class  BEARGRAPHICS_API  BearFrameBufferDescription
{
public:

	BearFrameBufferDescription() {}
	inline ~BearFrameBufferDescription() {}
	BearFrameBufferDescription(const BearFrameBufferDescription& Right) { Copy(Right); }
	BearFrameBufferDescription(BearFrameBufferDescription&& Right) { Swap(Right); }
	void Copy(const BearFrameBufferDescription& Right);
	void Swap(BearFrameBufferDescription& Right);
	inline BearFrameBufferDescription& operator=(const BearFrameBufferDescription& Right) { Copy(Right); return*this; }
	inline BearFrameBufferDescription& operator=(BearFrameBufferDescription&& Right) { Swap(Right); return*this; }

	BearFactoryPointer<BearRHI::BearRHITexture2D> RenderTargets[8];
	BearFactoryPointer<BearRHI::BearRHITexture2D> DepthStencil;
	BearFactoryPointer<BearRHI::BearRHIRenderPass> RenderPass;

};
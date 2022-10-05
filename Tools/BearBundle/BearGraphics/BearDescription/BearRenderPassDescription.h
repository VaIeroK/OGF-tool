#pragma once
struct BearRenderPassDescription
{
	BearRenderPassDescription()
	{
		DepthStencil.Format = BearDepthStencilFormat::None;
		DepthStencil.Clear = false;
	}
	struct RenderTarget
	{
		RenderTarget()
		{
			Clear = false;
			Format = BearRenderTargetFormat::None;
		}
		BearRenderTargetFormat Format;
		bool Clear;
		BearColor Color;
	}
	RenderTargets[8];
	struct
	{
		BearDepthStencilFormat Format;
		bool Clear;
		uint8 Stencil;
		float Depth;
	}
	DepthStencil;
};
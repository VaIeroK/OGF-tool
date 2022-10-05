#pragma once
struct BearDepthStencilStateDescription
{
	BearDepthStencilStateDescription(
		bool depth_enable = false,
		bool enable_depth_write = true,
		BearCompareFunction depth_test = BearCompareFunction::LessEqual,
		bool stencill_enable = false,
		bool back_stencill_enable = false,
		uint8 stencil_read_mask = 0xFF,
		uint8 stencil_write_mask = 0xFF)
		:DepthEnable(depth_enable),
		EnableDepthWrite(enable_depth_write),
		DepthTest(depth_test),
		StencillEnable(stencill_enable),
		BackStencillEnable(back_stencill_enable),
		StencilReadMask(stencil_read_mask),
		StencilWriteMask(stencil_write_mask)
	{

	}
	struct Stencill
	{
		Stencill(BearStencilOp stencil_fail_op = BearStencilOp::Keep,
			BearStencilOp stencil_depth_fail_op = BearStencilOp::Keep,
			BearStencilOp stencil_pass_op = BearStencilOp::Keep,
			BearCompareFunction stencil_test = BearCompareFunction::Always) :StencilFailOp(stencil_fail_op), StencilDepthFailOp(stencil_depth_fail_op), StencilPassOp(stencil_pass_op), StencilTest(stencil_test) {}

		BearStencilOp StencilFailOp;
		BearStencilOp StencilDepthFailOp;
		BearStencilOp StencilPassOp;
		BearCompareFunction StencilTest;
	};
	bool EnableDepthWrite;
	bool DepthEnable;
	BearCompareFunction DepthTest;
	bool StencillEnable;
	bool BackStencillEnable;
	uint8 StencilReadMask;
	uint8 StencilWriteMask;
	Stencill FrontFace;
	Stencill BackFace;
};
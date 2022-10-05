#pragma once
struct BearRasterizerStateDescription
{
	BearRasterizerStateDescription(
		BearRasterizerCullMode cull_mode = BearRasterizerCullMode::Back,
		BearRasterizerFillMode fill_mode = BearRasterizerFillMode::Solid,
		BearRasterizerFrontFace front_face = BearRasterizerFrontFace::FaceClockwise,
		float depth_bias = 0,
		float slope_scale_depth_bias = 0) :
		CullMode(cull_mode),
		FillMode(fill_mode),
		FrontFace(front_face),
		DepthBiasEnable(false),
		DepthBias(depth_bias),
		DepthSlopeScaleBias(slope_scale_depth_bias),
		DepthClampEnable(false),
		DepthClmapBias(0)
	{}
	BearRasterizerCullMode CullMode;
	BearRasterizerFillMode FillMode;
	BearRasterizerFrontFace FrontFace;

	bool DepthBiasEnable;
	float DepthBias;
	float DepthSlopeScaleBias;

	bool DepthClampEnable;
	float DepthClmapBias;
};
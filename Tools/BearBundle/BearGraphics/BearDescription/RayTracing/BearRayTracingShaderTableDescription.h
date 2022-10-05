#pragma once
class BEARGRAPHICS_API BearRayTracingShaderTableDescription
{
public:
	BearRayTracingShaderTableDescription(){}
	inline ~BearRayTracingShaderTableDescription(){}
	BearRayTracingShaderTableDescription(const BearRayTracingShaderTableDescription& Right) { Copy(Right); }
	BearRayTracingShaderTableDescription(BearRayTracingShaderTableDescription&& Right) { Swap(Right); }
	inline void Copy(const BearRayTracingShaderTableDescription& Right)
	{
		RayGenerateShader = Right.RayGenerateShader;
		MissShader = Right.MissShader;
		CallableShader = Right.CallableShader;
		HitGroups.copy(Right.HitGroups);
		Pipeline.copy(Right.Pipeline);
	}
	inline void Swap(BearRayTracingShaderTableDescription& Right)
	{
		RayGenerateShader.swap(Right.RayGenerateShader);
		MissShader.swap(Right.MissShader);
		CallableShader.swap(Right.CallableShader);
		HitGroups.swap(Right.HitGroups);
		Pipeline.swap(Right.Pipeline);
	}
	inline BearRayTracingShaderTableDescription& operator=(const BearRayTracingShaderTableDescription& Right) { Copy(Right); return*this; }
	inline BearRayTracingShaderTableDescription& operator=(BearRayTracingShaderTableDescription&& Right) { Swap(Right); return*this; }
	
	BearFactoryPointer<BearRHI::BearRHIPipelineRayTracing>	Pipeline;
	BearStringConteniarUnicode								RayGenerateShader;
	BearStringConteniarUnicode								MissShader;
	BearStringConteniarUnicode								CallableShader;
	BearVector<BearStringConteniarUnicode>					HitGroups;
};
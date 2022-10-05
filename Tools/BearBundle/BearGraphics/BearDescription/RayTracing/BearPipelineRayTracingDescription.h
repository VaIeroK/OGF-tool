#pragma once
class BEARGRAPHICS_API BearPipelineRayTracingDescription
{
public:
	BearPipelineRayTracingDescription();
	BearPipelineRayTracingDescription(const BearPipelineRayTracingDescription& Right) { Copy(Right); }
	BearPipelineRayTracingDescription(BearPipelineRayTracingDescription&& Right) { Swap(Right); }
	inline ~BearPipelineRayTracingDescription() {}

	void Copy(const BearPipelineRayTracingDescription& Right);
	void Swap(BearPipelineRayTracingDescription& Right);
	inline BearPipelineRayTracingDescription& operator=(const BearPipelineRayTracingDescription& Right) { Copy(Right); return*this; }
	inline BearPipelineRayTracingDescription& operator=(BearPipelineRayTracingDescription&& Right) { Swap(Right); return*this; }
	bool operator== (const BearPipelineRayTracingDescription& Right)const;
	inline bool operator!= (const BearPipelineRayTracingDescription& Right)const { return !((*this) == Right); }
	bool operator<(const BearPipelineRayTracingDescription& Right)const;

	struct HitGroupDescription
	{
		HitGroupDescription():Type(BearHitGroupType::Triangles){}
		BearHitGroupType	Type;
		BearStringConteniarUnicode NameExport;
		BearStringConteniarUnicode AnyHitShaderImport;
		BearStringConteniarUnicode ClosestHitShaderImport;
		BearStringConteniarUnicode IntersectionShaderImport;
	};
	BearVector<HitGroupDescription> HitGroups;

	struct ShaderDescription
	{
		struct ExportDescription
		{
			ExportDescription() :Type(BearRayTracingShaderType::RayGeneration) {}
			BearStringConteniarUnicode NameFunction;
			BearStringConteniarUnicode NameExport;
			BearRayTracingShaderType Type;
		};
		BearVector<ExportDescription> Exports;
		BearFactoryPointer<BearRHI::BearRHIShader>	Shader;
	};
	BearVector<ShaderDescription> Shaders;
	
	BearFactoryPointer<BearRHI::BearRHIRootSignature> GlobalRootSignature;

/*	struct LocalRootSignatureDescription
	{
		BearVector<BearStringConteniarUnicode> Exports;
		BearFactoryPointer<BearRHI::BearRHIRootSignature> RootSignature;
	};
	BearVector<LocalRootSignatureDescription> LocalRootSignatures;*/

	struct ShaderConfigDescription
	{
		uint32 MaxPayloadSizeInBytes;
		uint32 MaxAttributeSizeInBytes;
	} ShaderConfig;

	struct PipelineConfigDescription
	{
		uint32 MaxTraceRecursionDepth;
	}PipelineConfig;

};
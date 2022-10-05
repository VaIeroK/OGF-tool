#include "BearGraphics.hpp"
BearPipelineRayTracingDescription::BearPipelineRayTracingDescription()
{
	PipelineConfig.MaxTraceRecursionDepth = 1;
	ShaderConfig.MaxAttributeSizeInBytes = 2 * sizeof(float);
	ShaderConfig.MaxPayloadSizeInBytes = 4 * sizeof(float);
}
void BearPipelineRayTracingDescription::Copy(const BearPipelineRayTracingDescription& Right)
{
	bear_copy(&PipelineConfig, &Right.PipelineConfig,1);
	bear_copy(&ShaderConfig, &Right.ShaderConfig, 1);
	bear_copy(&GlobalRootSignature, &Right.GlobalRootSignature, 1);
	HitGroups.copy(Right.HitGroups);
	Shaders.copy(Right.Shaders);
	//LocalRootSignatures.copy(Right.LocalRootSignatures);
}
void BearPipelineRayTracingDescription::Swap(BearPipelineRayTracingDescription& Right)
{
	bear_swap(PipelineConfig, Right.PipelineConfig);
	bear_swap(ShaderConfig, Right.ShaderConfig);
	bear_swap(GlobalRootSignature, Right.GlobalRootSignature);
	HitGroups.swap(Right.HitGroups);
	Shaders.swap(Right.Shaders);
	//LocalRootSignatures.swap(Right.LocalRootSignatures);
}
bool BearPipelineRayTracingDescription::operator==(const BearPipelineRayTracingDescription& Right) const
{
	if (PipelineConfig.MaxTraceRecursionDepth != Right.PipelineConfig.MaxTraceRecursionDepth) 
		return false;

	if (ShaderConfig.MaxAttributeSizeInBytes != Right.ShaderConfig.MaxAttributeSizeInBytes) 
		return false;
	if (ShaderConfig.MaxPayloadSizeInBytes != Right.ShaderConfig.MaxPayloadSizeInBytes) 
		return false;

	if (GlobalRootSignature != Right.GlobalRootSignature)return false;

	if (HitGroups.size() == Right.HitGroups.size())
	{
		for (bsize i = 0; i < HitGroups.size(); i++)
		{
			if (HitGroups[i].Type != Right.HitGroups[i].Type)
				return false;
			if (HitGroups[i].AnyHitShaderImport != Right.HitGroups[i].AnyHitShaderImport)
				return false;
			if (HitGroups[i].ClosestHitShaderImport != Right.HitGroups[i].ClosestHitShaderImport)
				return false;
			if (HitGroups[i].IntersectionShaderImport != Right.HitGroups[i].IntersectionShaderImport)
				return false;
			if (HitGroups[i].NameExport != Right.HitGroups[i].NameExport)
				return false;
		}
	}
	else
	{
		return false;
	}
	if (Shaders.size() == Right.Shaders.size())
	{
		for (bsize i = 0; i < Shaders.size(); i++)
		{
			if (Shaders[i].Shader != Right.Shaders[i].Shader)
				return false;

			if (Shaders[i].Exports.size() != Right.Shaders[i].Exports.size())
				return false;

			for (bsize a = 0; a < Shaders[i].Exports.size(); a++)
			{
				if (Shaders[i].Exports[a].NameExport != Right.Shaders[i].Exports[a].NameExport)
					return false;
				if (Shaders[i].Exports[a].NameFunction != Right.Shaders[i].Exports[a].NameFunction)
					return false;
				if (Shaders[i].Exports[a].Type != Right.Shaders[i].Exports[a].Type)
					return false;
			}

		}
	}
	else
	{
		return false;
	}

	/*if (LocalRootSignatures.size() == Right.LocalRootSignatures.size())
	{
		for (bsize i = 0; i < LocalRootSignatures.size(); i++)
		{
			if (LocalRootSignatures[i].RootSignature != Right.LocalRootSignatures[i].RootSignature)
				return false;
			if (LocalRootSignatures[i].Exports.size() == Right.LocalRootSignatures[i].Exports.size())
			{
				for (bsize a = 0; a < LocalRootSignatures[i].Exports.size(); a++)
				{
					for (bsize a = 0; a < Shaders[i].Exports.size(); a++)
					{
						if (LocalRootSignatures[i].Exports[a] != Right.LocalRootSignatures[i].Exports[a])
							return false;
					}
				}
			}
			else
			{
				return false;
			}
		}
	}
	else
	{
		return false;
	}*/
	return true;
}
bool BearPipelineRayTracingDescription::operator<(const BearPipelineRayTracingDescription& Right) const
{
	if (PipelineConfig.MaxTraceRecursionDepth != Right.PipelineConfig.MaxTraceRecursionDepth)
		return PipelineConfig.MaxTraceRecursionDepth < Right.PipelineConfig.MaxTraceRecursionDepth;

	if (ShaderConfig.MaxAttributeSizeInBytes != Right.ShaderConfig.MaxAttributeSizeInBytes)
		return ShaderConfig.MaxAttributeSizeInBytes < Right.ShaderConfig.MaxAttributeSizeInBytes;
	if (ShaderConfig.MaxPayloadSizeInBytes != Right.ShaderConfig.MaxPayloadSizeInBytes)
		return ShaderConfig.MaxPayloadSizeInBytes < Right.ShaderConfig.MaxPayloadSizeInBytes;

	if (GlobalRootSignature != Right.GlobalRootSignature)
		return GlobalRootSignature < Right.GlobalRootSignature;

	if (HitGroups.size() == Right.HitGroups.size())
	{
		for (bsize i = 0; i < HitGroups.size(); i++)
		{
			if (HitGroups[i].Type != Right.HitGroups[i].Type)
				return HitGroups[i].Type < Right.HitGroups[i].Type;

			if (HitGroups[i].AnyHitShaderImport != Right.HitGroups[i].AnyHitShaderImport)
				return HitGroups[i].AnyHitShaderImport < Right.HitGroups[i].AnyHitShaderImport;

			if (HitGroups[i].ClosestHitShaderImport != Right.HitGroups[i].ClosestHitShaderImport)
				return HitGroups[i].ClosestHitShaderImport < Right.HitGroups[i].ClosestHitShaderImport;

			if (HitGroups[i].IntersectionShaderImport != Right.HitGroups[i].IntersectionShaderImport)
				return HitGroups[i].IntersectionShaderImport < Right.HitGroups[i].IntersectionShaderImport;

			if (HitGroups[i].NameExport != Right.HitGroups[i].NameExport)
				return HitGroups[i].NameExport< Right.HitGroups[i].NameExport;
		}
	}
	else
	{
		return HitGroups.size() < Right.HitGroups.size();
	}
	if (Shaders.size() == Right.Shaders.size())
	{
		for (bsize i = 0; i < Shaders.size(); i++)
		{
			if (Shaders[i].Shader != Right.Shaders[i].Shader)
				return Shaders[i].Shader < Right.Shaders[i].Shader;
			if (Shaders[i].Exports.size() == Right.Shaders[i].Exports.size())
			{
				for (bsize a = 0; a < Shaders[i].Exports.size(); a++)
				{
					if (Shaders[i].Exports[a].NameExport != Right.Shaders[i].Exports[a].NameExport)
						return Shaders[i].Exports[a].NameExport < Right.Shaders[i].Exports[a].NameExport;
					if (Shaders[i].Exports[a].NameFunction != Right.Shaders[i].Exports[a].NameFunction)
						return Shaders[i].Exports[a].NameFunction < Right.Shaders[i].Exports[a].NameFunction;
					if (Shaders[i].Exports[a].Type != Right.Shaders[i].Exports[a].Type)
						return Shaders[i].Exports[a].Type < Right.Shaders[i].Exports[a].Type;
				}
			}
			else
			{
				return Shaders[i].Exports.size() < Right.Shaders[i].Exports.size();

			}
		}
	}
	else
	{
		return Shaders.size() < Right.Shaders.size();
	}

	/*if (LocalRootSignatures.size() == Right.LocalRootSignatures.size())
	{
		for (bsize i = 0; i < LocalRootSignatures.size(); i++)
		{
			if (LocalRootSignatures[i].RootSignature != Right.LocalRootSignatures[i].RootSignature)
				return LocalRootSignatures[i].RootSignature < Right.LocalRootSignatures[i].RootSignature;
			if (LocalRootSignatures[i].Exports.size() == Right.LocalRootSignatures[i].Exports.size())
			{
				for (bsize a = 0; a < LocalRootSignatures[i].Exports.size(); a++)
				{
					for (bsize a = 0; a < Shaders[i].Exports.size(); a++)
					{
						if (LocalRootSignatures[i].Exports[a] != Right.LocalRootSignatures[i].Exports[a])
							return LocalRootSignatures[i].Exports[a] < Right.LocalRootSignatures[i].Exports[a];
					}
				}
			}
			else
			{
				return LocalRootSignatures[i].Exports.size() < Right.LocalRootSignatures[i].Exports.size();
			}
		}
	}
	else
	{
		return LocalRootSignatures.size() < Right.LocalRootSignatures.size();
	}*/
	return false;
}

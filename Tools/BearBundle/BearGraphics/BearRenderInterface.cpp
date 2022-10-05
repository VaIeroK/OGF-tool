#include "BearGraphics.hpp"
#include "BearRHI/BearRHIFactory.h"
#include "BearRHI/BearRHIStats.h"

BEARGRAPHICS_API BearRHI::BearRHIFactory* GFactory=0;
BEARGRAPHICS_API BearRHI::BearRHIStats* GStats = 0;
static BearStringConteniar LName;

bool BearRenderInterface::Initialize(BearStringConteniar name)
{
	Destroy();

	if (!BearManagerDynamicLibraries::CheckProject(name))
	{
		return false;
	}
	LName = name;
	auto RHIInitialize = BearManagerDynamicLibraries::GetFunctionInProject<void(*)()>(name, TEXT("RHIInitialize"));
	RHIInitialize();
	if (!GFactory)
	{
		Destroy();
		return false;
	}

	BEAR_CHECK(GStats);
	BEAR_PRINTF(TEXT("BearGraphics build %s"), *BearLog::GetBuild(2016, 11, 27));
	return true;
}

BearFactoryPointer<BearRHI::BearRHIViewport> BearRenderInterface::CreateViewport(void* handle, bsize width, bsize height, bool fullscreen, const BearViewportDescription& description, bool vsync)
{
	if (GFactory)return GFactory->CreateViewport(handle, width, height, fullscreen,vsync,description);
	return BearFactoryPointer<BearRHI::BearRHIViewport>();
}

BearFactoryPointer<BearRHI::BearRHIContext> BearRenderInterface::CreateContext()
{
	if (GFactory)return GFactory->CreateContext();
	return BearFactoryPointer<BearRHI::BearRHIContext>();
}
BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreateVertexShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::Vertex);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}
BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreatePixelShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::Pixel);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}

BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreateComputeShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::Compute);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}

BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreateMeshShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::Mesh);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}

BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreateAmplificationShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::Amplification);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}

BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreateRayTracingShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::RayTracing);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}

BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreateHullShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::Hull);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}

BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreateDomainShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::Domain);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}

BearFactoryPointer<BearRHI::BearRHIShader> BearRenderInterface::CreateGeometryShader()
{
	if (GFactory)return GFactory->CreateShader(BearShaderType::Geometry);
	return BearFactoryPointer<BearRHI::BearRHIShader>();
}

BearFactoryPointer<BearRHI::BearRHIVertexBuffer> BearRenderInterface::CreateVertexBuffer()
{
	if (GFactory)return GFactory->CreateVertexBuffer();
	return BearFactoryPointer<BearRHI::BearRHIVertexBuffer>();
}

BearFactoryPointer<BearRHI::BearRHIIndexBuffer> BearRenderInterface::CreateIndexBuffer()
{
	if (GFactory)return GFactory->CreateIndexBuffer();
	return BearFactoryPointer<BearRHI::BearRHIIndexBuffer>();
}

BearFactoryPointer<BearRHI::BearRHIPipelineGraphics> BearRenderInterface::CreatePipelineGraphics(const BearPipelineGraphicsDescription& description)
{
	if (GFactory)return GFactory->CreatePipelineGraphics(description);
	return BearFactoryPointer<BearRHI::BearRHIPipelineGraphics>();
}

BearFactoryPointer<BearRHI::BearRHIPipelineMesh> BearRenderInterface::CreatePipelineMesh(const BearPipelineMeshDescription& description)
{
	if (GFactory)return GFactory->CreatePipelineMesh(description);
	return BearFactoryPointer<BearRHI::BearRHIPipelineMesh>();
}

BearFactoryPointer<BearRHI::BearRHIPipelineRayTracing> BearRenderInterface::CreatePipelineRayTracing(const BearPipelineRayTracingDescription& description)
{
	if (GFactory)return GFactory->CreatePipelineRayTracing(description);
	return BearFactoryPointer<BearRHI::BearRHIPipelineRayTracing>();
}

BearFactoryPointer<BearRHI::BearRHIRayTracingBottomLevel> BearRenderInterface::CreateRayTracingBottomLevel(const BearRayTracingBottomLevelDescription& description)
{
	if (GFactory)return GFactory->CreateRayTracingBottomLevel(description);
	return BearFactoryPointer<BearRHI::BearRHIRayTracingBottomLevel>();
}

BearFactoryPointer<BearRHI::BearRHIRayTracingTopLevel> BearRenderInterface::CreateRayTracingTopLevel(const BearRayTracingTopLevelDescription& description)
{
	if (GFactory)return GFactory->CreateRayTracingTopLevel(description);
	return BearFactoryPointer<BearRHI::BearRHIRayTracingTopLevel>();
}

BearFactoryPointer<BearRHI::BearRHIRayTracingShaderTable> BearRenderInterface::CreateRayTracingShaderTable(const BearRayTracingShaderTableDescription& description)
{
	if (GFactory)return GFactory->CreateRayTracingShaderTable(description);
	return BearFactoryPointer<BearRHI::BearRHIRayTracingShaderTable>();
}


BearFactoryPointer<BearRHI::BearRHIRootSignature> BearRenderInterface::CreateRootSignature(const BearRootSignatureDescription& description)
{
	if (GFactory)return GFactory->CreateRootSignature(description);
	return BearFactoryPointer<BearRHI::BearRHIRootSignature>();
}

BearFactoryPointer<BearRHI::BearRHIDescriptorHeap> BearRenderInterface::CreateDescriptorHeap(const BearDescriptorHeapDescription& description)
{
	if (GFactory)return GFactory->CreateDescriptorHeap(description);
	return BearFactoryPointer<BearRHI::BearRHIDescriptorHeap>();
}

BearFactoryPointer<BearRHI::BearRHIUniformBuffer> BearRenderInterface::CreateUniformBuffer(bsize stride, bsize count, bool dynamic)
{
	if (GFactory)return GFactory->CreateUniformBuffer(stride,count,dynamic);
	return BearFactoryPointer<BearRHI::BearRHIUniformBuffer>();
}

BearFactoryPointer<BearRHI::BearRHITexture2D> BearRenderInterface::CreateTexture2D(bsize width, bsize height, bsize mips, bsize count, BearTexturePixelFormat PixelFormat, BearTextureUsage type_usage , const void* data)
{
	if (GFactory)return GFactory->CreateTexture2D(width, height, mips, count, PixelFormat, type_usage,const_cast<void*>( data));
	return BearFactoryPointer<BearRHI::BearRHITexture2D>();
}

BearFactoryPointer<BearRHI::BearRHITextureCube> BearRenderInterface::CreateTextureCube(bsize width, bsize height, bsize mips, bsize count, BearTexturePixelFormat PixelFormat, BearTextureUsage type_usage, const void* data)
{
	if (GFactory)return GFactory->CreateTextureCube(width, height, mips, count, PixelFormat, type_usage, const_cast<void*>(data));
	return BearFactoryPointer<BearRHI::BearRHITextureCube>();
}

BearFactoryPointer<BearRHI::BearRHIStructuredBuffer> BearRenderInterface::CreateStructuredBuffer(bsize size, const void* data, bool uav)
{
	if (GFactory)return GFactory->CreateStructuredBuffer(size, const_cast<void*>(data), uav);
	return BearFactoryPointer<BearRHI::BearRHIStructuredBuffer>();
}

BearFactoryPointer<BearRHI::BearRHISampler> BearRenderInterface::CreateSampler(const BearSamplerDescription& description)
{
	if (GFactory)return GFactory->CreateSampler(description);
	return BearFactoryPointer<BearRHI::BearRHISampler>();
}

BearFactoryPointer<BearRHI::BearRHITexture2D> BearRenderInterface::CreateTexture2D(bsize width, bsize height, BearRenderTargetFormat rtf)
{
	if (GFactory)return GFactory->CreateTexture2D(width, height, rtf);
	return BearFactoryPointer<BearRHI::BearRHITexture2D>();
}

BearFactoryPointer<BearRHI::BearRHITexture2D> BearRenderInterface::CreateTexture2D(bsize width, bsize height, BearDepthStencilFormat dsf)
{
	if (GFactory)return GFactory->CreateTexture2D(width, height, dsf);
	return BearFactoryPointer<BearRHI::BearRHITexture2D>();
}

BearFactoryPointer<BearRHI::BearRHIRenderPass> BearRenderInterface::CreateRenderPass(const BearRenderPassDescription& description)
{
	if (GFactory)return GFactory->CreateRenderPass(description);
	return BearFactoryPointer<BearRHI::BearRHIRenderPass>();
}

BearFactoryPointer<BearRHI::BearRHIFrameBuffer> BearRenderInterface::CreateFrameBuffer(const BearFrameBufferDescription& description)
{
	if (GFactory)return GFactory->CreateFrameBuffer(description);
	return BearFactoryPointer<BearRHI::BearRHIFrameBuffer>();
}


bool BearRenderInterface::RTXSupport()
{
	if (GFactory)return GFactory->SupportRayTracing();
	return false;
}

bool BearRenderInterface::MeshShaderSupport()
{
	if (GFactory)return GFactory->SupportMeshShader();
	return false;
}

void BearRenderInterface::Destroy()
{
	BearRenderStats::Cheak();

	if (GFactory)bear_delete(GFactory);
	if (GStats) bear_delete(GStats);
	GStats = nullptr;
	GFactory = nullptr;
	if ((*LName)[0])
	{
		BearManagerDynamicLibraries::UnLoad(LName);
	}
}

bool BearRenderInterface::Empty()
{
	return GFactory==nullptr;
}

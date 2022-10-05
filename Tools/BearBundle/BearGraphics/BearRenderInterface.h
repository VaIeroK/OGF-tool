#pragma once
class BEARGRAPHICS_API BearRenderInterface
{
public:
	static bool Initialize(BearStringConteniar name);
	static BearFactoryPointer<BearRHI::BearRHIShader> CreateVertexShader();
	static BearFactoryPointer<BearRHI::BearRHIShader> CreateHullShader();
	static BearFactoryPointer<BearRHI::BearRHIShader> CreateDomainShader();
	static BearFactoryPointer<BearRHI::BearRHIShader> CreateGeometryShader();
	static BearFactoryPointer<BearRHI::BearRHIShader> CreatePixelShader();
	static BearFactoryPointer<BearRHI::BearRHIShader> CreateComputeShader();
	static BearFactoryPointer<BearRHI::BearRHIShader> CreateMeshShader();
	static BearFactoryPointer<BearRHI::BearRHIShader> CreateAmplificationShader();
	static BearFactoryPointer<BearRHI::BearRHIShader> CreateRayTracingShader();

	static BearFactoryPointer<BearRHI::BearRHIViewport> CreateViewport(void* handle,bsize width,bsize height,bool fullscreen,const BearViewportDescription& description, bool vsync=false);
	static BearFactoryPointer<BearRHI::BearRHIContext> CreateContext();
	static BearFactoryPointer < BearRHI::BearRHIVertexBuffer> CreateVertexBuffer();
	static BearFactoryPointer < BearRHI::BearRHIIndexBuffer> CreateIndexBuffer();

	static BearFactoryPointer < BearRHI::BearRHIPipelineGraphics> CreatePipelineGraphics(const BearPipelineGraphicsDescription& description);
	static BearFactoryPointer < BearRHI::BearRHIPipelineMesh> CreatePipelineMesh(const BearPipelineMeshDescription& description);

	static BearFactoryPointer < BearRHI::BearRHIRootSignature> CreateRootSignature(const BearRootSignatureDescription& description);
	static BearFactoryPointer < BearRHI::BearRHIDescriptorHeap> CreateDescriptorHeap(const BearDescriptorHeapDescription& description);
	static BearFactoryPointer < BearRHI::BearRHIUniformBuffer> CreateUniformBuffer(bsize stride, bsize count, bool dynamic);
	static BearFactoryPointer < BearRHI::BearRHITexture2D> CreateTexture2D(bsize width, bsize height, bsize mips, bsize count, BearTexturePixelFormat pixel_format, BearTextureUsage type_usage = BearTextureUsage::Static,const void* data=0);
	static BearFactoryPointer < BearRHI::BearRHITextureCube> CreateTextureCube(bsize width, bsize height, bsize mips, bsize count, BearTexturePixelFormat pixel_format, BearTextureUsage type_usage = BearTextureUsage::Static, const void* data = 0);
	static BearFactoryPointer<BearRHI::BearRHIStructuredBuffer> CreateStructuredBuffer(bsize size, const void* data, bool uav = false);
	static BearFactoryPointer < BearRHI::BearRHISampler> CreateSampler(const BearSamplerDescription& description);

	static BearFactoryPointer < BearRHI::BearRHITexture2D> CreateTexture2D(bsize width, bsize height, BearRenderTargetFormat rtf);
	static BearFactoryPointer < BearRHI::BearRHITexture2D> CreateTexture2D(bsize width, bsize height, BearDepthStencilFormat dsf);
	static BearFactoryPointer < BearRHI::BearRHIRenderPass> CreateRenderPass(const BearRenderPassDescription& description);
	static BearFactoryPointer < BearRHI::BearRHIFrameBuffer> CreateFrameBuffer(const BearFrameBufferDescription& description);

	static BearFactoryPointer < BearRHI::BearRHIPipelineRayTracing> CreatePipelineRayTracing(const BearPipelineRayTracingDescription& description);
	static BearFactoryPointer < BearRHI::BearRHIRayTracingBottomLevel> CreateRayTracingBottomLevel(const BearRayTracingBottomLevelDescription& description);
	static BearFactoryPointer < BearRHI::BearRHIRayTracingTopLevel> CreateRayTracingTopLevel(const BearRayTracingTopLevelDescription& description);
	static BearFactoryPointer < BearRHI::BearRHIRayTracingShaderTable> CreateRayTracingShaderTable(const BearRayTracingShaderTableDescription& description);

	static bool RTXSupport();
	static bool MeshShaderSupport();
	static void Destroy();
	static bool Empty();
};
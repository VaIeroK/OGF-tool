#ifndef  RENDER_BEGIN_CLASS_REGISTRATION1
#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...)
#endif 
#ifndef  RENDER_BEGIN_CLASS_REGISTRATION2
#define RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name,__VA_ARGS__)
#endif 

#ifndef  RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY
#define RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY(Name,...) RENDER_BEGIN_CLASS_REGISTRATION1(Name,__VA_ARGS__)
#endif 
#ifndef  RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY
#define RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Name,Parent,...) RENDER_BEGIN_CLASS_REGISTRATION2(Name,Parent,__VA_ARGS__)
#endif 

#ifndef  RENDER_CONSTRUCTOR_REGISTRATION
#define RENDER_CONSTRUCTOR_REGISTRATION(Name,...) 
#endif 

#ifndef  RENDER_METHOD_REGISTRATION
#define RENDER_METHOD_REGISTRATION(RetVal,Name,...)
#endif 
#ifndef  RENDER_END_CLASS_REGISTRATION
#define RENDER_END_CLASS_REGISTRATION()
#endif 

#ifndef RENDER_LEVEL_0_REGISTER
#define RENDER_LEVEL_0_REGISTER 1
#endif
#ifndef RENDER_LEVEL_1_REGISTER
#define RENDER_LEVEL_1_REGISTER 1
#endif
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if RENDER_LEVEL_0_REGISTER == 1

RENDER_BEGIN_CLASS_REGISTRATION2(Shader, Object, BearShaderType type)

#ifdef DEVELOPER_VERSION	
RENDER_METHOD_REGISTRATION(bool, LoadAsText, const bchar* text, const bchar* entry_point, const BearMap<BearStringConteniar, BearStringConteniar>& defines, BearString& out_error, BearIncluder* includer, const bchar* file_name, const bchar* out_pdb)
RENDER_METHOD_REGISTRATION(void*, GetPointer)
RENDER_METHOD_REGISTRATION(bsize, GetSize)
#endif
RENDER_METHOD_REGISTRATION(void, LoadAsBinary, void* data, bsize size)
RENDER_END_CLASS_REGISTRATION()


RENDER_BEGIN_CLASS_REGISTRATION2(VertexBuffer, Object)
RENDER_METHOD_REGISTRATION(void, Create, bsize stride, bsize count, bool dynamic, void* data = 0)
RENDER_METHOD_REGISTRATION(void*, Lock)
RENDER_METHOD_REGISTRATION(void, Unlock)
RENDER_METHOD_REGISTRATION(void, Clear)
RENDER_METHOD_REGISTRATION(bsize, GetCount)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(IndexBuffer, Object)
RENDER_METHOD_REGISTRATION(void, Create, bsize count, bool dynamic, void* data = 0)
RENDER_METHOD_REGISTRATION(uint32*, Lock)
RENDER_METHOD_REGISTRATION(void, Unlock)
RENDER_METHOD_REGISTRATION(void, Clear)
RENDER_METHOD_REGISTRATION(bsize, GetCount)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(UniformBuffer, Object, bsize stride, bsize count, bool dynamic)
RENDER_METHOD_REGISTRATION(void*, Lock)
RENDER_METHOD_REGISTRATION(void, Unlock)
RENDER_METHOD_REGISTRATION(bsize, GetCount)
RENDER_METHOD_REGISTRATION(bsize, GetStride)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(Pipeline, Object)
RENDER_METHOD_REGISTRATION(BearPipelineType, GetType)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(PipelineGraphics, Pipeline, const BearPipelineGraphicsDescription& description)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(PipelineMesh, Pipeline, const BearPipelineMeshDescription& description)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(PipelineRayTracing, Pipeline, const BearPipelineRayTracingDescription& description)
RENDER_END_CLASS_REGISTRATION()


RENDER_BEGIN_CLASS_REGISTRATION2(Sampler, Object, const BearSamplerDescription& description)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(RenderPass, Object, const BearRenderPassDescription& description)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(FrameBuffer, Object, const BearFrameBufferDescription& description)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(RootSignature, Object, const BearRootSignatureDescription& description)
RENDER_END_CLASS_REGISTRATION()


RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(ShaderResource, Object)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY(UnorderedAccess, ShaderResource)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(Texture2D, UnorderedAccess, bsize width, bsize height, bsize mips, bsize count, BearTexturePixelFormat pixel_format, BearTextureUsage type_usage = BearTextureUsage::Static, void* data = 0)
RENDER_CONSTRUCTOR_REGISTRATION(Texture2D, bsize Width, bsize Height, BearRenderTargetFormat format)
RENDER_CONSTRUCTOR_REGISTRATION(Texture2D, bsize Width, bsize Height, BearDepthStencilFormat format)
RENDER_METHOD_REGISTRATION(void*, Lock, bsize mip = 0, bsize depth = 0)
RENDER_METHOD_REGISTRATION(void, Unlock)
RENDER_METHOD_REGISTRATION(BearTextureType, GetType)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(TextureCube, ShaderResource, bsize width, bsize height, bsize mips, bsize count, BearTexturePixelFormat pixel_format, BearTextureUsage type_usage = BearTextureUsage::Static, void* data = 0)
RENDER_METHOD_REGISTRATION(void*, Lock, bsize mip = 0, bsize depth = 0)
RENDER_METHOD_REGISTRATION(void, Unlock)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(StructuredBuffer, UnorderedAccess,bsize size,void* data = 0, bool uav = false)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(RayTracingBottomLevel, Object, const BearRayTracingBottomLevelDescription& description)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION2(RayTracingTopLevel, ShaderResource, const BearRayTracingTopLevelDescription& description)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION1(RayTracingShaderTable, const BearRayTracingShaderTableDescription& description)
RENDER_END_CLASS_REGISTRATION()
#endif
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if RENDER_LEVEL_1_REGISTER == 1


RENDER_BEGIN_CLASS_REGISTRATION1(DescriptorHeap, const BearDescriptorHeapDescription& Description)
RENDER_METHOD_REGISTRATION(void, SetUniformBuffer, bsize slot, BearFactoryPointer<BearRHIUniformBuffer> uniform_buffer, bsize offset = 0)
RENDER_METHOD_REGISTRATION(void, SetShaderResource, bsize slot, BearFactoryPointer<BearRHIShaderResource> shader_resource, bsize offset = 0)
RENDER_METHOD_REGISTRATION(void, SetSampler, bsize slot, BearFactoryPointer<BearRHISampler> sampler)
RENDER_METHOD_REGISTRATION(void, SetUnorderedAccess, bsize slot, BearFactoryPointer<BearRHIUnorderedAccess> unordered_access, bsize offset = 0)
RENDER_END_CLASS_REGISTRATION()


RENDER_BEGIN_CLASS_REGISTRATION1(Viewport, void* handle, bsize width, bsize height, bool fullscreen, bool vsync, const BearViewportDescription& description)
RENDER_METHOD_REGISTRATION(void, SetVSync, bool sync)
RENDER_METHOD_REGISTRATION(BearRenderTargetFormat, GetFormat)
RENDER_METHOD_REGISTRATION(void, SetFullScreen, bool fullscreen)
RENDER_METHOD_REGISTRATION(void, Resize, bsize width, bsize height)
RENDER_END_CLASS_REGISTRATION()

RENDER_BEGIN_CLASS_REGISTRATION1(Context)
RENDER_METHOD_REGISTRATION(void, Reset)
RENDER_METHOD_REGISTRATION(void, ClearFrameBuffer)
RENDER_METHOD_REGISTRATION(void, SetStencilRef, uint32 ref)
RENDER_METHOD_REGISTRATION(void, SetDescriptorHeap, BearFactoryPointer<BearRHIDescriptorHeap> descriptor_heap)
RENDER_METHOD_REGISTRATION(void, SetPipeline, BearFactoryPointer<BearRHIPipeline> pipeline)
RENDER_METHOD_REGISTRATION(void, SetVertexBuffer, BearFactoryPointer<BearRHIVertexBuffer> buffer)
RENDER_METHOD_REGISTRATION(void, SetIndexBuffer, BearFactoryPointer<BearRHIIndexBuffer> buffer)
RENDER_METHOD_REGISTRATION(void, SetViewport, float x, float y, float width, float height, float min_depth = 0.f, float max_depth = 1.f)
RENDER_METHOD_REGISTRATION(void, SetScissor, bool enable, float x, float y, float x1, float y1)
RENDER_METHOD_REGISTRATION(void, Draw, bsize count, bsize offset = 0)
RENDER_METHOD_REGISTRATION(void, DispatchRays, bsize count_x, bsize count_y, bsize count_z, BearFactoryPointer<BearRHIRayTracingShaderTable> shader_table)
RENDER_METHOD_REGISTRATION(void, DispatchMesh,bsize count_x, bsize count_y, bsize count_z)
RENDER_METHOD_REGISTRATION(void, DrawIndex, bsize count, bsize offset_index = 0, bsize offset_vertex = 0)
RENDER_METHOD_REGISTRATION(void, SetViewportAsFrameBuffer, BearFactoryPointer<BearRHIViewport> viewport)
RENDER_METHOD_REGISTRATION(void, SetFrameBuffer, BearFactoryPointer<BearRHIFrameBuffer> frame_buffer)
RENDER_METHOD_REGISTRATION(void, Flush, BearFactoryPointer<BearRHIViewport> viewport, bool wait)
RENDER_METHOD_REGISTRATION(void, Flush, bool wait)
RENDER_METHOD_REGISTRATION(void, Wait)
RENDER_METHOD_REGISTRATION(void, BeginEvent, char const* name, BearColor color = BearColor::White)
RENDER_METHOD_REGISTRATION(void, EndEvent)
RENDER_METHOD_REGISTRATION(void, Copy, BearFactoryPointer<BearRHIIndexBuffer> dst, BearFactoryPointer<BearRHIIndexBuffer> src)
RENDER_METHOD_REGISTRATION(void, Copy, BearFactoryPointer<BearRHIVertexBuffer> dst, BearFactoryPointer<BearRHIVertexBuffer> src)
RENDER_METHOD_REGISTRATION(void, Copy, BearFactoryPointer<BearRHIUniformBuffer> dst, BearFactoryPointer<BearRHIUniformBuffer> src)
RENDER_METHOD_REGISTRATION(void, Copy, BearFactoryPointer<BearRHITexture2D> dst, BearFactoryPointer<BearRHITexture2D> src)
RENDER_METHOD_REGISTRATION(void, Lock, BearFactoryPointer<BearRHIViewport> viewport)
RENDER_METHOD_REGISTRATION(void, Lock, BearFactoryPointer<BearRHIFrameBuffer> frame_buffer)
RENDER_METHOD_REGISTRATION(void, Unlock, BearFactoryPointer<BearRHIViewport> viewport)
RENDER_METHOD_REGISTRATION(void, Unlock, BearFactoryPointer<BearRHIFrameBuffer> frame_buffer)
RENDER_METHOD_REGISTRATION(void, Lock, BearFactoryPointer<BearRHIUnorderedAccess> unordered_access)
RENDER_METHOD_REGISTRATION(void, Unlock, BearFactoryPointer<BearRHIUnorderedAccess> unordered_access)
RENDER_END_CLASS_REGISTRATION()
#endif
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////

#ifdef RENDER_LEVEL_0_REGISTER
#undef RENDER_LEVEL_0_REGISTER 
#endif
#ifdef RENDER_LEVEL_1_REGISTER
#undef RENDER_LEVEL_1_REGISTER 
#endif


#ifdef  RENDER_BEGIN_CLASS_REGISTRATION1
#undef RENDER_BEGIN_CLASS_REGISTRATION1
#endif 
#ifdef  RENDER_BEGIN_CLASS_REGISTRATION2
#undef RENDER_BEGIN_CLASS_REGISTRATION2
#endif 
#ifdef  RENDER_METHOD_REGISTRATION
#undef RENDER_METHOD_REGISTRATION
#endif 
#ifdef  RENDER_END_CLASS_REGISTRATION
#undef RENDER_END_CLASS_REGISTRATION
#endif 
#ifdef RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY
#undef RENDER_BEGIN_CLASS_REGISTRATION1_WITHOUT_FACTORY
#endif
#ifdef RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY
#undef RENDER_BEGIN_CLASS_REGISTRATION2_WITHOUT_FACTORY
#endif
#ifdef RENDER_CONSTRUCTOR_REGISTRATION
#undef RENDER_CONSTRUCTOR_REGISTRATION
#endif
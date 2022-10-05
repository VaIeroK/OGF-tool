#pragma once
#define RENDER_BEGIN_CLASS_REGISTRATION1(Name,...)    GOT_##Name,
enum BearGraphicsObjectType
{
	GOT_None=0,
#include "BearTemplate/BearGraphicsObjectsList.h"
};
enum class BearWindowEventType
{
	None=0,
	KeyDouble,
	KeyDown,
	KeyUp,
	Char,
	Resize,
	Deactive,
	Active,
	MouseMove,
	MouseEnter,
	MouseLevae,

};
enum  class BearShaderType
{
	Null=0,
	Vertex,
	Hull,
	Domain,
	Geometry,
	Pixel,
	Compute,
	/// <summary>
	/// Mesh sahding
	/// </summary>
	Mesh,
	Amplification,
	/// <summary>
	/// RayTacing
	/// </summary>
	RayTracing,
	ST_Count,
	ALL,
};
enum class BearPipelineType
{
	Null = 0,
	Graphics,
	Compute,
	Mesh,
	RayTracing,
};
enum class BearSRVDescriptorType
{
	Image,
	Buffer,
	AccelerationStructure
};
enum class BearUAVDescriptorType
{
	Image,
	Buffer,
};
enum class BearVertexFormat
{
	None = 0,
	R16G16_SINT,
	R16G16B16A16_SINT,
	R16G16_FLOAT,
	R16G16B16A16_FLOAT,
	R32_FLOAT,
	R32G32_FLOAT,
	R32G32B32_FLOAT,
	R32G32B32A32_FLOAT,
	R32_INT,

	R8,
	R8G8,
	R8G8B8A8,
};
enum class BearRenderTargetFormat
{
	None = 0,
	R8,
	R8G8,
	B8G8R8A8,
	R8G8B8A8,
	R32F,
	R32G32F,
	R32G32B32F,
	R32G32B32A32F,
};
enum class BearTopologyType
{
	PointList = 0,
	LintList,
	LineStrip,
	TriangleList,
	TriangleStrip,
};
enum class BearTexturePixelFormat
{
	None = 0,
	R8 = 1,
	R8G8,
	R8G8B8,
	R8G8B8A8,
	R32F,
	R32G32F,
	R32G32B32F,
	R32G32B32A32F,
	DXT_1,//not Alpha
	DXT_1_Alpha,//Alpha  1 bit
	DXT_3,//alpga 
	DXT_5,//Alpha
	BC1 = DXT_1,
	BC1a = DXT_1_Alpha,
	BC2 = DXT_3,
	BC3 = DXT_5,
	BC4,//R 32 bit 16 pxiel
	BC5,//RB 64 bit 16 pxiel
	BC6,//RGBA FP16 64 bit 16 pxiel
	BC7//RGBA 64 bit 16 pxiel
};
enum class BearSamplerAddressMode
{
	Wrap = 0,
	Mirror,
	Clamp,
	Border,
};
enum class BearSamplerFilter
{
	MinMagMipPoint,
	MinMagLinearMipPoint,
	MinMagMipLinear,
	Anisotropic,
	ComparisonMinMagMipPoint,
	ComparisonMinMagLinearMipPoint,
	ComparisonMinMagMipLinear,
	ComparisonAnisotropic,
};
enum class BearRasterizerCullMode
{
	None,
	Front,
	Back,
};
enum class BearRasterizerFillMode
{
	Wireframe,
	Solid
};
enum class BearRasterizerFrontFace
{
	CounterClockwise,
	FaceClockwise ,
};

enum class BearBlendFactor
{
	Zero,
	One,
	SrcColor,
	InvSrcColor,
	SrcAlpha,
	InvSrcAlpha,
	DestAlpha,
	InvDestAlpha,
	DestColor,
	InvDestColor,
	BlendFactor,
	InvBlendFactor,
};
enum class BearBlendOp
{
	Add,
	Subtract,
	RevSubtract,
	Min,
	Max

};
enum class BearColorWriteMask
{
	R = 1 << 0,
	G = 1 << 1,
	B = 1 << 2,
	A = 1 << 3,
	RGB = R | G | B,
	RGBA =R | G | B | A,
};
using BearColorWriteFlags = BearFlags<uint32>;
enum class BearCompareFunction
{
	Never,//=0
	Always,//=1
	Equal,//==
	NotEqual,//!=
	Less,//<
	Greater,//>
	LessEqual,//<=
	GreaterEqual,//>=
};
enum class BearStencilOp
{
	Keep,
	Zero,
	Replace,
	IncrSat,
	DecrSat,
	Invert,
	Incr,
	Decr
};
enum class BearTextureUsage
{
	Static,
	Dynamic,
	Stating,
	//uav
	Storage,
};
enum class BearDepthStencilFormat
{
	None = 0,
	Depth16 ,
	Depth32F,
	Depth24Stencil8,
	Depth32FStencil8,
};
enum class BearTextureType
{
	Default,
	RenderTarget,
	DepthStencil,
};

enum class BearHitGroupType
{
	Triangles,
	ProceduralPrimitive,
};
enum class BearAccelerationStructureBuildFlags 
{
	AllowUpdate = 1 << 0,
	AllowCompaction = 1 << 1,
	PreferFastTrace = 1 << 2,
	PreferFastBuild = 1 << 3,
	MinimizeMemory = 1 << 4,
};
enum class BearRaytracingGeometryFlags
{
	Opaque = 1 << 0,
	NoDuplicateAnyhitInvocation = 1 << 1,
};
enum class BearRaytracingGeometryType
{
	Triangles,
	ProceduralPrimitiveAABBS
};
enum  class BearRayTracingShaderType
{
	RayGeneration,
	Miss,
	Callable,
	Intersection,
	ClosestHit,
	AnyHit,
};
enum class BearWindowType
{
	Popup = 1,
	OnlyClosed = 4,
	WihtoutClosed = 2,
};
enum class BearResizeFilter
{
	Default,  // use same filter type that easy-to-use API chooses
	Box,  // A trapezoid w/1-pixel wide ramps, same result as box for integer scale ratios
	Triangle,  // On upsampling, produces same results as bilinear texture filtering
	Cubicbspline,  // The cubic b-spline (aka Mitchell-Netrevalli with B=1,C=0), gaussian-esque
	Catmullrom ,  // An interpolating cubic spline
	Mitchell,  // Mitchell-Netrevalli filter with B=1/3, C=1/3
};
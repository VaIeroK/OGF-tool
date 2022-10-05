#pragma once
class BEARGRAPHICS_API BearRayTracingBottomLevelDescription
{
public:
	BearRayTracingBottomLevelDescription(){}
	inline ~BearRayTracingBottomLevelDescription(){}
	BearRayTracingBottomLevelDescription(const BearRayTracingBottomLevelDescription& Right) { Copy(Right); }
	BearRayTracingBottomLevelDescription(BearRayTracingBottomLevelDescription&& Right) { Swap(Right); }
	inline void Copy(const BearRayTracingBottomLevelDescription& Right)
	{
		GeometryDescriptions.copy(Right.GeometryDescriptions);
		BuildFlags.copy(Right.BuildFlags);
	}
	inline void Swap(BearRayTracingBottomLevelDescription& Right)
	{
		GeometryDescriptions.swap(Right.GeometryDescriptions);
		BuildFlags.swap(Right.BuildFlags);
	}
	inline BearRayTracingBottomLevelDescription& operator=(const BearRayTracingBottomLevelDescription& Right) { Copy(Right); return*this; }
	inline BearRayTracingBottomLevelDescription& operator=(BearRayTracingBottomLevelDescription&& Right) { Swap(Right); return*this; }
	struct GeometryDescription
	{
		GeometryDescription() { Type = BearRaytracingGeometryType::Triangles; }
		BearRaytracingGeometryType Type;
		BearFlags<uint32>  Flags;
		struct TrianglesDescription
		{
			TrianglesDescription() { VertexCount = 0; VertexOffset = 0; VertexFormat = BearVertexFormat::R32G32B32_FLOAT; IndexCount = 0; IndexOffset = 0; }
			BearFactoryPointer<BearRHI::BearRHIVertexBuffer> VertexBuffer;
			bsize VertexCount;
			bsize VertexOffset;
			BearVertexFormat VertexFormat;

			BearFactoryPointer<BearRHI::BearRHIIndexBuffer> IndexBuffer;
			bsize IndexCount;
			bsize IndexOffset;

		} Triangles;
		struct AABBDescription
		{
			AABBDescription() { Stride = 0; Count = 0; Offset = 0; }
			BearFactoryPointer<BearRHI::BearRHIStructuredBuffer> Buffer;
			bsize Stride;
			bsize Count;
			bsize Offset;
		}AABB;
	};
	BearVector<GeometryDescription> GeometryDescriptions;
	BearFlags<uint32>  BuildFlags;
	
};
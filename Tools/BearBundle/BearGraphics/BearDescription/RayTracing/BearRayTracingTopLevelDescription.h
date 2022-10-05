#pragma once
class BEARGRAPHICS_API BearRayTracingTopLevelDescription
{
public:
	BearRayTracingTopLevelDescription() {}
	inline ~BearRayTracingTopLevelDescription() {}
	BearRayTracingTopLevelDescription(const BearRayTracingTopLevelDescription& Right) { Copy(Right); }
	BearRayTracingTopLevelDescription(BearRayTracingTopLevelDescription&& Right) { Swap(Right); }
	inline void Copy(const BearRayTracingTopLevelDescription& Right)
	{
		InstanceDescriptions.copy(Right.InstanceDescriptions);
		BuildFlags.copy(Right.BuildFlags);
	}
	inline void Swap(BearRayTracingTopLevelDescription& Right)
	{
		InstanceDescriptions.swap(Right.InstanceDescriptions);
		BuildFlags.swap(Right.BuildFlags);
	}
	inline BearRayTracingTopLevelDescription& operator=(const BearRayTracingTopLevelDescription& Right) { Copy(Right); return*this; }
	inline BearRayTracingTopLevelDescription& operator=(BearRayTracingTopLevelDescription&& Right) { Swap(Right); return*this; }
	struct InstanceDescription
	{
		InstanceDescription()
		{
			Transform3x4.BuildIdentity();
			InstanceMask = 1;
			InstanceID = 0;
			InstanceContributionToHitGroupIndex = 0;
			Flags = 0;
		}
		BearMatrix			Transform3x4;
		uint32				InstanceID : 24;
		uint8				InstanceMask;
		uint32				InstanceContributionToHitGroupIndex : 24;
		BearFlags<uint8>	Flags;
		BearFactoryPointer<BearRHI::BearRHIRayTracingBottomLevel> BottomLevel;
	};
	BearVector<InstanceDescription> InstanceDescriptions;
	BearFlags<uint32>  BuildFlags;

};
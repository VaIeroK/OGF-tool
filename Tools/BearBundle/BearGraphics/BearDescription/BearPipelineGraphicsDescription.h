#pragma once 

class BEARGRAPHICS_API BearPipelineGraphicsDescription
{
public:


	BearPipelineGraphicsDescription();
	BearPipelineGraphicsDescription(const BearPipelineGraphicsDescription&Right);
	BearPipelineGraphicsDescription(BearPipelineGraphicsDescription&&Right);
	inline ~BearPipelineGraphicsDescription() {}
		
	void Copy(const BearPipelineGraphicsDescription&Right);
	void Swap(BearPipelineGraphicsDescription&Right);
	inline BearPipelineGraphicsDescription&operator=(const BearPipelineGraphicsDescription&Right) {			Copy(Right); return*this;		}
	inline BearPipelineGraphicsDescription&operator=(BearPipelineGraphicsDescription&&Right) { Swap(Right); return*this; }
	bool operator== (const BearPipelineGraphicsDescription& Right)const;
	inline bool operator!= (const BearPipelineGraphicsDescription& Right)const { return !((*this) == Right); }
	bool operator<(const BearPipelineGraphicsDescription& Right)const;
	struct
	{
		BearInputLayoutElement Elements[16];

	} InputLayout;

	BearBlendStateDescription BlendState;
	BearDepthStencilStateDescription DepthStencilState;
	BearRasterizerStateDescription RasterizerState;
	MultisampleStateDescription MultisampleState;
	struct
	{
		BearFactoryPointer<BearRHI::BearRHIShader> Vertex;
		BearFactoryPointer<BearRHI::BearRHIShader> Hull;
		BearFactoryPointer<BearRHI::BearRHIShader> Domain;
		BearFactoryPointer<BearRHI::BearRHIShader> Geometry;
		BearFactoryPointer<BearRHI::BearRHIShader> Pixel;
	}Shaders;
	
	BearFactoryPointer<BearRHI::BearRHIRenderPass>  RenderPass;
	BearTopologyType TopologyType;
	BearFactoryPointer<BearRHI::BearRHIRootSignature> RootSignature;
};

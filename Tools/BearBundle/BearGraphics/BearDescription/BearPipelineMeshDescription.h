#pragma once 

class BEARGRAPHICS_API BearPipelineMeshDescription
{
public:


	BearPipelineMeshDescription();
	BearPipelineMeshDescription(const BearPipelineMeshDescription&Right);
	BearPipelineMeshDescription(BearPipelineMeshDescription&&Right);
	inline ~BearPipelineMeshDescription() {}
		
	void Copy(const BearPipelineMeshDescription&Right);
	void Swap(BearPipelineMeshDescription&Right);
	inline BearPipelineMeshDescription&operator=(const BearPipelineMeshDescription&Right) {			Copy(Right); return*this;		}
	inline BearPipelineMeshDescription&operator=(BearPipelineMeshDescription&&Right) { Swap(Right); return*this; }
	bool operator== (const BearPipelineMeshDescription& Right)const;
	inline bool operator!= (const BearPipelineMeshDescription& Right)const { return !((*this) == Right); }
	bool operator<(const BearPipelineMeshDescription& Right)const;

	BearBlendStateDescription BlendState;
	BearDepthStencilStateDescription DepthStencilState;
	BearRasterizerStateDescription RasterizerState;
	MultisampleStateDescription MultisampleState;

	struct
	{

		BearFactoryPointer<BearRHI::BearRHIShader> Amplification;
		BearFactoryPointer<BearRHI::BearRHIShader> Mesh;
		BearFactoryPointer<BearRHI::BearRHIShader> Pixel;
	}Shaders;
	
	BearFactoryPointer<BearRHI::BearRHIRenderPass>  RenderPass;
	BearFactoryPointer<BearRHI::BearRHIRootSignature> RootSignature;
};

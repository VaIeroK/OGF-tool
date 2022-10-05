#include "BearGraphics.hpp"
BearPipelineMeshDescription::BearPipelineMeshDescription()
{
}

BearPipelineMeshDescription::BearPipelineMeshDescription(const BearPipelineMeshDescription& Right)
{
	Copy(Right);
}

BearPipelineMeshDescription::BearPipelineMeshDescription(BearPipelineMeshDescription&& Right)
{
	Swap(Right);
}


void BearPipelineMeshDescription::Copy(const BearPipelineMeshDescription& Right)
{
	RenderPass = Right.RenderPass;

	Shaders.Mesh = Right.Shaders.Mesh;
	Shaders.Amplification = Right.Shaders.Amplification;
	Shaders.Pixel = Right.Shaders.Pixel;

	bear_copy(&BlendState, &Right.BlendState,1);
	bear_copy(&DepthStencilState, &Right.DepthStencilState, 1);
	bear_copy(&RasterizerState, &Right.RasterizerState,1);

}

void BearPipelineMeshDescription::Swap(BearPipelineMeshDescription& Right)
{
	RenderPass.swap(Right.RenderPass);


	Shaders.Mesh.swap(Right.Shaders.Mesh);
	Shaders.Amplification.swap(Right.Shaders.Amplification);
	Shaders.Pixel.swap(Right.Shaders.Pixel);

	bear_swap(BlendState, Right.BlendState);
	bear_swap(DepthStencilState, Right.DepthStencilState);
	bear_swap(RasterizerState, Right.RasterizerState);
}



bool BearPipelineMeshDescription::operator==(const BearPipelineMeshDescription& Right)const
{
	if (RenderPass != Right.RenderPass)
		return false;

	if (Shaders.Mesh != Right.Shaders.Mesh)
		return false;
	if (Shaders.Amplification != Right.Shaders.Amplification)
		return false;
	if (Shaders.Pixel != Right.Shaders.Pixel)
		return false;


	if (bear_compare(&BlendState, &Right.BlendState,1 ) != 0)
		return false;
	if (bear_compare(&DepthStencilState, &Right.DepthStencilState,1) != 0)
		return false;

	if (bear_compare(&DepthStencilState, &Right.DepthStencilState, 1) != 0)
		return false;
	
	return true;
}
bool BearPipelineMeshDescription::operator<(const BearPipelineMeshDescription& Right) const
{
	if (RenderPass != Right.RenderPass)
		return RenderPass < Right.RenderPass;


	if (Shaders.Mesh != Right.Shaders.Mesh)
		return Shaders.Mesh < Right.Shaders.Mesh;
	if (Shaders.Amplification != Right.Shaders.Amplification)
		return Shaders.Amplification < Right.Shaders.Amplification;
	if (Shaders.Pixel != Right.Shaders.Pixel)
		return Shaders.Pixel < Right.Shaders.Pixel;

	bint result = 0;
	result = bear_compare(&BlendState, &Right.BlendState, 1);
	if (result != 0)
		return result < 0;
	result = bear_compare(&DepthStencilState, &Right.DepthStencilState, 1);
	if (result != 0)
		return result<0;
	return bear_compare(&DepthStencilState, &Right.DepthStencilState, 1)<0;
}
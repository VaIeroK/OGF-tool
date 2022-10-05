#include "BearGraphics.hpp"
BearPipelineGraphicsDescription::BearPipelineGraphicsDescription()
{
	TopologyType = BearTopologyType::TriangleList;
}

BearPipelineGraphicsDescription::BearPipelineGraphicsDescription(const BearPipelineGraphicsDescription& Right)
{
	TopologyType = BearTopologyType::TriangleList;
	Copy(Right);
}

BearPipelineGraphicsDescription::BearPipelineGraphicsDescription(BearPipelineGraphicsDescription&& Right)
{
	TopologyType = BearTopologyType::TriangleList;
	Swap(Right);
}


void BearPipelineGraphicsDescription::Copy(const BearPipelineGraphicsDescription& Right)
{
	for (bsize i = 0; i < 16; i++)
		InputLayout.Elements[i] = Right.InputLayout.Elements[i];
	RenderPass = Right.RenderPass;

	Shaders.Vertex = Right.Shaders.Vertex;
	Shaders.Hull = Right.Shaders.Hull;
	Shaders.Domain = Right.Shaders.Domain;
	Shaders.Geometry = Right.Shaders.Geometry;
	Shaders.Pixel = Right.Shaders.Pixel;

	TopologyType = Right.TopologyType;

	bear_copy(&BlendState, &Right.BlendState,1);
	bear_copy(&DepthStencilState, &Right.DepthStencilState, 1);
	bear_copy(&RasterizerState, &Right.RasterizerState, 1);

}

void BearPipelineGraphicsDescription::Swap(BearPipelineGraphicsDescription& Right)
{
	for (bsize i = 0; i < 16; i++)
		bear_swap(InputLayout.Elements[i], Right.InputLayout.Elements[i]);
	RenderPass.swap(Right.RenderPass);


	Shaders.Vertex.swap(Right.Shaders.Vertex);
	Shaders.Hull.swap(Right.Shaders.Hull);
	Shaders.Domain.swap(Right.Shaders.Domain);
	Shaders.Geometry.swap(Right.Shaders.Geometry);
	Shaders.Pixel.swap(Right.Shaders.Pixel);

	bear_swap(TopologyType, Right.TopologyType);
	bear_swap(BlendState, Right.BlendState);
	bear_swap(DepthStencilState, Right.DepthStencilState);
	bear_swap(RasterizerState, Right.RasterizerState);
}



bool BearPipelineGraphicsDescription::operator==(const BearPipelineGraphicsDescription& Right)const
{
	if (RenderPass != Right.RenderPass)
		return false;

	if (Shaders.Vertex != Right.Shaders.Vertex)
		return false;
	if (Shaders.Hull != Right.Shaders.Hull)
		return false;
	if (Shaders.Domain != Right.Shaders.Domain)
		return false;
	if (Shaders.Geometry != Right.Shaders.Geometry)
		return false;
	if (Shaders.Pixel != Right.Shaders.Pixel)
		return false;


	if (TopologyType != Right.TopologyType)
		return false;
	for (bsize i = 0; i < 16; i++)
	{
		if (InputLayout.Elements[i].Type ==  BearVertexFormat::None && Right.InputLayout.Elements[i].Type ==  BearVertexFormat::None)
			break;
		if (InputLayout.Elements[i]!=Right.InputLayout.Elements[i])
			return false;
	}
	if (bear_compare(&BlendState, &Right.BlendState, 1 ) != 0)
		return false;
	if (bear_compare(&DepthStencilState, &Right.DepthStencilState, 1) != 0)
		return false;

	if (bear_compare(&DepthStencilState, &Right.DepthStencilState, 1) != 0)
		return false;
	
	return true;
}
bool BearPipelineGraphicsDescription::operator<(const BearPipelineGraphicsDescription& Right) const
{
	if (RenderPass != Right.RenderPass)
		return RenderPass < Right.RenderPass;


	if (Shaders.Vertex != Right.Shaders.Vertex)
		return Shaders.Vertex < Right.Shaders.Vertex;
	if (Shaders.Hull != Right.Shaders.Hull)
		return Shaders.Hull < Right.Shaders.Hull;
	if (Shaders.Domain != Right.Shaders.Domain)
		return Shaders.Domain < Right.Shaders.Domain;
	if (Shaders.Geometry != Right.Shaders.Geometry)
		return Shaders.Geometry < Right.Shaders.Geometry;
	if (Shaders.Pixel != Right.Shaders.Pixel)
		return Shaders.Pixel < Right.Shaders.Pixel;

	if (TopologyType != Right.TopologyType)
		return TopologyType < Right.TopologyType;
	bint result = 0;
	for (bsize i = 0; i < 16; i++)
	{
		if (InputLayout.Elements[i].Type == BearVertexFormat::None && Right.InputLayout.Elements[i].Type == BearVertexFormat::None)
			break;
		result = InputLayout.Elements[i] < Right.InputLayout.Elements[i];
		if (result != 0)
			return result < 0;
	}
	if (result != 0)
		return result<0;
	result = bear_compare(&BlendState, &Right.BlendState,1);
	if (result != 0)
		return result < 0;
	result = bear_compare(&DepthStencilState, &Right.DepthStencilState, 1);
	if (result != 0)
		return result<0;
	return bear_compare(&DepthStencilState, &Right.DepthStencilState, 1)<0;
}
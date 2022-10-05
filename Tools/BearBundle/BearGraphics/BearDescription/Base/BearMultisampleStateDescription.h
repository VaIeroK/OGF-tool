#pragma once
struct MultisampleStateDescription
{
	MultisampleStateDescription() { AlphaToCoverageEnable = false; MultisampleEnable = false; }
	bool MultisampleEnable;
	bool AlphaToCoverageEnable;
};
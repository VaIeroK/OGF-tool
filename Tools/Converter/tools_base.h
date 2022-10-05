#ifndef __GNUC__
#pragma once
#endif
#ifndef __TOOLS_BASE_H__
#define __TOOLS_BASE_H__

#include <string>
#include <vector>
#include "xr_cl_parser.h"

class tools_base {
public:
	virtual		~tools_base();

	enum source_format {
		TOOLS_AUTO	= 0,

		TOOLS_OGF	= 0x001,
		TOOLS_OMF	= 0x002,
		TOOLS_DM	= 0x004,
	};

	virtual void	process(const cl_parser& cl) = 0;
	void		check_path(const char* path, bool& status) const;

	std::vector<std::string> motions_vec;
};

inline tools_base::~tools_base() {}

#endif

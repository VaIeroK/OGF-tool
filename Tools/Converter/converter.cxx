#include "converter.h"
#include "tools_base.h"
#include "ogf_tools.h"
#include "dm_tools.h"
#include "xr_file_system.h"
#include "xr_log.h"
#include <time.h>

using namespace xray_re;

int exit_code = 0;

void usage()
{
	printf("X-Ray game asset converter (A.0.2.0 %s)\n", BUILD_DATE);
	printf("Vendor: ZENOBIAN mod team\n");
	printf("Usage: converter <common-options> <format-specific-options> <input-objects>\n\n");
	printf("Common options:\n");
	printf(" -ro		perform all the steps but do not write anything on disk\n");
	printf(" -fs <SPEC>	use this file system specification file (default is %s)\n", DEFAULT_FS_SPEC);
	printf(" -out <PATH>	output file name (OGF, OMF, DM, XRDEMO and DB pack)\n");
	printf(" -dir <PATH>	output folder name (OGF, OMF, DM, XRDEMO and DB unpack)\n\n");
	printf("OGF options:\n");
	printf(" -ogf		assume OGF format for input file(s)\n");
	printf(" -object	save as .object file (default)\n");
	printf(" -skls		save all motions as .skls file\n");
	printf(" -skl <NAME>	save motion <NAME> as .skl file\n");
	printf(" -bones		save all bones as .bones file\n\n");
	printf("OMF options:\n");
	printf(" -omf		assume OMF format for input file(s)\n");
	printf(" -skls		save as .skls file (default)\n");
	printf(" -skl <NAME>	save motion <NAME> as .skl file\n\n");
	printf("DM options:\n");
	printf(" -dm		assume DM format for input file(s)\n");
	printf(" -object	save as .object file (default)\n");
	printf(" -info		display shader, texture, min/max scale and flags\n\n");
//	printf("XRDEMO options:\n");
//	printf(" -xrdemo	assume XRDEMO format for input file(s)\n\n");
	printf("Level options:\n");
	printf(" -level		assume game level format\n");
	printf(" -mode <MODE>	assume output format according to <MODE>:\n");
	printf("	maya	make single object for importing into Maya/3ds Max (default)\n");
	printf("	le	split into terrain, merged edge-linked groups, MU models\n");
	printf("	le2	split into terrain, raw edge-linked groups, MU models\n");
	printf(" -terrain	make terrain object only from faces with terrain texture\n");
	printf(" -with_lods	produce LOD textures for MU models\n");
	printf(" -fancy <SPEC>	scale detail models and fix fences according to <SPEC>\n\n");
	printf("OGG/WAV options:\n");
	printf(" -ogg2wav	restore *.wav/*.thm in $sounds$ using *.ogg from $game_sounds$\n\n");
	printf("DDS/TGA options:\n");
	printf(" -dds2tga	restore *.tga in $textures$ using *.dds from $game_textures$\n");
	printf(" -with_solid	don't ignore non-transparent textures (for xrLC -gi)\n");
	printf(" -with_bump	don't ignore bump textures\n\n");
	printf("DB options:\n");
	printf(" -unpack	unpack game archive (expects list of file names)\n");
	printf(" -pack		pack game archive (expects folder name)\n");
//	printf(" -strip_thm	remove attached image in texture descriptors\n");
	printf(" -11xx		assume 1114/1154 archive format (unpack only)\n");
	printf(" -2215		assume 2215 archive format (unpack only)\n");
	printf(" -2945		assume 2945/2939 archive format (unpack only)\n");
	printf(" -2947ru	assume release version format\n");
	printf(" -2947ww	assume world-wide release version and 3120 format\n");
	printf(" -xdb		assume .xdb or .db archive format\n");
	printf(" -xdb_ud <FILE>	attach user data from <FILE>\n");
	printf(" -flt <MASK> 	extract only files, filtered by mask\n");
}

std::vector<std::string> vmotions;

int main(int argc, char* argv[])
{
	static const cl_parser::option_desc options[] = {
		{"-ogf",	cl_parser::OT_BOOL},
		{"-omf",	cl_parser::OT_BOOL},

		{"-skl",	cl_parser::OT_STRING},
		{"-skls",	cl_parser::OT_BOOL},
		{"-bones",	cl_parser::OT_BOOL},
		{"-object",	cl_parser::OT_BOOL},

		{"-fs",		cl_parser::OT_STRING},
		{"-out",	cl_parser::OT_STRING},
		{"-dir",	cl_parser::OT_STRING},
		{"-flt",	cl_parser::OT_STRING},
	};

	cl_parser cl;
	if (!cl.parse(argc, argv, xr_dim(options), options)) {
		usage();
		return 1;
	}

	unsigned format = tools_base::TOOLS_AUTO;
	if (cl.exist("-ogf"))
		format |= tools_base::TOOLS_OGF;
	if (cl.exist("-omf"))
		format |= tools_base::TOOLS_OMF;
	if (cl.exist("-dm"))
		format |= tools_base::TOOLS_DM;

	if (format == tools_base::TOOLS_AUTO) 
	{
		std::string extension;
		size_t num_params = cl.num_params();
		for (size_t i = 0; i != num_params; ++i) 
		{
			xr_file_system::split_path(cl.param(i), 0, 0, &extension);
			if (extension == ".ogf")
				format |= tools_base::TOOLS_OGF;
			else if (extension == ".omf")
				format |= tools_base::TOOLS_OMF;
			else if (extension == ".dm")
				format |= tools_base::TOOLS_DM;
		}
		if (format == tools_base::TOOLS_AUTO) {
			if (num_params)
				msg("can't auto-detect the source format");
			else
				usage();
			return 1;
		}
	}
	if ((format & (format - 1)) != 0) {
		msg("conflicting source formats");
		return 1;
	}

	const char* fs_spec = 0;
	unsigned fs_flags = 0;
	if (cl.get_bool("-ro")) {
		fs_flags |= xr_file_system::FSF_READ_ONLY;
		msg("working in read-only mode");
	}
	xr_file_system& fs = xr_file_system::instance();
	if (!fs.initialize(fs_spec, fs_flags)) {
		msg("can't initialize the file system");
		return 1;
	}
	xr_log::instance().init("converter", 0);

	tools_base* tools = 0;
	switch (format) {
	case tools_base::TOOLS_OGF:
		tools = new ogf_tools;
		tools->motions_vec = vmotions;
		break;
	case tools_base::TOOLS_OMF:
		tools = new omf_tools;
		tools->motions_vec = vmotions;
		break;
	case tools_base::TOOLS_DM:
		tools = new dm_tools;
		tools->motions_vec = vmotions;
		break;
	}
	if (tools == 0) {
		msg("locked");
		return 0;
	}

	clock_t start = clock();
	tools->process(cl);
	msg("total time: %.3lfs", (clock() - start) / 1.0 / CLOCKS_PER_SEC);

	delete tools;
	return exit_code;
}

void tools_base::check_path(const char* path, bool& status) const
{
	xr_file_system& fs = xr_file_system::instance();
	if (!fs.folder_exist(path, "")) {
		msg("path %s does not exist", path);
		status = false;
	}
}

extern "C"
{
	_declspec(dllexport) int CSharpStartAgent(char* path, char* out_path, int mode, int convert_to_mode, const char* motions)
	{
		static const unsigned int size = 6;
		char *args[size];

		switch (mode)
		{
			case 0: // OGF
			{
				args[0] = "-ogf";
				switch (convert_to_mode)
				{
					case 0:	// to object
					{
						args[4] = "-object";
					}break;
					case 1:	// to bones
					{
						args[4] = "-bones";
					}break;
					case 2:	// to skl
					{
						args[4] = "-skl";
					}break;
					case 3:	// to skls
					{
						args[4] = "-skls";
					}break;
				}
			}break;
			case 1: // OMF
			{
				args[0] = "-omf";
				switch (convert_to_mode)
				{
					case 0:	// to skl
					{
						args[4] = "-skl";
					}break;
					case 1:	// to skls
					{
						args[4] = "-skls";
					}break;
				}
			}break;
			case 2: // DM
			{
				args[0] = "-dm";
				args[4] = "-object";
			}break;
		}

		args[1] = path;
		args[2] = "-out";
		args[3] = out_path;
		args[5] = "temp";

		const int ret_size = (args[4] == "-skl" ? size : size - 1);

		std::string motions_list = motions;
		std::string temp_motion = "";

		for (size_t i = 0; i < motions_list.size(); i++)
		{
			if (motions_list[i] != ',')
				temp_motion += motions_list[i];
			else
			{
				vmotions.push_back(temp_motion);
				temp_motion = "";
			}
		}

		if (temp_motion != "")
			vmotions.push_back(temp_motion);

		return main(ret_size, args);
	}
}
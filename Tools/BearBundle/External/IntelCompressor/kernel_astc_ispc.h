#pragma once
#include "IntelCompressor/ispc_texcomp.h"


///////////////////////////////////////////////////////////////////////////
// Functions exported from ispc code
 extern void astc_encode_ispc(struct rgba_surface * src, float * block_scores, uint8_t * dst, uint64_t * list, struct astc_enc_context * list_context, struct astc_enc_settings * settings);
    extern void astc_rank_ispc(struct rgba_surface * src, int32_t xx, int32_t yy, uint32_t * mode_buffer, struct astc_enc_settings * settings);
    extern int32_t get_programCount();


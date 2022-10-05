#pragma once

struct rgba_surface {
    uint8_t * ptr;
    int width;
    int height;
    int stride;
};
struct bc6h_enc_settings {
    bool slow_mode;
    bool fast_mode;
    int refineIterations_1p;
    int refineIterations_2p;
    int fastSkipTreshold;
};
struct bc7_enc_settings {
    bool mode_selection[4];
    int refineIterations[8];
    bool skip_mode2;
    int fastSkipTreshold_mode1;
    int fastSkipTreshold_mode3;
    int fastSkipTreshold_mode7;
    int mode45_channel0;
    int refineIterations_channel;
    int channels;
};
struct etc_enc_settings {
    int fastSkipTreshold;
};

struct astc_enc_settings
{
    int block_width;
    int block_height;
    int channels;

    int fastSkipTreshold;
    int refineIterations;
};
struct astc_enc_context {
    int width;
    int height;
    int channels;
    bool dual_plane;
    int partitions;
    int color_endpoint_pairs;
};



struct astc_block {
    int width;
    int height;
    bool dual_plane;
    int weight_range;
    uint8_t weights[64];
    int color_component_selector;
    int partitions;
    int partition_id;
    int color_endpoint_pairs;
    int channels;
    int color_endpoint_modes[4];
    int endpoint_range;
    uint8_t endpoints[18];
};


struct astc_enc_state
{
    float pixels[256];
    float scaled_pixels[256];
    uint32_t data[4];

    // settings
     int block_width;
     int block_height;
     int pitch;

     int refineIterations;
};

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OGF_tool
{
    public enum OGF
    {
        OGF_HEADER = 1,
        OGF_TEXTURE = 2,
        OGF_S_BONE_NAMES = 13,  // * For skeletons only
        OGF_S_MOTIONS = 14, // * For skeletons only

        //build 729
        OGF2_TEXTURE = 2,
        OGF2_TEXTURE_L = 3,
        OGF2_BBOX = 6,
        OGF2_VERTICES = 7,
        OGF2_INDICES = 8,
        OGF2_VCONTAINER = 11,
        OGF2_BSPHERE = 12,

        OGF3_TEXTURE_L = 3,
        OGF3_CHILD_REFS = 5,
        OGF3_BBOX = 6,
        OGF3_VERTICES = 7,
        OGF3_INDICES = 8,
        OGF3_LODDATA = 9, // not sure
        OGF3_VCONTAINER = 10,
        OGF3_BSPHERE = 11,
        OGF3_CHILDREN_L = 12,
        OGF3_DPATCH = 15,  // guessed name
        OGF3_LODS = 16,   // guessed name
        OGF3_CHILDREN = 17,
        OGF3_S_SMPARAMS = 18,// build 1469
        OGF3_ICONTAINER = 19,// build 1865
        OGF3_S_SMPARAMS_NEW = 20,// build 1472 - 1865
        OGF3_LODDEF2 = 21,// build 1865
        OGF3_TREEDEF2 = 22,// build 1865
        OGF3_S_IKDATA_0 = 23,// build 1475 - 1580
        OGF3_S_USERDATA = 24,// build 1537 - 1865
        OGF3_S_IKDATA = 25,// build 1616 - 1829, 1844
        OGF3_S_MOTIONS_NEW = 26,// build 1616 - 1865
        OGF3_S_DESC = 27,// build 1844
        OGF3_S_IKDATA_2 = 28,// build 1842 - 1865
        OGF3_S_MOTION_REFS = 29,// build 1842

        OGF4_VERTICES = 3,
        OGF4_INDICES = 4,
        OGF4_P_MAP = 5,  //---------------------- unused
        OGF4_SWIDATA = 6,
        OGF4_VCONTAINER = 7, // not used ??
        OGF4_ICONTAINER = 8, // not used ??
        OGF4_CHILDREN = 9,   // * For skeletons only
        OGF4_CHILDREN_L = 10,    // Link to child visuals
        OGF4_LODDEF2 = 11,   // + 5 channel data
        OGF4_TREEDEF2 = 12,  // + 5 channel data
        OGF4_S_SMPARAMS = 15,    // * For skeletons only
        OGF4_S_IKDATA = 16,  // * For skeletons only
        OGF4_S_USERDATA = 17,    // * For skeletons only (Ini-file)
        OGF4_S_DESC = 18,    // * For skeletons only
        OGF4_S_MOTION_REFS = 19, // * For skeletons only
        OGF4_SWICONTAINER = 20,  // * SlidingWindowItem record container
        OGF4_GCONTAINER = 21,    // * both VB&IB
        OGF4_FASTPATH = 22,  // * extended/fast geometry
        OGF4_S_LODS = 23,    // * For skeletons only (Ini-file)
        OGF4_S_MOTION_REFS2 = 24,    // * changes in format
        OGF4_COLLISION_VERTICES = 25,
        OGF4_COLLISION_INDICES = 26, 
    };

    public enum MTL
    {
        GAMEMTL_CURRENT_VERSION = 0x0001,
        GAMEMTLS_CHUNK_VERSION = 0x1000,
        GAMEMTLS_CHUNK_AUTOINC = 0x1001,
        GAMEMTLS_CHUNK_MTLS = 0x1002,
        GAMEMTLS_CHUNK_MTLS_PAIR = 0x1003,
        GAMEMTL_CHUNK_MAIN = 0x1000,
        GAMEMTL_CHUNK_FLAGS = 0x1001,
        GAMEMTL_CHUNK_PHYSICS = 0x1002,
        GAMEMTL_CHUNK_FACTORS = 0x1003,
        GAMEMTL_CHUNK_FLOTATION = 0x1004,
        GAMEMTL_CHUNK_DESC = 0x1005,
        GAMEMTL_CHUNK_INJURIOUS = 0x1006,
        GAMEMTL_CHUNK_DENSITY = 0x1007,
        GAMEMTL_CHUNK_FACTORS_MP = 0x1008,
        GAMEMTLPAIR_CHUNK_PAIR = 0x1000,
        GAMEMTLPAIR_CHUNK_BREAKING = 0x1002,
        GAMEMTLPAIR_CHUNK_STEP = 0x1003,
        GAMEMTLPAIR_CHUNK_COLLIDE = 0x1005
    }

    public enum MotionKeyFlags
    {
        flTKeyPresent = (1<<0),
        flRKeyAbsent  = (1<<1),
        flTKey16IsBit = (1<<2),
        flTKeyFFT_Bit = (1<<3),
    };

    public enum ModelType
    {
        MT3_NORMAL = 0, // Fvisual
        MT3_HIERRARHY = 1,    // FHierrarhyVisual
        MT3_PROGRESSIVE = 2,  // FProgressiveFixedVisual
        MT3_SKELETON_GEOMDEF_PM = 3,  // CSkeletonX_PM
        MT3_SKELETON_ANIM = 4,    // CKinematics
        MT3_DETAIL_PATCH = 6, // FDetailPatch
        MT3_SKELETON_GEOMDEF_ST = 7,  // CSkeletonX_ST
        MT3_CACHED = 8,   // FCached
        MT3_PARTICLE = 9, // CPSVisual
        MT3_PROGRESSIVE2 = 10, // FProgressive
        MT3_LOD = 11,  // FLOD build 1472 - 1865
        MT3_TREE = 12, // FTreeVisual build 1472 - 1865
                        //				= 0xd,	// CParticleEffect 1844
                        //				= 0xe,	// CParticleGroup 1844
        MT3_SKELETON_RIGID = 15,   // CSkeletonRigid 1844

        MT4_NORMAL = 0, // Fvisual
        MT4_HIERRARHY = 1,    // FHierrarhyVisual
        MT4_PROGRESSIVE = 2,  // FProgressive
        MT4_SKELETON_ANIM = 3,    // CKinematicsAnimated
        MT4_SKELETON_GEOMDEF_PM = 4,  // CSkeletonX_PM
        MT4_SKELETON_GEOMDEF_ST = 5,  // CSkeletonX_ST
        MT4_LOD = 6,  // FLOD
        MT4_TREE_ST = 7,  // FTreeVisual_ST
        MT4_PARTICLE_EFFECT = 8,  // PS::CParticleEffect
        MT4_PARTICLE_GROUP = 9,   // PS::CParticleGroup
        MT4_SKELETON_RIGID = 10,   // CKinematics
        MT4_TREE_PM = 11,  // FTreeVisual_PM

        MT4_OMF = 64, // fake model type to distinguish .omf
    };

    public class SSkelVert
    {
        public float[] uv;
        public float[] offs;
        public float[] norm;
        public float[] tang;
        public float[] binorm;

        public uint[] bones_id;
        public float[] bones_infl;

        public float[] local_offset;

        public SSkelVert()
        {
            uv = new float[2];
            offs = new float[3];
            norm = new float[3];
            tang = new float[3];
            binorm = new float[3];
            bones_id = new uint[4];
            bones_infl = new float[4];
            local_offset = new float[3];
        }

        public float[] Offset()
        {
            return FVec.Add(offs, local_offset);
        }
    };

    public class SSkelFace
    {
        public ushort[] v;
        public SSkelFace()
        {
            v = new ushort[3];
        }
    };

    public class VIPM_SWR
    {
        public uint offset;
        public ushort num_tris;
        public ushort num_verts;

        public VIPM_SWR()
        {
            offset = 0;
            num_tris = 0;
            num_verts = 0;
        }
    };

    public class OGF_Children
    {
        public bool IsDM;
        public uint BrokenType;
        public bool IsCopModel;

        public OGF_Header Header;
        public Description description;
        public List<OGF_Child> childs;
        public BoneData bonedata;
        public IK_Data ikdata;
        public UserData userdata;
        public Lod lod;
        public MotionRefs motion_refs;
        public OMF motions;

        public uint chunk_size;
        public long pos;

        public OGF_Children()
        {
            pos = 0;
            chunk_size = 0;
            BrokenType = 0;
            motions = new OMF();
            IsDM = false;
            description = null;
            childs = new List<OGF_Child>();
            bonedata = null;
            ikdata = null;
            userdata = null;
            lod = null;
            motion_refs = null;
            IsCopModel = false;
            Header = new OGF_Header();
        }

        public bool IsProgressive()
        {
            foreach (OGF_Child child in this.childs)
            {
                if (child.SWI.Count > 0) return true;
            }

            return false;
        }

        public bool IsSkeleton()
        {
            if (Header.format_version == 4)
                return Header.type == (byte)ModelType.MT4_SKELETON_ANIM || Header.type == (byte)ModelType.MT4_SKELETON_RIGID;
            else
                return Header.type == (byte)ModelType.MT3_SKELETON_ANIM || Header.type == (byte)ModelType.MT3_SKELETON_RIGID;
        }

        public bool IsAnimated()
        {
            if (Header.format_version == 4)
                return Header.type == (byte)ModelType.MT4_SKELETON_ANIM;
            else
                return Header.type == (byte)ModelType.MT3_SKELETON_ANIM;
        }

        public bool IsStatic()
        {
            if (Header.format_version == 4)
                return Header.type == (byte)ModelType.MT4_HIERRARHY;
            else
                return Header.type == (byte)ModelType.MT3_HIERRARHY;
        }

        public bool IsStaticSingle()
        {
            if (Header.format_version == 4)
                return Header.type == (byte)ModelType.MT4_NORMAL || Header.type == (byte)ModelType.MT4_PROGRESSIVE;
            else
                return Header.type == (byte)ModelType.MT3_NORMAL || Header.type == (byte)ModelType.MT3_PROGRESSIVE || Header.type == (byte)ModelType.MT3_PROGRESSIVE2;
        }

        public byte Skeleton()
        {
            if (Header.format_version == 4)
                return (byte)ModelType.MT4_SKELETON_RIGID;
            else
                return (byte)ModelType.MT3_SKELETON_RIGID;
        }


        public byte Animated()
        {
            if (Header.format_version == 4)
                return (byte)ModelType.MT4_SKELETON_ANIM;
            else
                return (byte)ModelType.MT3_SKELETON_ANIM;
        }

        public byte Static()
        {
            if (childs.Count == 1) return StaticSingle();

            if (Header.format_version == 4)
                return (byte)ModelType.MT4_HIERRARHY;
            else
                return (byte)ModelType.MT3_HIERRARHY;
        }

        private byte StaticSingle()
        {
            if (childs.Count > 1) return Static();

            if (Header.format_version == 4)
                return (childs[0].SWI.Count > 0) ? (byte)ModelType.MT4_PROGRESSIVE : (byte)ModelType.MT4_NORMAL;
            else
                return (childs[0].SWI.Count > 0) ? (byte)ModelType.MT3_PROGRESSIVE : (byte)ModelType.MT3_NORMAL;
        }
    }
}

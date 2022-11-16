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
        public float[] local_rotation;

        public float[] center;
        public bool rotation_local;

        public SSkelVert()
        {
            uv = new float[2] { 0.0f, 0.0f };
            offs = new float[3] { 0.0f, 0.0f, 0.0f };
            norm = new float[3] { 0.0f, 0.0f, 0.0f };
            tang = new float[3] { 0.0f, 0.0f, 0.0f };
            binorm = new float[3] { 0.0f, 0.0f, 0.0f };
            bones_id = new uint[4] { 0, 0, 0, 0 };
            bones_infl = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
            local_offset = new float[3] { 0.0f, 0.0f, 0.0f };
            local_rotation = new float[3] { 0.0f, 0.0f, 0.0f };
            center = new float[3] { 0.0f, 0.0f, 0.0f };
            rotation_local = false;
        }

        public float[] OffsetLocal()
        {
            return FVec.Add(offs, local_offset);
        }

        public float[] Offset()
        {
            return FVec.RotateXYZ(OffsetLocal(), local_rotation[0], local_rotation[1], local_rotation[2], rotation_local ? center : new float[3]);
        }

        public float[] Norm()
        {
            return FVec.RotateXYZ(norm, local_rotation[0], local_rotation[1], local_rotation[2]);
        }

        public float[] Tang()
        {
            return FVec.RotateXYZ(tang, local_rotation[0], local_rotation[1], local_rotation[2]);
        }

        public float[] Binorm()
        {
            return FVec.RotateXYZ(binorm, local_rotation[0], local_rotation[1], local_rotation[2]);
        }

        public static void GenerateNormals(ref List<SSkelVert> Vertices, List<SSkelFace> Faces, bool generate_normal = true)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (generate_normal)
                    Vertices[i].norm = new float[3] { 0.0f, 0.0f, 0.0f };
                Vertices[i].tang = new float[3] { 0.0f, 0.0f, 0.0f };
                Vertices[i].binorm = new float[3] { 0.0f, 0.0f, 0.0f };
            }

            for (int i = 0; i < Faces.Count; i++)
            {
                int ia = Faces[i].v[0];
                int ib = Faces[i].v[1];
                int ic = Faces[i].v[2];

                float[] dv1 = FVec.Sub(Vertices[ia].OffsetLocal(), Vertices[ib].OffsetLocal());
                float[] dv2 = FVec.Sub(Vertices[ic].OffsetLocal(), Vertices[ib].OffsetLocal());
                float[] duv1 = FVec2.Sub(Vertices[ia].uv, Vertices[ib].uv);
                float[] duv2 = FVec2.Sub(Vertices[ic].uv, Vertices[ib].uv);

                float r = 1.0f / (duv1[0] * duv2[1] - duv1[1] * duv2[0]);
                float[] tangent = FVec.Mul(FVec.Sub(FVec.Mul(dv1, duv2[1]), FVec.Mul(dv2, duv1[1])), r);
                float[] binormal = FVec.Mul(FVec.Sub(FVec.Mul(dv2, duv1[0]), FVec.Mul(dv1, duv2[0])), r);

                if (generate_normal)
                {
                    float[] normal = FVec.CrossProduct(dv1, dv2);
                    Vertices[ia].norm = FVec.Add(Vertices[ia].norm, normal);
                    Vertices[ib].norm = FVec.Add(Vertices[ib].norm, normal);
                    Vertices[ic].norm = FVec.Add(Vertices[ic].norm, normal);
                }

                Vertices[ia].tang = FVec.Add(Vertices[ia].tang, tangent);
                Vertices[ib].tang = FVec.Add(Vertices[ib].tang, tangent);
                Vertices[ic].tang = FVec.Add(Vertices[ic].tang, tangent);

                Vertices[ia].binorm = FVec.Add(Vertices[ia].binorm, binormal);
                Vertices[ib].binorm = FVec.Add(Vertices[ib].binorm, binormal);
                Vertices[ic].binorm = FVec.Add(Vertices[ic].binorm, binormal);
            }

            for (int i = 0; i < Vertices.Count; i++)
            {
                if (generate_normal)
                {
                    Vertices[i].norm = FVec.Normalize(Vertices[i].norm);
                    Vertices[i].norm = FVec.Mul(Vertices[i].norm, -1.0f);
                }
                Vertices[i].tang = FVec.Normalize(Vertices[i].tang);
                Vertices[i].binorm = FVec.Normalize(Vertices[i].binorm);

                if (FVec.IsNan(Vertices[i].tang))
                    Vertices[i].tang = new float[3] { 0.0f, 0.0f, 0.0f };

                if (FVec.IsNan(Vertices[i].binorm))
                    Vertices[i].binorm = new float[3] { 0.0f, 0.0f, 0.0f };
            }
        }
    };

    public class SSkelFace
    {
        public ushort[] v;
        public SSkelFace()
        {
            v = new ushort[3] { 0, 0, 0 };
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

    public class OGF_Model
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

        public OGF_Model()
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
            foreach (var child in childs)
            {
                if (child.Header.IsProgressive())
                    return true;
            }
            return false;
        }

        public void RemoveProgressive(float lod)
        {
            for (int idx = 0; idx < childs.Count; idx++)
            {
                if (Header.IsSkeleton())
                    childs[idx].Header.GeomdefST();
                else
                    childs[idx].Header.Normal();
                childs[idx].Faces = childs[idx].Faces_SWI(lod);
                childs[idx].SWI.Clear();
            }
        }
    }
}

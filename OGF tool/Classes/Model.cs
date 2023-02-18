using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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
        public float[] local_offset2;
        public float[] local_rotation2;

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
            local_offset2 = new float[3] { 0.0f, 0.0f, 0.0f };
            local_rotation2 = new float[3] { 0.0f, 0.0f, 0.0f };
            rotation_local = false;
        }

        public float[] Offset()
        {
            float[] offset = FVec.Add(offs, local_offset);
            offset = FVec.Add(offset, local_offset2);
            offset = FVec.RotateXYZ(offset, local_rotation2);
            return FVec.RotateXYZ(offset, local_rotation, rotation_local ? center : new float[3]);
        }

        public float[] Offset2()
        {
            float[] offset = FVec.Add(offs, local_offset);
            offset = FVec.RotateXYZ(offset, local_rotation2);
            return FVec.RotateXYZ(offset, local_rotation, rotation_local ? center : new float[3]);
        }

        public float[] Norm()
        {
            float[] vec = FVec.RotateXYZ(norm, local_rotation2);
            return FVec.RotateXYZ(vec, local_rotation);
        }

        public float[] Tang()
        {
            float[] vec = FVec.RotateXYZ(tang, local_rotation2);
            return FVec.RotateXYZ(vec, local_rotation);
        }

        public float[] Binorm()
        {
            float[] vec = FVec.RotateXYZ(binorm, local_rotation2);
            return FVec.RotateXYZ(vec, local_rotation);
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

                float[] dv1 = FVec.Sub(Vertices[ia].offs, Vertices[ib].offs);
                float[] dv2 = FVec.Sub(Vertices[ic].offs, Vertices[ib].offs);
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

    public class XRay_Model
    {
        public enum ModelFormat
        {
            eUnknown = 0,
            eOGF = 1,
            eDM = 2,
            eDetail = 4,
            eObj = 8
        }

        [DllImport("Converter.dll")]
        private static extern void CalcBones([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 1)] ref BoneRenderTransform[] bones, int length, string child_list);

        [DllImport("Converter.dll")]
        private static extern void FixBonesBind([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 1)] ref BoneRenderTransform[] bones, int length, string child_list);

        [DllImport("Converter.dll")]
        private static extern void FixVertexOffset([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 1)] ref BoneRenderTransform[] bones, int length, string child_list, int bone_0, float x, float y, float z);

        [DllImport("Converter.dll")]
        private static extern int StartConvert(string path, string out_path, int mode, int convert_to_mode, string motion_list);

        private int RunConverter(string path, string out_path, int mode, int convert_to_mode)
        {
            string dll_path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\')) + "\\converter.dll";
            if (File.Exists(dll_path))
                return StartConvert(path, out_path, mode, convert_to_mode, "");
            else
            {
                MessageBox.Show("Can't find Converter.dll", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        delegate void WriteObj(List<SSkelVert> Verts, List<SSkelFace> Faces, string texture);
        delegate void AddChild();

        public int Format;
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

        public float[] local_offset;
        public float[] local_rotation;

        public byte[] source_data;
        public bool Opened;
        public string FileName;

        StreamWriter ObjWriter = null; // for closing

        public XRay_Model()
        {
            Invalidate();
            Opened = false;
        }

        public void Copy(XRay_Model model)
        {
            pos = model.pos;
            chunk_size = model.chunk_size;
            BrokenType = model.BrokenType;
            motions = model.motions;
            description = model.description;
            childs = model.childs;
            bonedata = model.bonedata;
            ikdata = model.ikdata;
            userdata = model.userdata;
            lod = model.lod;
            motion_refs = model.motion_refs;
            IsCopModel = model.IsCopModel;
            Header = model.Header;
            source_data = model.source_data;

            local_offset = model.local_offset;
            local_rotation = model.local_rotation;
            Format = model.Format;
        }

        public void Invalidate()
        {
            pos = 0;
            chunk_size = 0;
            BrokenType = 0;
            motions = new OMF();
            description = null;
            childs = new List<OGF_Child>();
            bonedata = null;
            ikdata = null;
            userdata = null;
            lod = null;
            motion_refs = null;
            IsCopModel = false;
            Header = new OGF_Header();
            source_data = null;

            local_offset = new float[3];
            local_rotation = new float[3];
            Format = (int)ModelFormat.eUnknown;
        }

        public void Destroy()
        {
            if (ObjWriter != null)
            {
                ObjWriter.Close();
                ObjWriter.Dispose();
                ObjWriter = null;
            }
        }

        public bool IsProgressive()
        {
            foreach (var child in childs)
            {
                if (child.Header != null && child.Header.IsProgressive())
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

        public void CalcBonesTransform()
        {
            if (bonedata != null && ikdata != null)
            {
                string child_list;
                BoneRenderTransform[] transforms = BoneRenderTransform.Setup(this, out child_list);

                CalcBones(ref transforms, transforms.Length, child_list);

                for (int i = 0; i < bonedata.bones.Count; i++)
                {
                    ikdata.bones[i].render_transform = transforms[i].OutPos();
                }
            }
        }

        public void FixOldBonesBind()
        {
            if (bonedata != null && ikdata != null && ikdata.chunk_version == 2)
            {
                string child_list;
                byte old_ver = ikdata.chunk_version;
                ikdata.chunk_version = 0;
                BoneRenderTransform[] transforms = BoneRenderTransform.Setup(this, out child_list);
                ikdata.chunk_version = old_ver;

                FixBonesBind(ref transforms, transforms.Length, child_list);

                for (int i = 0; i < bonedata.bones.Count; i++)
                {
                    ikdata.bones[i].fixed_position = transforms[i].OutPos();
                    ikdata.bones[i].fixed_rotation = transforms[i].OutRot();
                }
            }
        }

        public float[] FixOldVertexOffset(SSkelVert vert)
        {
            string child_list;
            BoneRenderTransform[] transforms = BoneRenderTransform.Setup(this, out child_list);
            FixVertexOffset(ref transforms, transforms.Length, child_list, (int)vert.bones_id[0], vert.Offset()[0], vert.Offset()[1], vert.Offset()[2]);

            return new float[3] { transforms[0].OutPosX, transforms[0].OutPosY, transforms[0].OutPosZ };
        }

        public bool Is(ModelFormat fmt)
        {
            return BitMask.IsSet(Format, (int)fmt);
        }

        public bool OpenFile(string filename, bool silent = false)
        {
            if (OpenFileInternal(filename, silent))
            { 
                Opened = true;
                FileName = filename;
                return true;
            }

            return false;
        }

        private bool LoadOGF(string filename, bool silent = false)
        {
            var xr_loader = new XRayLoader();
            XRay_Model model = new XRay_Model();

            model.source_data = File.ReadAllBytes(filename);

            using (var r = new BinaryReader(new MemoryStream(model.source_data)))
            {
                xr_loader.SetStream(r.BaseStream);

                if (!xr_loader.find_chunk((int)OGF.OGF_HEADER, false, true))
                {
                    if (!silent)
                        MessageBox.Show("Unsupported OGF format! Can't find header chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    model.Header.Load(xr_loader);

                    if (model.Header.format_version < 3)
                    {
                        if (!silent)
                            MessageBox.Show($"Unsupported OGF version: {model.Header.format_version}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                int DescChunk = (model.Header.format_version == 4 ? (int)OGF.OGF4_S_DESC : (int)OGF.OGF3_S_DESC);
                uint DescriptionSize = xr_loader.find_chunkSize(DescChunk, false, true);
                if (DescriptionSize > 0)
                {
                    model.description = new Description();
                    model.BrokenType = model.description.Load(xr_loader, DescriptionSize);
                }

                int ChildChunk = (model.Header.format_version == 4 ? (int)OGF.OGF4_CHILDREN : (int)OGF.OGF3_CHILDREN);
                bool bFindChunk = xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(ChildChunk, false, true));

                model.pos = xr_loader.chunk_pos;

                int id = 0;

                // Childs
                if (bFindChunk)
                {
                    while (true)
                    {
                        if (!xr_loader.find_chunk(id)) break;

                        Stream temp = xr_loader.reader.BaseStream;

                        if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(id, false, true))) break;

                        OGF_Child Child = new OGF_Child();
                        if (!Child.Load(xr_loader))
                            break;

                        model.childs.Add(Child);

                        id++;
                        xr_loader.SetStream(temp);
                    }

                    xr_loader.SetStream(r.BaseStream);
                }
                else
                {
                    OGF_Child Child = new OGF_Child();
                    if (Child.Load(xr_loader))
                        model.childs.Add(Child);
                }

                if (model.childs.Count == 0)
                {
                    if (!silent)
                        MessageBox.Show("Unsupported OGF format! Can't find children chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (model.Header.IsSkeleton())
                {
                    // Bones
                    if (!xr_loader.find_chunk((int)OGF.OGF_S_BONE_NAMES, false, true))
                    {
                        if (!silent)
                            MessageBox.Show("Unsupported OGF format! Can't find bones chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        if (xr_loader.chunk_pos < model.pos)
                            model.BrokenType = 2;

                        model.bonedata = new BoneData();
                        model.bonedata.Load(xr_loader);
                    }

                    // Ik Data
                    byte IKDataVers = 0;

                    int IKDataChunkRelease = (model.Header.format_version == 4 ? (int)OGF.OGF4_S_IKDATA : (int)OGF.OGF3_S_IKDATA_2);
                    bool IKDataChunkFind = xr_loader.find_chunk(IKDataChunkRelease, false, true);

                    if (IKDataChunkFind) // Load Release chunk
                        IKDataVers = 4;
                    else
                    {
                        IKDataChunkFind = model.Header.format_version == 3 && xr_loader.find_chunk((int)OGF.OGF3_S_IKDATA, false, true);

                        if (IKDataChunkFind) // Load Pre Release chunk
                            IKDataVers = 3;
                        else
                        {
                            IKDataChunkFind = model.Header.format_version == 3 && xr_loader.find_chunk((int)OGF.OGF3_S_IKDATA_0, false, true);

                            if (IKDataChunkFind) // Load Builds chunk
                                IKDataVers = 2;
                        }
                    }

                    if (IKDataVers != 0)
                    {
                        model.ikdata = new IK_Data();
                        model.ikdata.Load(xr_loader, model.bonedata.bones.Count, IKDataVers);

                        model.FixOldBonesBind();
                        model.CalcBonesTransform();
                    }
                    else if (model.Header.format_version == 4) // Chunk not find, exit if Release OGF
                    {
                        if (!silent)
                            MessageBox.Show("Unsupported OGF format! Can't find ik data chunk!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Userdata
                    int UserDataChunk = (model.Header.format_version == 4 ? (int)OGF.OGF4_S_USERDATA : (int)OGF.OGF3_S_USERDATA);
                    uint UserDataSize = xr_loader.find_chunkSize(UserDataChunk, false, true);
                    if (UserDataSize > 0)
                    {
                        model.userdata = new UserData();
                        model.userdata.Load(xr_loader, UserDataSize);
                    }

                    // Lod ref
                    if (model.Header.format_version == 4 && xr_loader.find_chunk((int)OGF.OGF4_S_LODS, false, true))
                    {
                        model.lod = new Lod();
                        model.lod.Load(xr_loader);
                    }

                    // Motion Refs
                    int RefsChunk = (model.Header.format_version == 4 ? (int)OGF.OGF4_S_MOTION_REFS : (int)OGF.OGF3_S_MOTION_REFS);
                    bool StringRefs = xr_loader.find_chunk(RefsChunk, false, true);

                    if (StringRefs || model.Header.format_version == 4 && xr_loader.find_chunk((int)OGF.OGF4_S_MOTION_REFS2, false, true))
                    {
                        model.motion_refs = new MotionRefs();
                        model.motion_refs.Load(xr_loader, StringRefs);
                    }

                    //Motions
                    if (xr_loader.find_chunk((int)OGF.OGF_S_MOTIONS, false, true))
                    {
                        xr_loader.reader.BaseStream.Position -= 8;
                        byte[] OMF = xr_loader.ReadBytes((int)xr_loader.reader.BaseStream.Length - (int)xr_loader.reader.BaseStream.Position);
                        model.motions.SetData(OMF);
                    }
                }
            }

            BitMask.Null(ref model.Format);
            BitMask.Set(ref model.Format, (int)ModelFormat.eOGF);

            Copy(model);
            return true;
        }

        private bool LoadObj(string filename)
        {
            Invalidate();
            source_data = File.ReadAllBytes(filename);
            description = new Description();

            using (var r = new StreamReader(new MemoryStream(source_data)))
            {
                int verts_counter = 0;
                int normals_counter = 0;
                int tang_counter = 0;
                int binorm_counter = 0;
                int uv_counter = 0;
                int face_offset = 0;
                string texture = "";
             
                OGF_Child child = null;
                List<SSkelVert> Vertices = new List<SSkelVert>();
                List<SSkelFace> Faces = new List<SSkelFace>();

                AddChild FlushChild = () =>
                {
                    if (child != null)
                    {
                        child.Vertices = Vertices;
                        child.Faces = Faces;
                        child.m_texture = texture;
                        child.m_shader = "default";
                        child.Header = new OGF_Header();

                        childs.Add(child);

                        texture = "";
                        verts_counter = 0;
                        normals_counter = 0;
                        tang_counter = 0;
                        binorm_counter = 0;
                        uv_counter = 0;
                        face_offset += child.Vertices.Count;
                        Vertices = new List<SSkelVert>();
                        Faces = new List<SSkelFace>();
                    }
                };

                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    if (!line.Contains("#"))
                    {
                        string[] data = line.Split(' ');
                        switch (data[0])
                        {
                            case "o":
                            case "g":
                                FlushChild();
                                child = new OGF_Child();
                                break;
                            case "usemtl":
                                texture = data[1].Trim(new char[] { '\"' });
                                break;
                            case "v":
                                Vertices.Add(new SSkelVert());
                                Vertices[verts_counter].offs = new float[3] { Convert.ToSingle(data[1]), Convert.ToSingle(data[2]), -Convert.ToSingle(data[3]) };
                                verts_counter++;
                                break;
                            case "vt":
                                if (Vertices.Count > uv_counter)
                                {
                                    try
                                    {
                                        Vertices[uv_counter].uv = new float[2] { Convert.ToSingle(data[1]), Math.Abs(1.0f - Convert.ToSingle(data[2])) };
                                    }
                                    catch { }
                                }
                                uv_counter++;
                                break;
                            case "vn":
                                if (Vertices.Count > normals_counter)
                                {
                                    try
                                    {
                                        Vertices[normals_counter].norm = new float[3] { Convert.ToSingle(data[1]), Convert.ToSingle(data[2]), -Convert.ToSingle(data[3]) };
                                    }
                                    catch { }
                                }
                                normals_counter++;
                                break;
                            case "vg":
                                if (Vertices.Count > tang_counter)
                                {
                                    try
                                    {
                                        Vertices[tang_counter].tang = new float[3] { Convert.ToSingle(data[1]), Convert.ToSingle(data[2]), -Convert.ToSingle(data[3]) };
                                    }
                                    catch { }
                                }
                                tang_counter++;
                                break;
                            case "vb":
                                if (Vertices.Count > binorm_counter)
                                {
                                    try
                                    {
                                        Vertices[binorm_counter].binorm = new float[3] { Convert.ToSingle(data[1]), Convert.ToSingle(data[2]), -Convert.ToSingle(data[3]) };
                                    }
                                    catch { }
                                }
                                binorm_counter++;
                                break;
                            case "f":
                                string[] v2 = data[1].Split('/');
                                string[] v1 = data[2].Split('/');
                                string[] v0 = data[3].Split('/');

                                SSkelFace Face = new SSkelFace();
                                Face.v[0] = (ushort)(Convert.ToUInt16(v0[0]) - 1 - face_offset);
                                Face.v[1] = (ushort)(Convert.ToUInt16(v1[0]) - 1 - face_offset);
                                Face.v[2] = (ushort)(Convert.ToUInt16(v2[0]) - 1 - face_offset);

                                Faces.Add(Face);
                                break;
                        }
                    }
                }
                FlushChild();
            }

            RecalcBBox(true);
            BitMask.Null(ref Format);
            BitMask.Set(ref Format, (int)ModelFormat.eObj);
            return true;
        }

        private bool LoadDM(string filename)
        {
            Invalidate();
            source_data = File.ReadAllBytes(filename);

            var xr_loader = new XRayLoader();

            using (var r = new BinaryReader(new MemoryStream(source_data)))
            {
                xr_loader.SetStream(r.BaseStream);

                OGF_Child chld = new OGF_Child();
                chld.LoadDM(xr_loader);
                childs.Add(chld);
            }

            BitMask.Null(ref Format);
            BitMask.Set(ref Format, (int)ModelFormat.eDM);
            return true;
        }

        private bool LoadDetail(string filename)
        {
            Invalidate();
            source_data = File.ReadAllBytes(filename);

            var xr_loader = new XRayLoader();

            using (var r = new BinaryReader(new MemoryStream(source_data)))
            {
                xr_loader.SetStream(r.BaseStream);
                xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(1, false, true));

                int det_id = 0;

                while (true)
                {
                    if (!xr_loader.find_chunk(det_id)) break;

                    Stream temp = xr_loader.reader.BaseStream;

                    if (!xr_loader.SetData(xr_loader.find_and_return_chunk_in_chunk(det_id, false, true))) break;

                    OGF_Child chld = new OGF_Child();
                    chld.LoadDM(xr_loader);
                    childs.Add(chld);

                    det_id++;
                    xr_loader.SetStream(temp);
                }

                float step_radius = 1.2f;
                float view_offsX = 0.0f;
                float view_offsZ = 0.0f;
                int size = (int)Math.Round(Math.Sqrt(childs.Count), 0);

                for (int i = 0; i < childs.Count; i++)
                {
                    if (i % size == 0)
                    {
                        view_offsX = 0.0f;
                        view_offsZ += step_radius;
                    }

                    childs[i].SetLocalOffsetMain(new float[3] { view_offsX, 0.0f, view_offsZ });

                    view_offsX += step_radius;
                }
            }

            BitMask.Null(ref Format);
            BitMask.Set(ref Format, (int)ModelFormat.eDM);
            BitMask.Set(ref Format, (int)ModelFormat.eDetail);
            return true;
        }

        private bool OpenFileInternal(string filename, bool silent = false)
        {
            string format = Path.GetExtension(filename);

            if (format == ".dm")
                return LoadDM(filename);
            else if (format == ".details")
                return LoadDetail(filename);
            else if (format == ".ogf")
                return LoadOGF(filename, silent);
            else if (format == ".obj")
                return LoadObj(filename);

            return false;
        }

        public void SaveFile(string filename, bool backup = false, ModelFormat override_fmt = ModelFormat.eUnknown)
        {
            if (source_data == null) return;

            if (override_fmt != ModelFormat.eUnknown)
                SaveFileAsFmt(filename, backup, override_fmt);
            else if (BitMask.IsSet(Format, (int)ModelFormat.eDetail))
                SaveDetail(filename, backup);
            else if (BitMask.IsSet(Format, (int)ModelFormat.eDM))
                SaveDM(filename, backup);
            else if (BitMask.IsSet(Format, (int)ModelFormat.eOGF))
                SaveOGF(filename, backup);
            else if (BitMask.IsSet(Format, (int)ModelFormat.eObj))
            {
                SaveObj saveObj = new SaveObj();
                saveObj.ShowDialog();

                switch (saveObj.Fmt)
                {
                    case Editor.ExportFormat.OGF:
                        SaveOGF(Path.ChangeExtension(filename, ".ogf"), backup);
                        break;
                    case Editor.ExportFormat.DM:
                        SaveDM(Path.ChangeExtension(filename, ".dm"), backup);
                        break;
                    case Editor.ExportFormat.Object:
                        SaveObject(Path.ChangeExtension(filename, ".object"), backup);
                        break;
                }
            }
        }

        private void SaveFileAsFmt(string filename, bool backup, ModelFormat fmt)
        {
            if (BitMask.IsSet((int)fmt, (int)ModelFormat.eDetail))
                SaveDetail(filename, backup);
            else if (BitMask.IsSet((int)fmt, (int)ModelFormat.eDM))
                SaveDM(filename, backup);
            else if (BitMask.IsSet((int)fmt, (int)ModelFormat.eOGF))
                SaveOGF(filename, backup);
        }

        public int SaveObject(string filename, bool backup = false)
        {
            string ext = Is(ModelFormat.eDM) && !Is(ModelFormat.eObj) ? ".dm" : ".ogf"; // Create temp ext

            if (File.Exists(filename + ext)) // Remove temp file if exist
                File.Delete(filename + ext);

            SaveFile(filename + ext, backup, Is(ModelFormat.eObj) ? ModelFormat.eOGF : ModelFormat.eUnknown);

            int code = RunConverter(filename + ext, filename, ext == ".dm" ? 2 : 0, 0);

            if (File.Exists(filename + ext)) // Remove temp file
                File.Delete(filename + ext);

            return code;
        }

        public int SaveBones(string filename)
        {
            return RunConverter(FileName, filename, 0, 1);
        }

        public int SaveSkl(string filename)
        {
            return RunConverter(FileName, filename, 0, 2);
        }

        public int SaveSkls(string filename)
        {
            return RunConverter(FileName, filename, 0, 3);
        }

        public bool SaveOMF(string filename)
        {
            using (var fileStream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                fileStream.Write(motions.data(), 0, motions.data().Length);
                fileStream.Close();
            }

            return true;
        }

        public void SaveOGF(string filename, bool backup = false)
        {
            List<byte> file_bytes = new List<byte>();

            TryRepairUserdata(userdata);
            using (var fileStream = new BinaryReader(new MemoryStream(source_data)))
            {
                byte[] temp;
                if (!Header.IsStaticSingle())
                    file_bytes.AddRange(Header.data());

                if (description != null)
                {
                    byte[] DescriptionData = description.data();

                    file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_DESC));
                    file_bytes.AddRange(BitConverter.GetBytes(DescriptionData.Length));
                    file_bytes.AddRange(DescriptionData);
                }

                if (Header.IsStaticSingle()) // Single mesh
                {
                    file_bytes.AddRange(childs[0].data());
                    fileStream.BaseStream.Position += childs[0].old_size;
                }
                else // Hierrarhy mesh
                {
                    fileStream.ReadBytes((int)(pos - fileStream.BaseStream.Position));

                    fileStream.ReadBytes(4);
                    uint OldChildrenChunkSize = fileStream.ReadUInt32();
                    fileStream.BaseStream.Position += OldChildrenChunkSize;

                    uint ChildrenChunkSize = 0;
                    foreach (var ch in childs)
                    {
                        if (!ch.to_delete)
                            ChildrenChunkSize += (uint)ch.data().Length + 8;
                    }

                    int ChildrenChunk = (Header.format_version == 4 ? (int)OGF.OGF4_CHILDREN : (int)OGF.OGF3_CHILDREN);
                    file_bytes.AddRange(BitConverter.GetBytes(ChildrenChunk));
                    file_bytes.AddRange(BitConverter.GetBytes(ChildrenChunkSize));

                    int ChildChunk = 0;
                    foreach (var ch in childs)
                    {
                        if (ch.to_delete) continue;

                        byte[] ChildData = ch.data();

                        file_bytes.AddRange(BitConverter.GetBytes(ChildChunk));
                        file_bytes.AddRange(BitConverter.GetBytes(ChildData.Length));
                        file_bytes.AddRange(ChildData);
                        ChildChunk++;
                    }
                }

                if (Header.IsSkeleton())
                {
                    if (bonedata != null)
                    {
                        if (BrokenType == 0 && bonedata.pos > 0 && (bonedata.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
                        {
                            temp = fileStream.ReadBytes((int)(bonedata.pos - fileStream.BaseStream.Position));
                            file_bytes.AddRange(temp);
                        }

                        byte[] BonesData = bonedata.data(BrokenType == 2);

                        file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF_S_BONE_NAMES));
                        file_bytes.AddRange(BitConverter.GetBytes(BonesData.Length));
                        file_bytes.AddRange(BonesData);

                        fileStream.ReadBytes(bonedata.old_size + 8);
                    }

                    if (ikdata != null)
                    {
                        if (BrokenType == 0 && ikdata.pos > 0 && (ikdata.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
                        {
                            temp = fileStream.ReadBytes((int)(ikdata.pos - fileStream.BaseStream.Position));
                            file_bytes.AddRange(temp);
                        }

                        byte[] IKDataData = ikdata.data();

                        file_bytes.AddRange(BitConverter.GetBytes(ikdata.ChunkID(Header.format_version)));
                        file_bytes.AddRange(BitConverter.GetBytes(IKDataData.Length));
                        file_bytes.AddRange(IKDataData);

                        fileStream.ReadBytes(ikdata.old_size + 8);
                    }

                    if (userdata != null)
                    {
                        if (userdata.pos > 0 && (userdata.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
                        {
                            temp = fileStream.ReadBytes((int)(userdata.pos - fileStream.BaseStream.Position));
                            file_bytes.AddRange(temp);
                        }

                        if (userdata.userdata != "") // Пишем если есть что писать
                        {
                            uint UserDataChunk = (Header.format_version == 4 ? (uint)OGF.OGF4_S_USERDATA : (uint)OGF.OGF3_S_USERDATA);
                            byte[] UserDataData = userdata.data();

                            file_bytes.AddRange(BitConverter.GetBytes(UserDataChunk));
                            file_bytes.AddRange(BitConverter.GetBytes(UserDataData.Length));
                            file_bytes.AddRange(UserDataData);
                        }

                        if (userdata.old_size > 0) // Сдвигаем позицию риадера если в модели был чанк
                            fileStream.ReadBytes(userdata.old_size + 8);
                    }

                    if (lod != null && Header.format_version == 4) // Стринг лод только у релизных OGF
                    {
                        if (lod.pos > 0 && (lod.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
                        {
                            temp = fileStream.ReadBytes((int)(lod.pos - fileStream.BaseStream.Position));
                            file_bytes.AddRange(temp);
                        }

                        if (lod.lod_path != "") // Пишем если есть что писать
                        {
                            byte[] LodData = lod.data();

                            file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_LODS));
                            file_bytes.AddRange(BitConverter.GetBytes(LodData.Length));
                            file_bytes.AddRange(LodData);
                        }

                        if (lod.old_size > 0) // Сдвигаем позицию риадера если в модели был чанк
                            fileStream.ReadBytes(lod.old_size + 8);
                    }

                    bool refs_created = false;
                    if (motion_refs != null)
                    {
                        if (motion_refs.pos > 0 && (motion_refs.pos - fileStream.BaseStream.Position) > 0) // Двигаемся до текущего чанка
                        {
                            temp = fileStream.ReadBytes((int)(motion_refs.pos - fileStream.BaseStream.Position));
                            file_bytes.AddRange(temp);
                        }

                        if (motion_refs.refs.Count > 0) // Пишем если есть что писать
                        {
                            refs_created = true;
                            byte[] MotionRefsData = motion_refs.data(motion_refs.soc);

                            if (!motion_refs.soc)
                                file_bytes.AddRange(BitConverter.GetBytes((uint)OGF.OGF4_S_MOTION_REFS2));
                            else
                            {
                                uint RefsChunk = (Header.format_version == 4 ? (uint)OGF.OGF4_S_MOTION_REFS : (uint)OGF.OGF3_S_MOTION_REFS);
                                file_bytes.AddRange(BitConverter.GetBytes(RefsChunk));
                            }
                            file_bytes.AddRange(BitConverter.GetBytes(MotionRefsData.Length));
                            file_bytes.AddRange(MotionRefsData);
                        }

                        if (motion_refs.old_size > 0) // Сдвигаем позицию риадера если в модели был чанк
                            fileStream.ReadBytes(motion_refs.old_size + 8);
                    }

                    if (motions.data() != null && !refs_created)
                        file_bytes.AddRange(motions.data());
                }
                else if (!Is(ModelFormat.eObj))
                {
                    temp = fileStream.ReadBytes((int)(fileStream.BaseStream.Length - fileStream.BaseStream.Position));
                    file_bytes.AddRange(temp);
                }
            }

            WriteFile(filename, file_bytes.ToArray(), backup);
        }

        public void SaveDetail(string filename, bool backup = false)
        {
            List<byte> file_bytes = new List<byte>();
            using (var fileStream = new BinaryReader(new MemoryStream(source_data)))
            {
                fileStream.ReadBytes(4);
                uint OldDetailsSize = fileStream.ReadUInt32();
                fileStream.BaseStream.Position += OldDetailsSize;

                uint DetailsChunkSize = 0;
                foreach (var ch in childs)
                {
                    if (!ch.to_delete)
                        DetailsChunkSize += (uint)ch.dm_data().Length + 8;
                }

                file_bytes.AddRange(BitConverter.GetBytes(1));
                file_bytes.AddRange(BitConverter.GetBytes(DetailsChunkSize));

                int DetailID = 0;
                foreach (var ch in childs)
                {
                    if (ch.to_delete) continue;

                    byte[] DetailData = ch.dm_data();

                    file_bytes.AddRange(BitConverter.GetBytes(DetailID));
                    file_bytes.AddRange(BitConverter.GetBytes(DetailData.Length));
                    file_bytes.AddRange(DetailData);
                    DetailID++;
                }
                byte[] dm_data = fileStream.ReadBytes((int)(fileStream.BaseStream.Length - fileStream.BaseStream.Position));
                file_bytes.AddRange(dm_data);
                WriteFile(filename, file_bytes.ToArray(), backup);
            }
        }

        private void WriteFile(string filename, byte[] data, bool bkp)
        {
            if (bkp)
            {
                string backup_path = filename + ".bak";

                if (File.Exists(backup_path))
                    File.Delete(backup_path);

                File.Copy(filename, backup_path);
            }

            using (var fileStream = new FileStream(filename, File.Exists(filename) ? FileMode.Truncate : FileMode.Create))
            {
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();
            }
        }

        public void SaveDM(string filename, bool backup = false)
        {
            SaveDM(filename, 0, backup);
        }

        public void SaveDM(string filename, int child, bool bkp = false)
        {
            List<byte> file_bytes = new List<byte>();

            byte[] dm_data = childs[child].dm_data();
            file_bytes.AddRange(dm_data);
            WriteFile(filename, file_bytes.ToArray(), bkp);
        }

        public void SaveObj(string filename, float lod = 0.0f, bool viewport_bones = false, bool viewport_bbox = false, bool viewport_textures = false)
        {
            using (ObjWriter = File.CreateText(filename))
            {
                uint v_offs = 0;
                uint model_id = 0;

                string mtl_name = Path.ChangeExtension(filename, ".mtl");
                SaveMtl(mtl_name, viewport_bones, viewport_bbox, viewport_textures);

                try
                {
                    ObjWriter.WriteLine("# This file uses meters as units for non-parametric coordinates.");
                    ObjWriter.WriteLine("mtllib " + Path.GetFileName(mtl_name));

                    WriteObj Writer = (Vertices, Faces, Texture) =>
                    {
                        ObjWriter.WriteLine($"g {model_id}");
                        ObjWriter.WriteLine($"usemtl \"{Path.GetFileName(Texture)}\"");
                        model_id++;

                        for (int i = 0; i < Vertices.Count; i++)
                        {
                            ObjWriter.WriteLine($"v {FVec.vPUSH(FVec.MirrorZ(SetupObjOffset(Vertices[i])), "0.000000")}");
                        }

                        for (int i = 0; i < Vertices.Count; i++)
                        {
                            float x = Vertices[i].uv[0];
                            float y = Math.Abs(1.0f - Vertices[i].uv[1]);
                            ObjWriter.WriteLine($"vt {x.ToString("0.000000")} {y.ToString("0.000000")}");
                        }

                        for (int i = 0; i < Vertices.Count; i++)
                        {
                            ObjWriter.WriteLine($"vn {FVec.vPUSH(FVec.MirrorZ(Vertices[i].Norm()), "0.000000")}");
                        }

                        for (int i = 0; i < Vertices.Count; i++)
                        {
                            ObjWriter.WriteLine($"vg {FVec.vPUSH(FVec.MirrorZ(Vertices[i].Tang()), "0.000000")}");
                        }

                        for (int i = 0; i < Vertices.Count; i++)
                        {
                            ObjWriter.WriteLine($"vb {FVec.vPUSH(FVec.MirrorZ(Vertices[i].Binorm()), "0.000000")}");
                        }

                        foreach (var f_it in Faces)
                        {
                            string tmp = $"f {v_offs+f_it.v[2]+1}/{v_offs+f_it.v[2]+1}/{v_offs+f_it.v[2]+1} {v_offs+f_it.v[1]+1}/{v_offs+f_it.v[1]+1}/{v_offs+f_it.v[1]+1} {v_offs+f_it.v[0]+1}/{v_offs+f_it.v[0]+1}/{v_offs+f_it.v[0]+1}";
                            ObjWriter.WriteLine(tmp);
                        }
                        v_offs += (uint)Vertices.Count;
                    };


                    foreach (var ch in childs)
                    {
                        if (ch.to_delete) continue;

                        List<SSkelVert> sSkelVerts = new List<SSkelVert>();
                        sSkelVerts.AddRange(ch.Vertices);

                        List<SSkelFace> Faces = new List<SSkelFace>();
                        Faces.AddRange(ch.Faces_SWI(lod));

                        Writer(sSkelVerts, Faces, viewport_bones ? "null_texture" : ch.m_texture);
                    }

                    if (viewport_bbox)
                    {
                        List<SSkelVert> sSkelVerts = new List<SSkelVert>();
                        List<SSkelFace> Faces = new List<SSkelFace>();

                        if (!Header.IsStaticSingle())
                        {
                            sSkelVerts.AddRange(Header.bb.GetVisualVerts());
                            Faces.AddRange(Header.bb.GetVisualFaces(sSkelVerts));

                            Writer(sSkelVerts, Faces, "bbox_main_texture");
                        }

                        foreach (var ch in childs)
                        {
                            if (ch.to_delete) continue;

                            sSkelVerts.Clear();
                            sSkelVerts.AddRange(ch.Header.bb.GetVisualVerts());

                            Faces.Clear();
                            Faces.AddRange(ch.Header.bb.GetVisualFaces(sSkelVerts));

                            Writer(sSkelVerts, Faces, "bbox_texture");
                        }
                    }

                    if (viewport_bones)
                    {
                        for (int i = 0; i < ikdata.bones.Count; i++)
                        {
                            List<SSkelVert> sSkelVerts = new List<SSkelVert>();
                            List<SSkelFace> Faces = new List<SSkelFace>();

                            float bbox_size = 0.024f;
                            BBox bone_box = new BBox();
                            bone_box.min = new float[3] { -bbox_size / 2, -bbox_size / 2, -bbox_size / 2 };
                            bone_box.max = new float[3] { bbox_size / 2, bbox_size / 2, bbox_size / 2 };

                            bone_box.min = FVec.Add(bone_box.min, ikdata.bones[i].render_transform);
                            bone_box.max = FVec.Add(bone_box.max, ikdata.bones[i].render_transform);

                            sSkelVerts.AddRange(bone_box.GetVisualVerts());
                            Faces.AddRange(bone_box.GetVisualFaces(sSkelVerts));

                            Writer(sSkelVerts, Faces, bonedata.bones[i].name);
                        }
                    }

                    ObjWriter.Close();
                    ObjWriter = null;
                }
                catch (Exception) { }
            }
        }

        private float[] SetupObjOffset(SSkelVert vert)
        {
            if (!Header.IsStaticSingle() && ikdata != null && ikdata.chunk_version == 2)
                return FixOldVertexOffset(vert);

            return vert.Offset();
        }

        public void SaveMtl(string filename, bool viewport_bones = false, bool viewport_bbox = false, bool viewport_textures = false)
        {
            using (StreamWriter writer = File.CreateText(filename))
            {
                if (viewport_bones)
                {
                    writer.WriteLine("newmtl \"null_texture\"");
                    writer.WriteLine("Ka  0 0 0");
                    writer.WriteLine("Kd  1 1 1");
                    writer.WriteLine("Ks  0 0 0");
                    writer.WriteLine("map_Kd null_texture.png\n");

                    for (int i = 0; i < bonedata.bones.Count; i++)
                    {
                        writer.WriteLine($"newmtl \"{bonedata.bones[i].name}\"");
                        writer.WriteLine("Ka  0 0 0");
                        writer.WriteLine("Kd  1 1 1");
                        writer.WriteLine("Ks  0 0 0");
                        writer.WriteLine($"map_Kd bones\\{bonedata.bones[i].GetNotNullName()}.png\n");
                    }
                }
                else
                {
                    foreach (var ch in childs)
                    {
                        if (ch.to_delete) continue;

                        writer.WriteLine("newmtl \"" + Path.GetFileName(ch.m_texture) + "\"");
                        writer.WriteLine("Ka  0 0 0");
                        writer.WriteLine("Kd  1 1 1");
                        writer.WriteLine("Ks  0 0 0");
                        if (viewport_textures)
                            writer.WriteLine("map_Kd " + Path.GetFileName(ch.m_texture) + ".png\n");
                    }
                }

                if (viewport_bbox)
                {
                    writer.WriteLine("newmtl \"bbox_main_texture\"");
                    writer.WriteLine("Ka  0 0 0");
                    writer.WriteLine("Kd  1 1 1");
                    writer.WriteLine("Ks  0 0 0");
                    writer.WriteLine("map_Kd bbox_main_texture.png\n");

                    writer.WriteLine("newmtl bbox_texture");
                    writer.WriteLine("Ka  0 0 0");
                    writer.WriteLine("Kd  1 1 1");
                    writer.WriteLine("Ks  0 0 0");
                    writer.WriteLine("map_Kd bbox_texture.png\n");
                }

                writer.Close();
            }
        }

        public bool NeedRepair()
        {
            return BrokenType > 0;
        }

        private void TryRepairUserdata(UserData data)
        {
            if (Header.format_version == 4 && data != null && data.old_format && MessageBox.Show("Userdata has old format, update?", "OGF Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                data.old_format = false;
        }

        public void RecalcBBox(bool recalc_childs)
        {
            if (Header == null)
                Header = new OGF_Header();

            Header.bb.Invalidate();

            foreach (OGF_Child child in childs)
            {
                if (!child.to_delete)
                {
                    if (recalc_childs)
                        child.RecalcBBox();
                    Header.bb.Merge(child.Header.bb);
                }
            }

            Header.bs.CreateSphere(Header.bb);
        }

        public void AddBone(string name, string parent_bone, int pos)
        {
            if (Opened && Header.IsSkeleton())
            {
                // Create null OBB
                List<byte> obb = new List<byte>();
                for (int i = 0; i < 60; i++)
                    obb.Add(0);

                Bone bone = new Bone();
                bone.name = name;
                bone.parent_name = parent_bone;
                bone.fobb = obb.ToArray();

                bonedata.bones.Insert(pos, bone);

                if (ikdata != null)
                {
                    IK_Bone ikbone = new IK_Bone();
                    int ImportBytes = ((ikdata.chunk_version == 4) ? 76 : ((ikdata.chunk_version == 3) ? 72 : 60));

                    // Create null Bone Shape
                    List<byte> shape = new List<byte>();
                    for (int i = 0; i < 112 + ImportBytes; i++)
                        shape.Add(0);

                    if (ikdata.chunk_version == 4)
                        ikbone.version = 1;

                    ikbone.material = "default_object";
                    ikbone.kinematic_data = shape.ToArray();
                    ikbone.rotation = new float[3];
                    ikbone.position = new float[3];
                    ikbone.mass = 10.0f;
                    ikbone.center_mass = new float[3];

                    ikdata.bones.Insert(pos, ikbone);
                }
            }
        }

        public void RemoveBone(string bone)
        {
            if (Opened && Header.IsSkeleton() && bonedata != null)
            {
                RemoveBone(bonedata.GetBoneID(bone));
            }
        }

        public void RemoveBone(int bone)
        {
            if (Opened && Header.IsSkeleton())
            {
                bonedata.RemoveBone(bone);

                if (ikdata != null)
                    ikdata.RemoveBone(bone);
            }
        }

        public void ChangeParent(string old, string _new)
        {
            if (Opened && Header.IsSkeleton())
            {
                for (int i = 0; i < bonedata.bones.Count; i++)
                {
                    if (bonedata.bones[i].parent_name == old)
                        bonedata.bones[i].parent_name = _new;
                }
            }
        }

        public void ChangeModelFormat()
        {
            if (Opened)
            {
                if (Header.format_version != 4)
                {
                    MessageBox.Show("Can't convert model. Unsupported OGF version: " + Header.format_version.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IsCopModel = !IsCopModel;

                if (IsCopModel)
                {
                    if (motion_refs != null)
                        motion_refs.soc = false;

                    foreach (var ch in childs)
                    {
                        if (ch.links >= 0x12071980)
                            ch.links /= 0x12071980;
                    }
                }
                else
                {
                    uint links = 0;

                    foreach (var ch in childs)
                        links = Math.Max(links, ch.LinksCount());

                    if (links > 2 && MessageBox.Show("Model has more than 2 links. After converting to SoC model will lose influence data, continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        IsCopModel = !IsCopModel;
                        return;
                    }

                    foreach (var ch in childs)
                    {
                        if (ch.LinksCount() > 2)
                            ch.SetLinks(1);
                    }

                    if (motions.Anims != null)
                    {
                        foreach (var Anim in motions.Anims)
                        {
                            bool key16bit = (Anim.flags & (int)MotionKeyFlags.flTKey16IsBit) == (int)MotionKeyFlags.flTKey16IsBit;
                            bool keynocompressbit = (Anim.flags & (int)MotionKeyFlags.flTKeyFFT_Bit) == (int)MotionKeyFlags.flTKeyFFT_Bit;

                            if (key16bit || keynocompressbit)
                            {
                                if (MessageBox.Show("Build-in motions are in " + (keynocompressbit ? "no compression" : "16 bit compression") + " format, not supported in SoC. Delete motions?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                    motions.SetData(null);
                                break;
                            }
                        }
                    }

                    if (motion_refs != null)
                        motion_refs.soc = true;

                    foreach (var ch in childs)
                    {
                        if (ch.links < 0x12071980)
                            ch.links *= 0x12071980;
                    }
                }
            }
        }
    }
}

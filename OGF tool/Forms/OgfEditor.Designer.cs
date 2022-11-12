
namespace OGF_tool
{
    partial class OGF_Editor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OGF_Editor));
            this.OpenOGFDialog = new System.Windows.Forms.OpenFileDialog();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TexturesPage = new System.Windows.Forms.TabPage();
            this.TextureContextStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddMeshesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LodMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TexturesGropuBox = new System.Windows.Forms.GroupBox();
            this.MoveMeshButton = new System.Windows.Forms.Button();
            this.LodLabel = new System.Windows.Forms.Label();
            this.LinksLabel = new System.Windows.Forms.Label();
            this.DeleteMesh = new System.Windows.Forms.Button();
            this.VertsLabel = new System.Windows.Forms.Label();
            this.FaceLabel = new System.Windows.Forms.Label();
            this.ShaderNameLabelEx = new System.Windows.Forms.Label();
            this.TexturesPathLabelEx = new System.Windows.Forms.Label();
            this.ShaderTextBoxEx = new System.Windows.Forms.TextBox();
            this.TexturesTextBoxEx = new System.Windows.Forms.TextBox();
            this.UserDataPage = new System.Windows.Forms.TabPage();
            this.CreateUserdataButton = new System.Windows.Forms.Button();
            this.UserDataBox = new System.Windows.Forms.RichTextBox();
            this.MotionRefsPage = new System.Windows.Forms.TabPage();
            this.CreateMotionRefsButton = new System.Windows.Forms.Button();
            this.MotionRefsBox = new System.Windows.Forms.RichTextBox();
            this.MotionPage = new System.Windows.Forms.TabPage();
            this.AppendOMFButton = new System.Windows.Forms.Button();
            this.MotionBox = new System.Windows.Forms.RichTextBox();
            this.MotionsContextStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.BoneNamesPage = new System.Windows.Forms.TabPage();
            this.BoneNamesBox = new System.Windows.Forms.RichTextBox();
            this.BoneParamsPage = new System.Windows.Forms.TabPage();
            this.BoneParamsGroupBox = new System.Windows.Forms.GroupBox();
            this.RotationLabelEx = new System.Windows.Forms.Label();
            this.PositionLabelEx = new System.Windows.Forms.Label();
            this.CenterOfMassLabelEx = new System.Windows.Forms.Label();
            this.MassLabelEx = new System.Windows.Forms.Label();
            this.BonesParamsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RotationZTextBox = new System.Windows.Forms.TextBox();
            this.RotationYTextBox = new System.Windows.Forms.TextBox();
            this.RotationXTextBox = new System.Windows.Forms.TextBox();
            this.PositionZTextBox = new System.Windows.Forms.TextBox();
            this.PositionYTextBox = new System.Windows.Forms.TextBox();
            this.PositionXTextBox = new System.Windows.Forms.TextBox();
            this.CenterOfMassZTextBox = new System.Windows.Forms.TextBox();
            this.CenterOfMassYTextBox = new System.Windows.Forms.TextBox();
            this.CenterOfMassXTextBox = new System.Windows.Forms.TextBox();
            this.MassTextBoxEx = new System.Windows.Forms.TextBox();
            this.MaterialLabelEx = new System.Windows.Forms.Label();
            this.MaterialTextBoxEx = new System.Windows.Forms.TextBox();
            this.ParentBoneTextBoxEx = new System.Windows.Forms.TextBox();
            this.BoneNameTextBoxEx = new System.Windows.Forms.TextBox();
            this.ParentBoneLabelEx = new System.Windows.Forms.Label();
            this.BoneNameLabelEx = new System.Windows.Forms.Label();
            this.LodPage = new System.Windows.Forms.TabPage();
            this.CreateLodButton = new System.Windows.Forms.Button();
            this.LodPathBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ViewPage = new System.Windows.Forms.TabPage();
            this.MenuPanel = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadMenuParam = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuParam = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.omfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sklToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sklsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenInObjectEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.importDataFromModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recalcNormalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeProgressiveMeshesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.converterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nPCCoPToSoCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nPCSoCToCoPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OgfInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CurrentFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.viewPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.DisableTexturesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableAlphaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMeshesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openImageFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusPanel = new System.Windows.Forms.StatusStrip();
            this.BkpCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveAsDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenOMFDialog = new System.Windows.Forms.OpenFileDialog();
            this.OpenProgramDialog = new System.Windows.Forms.OpenFileDialog();
            this.OpenOGF_DmDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveSklsDialog = new System.Windows.Forms.SaveFileDialog();
            this.SaveOmfDialog = new System.Windows.Forms.SaveFileDialog();
            this.SaveBonesDialog = new System.Windows.Forms.SaveFileDialog();
            this.SaveObjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.LabelBroken = new System.Windows.Forms.Label();
            this.SaveObjDialog = new System.Windows.Forms.SaveFileDialog();
            this.TabControl.SuspendLayout();
            this.TexturesPage.SuspendLayout();
            this.TextureContextStrip.SuspendLayout();
            this.TexturesGropuBox.SuspendLayout();
            this.UserDataPage.SuspendLayout();
            this.MotionRefsPage.SuspendLayout();
            this.MotionPage.SuspendLayout();
            this.MotionsContextStrip.SuspendLayout();
            this.BoneNamesPage.SuspendLayout();
            this.BoneParamsPage.SuspendLayout();
            this.BoneParamsGroupBox.SuspendLayout();
            this.BonesParamsPanel.SuspendLayout();
            this.LodPage.SuspendLayout();
            this.MenuPanel.SuspendLayout();
            this.StatusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpenOGFDialog
            // 
            resources.ApplyResources(this.OpenOGFDialog, "OpenOGFDialog");
            // 
            // TabControl
            // 
            resources.ApplyResources(this.TabControl, "TabControl");
            this.TabControl.Controls.Add(this.TexturesPage);
            this.TabControl.Controls.Add(this.UserDataPage);
            this.TabControl.Controls.Add(this.MotionRefsPage);
            this.TabControl.Controls.Add(this.MotionPage);
            this.TabControl.Controls.Add(this.BoneNamesPage);
            this.TabControl.Controls.Add(this.BoneParamsPage);
            this.TabControl.Controls.Add(this.LodPage);
            this.TabControl.Controls.Add(this.ViewPage);
            this.TabControl.Multiline = true;
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // TexturesPage
            // 
            resources.ApplyResources(this.TexturesPage, "TexturesPage");
            this.TexturesPage.BackColor = System.Drawing.SystemColors.Control;
            this.TexturesPage.ContextMenuStrip = this.TextureContextStrip;
            this.TexturesPage.Controls.Add(this.TexturesGropuBox);
            this.TexturesPage.Name = "TexturesPage";
            // 
            // TextureContextStrip
            // 
            this.TextureContextStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddMeshesMenuItem,
            this.LodMenuItem});
            this.TextureContextStrip.Name = "TextureContextStrip";
            resources.ApplyResources(this.TextureContextStrip, "TextureContextStrip");
            // 
            // AddMeshesMenuItem
            // 
            this.AddMeshesMenuItem.Name = "AddMeshesMenuItem";
            resources.ApplyResources(this.AddMeshesMenuItem, "AddMeshesMenuItem");
            this.AddMeshesMenuItem.Click += new System.EventHandler(this.addMeshesToolStripMenuItem_Click);
            // 
            // LodMenuItem
            // 
            this.LodMenuItem.Name = "LodMenuItem";
            resources.ApplyResources(this.LodMenuItem, "LodMenuItem");
            this.LodMenuItem.Click += new System.EventHandler(this.changeLodToolStripMenuItem_Click);
            // 
            // TexturesGropuBox
            // 
            this.TexturesGropuBox.Controls.Add(this.MoveMeshButton);
            this.TexturesGropuBox.Controls.Add(this.LodLabel);
            this.TexturesGropuBox.Controls.Add(this.LinksLabel);
            this.TexturesGropuBox.Controls.Add(this.DeleteMesh);
            this.TexturesGropuBox.Controls.Add(this.VertsLabel);
            this.TexturesGropuBox.Controls.Add(this.FaceLabel);
            this.TexturesGropuBox.Controls.Add(this.ShaderNameLabelEx);
            this.TexturesGropuBox.Controls.Add(this.TexturesPathLabelEx);
            this.TexturesGropuBox.Controls.Add(this.ShaderTextBoxEx);
            this.TexturesGropuBox.Controls.Add(this.TexturesTextBoxEx);
            resources.ApplyResources(this.TexturesGropuBox, "TexturesGropuBox");
            this.TexturesGropuBox.Name = "TexturesGropuBox";
            this.TexturesGropuBox.TabStop = false;
            // 
            // MoveMeshButton
            // 
            resources.ApplyResources(this.MoveMeshButton, "MoveMeshButton");
            this.MoveMeshButton.Name = "MoveMeshButton";
            this.MoveMeshButton.UseVisualStyleBackColor = true;
            // 
            // LodLabel
            // 
            resources.ApplyResources(this.LodLabel, "LodLabel");
            this.LodLabel.Name = "LodLabel";
            // 
            // LinksLabel
            // 
            resources.ApplyResources(this.LinksLabel, "LinksLabel");
            this.LinksLabel.Name = "LinksLabel";
            // 
            // DeleteMesh
            // 
            resources.ApplyResources(this.DeleteMesh, "DeleteMesh");
            this.DeleteMesh.Name = "DeleteMesh";
            this.DeleteMesh.UseVisualStyleBackColor = true;
            // 
            // VertsLabel
            // 
            resources.ApplyResources(this.VertsLabel, "VertsLabel");
            this.VertsLabel.Name = "VertsLabel";
            // 
            // FaceLabel
            // 
            resources.ApplyResources(this.FaceLabel, "FaceLabel");
            this.FaceLabel.Name = "FaceLabel";
            // 
            // ShaderNameLabelEx
            // 
            resources.ApplyResources(this.ShaderNameLabelEx, "ShaderNameLabelEx");
            this.ShaderNameLabelEx.Name = "ShaderNameLabelEx";
            // 
            // TexturesPathLabelEx
            // 
            resources.ApplyResources(this.TexturesPathLabelEx, "TexturesPathLabelEx");
            this.TexturesPathLabelEx.Name = "TexturesPathLabelEx";
            // 
            // ShaderTextBoxEx
            // 
            resources.ApplyResources(this.ShaderTextBoxEx, "ShaderTextBoxEx");
            this.ShaderTextBoxEx.Name = "ShaderTextBoxEx";
            // 
            // TexturesTextBoxEx
            // 
            resources.ApplyResources(this.TexturesTextBoxEx, "TexturesTextBoxEx");
            this.TexturesTextBoxEx.Name = "TexturesTextBoxEx";
            // 
            // UserDataPage
            // 
            this.UserDataPage.Controls.Add(this.CreateUserdataButton);
            this.UserDataPage.Controls.Add(this.UserDataBox);
            resources.ApplyResources(this.UserDataPage, "UserDataPage");
            this.UserDataPage.Name = "UserDataPage";
            // 
            // CreateUserdataButton
            // 
            resources.ApplyResources(this.CreateUserdataButton, "CreateUserdataButton");
            this.CreateUserdataButton.Name = "CreateUserdataButton";
            this.CreateUserdataButton.UseVisualStyleBackColor = true;
            this.CreateUserdataButton.Click += new System.EventHandler(this.CreateUserdataButton_Click);
            // 
            // UserDataBox
            // 
            resources.ApplyResources(this.UserDataBox, "UserDataBox");
            this.UserDataBox.Name = "UserDataBox";
            this.UserDataBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RichTextBoxImgDefender);
            // 
            // MotionRefsPage
            // 
            this.MotionRefsPage.Controls.Add(this.CreateMotionRefsButton);
            this.MotionRefsPage.Controls.Add(this.MotionRefsBox);
            resources.ApplyResources(this.MotionRefsPage, "MotionRefsPage");
            this.MotionRefsPage.Name = "MotionRefsPage";
            // 
            // CreateMotionRefsButton
            // 
            resources.ApplyResources(this.CreateMotionRefsButton, "CreateMotionRefsButton");
            this.CreateMotionRefsButton.Name = "CreateMotionRefsButton";
            this.CreateMotionRefsButton.UseVisualStyleBackColor = true;
            this.CreateMotionRefsButton.Click += new System.EventHandler(this.CreateMotionRefsButton_Click);
            // 
            // MotionRefsBox
            // 
            this.MotionRefsBox.DetectUrls = false;
            resources.ApplyResources(this.MotionRefsBox, "MotionRefsBox");
            this.MotionRefsBox.Name = "MotionRefsBox";
            this.MotionRefsBox.TextChanged += new System.EventHandler(this.RichTextBoxTextChanged);
            this.MotionRefsBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RichTextBoxImgDefender);
            // 
            // MotionPage
            // 
            this.MotionPage.Controls.Add(this.AppendOMFButton);
            this.MotionPage.Controls.Add(this.MotionBox);
            resources.ApplyResources(this.MotionPage, "MotionPage");
            this.MotionPage.Name = "MotionPage";
            // 
            // AppendOMFButton
            // 
            resources.ApplyResources(this.AppendOMFButton, "AppendOMFButton");
            this.AppendOMFButton.Name = "AppendOMFButton";
            this.AppendOMFButton.UseVisualStyleBackColor = true;
            this.AppendOMFButton.Click += new System.EventHandler(this.AppendOMFButton_Click);
            // 
            // MotionBox
            // 
            this.MotionBox.ContextMenuStrip = this.MotionsContextStrip;
            resources.ApplyResources(this.MotionBox, "MotionBox");
            this.MotionBox.Name = "MotionBox";
            this.MotionBox.ReadOnly = true;
            // 
            // MotionsContextStrip
            // 
            this.MotionsContextStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditToolStrip,
            this.LoadToolStrip,
            this.DeleteToolStrip});
            this.MotionsContextStrip.Name = "MotionsContextStrip";
            resources.ApplyResources(this.MotionsContextStrip, "MotionsContextStrip");
            // 
            // EditToolStrip
            // 
            this.EditToolStrip.Name = "EditToolStrip";
            resources.ApplyResources(this.EditToolStrip, "EditToolStrip");
            this.EditToolStrip.Click += new System.EventHandler(this.EditInOmfEditor);
            // 
            // LoadToolStrip
            // 
            this.LoadToolStrip.Name = "LoadToolStrip";
            resources.ApplyResources(this.LoadToolStrip, "LoadToolStrip");
            this.LoadToolStrip.Click += new System.EventHandler(this.AppendOMFButton_Click);
            // 
            // DeleteToolStrip
            // 
            this.DeleteToolStrip.Name = "DeleteToolStrip";
            resources.ApplyResources(this.DeleteToolStrip, "DeleteToolStrip");
            this.DeleteToolStrip.Click += new System.EventHandler(this.DeleteOmf);
            // 
            // BoneNamesPage
            // 
            this.BoneNamesPage.Controls.Add(this.BoneNamesBox);
            resources.ApplyResources(this.BoneNamesPage, "BoneNamesPage");
            this.BoneNamesPage.Name = "BoneNamesPage";
            // 
            // BoneNamesBox
            // 
            this.BoneNamesBox.DetectUrls = false;
            resources.ApplyResources(this.BoneNamesBox, "BoneNamesBox");
            this.BoneNamesBox.Name = "BoneNamesBox";
            this.BoneNamesBox.ReadOnly = true;
            // 
            // BoneParamsPage
            // 
            resources.ApplyResources(this.BoneParamsPage, "BoneParamsPage");
            this.BoneParamsPage.Controls.Add(this.BoneParamsGroupBox);
            this.BoneParamsPage.Name = "BoneParamsPage";
            // 
            // BoneParamsGroupBox
            // 
            this.BoneParamsGroupBox.Controls.Add(this.RotationLabelEx);
            this.BoneParamsGroupBox.Controls.Add(this.PositionLabelEx);
            this.BoneParamsGroupBox.Controls.Add(this.CenterOfMassLabelEx);
            this.BoneParamsGroupBox.Controls.Add(this.MassLabelEx);
            this.BoneParamsGroupBox.Controls.Add(this.BonesParamsPanel);
            this.BoneParamsGroupBox.Controls.Add(this.MaterialLabelEx);
            this.BoneParamsGroupBox.Controls.Add(this.MaterialTextBoxEx);
            this.BoneParamsGroupBox.Controls.Add(this.ParentBoneTextBoxEx);
            this.BoneParamsGroupBox.Controls.Add(this.BoneNameTextBoxEx);
            this.BoneParamsGroupBox.Controls.Add(this.ParentBoneLabelEx);
            this.BoneParamsGroupBox.Controls.Add(this.BoneNameLabelEx);
            resources.ApplyResources(this.BoneParamsGroupBox, "BoneParamsGroupBox");
            this.BoneParamsGroupBox.Name = "BoneParamsGroupBox";
            this.BoneParamsGroupBox.TabStop = false;
            // 
            // RotationLabelEx
            // 
            resources.ApplyResources(this.RotationLabelEx, "RotationLabelEx");
            this.RotationLabelEx.Name = "RotationLabelEx";
            // 
            // PositionLabelEx
            // 
            resources.ApplyResources(this.PositionLabelEx, "PositionLabelEx");
            this.PositionLabelEx.Name = "PositionLabelEx";
            // 
            // CenterOfMassLabelEx
            // 
            resources.ApplyResources(this.CenterOfMassLabelEx, "CenterOfMassLabelEx");
            this.CenterOfMassLabelEx.Name = "CenterOfMassLabelEx";
            // 
            // MassLabelEx
            // 
            resources.ApplyResources(this.MassLabelEx, "MassLabelEx");
            this.MassLabelEx.Name = "MassLabelEx";
            // 
            // BonesParamsPanel
            // 
            resources.ApplyResources(this.BonesParamsPanel, "BonesParamsPanel");
            this.BonesParamsPanel.Controls.Add(this.RotationZTextBox, 2, 3);
            this.BonesParamsPanel.Controls.Add(this.RotationYTextBox, 1, 3);
            this.BonesParamsPanel.Controls.Add(this.RotationXTextBox, 0, 3);
            this.BonesParamsPanel.Controls.Add(this.PositionZTextBox, 2, 2);
            this.BonesParamsPanel.Controls.Add(this.PositionYTextBox, 1, 2);
            this.BonesParamsPanel.Controls.Add(this.PositionXTextBox, 0, 2);
            this.BonesParamsPanel.Controls.Add(this.CenterOfMassZTextBox, 2, 1);
            this.BonesParamsPanel.Controls.Add(this.CenterOfMassYTextBox, 1, 1);
            this.BonesParamsPanel.Controls.Add(this.CenterOfMassXTextBox, 0, 1);
            this.BonesParamsPanel.Controls.Add(this.MassTextBoxEx, 0, 0);
            this.BonesParamsPanel.Name = "BonesParamsPanel";
            // 
            // RotationZTextBox
            // 
            resources.ApplyResources(this.RotationZTextBox, "RotationZTextBox");
            this.RotationZTextBox.Name = "RotationZTextBox";
            this.RotationZTextBox.Tag = "float";
            // 
            // RotationYTextBox
            // 
            resources.ApplyResources(this.RotationYTextBox, "RotationYTextBox");
            this.RotationYTextBox.Name = "RotationYTextBox";
            this.RotationYTextBox.Tag = "float";
            // 
            // RotationXTextBox
            // 
            resources.ApplyResources(this.RotationXTextBox, "RotationXTextBox");
            this.RotationXTextBox.Name = "RotationXTextBox";
            this.RotationXTextBox.Tag = "float";
            // 
            // PositionZTextBox
            // 
            resources.ApplyResources(this.PositionZTextBox, "PositionZTextBox");
            this.PositionZTextBox.Name = "PositionZTextBox";
            this.PositionZTextBox.Tag = "float";
            // 
            // PositionYTextBox
            // 
            resources.ApplyResources(this.PositionYTextBox, "PositionYTextBox");
            this.PositionYTextBox.Name = "PositionYTextBox";
            this.PositionYTextBox.Tag = "float";
            // 
            // PositionXTextBox
            // 
            resources.ApplyResources(this.PositionXTextBox, "PositionXTextBox");
            this.PositionXTextBox.Name = "PositionXTextBox";
            this.PositionXTextBox.Tag = "float";
            // 
            // CenterOfMassZTextBox
            // 
            resources.ApplyResources(this.CenterOfMassZTextBox, "CenterOfMassZTextBox");
            this.CenterOfMassZTextBox.Name = "CenterOfMassZTextBox";
            this.CenterOfMassZTextBox.Tag = "float";
            // 
            // CenterOfMassYTextBox
            // 
            resources.ApplyResources(this.CenterOfMassYTextBox, "CenterOfMassYTextBox");
            this.CenterOfMassYTextBox.Name = "CenterOfMassYTextBox";
            this.CenterOfMassYTextBox.Tag = "float";
            // 
            // CenterOfMassXTextBox
            // 
            resources.ApplyResources(this.CenterOfMassXTextBox, "CenterOfMassXTextBox");
            this.CenterOfMassXTextBox.Name = "CenterOfMassXTextBox";
            this.CenterOfMassXTextBox.Tag = "float";
            // 
            // MassTextBoxEx
            // 
            resources.ApplyResources(this.MassTextBoxEx, "MassTextBoxEx");
            this.MassTextBoxEx.Name = "MassTextBoxEx";
            this.MassTextBoxEx.Tag = "float";
            // 
            // MaterialLabelEx
            // 
            resources.ApplyResources(this.MaterialLabelEx, "MaterialLabelEx");
            this.MaterialLabelEx.Name = "MaterialLabelEx";
            // 
            // MaterialTextBoxEx
            // 
            resources.ApplyResources(this.MaterialTextBoxEx, "MaterialTextBoxEx");
            this.MaterialTextBoxEx.Name = "MaterialTextBoxEx";
            this.MaterialTextBoxEx.Tag = "string";
            // 
            // ParentBoneTextBoxEx
            // 
            resources.ApplyResources(this.ParentBoneTextBoxEx, "ParentBoneTextBoxEx");
            this.ParentBoneTextBoxEx.Name = "ParentBoneTextBoxEx";
            this.ParentBoneTextBoxEx.ReadOnly = true;
            this.ParentBoneTextBoxEx.Tag = "string";
            // 
            // BoneNameTextBoxEx
            // 
            resources.ApplyResources(this.BoneNameTextBoxEx, "BoneNameTextBoxEx");
            this.BoneNameTextBoxEx.Name = "BoneNameTextBoxEx";
            this.BoneNameTextBoxEx.Tag = "string";
            // 
            // ParentBoneLabelEx
            // 
            resources.ApplyResources(this.ParentBoneLabelEx, "ParentBoneLabelEx");
            this.ParentBoneLabelEx.Name = "ParentBoneLabelEx";
            // 
            // BoneNameLabelEx
            // 
            resources.ApplyResources(this.BoneNameLabelEx, "BoneNameLabelEx");
            this.BoneNameLabelEx.Name = "BoneNameLabelEx";
            // 
            // LodPage
            // 
            this.LodPage.Controls.Add(this.CreateLodButton);
            this.LodPage.Controls.Add(this.LodPathBox);
            this.LodPage.Controls.Add(this.label1);
            resources.ApplyResources(this.LodPage, "LodPage");
            this.LodPage.Name = "LodPage";
            // 
            // CreateLodButton
            // 
            resources.ApplyResources(this.CreateLodButton, "CreateLodButton");
            this.CreateLodButton.Name = "CreateLodButton";
            this.CreateLodButton.UseVisualStyleBackColor = true;
            this.CreateLodButton.Click += new System.EventHandler(this.CreateLodButton_Click);
            // 
            // LodPathBox
            // 
            resources.ApplyResources(this.LodPathBox, "LodPathBox");
            this.LodPathBox.Name = "LodPathBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ViewPage
            // 
            this.ViewPage.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.ViewPage, "ViewPage");
            this.ViewPage.Name = "ViewPage";
            this.ViewPage.Resize += new System.EventHandler(this.ResizeEmbeddedApp);
            // 
            // MenuPanel
            // 
            resources.ApplyResources(this.MenuPanel, "MenuPanel");
            this.MenuPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ToolsMenuItem,
            this.OgfInfo,
            this.settingsToolStripMenuItem,
            this.CurrentFormat,
            this.viewPortToolStripMenuItem});
            this.MenuPanel.Name = "MenuPanel";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadMenuParam,
            this.SaveMenuParam,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator1,
            this.reloadToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            resources.ApplyResources(this.FileMenuItem, "FileMenuItem");
            // 
            // LoadMenuParam
            // 
            this.LoadMenuParam.Name = "LoadMenuParam";
            resources.ApplyResources(this.LoadMenuParam, "LoadMenuParam");
            this.LoadMenuParam.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // SaveMenuParam
            // 
            this.SaveMenuParam.Name = "SaveMenuParam";
            resources.ApplyResources(this.SaveMenuParam, "SaveMenuParam");
            this.SaveMenuParam.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objectToolStripMenuItem,
            this.bonesToolStripMenuItem,
            this.objToolStripMenuItem,
            this.omfToolStripMenuItem,
            this.sklToolStripMenuItem,
            this.sklsToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            resources.ApplyResources(this.exportToolStripMenuItem, "exportToolStripMenuItem");
            // 
            // objectToolStripMenuItem
            // 
            this.objectToolStripMenuItem.Name = "objectToolStripMenuItem";
            resources.ApplyResources(this.objectToolStripMenuItem, "objectToolStripMenuItem");
            this.objectToolStripMenuItem.Click += new System.EventHandler(this.objectToolStripMenuItem_Click);
            // 
            // bonesToolStripMenuItem
            // 
            this.bonesToolStripMenuItem.Name = "bonesToolStripMenuItem";
            resources.ApplyResources(this.bonesToolStripMenuItem, "bonesToolStripMenuItem");
            this.bonesToolStripMenuItem.Click += new System.EventHandler(this.bonesToolStripMenuItem_Click);
            // 
            // objToolStripMenuItem
            // 
            this.objToolStripMenuItem.Name = "objToolStripMenuItem";
            resources.ApplyResources(this.objToolStripMenuItem, "objToolStripMenuItem");
            this.objToolStripMenuItem.Click += new System.EventHandler(this.objToolStripMenuItem_Click);
            // 
            // omfToolStripMenuItem
            // 
            this.omfToolStripMenuItem.Name = "omfToolStripMenuItem";
            resources.ApplyResources(this.omfToolStripMenuItem, "omfToolStripMenuItem");
            this.omfToolStripMenuItem.Click += new System.EventHandler(this.omfToolStripMenuItem_Click);
            // 
            // sklToolStripMenuItem
            // 
            this.sklToolStripMenuItem.Name = "sklToolStripMenuItem";
            resources.ApplyResources(this.sklToolStripMenuItem, "sklToolStripMenuItem");
            this.sklToolStripMenuItem.Click += new System.EventHandler(this.sklToolStripMenuItem_Click);
            // 
            // sklsToolStripMenuItem
            // 
            this.sklsToolStripMenuItem.Name = "sklsToolStripMenuItem";
            resources.ApplyResources(this.sklsToolStripMenuItem, "sklsToolStripMenuItem");
            this.sklsToolStripMenuItem.Click += new System.EventHandler(this.sklsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            resources.ApplyResources(this.reloadToolStripMenuItem, "reloadToolStripMenuItem");
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenInObjectEditor,
            this.importDataFromModelToolStripMenuItem,
            this.recalcNormalsToolStripMenuItem,
            this.removeProgressiveMeshesToolStripMenuItem,
            this.converterToolStripMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            resources.ApplyResources(this.ToolsMenuItem, "ToolsMenuItem");
            // 
            // OpenInObjectEditor
            // 
            this.OpenInObjectEditor.Name = "OpenInObjectEditor";
            resources.ApplyResources(this.OpenInObjectEditor, "OpenInObjectEditor");
            this.OpenInObjectEditor.Click += new System.EventHandler(this.openSkeletonInObjectEditorToolStripMenuItem_Click);
            // 
            // importDataFromModelToolStripMenuItem
            // 
            this.importDataFromModelToolStripMenuItem.Name = "importDataFromModelToolStripMenuItem";
            resources.ApplyResources(this.importDataFromModelToolStripMenuItem, "importDataFromModelToolStripMenuItem");
            this.importDataFromModelToolStripMenuItem.Click += new System.EventHandler(this.importDataFromModelToolStripMenuItem_Click);
            // 
            // recalcNormalsToolStripMenuItem
            // 
            this.recalcNormalsToolStripMenuItem.Name = "recalcNormalsToolStripMenuItem";
            resources.ApplyResources(this.recalcNormalsToolStripMenuItem, "recalcNormalsToolStripMenuItem");
            this.recalcNormalsToolStripMenuItem.Click += new System.EventHandler(this.recalcNormalsToolStripMenuItem_Click);
            // 
            // removeProgressiveMeshesToolStripMenuItem
            // 
            this.removeProgressiveMeshesToolStripMenuItem.Name = "removeProgressiveMeshesToolStripMenuItem";
            resources.ApplyResources(this.removeProgressiveMeshesToolStripMenuItem, "removeProgressiveMeshesToolStripMenuItem");
            this.removeProgressiveMeshesToolStripMenuItem.Click += new System.EventHandler(this.removeProgressiveMeshesToolStripMenuItem_Click);
            // 
            // converterToolStripMenuItem
            // 
            this.converterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nPCCoPToSoCToolStripMenuItem,
            this.nPCSoCToCoPToolStripMenuItem});
            this.converterToolStripMenuItem.Name = "converterToolStripMenuItem";
            resources.ApplyResources(this.converterToolStripMenuItem, "converterToolStripMenuItem");
            // 
            // nPCCoPToSoCToolStripMenuItem
            // 
            this.nPCCoPToSoCToolStripMenuItem.Name = "nPCCoPToSoCToolStripMenuItem";
            resources.ApplyResources(this.nPCCoPToSoCToolStripMenuItem, "nPCCoPToSoCToolStripMenuItem");
            this.nPCCoPToSoCToolStripMenuItem.Click += new System.EventHandler(this.NPC_ToSoC);
            // 
            // nPCSoCToCoPToolStripMenuItem
            // 
            this.nPCSoCToCoPToolStripMenuItem.Name = "nPCSoCToCoPToolStripMenuItem";
            resources.ApplyResources(this.nPCSoCToCoPToolStripMenuItem, "nPCSoCToCoPToolStripMenuItem");
            this.nPCSoCToCoPToolStripMenuItem.Click += new System.EventHandler(this.NPC_ToCoP);
            // 
            // OgfInfo
            // 
            this.OgfInfo.Name = "OgfInfo";
            resources.ApplyResources(this.OgfInfo, "OgfInfo");
            this.OgfInfo.Click += new System.EventHandler(this.oGFInfoToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // CurrentFormat
            // 
            this.CurrentFormat.Name = "CurrentFormat";
            resources.ApplyResources(this.CurrentFormat, "CurrentFormat");
            this.CurrentFormat.Click += new System.EventHandler(this.ChangeModelFormat);
            // 
            // viewPortToolStripMenuItem
            // 
            this.viewPortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem1,
            this.DisableTexturesMenuItem,
            this.disableAlphaToolStripMenuItem,
            this.showBBoxToolStripMenuItem,
            this.openImageFolderToolStripMenuItem});
            this.viewPortToolStripMenuItem.Name = "viewPortToolStripMenuItem";
            resources.ApplyResources(this.viewPortToolStripMenuItem, "viewPortToolStripMenuItem");
            // 
            // reloadToolStripMenuItem1
            // 
            this.reloadToolStripMenuItem1.Name = "reloadToolStripMenuItem1";
            resources.ApplyResources(this.reloadToolStripMenuItem1, "reloadToolStripMenuItem1");
            this.reloadToolStripMenuItem1.Click += new System.EventHandler(this.reloadToolStripMenuItem1_Click);
            // 
            // DisableTexturesMenuItem
            // 
            this.DisableTexturesMenuItem.Name = "DisableTexturesMenuItem";
            resources.ApplyResources(this.DisableTexturesMenuItem, "DisableTexturesMenuItem");
            this.DisableTexturesMenuItem.Click += new System.EventHandler(this.DisableTexturesMenuItem_Click);
            // 
            // disableAlphaToolStripMenuItem
            // 
            this.disableAlphaToolStripMenuItem.Name = "disableAlphaToolStripMenuItem";
            resources.ApplyResources(this.disableAlphaToolStripMenuItem, "disableAlphaToolStripMenuItem");
            this.disableAlphaToolStripMenuItem.Click += new System.EventHandler(this.disableAlphaToolStripMenuItem_Click);
            // 
            // showBBoxToolStripMenuItem
            // 
            this.showBBoxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMainToolStripMenuItem,
            this.showMeshesToolStripMenuItem});
            this.showBBoxToolStripMenuItem.Name = "showBBoxToolStripMenuItem";
            resources.ApplyResources(this.showBBoxToolStripMenuItem, "showBBoxToolStripMenuItem");
            // 
            // showMainToolStripMenuItem
            // 
            this.showMainToolStripMenuItem.Name = "showMainToolStripMenuItem";
            resources.ApplyResources(this.showMainToolStripMenuItem, "showMainToolStripMenuItem");
            this.showMainToolStripMenuItem.Click += new System.EventHandler(this.showBBoxToolStripMenuItem_Click);
            // 
            // showMeshesToolStripMenuItem
            // 
            this.showMeshesToolStripMenuItem.Name = "showMeshesToolStripMenuItem";
            resources.ApplyResources(this.showMeshesToolStripMenuItem, "showMeshesToolStripMenuItem");
            this.showMeshesToolStripMenuItem.Click += new System.EventHandler(this.showMeshesToolStripMenuItem_Click);
            // 
            // openImageFolderToolStripMenuItem
            // 
            this.openImageFolderToolStripMenuItem.Name = "openImageFolderToolStripMenuItem";
            resources.ApplyResources(this.openImageFolderToolStripMenuItem, "openImageFolderToolStripMenuItem");
            this.openImageFolderToolStripMenuItem.Click += new System.EventHandler(this.openImageFolderToolStripMenuItem_Click);
            // 
            // FileLabel
            // 
            this.FileLabel.Name = "FileLabel";
            resources.ApplyResources(this.FileLabel, "FileLabel");
            // 
            // StatusFile
            // 
            this.StatusFile.Name = "StatusFile";
            resources.ApplyResources(this.StatusFile, "StatusFile");
            // 
            // StatusPanel
            // 
            this.StatusPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileLabel,
            this.StatusFile});
            resources.ApplyResources(this.StatusPanel, "StatusPanel");
            this.StatusPanel.Name = "StatusPanel";
            // 
            // BkpCheckBox
            // 
            resources.ApplyResources(this.BkpCheckBox, "BkpCheckBox");
            this.BkpCheckBox.Name = "BkpCheckBox";
            this.BkpCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveAsDialog
            // 
            resources.ApplyResources(this.SaveAsDialog, "SaveAsDialog");
            // 
            // OpenOMFDialog
            // 
            resources.ApplyResources(this.OpenOMFDialog, "OpenOMFDialog");
            this.OpenOMFDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.AppendMotion);
            // 
            // OpenProgramDialog
            // 
            resources.ApplyResources(this.OpenProgramDialog, "OpenProgramDialog");
            // 
            // OpenOGF_DmDialog
            // 
            resources.ApplyResources(this.OpenOGF_DmDialog, "OpenOGF_DmDialog");
            // 
            // SaveSklsDialog
            // 
            resources.ApplyResources(this.SaveSklsDialog, "SaveSklsDialog");
            // 
            // SaveOmfDialog
            // 
            resources.ApplyResources(this.SaveOmfDialog, "SaveOmfDialog");
            // 
            // SaveBonesDialog
            // 
            resources.ApplyResources(this.SaveBonesDialog, "SaveBonesDialog");
            // 
            // SaveObjectDialog
            // 
            resources.ApplyResources(this.SaveObjectDialog, "SaveObjectDialog");
            // 
            // LabelBroken
            // 
            resources.ApplyResources(this.LabelBroken, "LabelBroken");
            this.LabelBroken.Name = "LabelBroken";
            // 
            // SaveObjDialog
            // 
            resources.ApplyResources(this.SaveObjDialog, "SaveObjDialog");
            // 
            // OGF_Editor
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.LabelBroken);
            this.Controls.Add(this.MenuPanel);
            this.Controls.Add(this.BkpCheckBox);
            this.Controls.Add(this.StatusPanel);
            this.Controls.Add(this.TabControl);
            this.MainMenuStrip = this.MenuPanel;
            this.Name = "OGF_Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingForm);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClosedForm);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropCallback);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterCallback);
            this.TabControl.ResumeLayout(false);
            this.TexturesPage.ResumeLayout(false);
            this.TextureContextStrip.ResumeLayout(false);
            this.TexturesGropuBox.ResumeLayout(false);
            this.TexturesGropuBox.PerformLayout();
            this.UserDataPage.ResumeLayout(false);
            this.MotionRefsPage.ResumeLayout(false);
            this.MotionPage.ResumeLayout(false);
            this.MotionsContextStrip.ResumeLayout(false);
            this.BoneNamesPage.ResumeLayout(false);
            this.BoneParamsPage.ResumeLayout(false);
            this.BoneParamsGroupBox.ResumeLayout(false);
            this.BoneParamsGroupBox.PerformLayout();
            this.BonesParamsPanel.ResumeLayout(false);
            this.BonesParamsPanel.PerformLayout();
            this.LodPage.ResumeLayout(false);
            this.LodPage.PerformLayout();
            this.MenuPanel.ResumeLayout(false);
            this.MenuPanel.PerformLayout();
            this.StatusPanel.ResumeLayout(false);
            this.StatusPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog OpenOGFDialog;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage TexturesPage;
        private System.Windows.Forms.TabPage MotionRefsPage;
        private System.Windows.Forms.RichTextBox MotionRefsBox;
        private System.Windows.Forms.TabPage UserDataPage;
        private System.Windows.Forms.RichTextBox UserDataBox;
        private System.Windows.Forms.MenuStrip MenuPanel;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveMenuParam;
        private System.Windows.Forms.ToolStripMenuItem LoadMenuParam;
        private System.Windows.Forms.ToolStripStatusLabel FileLabel;
        private System.Windows.Forms.ToolStripStatusLabel StatusFile;
        private System.Windows.Forms.StatusStrip StatusPanel;
        private System.Windows.Forms.ToolStripMenuItem OgfInfo;
        private System.Windows.Forms.CheckBox BkpCheckBox;
        private System.Windows.Forms.TabPage BoneParamsPage;
        private System.Windows.Forms.TabPage MotionPage;
        private System.Windows.Forms.RichTextBox MotionBox;
        private System.Windows.Forms.TabPage BoneNamesPage;
        private System.Windows.Forms.RichTextBox BoneNamesBox;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveAsDialog;
        private System.Windows.Forms.Button CreateUserdataButton;
        private System.Windows.Forms.Button CreateMotionRefsButton;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.Button AppendOMFButton;
        private System.Windows.Forms.OpenFileDialog OpenOMFDialog;
        private System.Windows.Forms.OpenFileDialog OpenProgramDialog;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenInObjectEditor;
        private System.Windows.Forms.TabPage LodPage;
        private System.Windows.Forms.TextBox LodPathBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CreateLodButton;
        private System.Windows.Forms.ToolStripMenuItem importDataFromModelToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenOGF_DmDialog;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bonesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sklToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sklsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem omfToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveSklsDialog;
        private System.Windows.Forms.SaveFileDialog SaveOmfDialog;
        private System.Windows.Forms.SaveFileDialog SaveBonesDialog;
        private System.Windows.Forms.SaveFileDialog SaveObjectDialog;
        private System.Windows.Forms.Label LabelBroken;
        private System.Windows.Forms.GroupBox BoneParamsGroupBox;
        private System.Windows.Forms.TextBox BoneNameTextBoxEx;
        private System.Windows.Forms.Label ParentBoneLabelEx;
        private System.Windows.Forms.Label BoneNameLabelEx;
        private System.Windows.Forms.TextBox ParentBoneTextBoxEx;
        private System.Windows.Forms.Label MaterialLabelEx;
        private System.Windows.Forms.TextBox MaterialTextBoxEx;
        private System.Windows.Forms.TableLayoutPanel BonesParamsPanel;
        private System.Windows.Forms.TextBox RotationZTextBox;
        private System.Windows.Forms.TextBox RotationYTextBox;
        private System.Windows.Forms.TextBox RotationXTextBox;
        private System.Windows.Forms.TextBox PositionZTextBox;
        private System.Windows.Forms.TextBox PositionYTextBox;
        private System.Windows.Forms.TextBox PositionXTextBox;
        private System.Windows.Forms.TextBox CenterOfMassZTextBox;
        private System.Windows.Forms.TextBox CenterOfMassYTextBox;
        private System.Windows.Forms.TextBox CenterOfMassXTextBox;
        private System.Windows.Forms.TextBox MassTextBoxEx;
        private System.Windows.Forms.Label RotationLabelEx;
        private System.Windows.Forms.Label PositionLabelEx;
        private System.Windows.Forms.Label CenterOfMassLabelEx;
        private System.Windows.Forms.Label MassLabelEx;
        private System.Windows.Forms.GroupBox TexturesGropuBox;
        private System.Windows.Forms.Label TexturesPathLabelEx;
        private System.Windows.Forms.TextBox ShaderTextBoxEx;
        private System.Windows.Forms.TextBox TexturesTextBoxEx;
        private System.Windows.Forms.Label ShaderNameLabelEx;
        private System.Windows.Forms.ToolStripMenuItem CurrentFormat;
        private System.Windows.Forms.Label FaceLabel;
        private System.Windows.Forms.Label VertsLabel;
        private System.Windows.Forms.Button DeleteMesh;
        private System.Windows.Forms.TabPage ViewPage;
        private System.Windows.Forms.ToolStripMenuItem objToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveObjDialog;
        private System.Windows.Forms.Label LinksLabel;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewPortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openImageFolderToolStripMenuItem;
        private System.Windows.Forms.Label LodLabel;
        private System.Windows.Forms.ToolStripMenuItem disableAlphaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem converterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nPCCoPToSoCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nPCSoCToCoPToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip MotionsContextStrip;
        private System.Windows.Forms.ToolStripMenuItem EditToolStrip;
        private System.Windows.Forms.ToolStripMenuItem LoadToolStrip;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStrip;
        private System.Windows.Forms.ContextMenuStrip TextureContextStrip;
        private System.Windows.Forms.ToolStripMenuItem LodMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddMeshesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recalcNormalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisableTexturesMenuItem;
        private System.Windows.Forms.Button MoveMeshButton;
        private System.Windows.Forms.ToolStripMenuItem showBBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeProgressiveMeshesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMeshesToolStripMenuItem;
    }
}


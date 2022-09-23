
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OGF_Editor));
            this.OpenOGFDialog = new System.Windows.Forms.OpenFileDialog();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TexturesPage = new System.Windows.Forms.TabPage();
            this.TexturesGropuBox = new System.Windows.Forms.GroupBox();
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
            this.openSkeletonInObjectEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.motionToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMotionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editInOMFEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMotionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.importDataFromModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeLodButton = new System.Windows.Forms.ToolStripMenuItem();
            this.OgfInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CurrentFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.viewPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.disableAlphaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.button1 = new System.Windows.Forms.Button();
            this.TabControl.SuspendLayout();
            this.TexturesPage.SuspendLayout();
            this.TexturesGropuBox.SuspendLayout();
            this.UserDataPage.SuspendLayout();
            this.MotionRefsPage.SuspendLayout();
            this.MotionPage.SuspendLayout();
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
            this.OpenOGFDialog.Filter = "OGF file|*.ogf";
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.TabControl.Controls.Add(this.TexturesPage);
            this.TabControl.Controls.Add(this.UserDataPage);
            this.TabControl.Controls.Add(this.MotionRefsPage);
            this.TabControl.Controls.Add(this.MotionPage);
            this.TabControl.Controls.Add(this.BoneNamesPage);
            this.TabControl.Controls.Add(this.BoneParamsPage);
            this.TabControl.Controls.Add(this.LodPage);
            this.TabControl.Controls.Add(this.ViewPage);
            this.TabControl.Location = new System.Drawing.Point(12, 27);
            this.TabControl.Multiline = true;
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(496, 310);
            this.TabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.TabControl.TabIndex = 7;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // TexturesPage
            // 
            this.TexturesPage.AutoScroll = true;
            this.TexturesPage.BackColor = System.Drawing.SystemColors.Control;
            this.TexturesPage.Controls.Add(this.TexturesGropuBox);
            this.TexturesPage.Location = new System.Drawing.Point(4, 25);
            this.TexturesPage.Margin = new System.Windows.Forms.Padding(0);
            this.TexturesPage.Name = "TexturesPage";
            this.TexturesPage.Size = new System.Drawing.Size(488, 281);
            this.TexturesPage.TabIndex = 0;
            this.TexturesPage.Text = "Textures";
            // 
            // TexturesGropuBox
            // 
            this.TexturesGropuBox.Controls.Add(this.LodLabel);
            this.TexturesGropuBox.Controls.Add(this.LinksLabel);
            this.TexturesGropuBox.Controls.Add(this.DeleteMesh);
            this.TexturesGropuBox.Controls.Add(this.VertsLabel);
            this.TexturesGropuBox.Controls.Add(this.FaceLabel);
            this.TexturesGropuBox.Controls.Add(this.ShaderNameLabelEx);
            this.TexturesGropuBox.Controls.Add(this.TexturesPathLabelEx);
            this.TexturesGropuBox.Controls.Add(this.ShaderTextBoxEx);
            this.TexturesGropuBox.Controls.Add(this.TexturesTextBoxEx);
            this.TexturesGropuBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.TexturesGropuBox.Location = new System.Drawing.Point(0, 0);
            this.TexturesGropuBox.Name = "TexturesGropuBox";
            this.TexturesGropuBox.Size = new System.Drawing.Size(488, 127);
            this.TexturesGropuBox.TabIndex = 0;
            this.TexturesGropuBox.TabStop = false;
            this.TexturesGropuBox.Text = "Mesh: [-]";
            // 
            // LodLabel
            // 
            this.LodLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LodLabel.AutoSize = true;
            this.LodLabel.Location = new System.Drawing.Point(314, 104);
            this.LodLabel.Name = "LodLabel";
            this.LodLabel.Size = new System.Drawing.Size(36, 13);
            this.LodLabel.TabIndex = 8;
            this.LodLabel.Text = "Lods: ";
            this.LodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LinksLabel
            // 
            this.LinksLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LinksLabel.AutoSize = true;
            this.LinksLabel.Location = new System.Drawing.Point(354, 104);
            this.LinksLabel.Name = "LinksLabel";
            this.LinksLabel.Size = new System.Drawing.Size(38, 13);
            this.LinksLabel.TabIndex = 7;
            this.LinksLabel.Text = "Links: ";
            this.LinksLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DeleteMesh
            // 
            this.DeleteMesh.Location = new System.Drawing.Point(5, 99);
            this.DeleteMesh.Name = "DeleteMesh";
            this.DeleteMesh.Size = new System.Drawing.Size(82, 23);
            this.DeleteMesh.TabIndex = 6;
            this.DeleteMesh.Text = "Delete Mesh";
            this.DeleteMesh.UseVisualStyleBackColor = true;
            // 
            // VertsLabel
            // 
            this.VertsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VertsLabel.AutoSize = true;
            this.VertsLabel.Location = new System.Drawing.Point(395, 104);
            this.VertsLabel.Name = "VertsLabel";
            this.VertsLabel.Size = new System.Drawing.Size(37, 13);
            this.VertsLabel.TabIndex = 5;
            this.VertsLabel.Text = "Verts: ";
            this.VertsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FaceLabel
            // 
            this.FaceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FaceLabel.AutoSize = true;
            this.FaceLabel.Location = new System.Drawing.Point(434, 104);
            this.FaceLabel.Name = "FaceLabel";
            this.FaceLabel.Size = new System.Drawing.Size(42, 13);
            this.FaceLabel.TabIndex = 4;
            this.FaceLabel.Text = "Faces: ";
            this.FaceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ShaderNameLabelEx
            // 
            this.ShaderNameLabelEx.AutoSize = true;
            this.ShaderNameLabelEx.Location = new System.Drawing.Point(3, 60);
            this.ShaderNameLabelEx.Name = "ShaderNameLabelEx";
            this.ShaderNameLabelEx.Size = new System.Drawing.Size(75, 13);
            this.ShaderNameLabelEx.TabIndex = 3;
            this.ShaderNameLabelEx.Text = "Shader Name:";
            // 
            // TexturesPathLabelEx
            // 
            this.TexturesPathLabelEx.AutoSize = true;
            this.TexturesPathLabelEx.Location = new System.Drawing.Point(3, 17);
            this.TexturesPathLabelEx.Name = "TexturesPathLabelEx";
            this.TexturesPathLabelEx.Size = new System.Drawing.Size(76, 13);
            this.TexturesPathLabelEx.TabIndex = 2;
            this.TexturesPathLabelEx.Text = "Textures Path:";
            // 
            // ShaderTextBoxEx
            // 
            this.ShaderTextBoxEx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShaderTextBoxEx.Location = new System.Drawing.Point(6, 76);
            this.ShaderTextBoxEx.Name = "ShaderTextBoxEx";
            this.ShaderTextBoxEx.Size = new System.Drawing.Size(476, 20);
            this.ShaderTextBoxEx.TabIndex = 1;
            // 
            // TexturesTextBoxEx
            // 
            this.TexturesTextBoxEx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TexturesTextBoxEx.Location = new System.Drawing.Point(6, 33);
            this.TexturesTextBoxEx.Name = "TexturesTextBoxEx";
            this.TexturesTextBoxEx.Size = new System.Drawing.Size(476, 20);
            this.TexturesTextBoxEx.TabIndex = 0;
            // 
            // UserDataPage
            // 
            this.UserDataPage.Controls.Add(this.CreateUserdataButton);
            this.UserDataPage.Controls.Add(this.UserDataBox);
            this.UserDataPage.Location = new System.Drawing.Point(4, 25);
            this.UserDataPage.Margin = new System.Windows.Forms.Padding(0);
            this.UserDataPage.Name = "UserDataPage";
            this.UserDataPage.Size = new System.Drawing.Size(488, 281);
            this.UserDataPage.TabIndex = 2;
            this.UserDataPage.Text = "UserData";
            // 
            // CreateUserdataButton
            // 
            this.CreateUserdataButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreateUserdataButton.Location = new System.Drawing.Point(0, 0);
            this.CreateUserdataButton.Name = "CreateUserdataButton";
            this.CreateUserdataButton.Size = new System.Drawing.Size(488, 281);
            this.CreateUserdataButton.TabIndex = 1;
            this.CreateUserdataButton.Text = "Create UserData";
            this.CreateUserdataButton.UseVisualStyleBackColor = true;
            this.CreateUserdataButton.Click += new System.EventHandler(this.CreateUserdataButton_Click);
            // 
            // UserDataBox
            // 
            this.UserDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserDataBox.Location = new System.Drawing.Point(0, 0);
            this.UserDataBox.Name = "UserDataBox";
            this.UserDataBox.Size = new System.Drawing.Size(488, 281);
            this.UserDataBox.TabIndex = 0;
            this.UserDataBox.Text = "";
            this.UserDataBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RichTextBoxImgDefender);
            // 
            // MotionRefsPage
            // 
            this.MotionRefsPage.Controls.Add(this.CreateMotionRefsButton);
            this.MotionRefsPage.Controls.Add(this.MotionRefsBox);
            this.MotionRefsPage.Location = new System.Drawing.Point(4, 25);
            this.MotionRefsPage.Margin = new System.Windows.Forms.Padding(0);
            this.MotionRefsPage.Name = "MotionRefsPage";
            this.MotionRefsPage.Size = new System.Drawing.Size(488, 281);
            this.MotionRefsPage.TabIndex = 1;
            this.MotionRefsPage.Text = "Motion Refs";
            // 
            // CreateMotionRefsButton
            // 
            this.CreateMotionRefsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreateMotionRefsButton.Location = new System.Drawing.Point(0, 0);
            this.CreateMotionRefsButton.Name = "CreateMotionRefsButton";
            this.CreateMotionRefsButton.Size = new System.Drawing.Size(488, 281);
            this.CreateMotionRefsButton.TabIndex = 1;
            this.CreateMotionRefsButton.Text = "Create Motion Refs";
            this.CreateMotionRefsButton.UseVisualStyleBackColor = true;
            this.CreateMotionRefsButton.Click += new System.EventHandler(this.CreateMotionRefsButton_Click);
            // 
            // MotionRefsBox
            // 
            this.MotionRefsBox.DetectUrls = false;
            this.MotionRefsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MotionRefsBox.Location = new System.Drawing.Point(0, 0);
            this.MotionRefsBox.Name = "MotionRefsBox";
            this.MotionRefsBox.Size = new System.Drawing.Size(488, 281);
            this.MotionRefsBox.TabIndex = 0;
            this.MotionRefsBox.Text = "";
            this.MotionRefsBox.WordWrap = false;
            this.MotionRefsBox.TextChanged += new System.EventHandler(this.RichTextBoxTextChanged);
            this.MotionRefsBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RichTextBoxImgDefender);
            // 
            // MotionPage
            // 
            this.MotionPage.Controls.Add(this.AppendOMFButton);
            this.MotionPage.Controls.Add(this.MotionBox);
            this.MotionPage.Location = new System.Drawing.Point(4, 25);
            this.MotionPage.Margin = new System.Windows.Forms.Padding(0);
            this.MotionPage.Name = "MotionPage";
            this.MotionPage.Size = new System.Drawing.Size(488, 281);
            this.MotionPage.TabIndex = 4;
            this.MotionPage.Text = "Motions";
            // 
            // AppendOMFButton
            // 
            this.AppendOMFButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AppendOMFButton.Location = new System.Drawing.Point(0, 0);
            this.AppendOMFButton.Name = "AppendOMFButton";
            this.AppendOMFButton.Size = new System.Drawing.Size(488, 281);
            this.AppendOMFButton.TabIndex = 4;
            this.AppendOMFButton.Text = "Append OMF";
            this.AppendOMFButton.UseVisualStyleBackColor = true;
            this.AppendOMFButton.Click += new System.EventHandler(this.AppendOMFButton_Click);
            // 
            // MotionBox
            // 
            this.MotionBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MotionBox.Location = new System.Drawing.Point(0, 0);
            this.MotionBox.Name = "MotionBox";
            this.MotionBox.ReadOnly = true;
            this.MotionBox.Size = new System.Drawing.Size(488, 281);
            this.MotionBox.TabIndex = 2;
            this.MotionBox.Text = "";
            // 
            // BoneNamesPage
            // 
            this.BoneNamesPage.Controls.Add(this.BoneNamesBox);
            this.BoneNamesPage.Location = new System.Drawing.Point(4, 25);
            this.BoneNamesPage.Margin = new System.Windows.Forms.Padding(0);
            this.BoneNamesPage.Name = "BoneNamesPage";
            this.BoneNamesPage.Size = new System.Drawing.Size(488, 281);
            this.BoneNamesPage.TabIndex = 5;
            this.BoneNamesPage.Text = "Bone Names";
            // 
            // BoneNamesBox
            // 
            this.BoneNamesBox.DetectUrls = false;
            this.BoneNamesBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BoneNamesBox.Location = new System.Drawing.Point(0, 0);
            this.BoneNamesBox.Name = "BoneNamesBox";
            this.BoneNamesBox.ReadOnly = true;
            this.BoneNamesBox.Size = new System.Drawing.Size(488, 281);
            this.BoneNamesBox.TabIndex = 1;
            this.BoneNamesBox.Text = "";
            this.BoneNamesBox.WordWrap = false;
            // 
            // BoneParamsPage
            // 
            this.BoneParamsPage.AutoScroll = true;
            this.BoneParamsPage.Controls.Add(this.BoneParamsGroupBox);
            this.BoneParamsPage.Location = new System.Drawing.Point(4, 25);
            this.BoneParamsPage.Margin = new System.Windows.Forms.Padding(0);
            this.BoneParamsPage.Name = "BoneParamsPage";
            this.BoneParamsPage.Size = new System.Drawing.Size(488, 281);
            this.BoneParamsPage.TabIndex = 3;
            this.BoneParamsPage.Text = "Bone Params";
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
            this.BoneParamsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.BoneParamsGroupBox.Location = new System.Drawing.Point(0, 0);
            this.BoneParamsGroupBox.Name = "BoneParamsGroupBox";
            this.BoneParamsGroupBox.Size = new System.Drawing.Size(488, 213);
            this.BoneParamsGroupBox.TabIndex = 0;
            this.BoneParamsGroupBox.TabStop = false;
            this.BoneParamsGroupBox.Text = "Bone id: [-]";
            // 
            // RotationLabelEx
            // 
            this.RotationLabelEx.AutoSize = true;
            this.RotationLabelEx.Location = new System.Drawing.Point(7, 184);
            this.RotationLabelEx.Name = "RotationLabelEx";
            this.RotationLabelEx.Size = new System.Drawing.Size(50, 13);
            this.RotationLabelEx.TabIndex = 10;
            this.RotationLabelEx.Text = "Rotation:";
            // 
            // PositionLabelEx
            // 
            this.PositionLabelEx.AutoSize = true;
            this.PositionLabelEx.Location = new System.Drawing.Point(7, 157);
            this.PositionLabelEx.Name = "PositionLabelEx";
            this.PositionLabelEx.Size = new System.Drawing.Size(47, 13);
            this.PositionLabelEx.TabIndex = 9;
            this.PositionLabelEx.Text = "Position:";
            // 
            // CenterOfMassLabelEx
            // 
            this.CenterOfMassLabelEx.AutoSize = true;
            this.CenterOfMassLabelEx.Location = new System.Drawing.Point(7, 130);
            this.CenterOfMassLabelEx.Name = "CenterOfMassLabelEx";
            this.CenterOfMassLabelEx.Size = new System.Drawing.Size(81, 13);
            this.CenterOfMassLabelEx.TabIndex = 8;
            this.CenterOfMassLabelEx.Text = "Center of Mass:";
            // 
            // MassLabelEx
            // 
            this.MassLabelEx.AutoSize = true;
            this.MassLabelEx.Location = new System.Drawing.Point(7, 103);
            this.MassLabelEx.Name = "MassLabelEx";
            this.MassLabelEx.Size = new System.Drawing.Size(35, 13);
            this.MassLabelEx.TabIndex = 7;
            this.MassLabelEx.Text = "Mass:";
            // 
            // BonesParamsPanel
            // 
            this.BonesParamsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BonesParamsPanel.ColumnCount = 3;
            this.BonesParamsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.BonesParamsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.BonesParamsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
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
            this.BonesParamsPanel.Location = new System.Drawing.Point(93, 97);
            this.BonesParamsPanel.Name = "BonesParamsPanel";
            this.BonesParamsPanel.RowCount = 4;
            this.BonesParamsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BonesParamsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BonesParamsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BonesParamsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BonesParamsPanel.Size = new System.Drawing.Size(392, 111);
            this.BonesParamsPanel.TabIndex = 6;
            // 
            // RotationZTextBox
            // 
            this.RotationZTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RotationZTextBox.Location = new System.Drawing.Point(263, 84);
            this.RotationZTextBox.Name = "RotationZTextBox";
            this.RotationZTextBox.Size = new System.Drawing.Size(126, 20);
            this.RotationZTextBox.TabIndex = 11;
            // 
            // RotationYTextBox
            // 
            this.RotationYTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RotationYTextBox.Location = new System.Drawing.Point(133, 84);
            this.RotationYTextBox.Name = "RotationYTextBox";
            this.RotationYTextBox.Size = new System.Drawing.Size(124, 20);
            this.RotationYTextBox.TabIndex = 10;
            // 
            // RotationXTextBox
            // 
            this.RotationXTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RotationXTextBox.Location = new System.Drawing.Point(3, 84);
            this.RotationXTextBox.Name = "RotationXTextBox";
            this.RotationXTextBox.Size = new System.Drawing.Size(124, 20);
            this.RotationXTextBox.TabIndex = 9;
            // 
            // PositionZTextBox
            // 
            this.PositionZTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionZTextBox.Location = new System.Drawing.Point(263, 57);
            this.PositionZTextBox.Name = "PositionZTextBox";
            this.PositionZTextBox.Size = new System.Drawing.Size(126, 20);
            this.PositionZTextBox.TabIndex = 8;
            // 
            // PositionYTextBox
            // 
            this.PositionYTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionYTextBox.Location = new System.Drawing.Point(133, 57);
            this.PositionYTextBox.Name = "PositionYTextBox";
            this.PositionYTextBox.Size = new System.Drawing.Size(124, 20);
            this.PositionYTextBox.TabIndex = 7;
            // 
            // PositionXTextBox
            // 
            this.PositionXTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionXTextBox.Location = new System.Drawing.Point(3, 57);
            this.PositionXTextBox.Name = "PositionXTextBox";
            this.PositionXTextBox.Size = new System.Drawing.Size(124, 20);
            this.PositionXTextBox.TabIndex = 6;
            // 
            // CenterOfMassZTextBox
            // 
            this.CenterOfMassZTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CenterOfMassZTextBox.Location = new System.Drawing.Point(263, 30);
            this.CenterOfMassZTextBox.Name = "CenterOfMassZTextBox";
            this.CenterOfMassZTextBox.Size = new System.Drawing.Size(126, 20);
            this.CenterOfMassZTextBox.TabIndex = 5;
            // 
            // CenterOfMassYTextBox
            // 
            this.CenterOfMassYTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CenterOfMassYTextBox.Location = new System.Drawing.Point(133, 30);
            this.CenterOfMassYTextBox.Name = "CenterOfMassYTextBox";
            this.CenterOfMassYTextBox.Size = new System.Drawing.Size(124, 20);
            this.CenterOfMassYTextBox.TabIndex = 4;
            // 
            // CenterOfMassXTextBox
            // 
            this.CenterOfMassXTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CenterOfMassXTextBox.Location = new System.Drawing.Point(3, 30);
            this.CenterOfMassXTextBox.Name = "CenterOfMassXTextBox";
            this.CenterOfMassXTextBox.Size = new System.Drawing.Size(124, 20);
            this.CenterOfMassXTextBox.TabIndex = 3;
            // 
            // MassTextBoxEx
            // 
            this.MassTextBoxEx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MassTextBoxEx.Location = new System.Drawing.Point(3, 3);
            this.MassTextBoxEx.Name = "MassTextBoxEx";
            this.MassTextBoxEx.Size = new System.Drawing.Size(124, 20);
            this.MassTextBoxEx.TabIndex = 0;
            // 
            // MaterialLabelEx
            // 
            this.MaterialLabelEx.AutoSize = true;
            this.MaterialLabelEx.Location = new System.Drawing.Point(7, 76);
            this.MaterialLabelEx.Name = "MaterialLabelEx";
            this.MaterialLabelEx.Size = new System.Drawing.Size(47, 13);
            this.MaterialLabelEx.TabIndex = 5;
            this.MaterialLabelEx.Text = "Material:";
            // 
            // MaterialTextBoxEx
            // 
            this.MaterialTextBoxEx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaterialTextBoxEx.Location = new System.Drawing.Point(96, 73);
            this.MaterialTextBoxEx.Name = "MaterialTextBoxEx";
            this.MaterialTextBoxEx.Size = new System.Drawing.Size(386, 20);
            this.MaterialTextBoxEx.TabIndex = 4;
            // 
            // ParentBoneTextBoxEx
            // 
            this.ParentBoneTextBoxEx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParentBoneTextBoxEx.Location = new System.Drawing.Point(96, 46);
            this.ParentBoneTextBoxEx.Name = "ParentBoneTextBoxEx";
            this.ParentBoneTextBoxEx.ReadOnly = true;
            this.ParentBoneTextBoxEx.Size = new System.Drawing.Size(386, 20);
            this.ParentBoneTextBoxEx.TabIndex = 3;
            // 
            // BoneNameTextBoxEx
            // 
            this.BoneNameTextBoxEx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BoneNameTextBoxEx.Location = new System.Drawing.Point(96, 19);
            this.BoneNameTextBoxEx.Name = "BoneNameTextBoxEx";
            this.BoneNameTextBoxEx.Size = new System.Drawing.Size(386, 20);
            this.BoneNameTextBoxEx.TabIndex = 2;
            // 
            // ParentBoneLabelEx
            // 
            this.ParentBoneLabelEx.AutoSize = true;
            this.ParentBoneLabelEx.Location = new System.Drawing.Point(7, 49);
            this.ParentBoneLabelEx.Name = "ParentBoneLabelEx";
            this.ParentBoneLabelEx.Size = new System.Drawing.Size(68, 13);
            this.ParentBoneLabelEx.TabIndex = 1;
            this.ParentBoneLabelEx.Text = "Parent bone:";
            // 
            // BoneNameLabelEx
            // 
            this.BoneNameLabelEx.AutoSize = true;
            this.BoneNameLabelEx.Location = new System.Drawing.Point(7, 22);
            this.BoneNameLabelEx.Name = "BoneNameLabelEx";
            this.BoneNameLabelEx.Size = new System.Drawing.Size(64, 13);
            this.BoneNameLabelEx.TabIndex = 0;
            this.BoneNameLabelEx.Text = "Bone name:";
            // 
            // LodPage
            // 
            this.LodPage.Controls.Add(this.CreateLodButton);
            this.LodPage.Controls.Add(this.LodPathBox);
            this.LodPage.Controls.Add(this.label1);
            this.LodPage.Location = new System.Drawing.Point(4, 25);
            this.LodPage.Margin = new System.Windows.Forms.Padding(0);
            this.LodPage.Name = "LodPage";
            this.LodPage.Size = new System.Drawing.Size(488, 281);
            this.LodPage.TabIndex = 6;
            this.LodPage.Text = "Lod";
            // 
            // CreateLodButton
            // 
            this.CreateLodButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreateLodButton.Location = new System.Drawing.Point(0, 0);
            this.CreateLodButton.Name = "CreateLodButton";
            this.CreateLodButton.Size = new System.Drawing.Size(488, 281);
            this.CreateLodButton.TabIndex = 2;
            this.CreateLodButton.Text = "Create Lod";
            this.CreateLodButton.UseVisualStyleBackColor = true;
            this.CreateLodButton.Click += new System.EventHandler(this.CreateLodButton_Click);
            // 
            // LodPathBox
            // 
            this.LodPathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LodPathBox.Location = new System.Drawing.Point(3, 23);
            this.LodPathBox.Name = "LodPathBox";
            this.LodPathBox.Size = new System.Drawing.Size(482, 20);
            this.LodPathBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lod path:";
            // 
            // ViewPage
            // 
            this.ViewPage.BackColor = System.Drawing.Color.Transparent;
            this.ViewPage.Location = new System.Drawing.Point(4, 25);
            this.ViewPage.Margin = new System.Windows.Forms.Padding(0);
            this.ViewPage.Name = "ViewPage";
            this.ViewPage.Size = new System.Drawing.Size(488, 281);
            this.ViewPage.TabIndex = 7;
            this.ViewPage.Text = "ViewPort";
            this.ViewPage.Resize += new System.EventHandler(this.ResizeEmbeddedApp);
            // 
            // MenuPanel
            // 
            this.MenuPanel.Dock = System.Windows.Forms.DockStyle.None;
            this.MenuPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ToolsMenuItem,
            this.OgfInfo,
            this.settingsToolStripMenuItem,
            this.CurrentFormat,
            this.viewPortToolStripMenuItem});
            this.MenuPanel.Location = new System.Drawing.Point(0, 2);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(381, 24);
            this.MenuPanel.TabIndex = 8;
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
            this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileMenuItem.Text = "File";
            // 
            // LoadMenuParam
            // 
            this.LoadMenuParam.Name = "LoadMenuParam";
            this.LoadMenuParam.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.LoadMenuParam.Size = new System.Drawing.Size(140, 22);
            this.LoadMenuParam.Text = "Load";
            this.LoadMenuParam.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // SaveMenuParam
            // 
            this.SaveMenuParam.Name = "SaveMenuParam";
            this.SaveMenuParam.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveMenuParam.Size = new System.Drawing.Size(140, 22);
            this.SaveMenuParam.Text = "Save";
            this.SaveMenuParam.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
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
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // objectToolStripMenuItem
            // 
            this.objectToolStripMenuItem.Name = "objectToolStripMenuItem";
            this.objectToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.objectToolStripMenuItem.Text = "Object";
            this.objectToolStripMenuItem.Click += new System.EventHandler(this.objectToolStripMenuItem_Click);
            // 
            // bonesToolStripMenuItem
            // 
            this.bonesToolStripMenuItem.Name = "bonesToolStripMenuItem";
            this.bonesToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.bonesToolStripMenuItem.Text = "Bones";
            this.bonesToolStripMenuItem.Click += new System.EventHandler(this.bonesToolStripMenuItem_Click);
            // 
            // objToolStripMenuItem
            // 
            this.objToolStripMenuItem.Name = "objToolStripMenuItem";
            this.objToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.objToolStripMenuItem.Text = "Obj";
            this.objToolStripMenuItem.Click += new System.EventHandler(this.objToolStripMenuItem_Click);
            // 
            // omfToolStripMenuItem
            // 
            this.omfToolStripMenuItem.Name = "omfToolStripMenuItem";
            this.omfToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.omfToolStripMenuItem.Text = "OMF";
            this.omfToolStripMenuItem.Click += new System.EventHandler(this.omfToolStripMenuItem_Click);
            // 
            // sklToolStripMenuItem
            // 
            this.sklToolStripMenuItem.Name = "sklToolStripMenuItem";
            this.sklToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.sklToolStripMenuItem.Text = "Skl";
            this.sklToolStripMenuItem.Click += new System.EventHandler(this.sklToolStripMenuItem_Click);
            // 
            // sklsToolStripMenuItem
            // 
            this.sklsToolStripMenuItem.Name = "sklsToolStripMenuItem";
            this.sklsToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.sklsToolStripMenuItem.Text = "Skls";
            this.sklsToolStripMenuItem.Click += new System.EventHandler(this.sklsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(137, 6);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSkeletonInObjectEditorToolStripMenuItem,
            this.motionToolsToolStripMenuItem,
            this.importDataFromModelToolStripMenuItem,
            this.ChangeLodButton});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(46, 20);
            this.ToolsMenuItem.Text = "Tools";
            this.ToolsMenuItem.Click += new System.EventHandler(this.ToolsClick);
            // 
            // openSkeletonInObjectEditorToolStripMenuItem
            // 
            this.openSkeletonInObjectEditorToolStripMenuItem.Name = "openSkeletonInObjectEditorToolStripMenuItem";
            this.openSkeletonInObjectEditorToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.openSkeletonInObjectEditorToolStripMenuItem.Text = "Open skeleton in Object Editor";
            this.openSkeletonInObjectEditorToolStripMenuItem.Click += new System.EventHandler(this.openSkeletonInObjectEditorToolStripMenuItem_Click);
            // 
            // motionToolsToolStripMenuItem
            // 
            this.motionToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteMotionsToolStripMenuItem,
            this.editInOMFEditorToolStripMenuItem,
            this.replaceMotionsToolStripMenuItem1});
            this.motionToolsToolStripMenuItem.Name = "motionToolsToolStripMenuItem";
            this.motionToolsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.motionToolsToolStripMenuItem.Text = "Motion tools";
            // 
            // deleteMotionsToolStripMenuItem
            // 
            this.deleteMotionsToolStripMenuItem.Name = "deleteMotionsToolStripMenuItem";
            this.deleteMotionsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.deleteMotionsToolStripMenuItem.Text = "Delete motions";
            this.deleteMotionsToolStripMenuItem.Click += new System.EventHandler(this.deleteChunkToolStripMenuItem_Click);
            // 
            // editInOMFEditorToolStripMenuItem
            // 
            this.editInOMFEditorToolStripMenuItem.Name = "editInOMFEditorToolStripMenuItem";
            this.editInOMFEditorToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.editInOMFEditorToolStripMenuItem.Text = "Edit in OMF Editor";
            this.editInOMFEditorToolStripMenuItem.Click += new System.EventHandler(this.editImOMFEditorToolStripMenuItem_Click);
            // 
            // replaceMotionsToolStripMenuItem1
            // 
            this.replaceMotionsToolStripMenuItem1.Name = "replaceMotionsToolStripMenuItem1";
            this.replaceMotionsToolStripMenuItem1.Size = new System.Drawing.Size(170, 22);
            this.replaceMotionsToolStripMenuItem1.Text = "Replace motions";
            this.replaceMotionsToolStripMenuItem1.Click += new System.EventHandler(this.AppendOMFButton_Click);
            // 
            // importDataFromModelToolStripMenuItem
            // 
            this.importDataFromModelToolStripMenuItem.Name = "importDataFromModelToolStripMenuItem";
            this.importDataFromModelToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.importDataFromModelToolStripMenuItem.Text = "Import OGF Params";
            this.importDataFromModelToolStripMenuItem.Click += new System.EventHandler(this.importDataFromModelToolStripMenuItem_Click);
            // 
            // ChangeLodButton
            // 
            this.ChangeLodButton.Name = "ChangeLodButton";
            this.ChangeLodButton.Size = new System.Drawing.Size(235, 22);
            this.ChangeLodButton.Text = "Change lod";
            this.ChangeLodButton.Click += new System.EventHandler(this.changeLodToolStripMenuItem_Click);
            // 
            // OgfInfo
            // 
            this.OgfInfo.Name = "OgfInfo";
            this.OgfInfo.Size = new System.Drawing.Size(66, 20);
            this.OgfInfo.Text = "OGF Info";
            this.OgfInfo.Click += new System.EventHandler(this.oGFInfoToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // CurrentFormat
            // 
            this.CurrentFormat.Name = "CurrentFormat";
            this.CurrentFormat.Size = new System.Drawing.Size(97, 20);
            this.CurrentFormat.Text = "Model Format:";
            this.CurrentFormat.Click += new System.EventHandler(this.ChangeRefsFormat);
            // 
            // viewPortToolStripMenuItem
            // 
            this.viewPortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadToolStripMenuItem1,
            this.disableAlphaToolStripMenuItem,
            this.openImageFolderToolStripMenuItem});
            this.viewPortToolStripMenuItem.Name = "viewPortToolStripMenuItem";
            this.viewPortToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.viewPortToolStripMenuItem.Text = "ViewPort";
            // 
            // reloadToolStripMenuItem1
            // 
            this.reloadToolStripMenuItem1.Name = "reloadToolStripMenuItem1";
            this.reloadToolStripMenuItem1.Size = new System.Drawing.Size(173, 22);
            this.reloadToolStripMenuItem1.Text = "Reload";
            this.reloadToolStripMenuItem1.Click += new System.EventHandler(this.reloadToolStripMenuItem1_Click);
            // 
            // disableAlphaToolStripMenuItem
            // 
            this.disableAlphaToolStripMenuItem.Name = "disableAlphaToolStripMenuItem";
            this.disableAlphaToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.disableAlphaToolStripMenuItem.Text = "Disable Alpha";
            this.disableAlphaToolStripMenuItem.Click += new System.EventHandler(this.disableAlphaToolStripMenuItem_Click);
            // 
            // openImageFolderToolStripMenuItem
            // 
            this.openImageFolderToolStripMenuItem.Name = "openImageFolderToolStripMenuItem";
            this.openImageFolderToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.openImageFolderToolStripMenuItem.Text = "Open image folder";
            this.openImageFolderToolStripMenuItem.Click += new System.EventHandler(this.openImageFolderToolStripMenuItem_Click);
            // 
            // FileLabel
            // 
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(28, 17);
            this.FileLabel.Text = "File:";
            // 
            // StatusFile
            // 
            this.StatusFile.Name = "StatusFile";
            this.StatusFile.Size = new System.Drawing.Size(12, 17);
            this.StatusFile.Text = "-";
            // 
            // StatusPanel
            // 
            this.StatusPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileLabel,
            this.StatusFile});
            this.StatusPanel.Location = new System.Drawing.Point(0, 340);
            this.StatusPanel.Name = "StatusPanel";
            this.StatusPanel.Size = new System.Drawing.Size(520, 22);
            this.StatusPanel.TabIndex = 30;
            this.StatusPanel.Text = "statusStrip1";
            // 
            // BkpCheckBox
            // 
            this.BkpCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BkpCheckBox.AutoSize = true;
            this.BkpCheckBox.Location = new System.Drawing.Point(417, 7);
            this.BkpCheckBox.Name = "BkpCheckBox";
            this.BkpCheckBox.Size = new System.Drawing.Size(96, 17);
            this.BkpCheckBox.TabIndex = 31;
            this.BkpCheckBox.Text = "Create backup";
            this.BkpCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveAsDialog
            // 
            this.SaveAsDialog.Filter = "OGF file|*.ogf|Object file|*.object|Bones file|*.bones|Skl file|*.skl|Skls file|*" +
    ".skls|OMF file|*.omf";
            // 
            // OpenOMFDialog
            // 
            this.OpenOMFDialog.Filter = "OMF file|*.omf";
            this.OpenOMFDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.AppendMotion);
            // 
            // OpenProgramDialog
            // 
            this.OpenProgramDialog.Filter = "Program|*.exe";
            // 
            // OpenOGF_DmDialog
            // 
            this.OpenOGF_DmDialog.Filter = "OGF/DM file|*.ogf;*.dm";
            // 
            // SaveSklsDialog
            // 
            this.SaveSklsDialog.Filter = "Skls file|*.skls";
            // 
            // SaveOmfDialog
            // 
            this.SaveOmfDialog.Filter = "OMF file|*.omf";
            // 
            // SaveBonesDialog
            // 
            this.SaveBonesDialog.Filter = "Bones file|*.bones";
            // 
            // SaveObjectDialog
            // 
            this.SaveObjectDialog.Filter = "Object file|*.object";
            // 
            // LabelBroken
            // 
            this.LabelBroken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelBroken.AutoSize = true;
            this.LabelBroken.Location = new System.Drawing.Point(431, 344);
            this.LabelBroken.Name = "LabelBroken";
            this.LabelBroken.Size = new System.Drawing.Size(67, 13);
            this.LabelBroken.TabIndex = 32;
            this.LabelBroken.Text = "Broken type:";
            // 
            // SaveObjDialog
            // 
            this.SaveObjDialog.Filter = "Obj file|*.obj";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(188, 338);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OGF_Editor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(520, 362);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LabelBroken);
            this.Controls.Add(this.MenuPanel);
            this.Controls.Add(this.BkpCheckBox);
            this.Controls.Add(this.StatusPanel);
            this.Controls.Add(this.TabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuPanel;
            this.MinimumSize = new System.Drawing.Size(536, 251);
            this.Name = "OGF_Editor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OGF Params Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingForm);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClosedForm);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDropCallback);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterCallback);
            this.TabControl.ResumeLayout(false);
            this.TexturesPage.ResumeLayout(false);
            this.TexturesGropuBox.ResumeLayout(false);
            this.TexturesGropuBox.PerformLayout();
            this.UserDataPage.ResumeLayout(false);
            this.MotionRefsPage.ResumeLayout(false);
            this.MotionPage.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem openSkeletonInObjectEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem motionToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMotionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editInOMFEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceMotionsToolStripMenuItem1;
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
        private System.Windows.Forms.ToolStripMenuItem ChangeLodButton;
        private System.Windows.Forms.Label LodLabel;
        private System.Windows.Forms.ToolStripMenuItem disableAlphaToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}


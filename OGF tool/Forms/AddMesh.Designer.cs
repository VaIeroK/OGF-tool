namespace OGF_tool
{
    partial class AddMesh
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMesh));
            this.MeshPanel = new System.Windows.Forms.Panel();
            this.MeshGroupBox = new System.Windows.Forms.GroupBox();
            this.Reassign_Label = new System.Windows.Forms.Label();
            this.OldBone_Textbox = new System.Windows.Forms.TextBox();
            this.Shader_Textbox = new System.Windows.Forms.TextBox();
            this.Texture_Textbox = new System.Windows.Forms.TextBox();
            this.Shader_Label = new System.Windows.Forms.Label();
            this.AddMeshButton = new System.Windows.Forms.Button();
            this.Bone_ComboBox = new System.Windows.Forms.ComboBox();
            this.Bone_Label = new System.Windows.Forms.Label();
            this.Texture_Label = new System.Windows.Forms.Label();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.MeshPanel.SuspendLayout();
            this.MeshGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MeshPanel
            // 
            this.MeshPanel.AutoScroll = true;
            this.MeshPanel.Controls.Add(this.MeshGroupBox);
            this.MeshPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MeshPanel.Location = new System.Drawing.Point(3, 3);
            this.MeshPanel.Name = "MeshPanel";
            this.MeshPanel.Padding = new System.Windows.Forms.Padding(3);
            this.MeshPanel.Size = new System.Drawing.Size(442, 307);
            this.MeshPanel.TabIndex = 0;
            // 
            // MeshGroupBox
            // 
            this.MeshGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MeshGroupBox.Controls.Add(this.Reassign_Label);
            this.MeshGroupBox.Controls.Add(this.OldBone_Textbox);
            this.MeshGroupBox.Controls.Add(this.Shader_Textbox);
            this.MeshGroupBox.Controls.Add(this.Texture_Textbox);
            this.MeshGroupBox.Controls.Add(this.Shader_Label);
            this.MeshGroupBox.Controls.Add(this.AddMeshButton);
            this.MeshGroupBox.Controls.Add(this.Bone_ComboBox);
            this.MeshGroupBox.Controls.Add(this.Bone_Label);
            this.MeshGroupBox.Controls.Add(this.Texture_Label);
            this.MeshGroupBox.Location = new System.Drawing.Point(3, 3);
            this.MeshGroupBox.Name = "MeshGroupBox";
            this.MeshGroupBox.Size = new System.Drawing.Size(436, 128);
            this.MeshGroupBox.TabIndex = 0;
            this.MeshGroupBox.TabStop = false;
            this.MeshGroupBox.Text = "Mesh:";
            // 
            // Reassign_Label
            // 
            this.Reassign_Label.AutoSize = true;
            this.Reassign_Label.Location = new System.Drawing.Point(240, 71);
            this.Reassign_Label.Name = "Reassign_Label";
            this.Reassign_Label.Size = new System.Drawing.Size(66, 13);
            this.Reassign_Label.TabIndex = 14;
            this.Reassign_Label.Text = "Reassign to:";
            this.Reassign_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OldBone_Textbox
            // 
            this.OldBone_Textbox.Location = new System.Drawing.Point(117, 68);
            this.OldBone_Textbox.Name = "OldBone_Textbox";
            this.OldBone_Textbox.ReadOnly = true;
            this.OldBone_Textbox.Size = new System.Drawing.Size(117, 20);
            this.OldBone_Textbox.TabIndex = 13;
            // 
            // Shader_Textbox
            // 
            this.Shader_Textbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Shader_Textbox.Location = new System.Drawing.Point(62, 42);
            this.Shader_Textbox.Name = "Shader_Textbox";
            this.Shader_Textbox.ReadOnly = true;
            this.Shader_Textbox.Size = new System.Drawing.Size(368, 20);
            this.Shader_Textbox.TabIndex = 12;
            // 
            // Texture_Textbox
            // 
            this.Texture_Textbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Texture_Textbox.Location = new System.Drawing.Point(62, 16);
            this.Texture_Textbox.Name = "Texture_Textbox";
            this.Texture_Textbox.ReadOnly = true;
            this.Texture_Textbox.Size = new System.Drawing.Size(368, 20);
            this.Texture_Textbox.TabIndex = 11;
            // 
            // Shader_Label
            // 
            this.Shader_Label.AutoSize = true;
            this.Shader_Label.Location = new System.Drawing.Point(10, 45);
            this.Shader_Label.Name = "Shader_Label";
            this.Shader_Label.Size = new System.Drawing.Size(44, 13);
            this.Shader_Label.TabIndex = 10;
            this.Shader_Label.Text = "Shader:";
            // 
            // AddMeshButton
            // 
            this.AddMeshButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddMeshButton.BackColor = System.Drawing.SystemColors.Control;
            this.AddMeshButton.Location = new System.Drawing.Point(5, 94);
            this.AddMeshButton.Name = "AddMeshButton";
            this.AddMeshButton.Size = new System.Drawing.Size(426, 31);
            this.AddMeshButton.TabIndex = 9;
            this.AddMeshButton.Text = "Add Mesh";
            this.AddMeshButton.UseVisualStyleBackColor = false;
            // 
            // Bone_ComboBox
            // 
            this.Bone_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bone_ComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.Bone_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bone_ComboBox.FormattingEnabled = true;
            this.Bone_ComboBox.Location = new System.Drawing.Point(309, 68);
            this.Bone_ComboBox.Name = "Bone_ComboBox";
            this.Bone_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.Bone_ComboBox.TabIndex = 5;
            // 
            // Bone_Label
            // 
            this.Bone_Label.AutoSize = true;
            this.Bone_Label.Location = new System.Drawing.Point(10, 71);
            this.Bone_Label.Name = "Bone_Label";
            this.Bone_Label.Size = new System.Drawing.Size(101, 13);
            this.Bone_Label.TabIndex = 1;
            this.Bone_Label.Text = "Bone 1 assigned to:";
            // 
            // Texture_Label
            // 
            this.Texture_Label.AutoSize = true;
            this.Texture_Label.Location = new System.Drawing.Point(10, 19);
            this.Texture_Label.Name = "Texture_Label";
            this.Texture_Label.Size = new System.Drawing.Size(46, 13);
            this.Texture_Label.TabIndex = 0;
            this.Texture_Label.Text = "Texture:";
            // 
            // ApplyButton
            // 
            this.ApplyButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ApplyButton.Location = new System.Drawing.Point(3, 316);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(442, 28);
            this.ApplyButton.TabIndex = 1;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // AddMesh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(448, 347);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.MeshPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddMesh";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mesh selector";
            this.MeshPanel.ResumeLayout(false);
            this.MeshGroupBox.ResumeLayout(false);
            this.MeshGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MeshPanel;
        private System.Windows.Forms.GroupBox MeshGroupBox;
        private System.Windows.Forms.Label Texture_Label;
        private System.Windows.Forms.Label Bone_Label;
        private System.Windows.Forms.ComboBox Bone_ComboBox;
        private System.Windows.Forms.Button AddMeshButton;
        private System.Windows.Forms.Label Shader_Label;
        private System.Windows.Forms.TextBox Shader_Textbox;
        private System.Windows.Forms.TextBox Texture_Textbox;
        private System.Windows.Forms.Label Reassign_Label;
        private System.Windows.Forms.TextBox OldBone_Textbox;
        private System.Windows.Forms.Button ApplyButton;
    }
}
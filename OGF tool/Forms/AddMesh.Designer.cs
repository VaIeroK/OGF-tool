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
            this.TextureNameLabel = new System.Windows.Forms.Label();
            this.Bone1_Label = new System.Windows.Forms.Label();
            this.Bone2_Label = new System.Windows.Forms.Label();
            this.Bone3_Label = new System.Windows.Forms.Label();
            this.Bone4_Label = new System.Windows.Forms.Label();
            this.Bone1_ComboBox = new System.Windows.Forms.ComboBox();
            this.Bone2_ComboBox = new System.Windows.Forms.ComboBox();
            this.Bone3_ComboBox = new System.Windows.Forms.ComboBox();
            this.Bone4_ComboBox = new System.Windows.Forms.ComboBox();
            this.AddMeshButton = new System.Windows.Forms.Button();
            this.MeshPanel.SuspendLayout();
            this.MeshGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MeshPanel
            // 
            this.MeshPanel.AutoScroll = true;
            this.MeshPanel.Controls.Add(this.MeshGroupBox);
            this.MeshPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MeshPanel.Location = new System.Drawing.Point(3, 3);
            this.MeshPanel.Name = "MeshPanel";
            this.MeshPanel.Padding = new System.Windows.Forms.Padding(3);
            this.MeshPanel.Size = new System.Drawing.Size(442, 307);
            this.MeshPanel.TabIndex = 0;
            // 
            // MeshGroupBox
            // 
            this.MeshGroupBox.Controls.Add(this.AddMeshButton);
            this.MeshGroupBox.Controls.Add(this.Bone4_ComboBox);
            this.MeshGroupBox.Controls.Add(this.Bone3_ComboBox);
            this.MeshGroupBox.Controls.Add(this.Bone2_ComboBox);
            this.MeshGroupBox.Controls.Add(this.Bone1_ComboBox);
            this.MeshGroupBox.Controls.Add(this.Bone4_Label);
            this.MeshGroupBox.Controls.Add(this.Bone3_Label);
            this.MeshGroupBox.Controls.Add(this.Bone2_Label);
            this.MeshGroupBox.Controls.Add(this.Bone1_Label);
            this.MeshGroupBox.Controls.Add(this.TextureNameLabel);
            this.MeshGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.MeshGroupBox.Location = new System.Drawing.Point(3, 3);
            this.MeshGroupBox.Name = "MeshGroupBox";
            this.MeshGroupBox.Size = new System.Drawing.Size(436, 178);
            this.MeshGroupBox.TabIndex = 0;
            this.MeshGroupBox.TabStop = false;
            this.MeshGroupBox.Text = "Mesh:";
            // 
            // TextureNameLabel
            // 
            this.TextureNameLabel.AutoSize = true;
            this.TextureNameLabel.Location = new System.Drawing.Point(10, 16);
            this.TextureNameLabel.Name = "TextureNameLabel";
            this.TextureNameLabel.Size = new System.Drawing.Size(72, 13);
            this.TextureNameLabel.TabIndex = 0;
            this.TextureNameLabel.Text = "Texture name";
            // 
            // Bone1_Label
            // 
            this.Bone1_Label.AutoSize = true;
            this.Bone1_Label.Location = new System.Drawing.Point(10, 37);
            this.Bone1_Label.Name = "Bone1_Label";
            this.Bone1_Label.Size = new System.Drawing.Size(44, 13);
            this.Bone1_Label.TabIndex = 1;
            this.Bone1_Label.Text = "Bone 1:";
            // 
            // Bone2_Label
            // 
            this.Bone2_Label.AutoSize = true;
            this.Bone2_Label.Location = new System.Drawing.Point(10, 64);
            this.Bone2_Label.Name = "Bone2_Label";
            this.Bone2_Label.Size = new System.Drawing.Size(44, 13);
            this.Bone2_Label.TabIndex = 2;
            this.Bone2_Label.Text = "Bone 2:";
            // 
            // Bone3_Label
            // 
            this.Bone3_Label.AutoSize = true;
            this.Bone3_Label.Location = new System.Drawing.Point(10, 91);
            this.Bone3_Label.Name = "Bone3_Label";
            this.Bone3_Label.Size = new System.Drawing.Size(44, 13);
            this.Bone3_Label.TabIndex = 3;
            this.Bone3_Label.Text = "Bone 3:";
            // 
            // Bone4_Label
            // 
            this.Bone4_Label.AutoSize = true;
            this.Bone4_Label.Location = new System.Drawing.Point(10, 118);
            this.Bone4_Label.Name = "Bone4_Label";
            this.Bone4_Label.Size = new System.Drawing.Size(44, 13);
            this.Bone4_Label.TabIndex = 4;
            this.Bone4_Label.Text = "Bone 4:";
            // 
            // Bone1_ComboBox
            // 
            this.Bone1_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bone1_ComboBox.FormattingEnabled = true;
            this.Bone1_ComboBox.Location = new System.Drawing.Point(309, 34);
            this.Bone1_ComboBox.Name = "Bone1_ComboBox";
            this.Bone1_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.Bone1_ComboBox.TabIndex = 5;
            // 
            // Bone2_ComboBox
            // 
            this.Bone2_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bone2_ComboBox.FormattingEnabled = true;
            this.Bone2_ComboBox.Location = new System.Drawing.Point(309, 61);
            this.Bone2_ComboBox.Name = "Bone2_ComboBox";
            this.Bone2_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.Bone2_ComboBox.TabIndex = 6;
            // 
            // Bone3_ComboBox
            // 
            this.Bone3_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bone3_ComboBox.FormattingEnabled = true;
            this.Bone3_ComboBox.Location = new System.Drawing.Point(309, 88);
            this.Bone3_ComboBox.Name = "Bone3_ComboBox";
            this.Bone3_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.Bone3_ComboBox.TabIndex = 7;
            // 
            // Bone4_ComboBox
            // 
            this.Bone4_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bone4_ComboBox.FormattingEnabled = true;
            this.Bone4_ComboBox.Location = new System.Drawing.Point(309, 115);
            this.Bone4_ComboBox.Name = "Bone4_ComboBox";
            this.Bone4_ComboBox.Size = new System.Drawing.Size(121, 21);
            this.Bone4_ComboBox.TabIndex = 8;
            // 
            // AddMeshButton
            // 
            this.AddMeshButton.Location = new System.Drawing.Point(6, 142);
            this.AddMeshButton.Name = "AddMeshButton";
            this.AddMeshButton.Size = new System.Drawing.Size(424, 31);
            this.AddMeshButton.TabIndex = 9;
            this.AddMeshButton.Text = "Add Mesh";
            this.AddMeshButton.UseVisualStyleBackColor = true;
            // 
            // AddMesh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(448, 313);
            this.Controls.Add(this.MeshPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddMesh";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddMesh";
            this.MeshPanel.ResumeLayout(false);
            this.MeshGroupBox.ResumeLayout(false);
            this.MeshGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MeshPanel;
        private System.Windows.Forms.GroupBox MeshGroupBox;
        private System.Windows.Forms.Label TextureNameLabel;
        private System.Windows.Forms.Label Bone4_Label;
        private System.Windows.Forms.Label Bone3_Label;
        private System.Windows.Forms.Label Bone2_Label;
        private System.Windows.Forms.Label Bone1_Label;
        private System.Windows.Forms.ComboBox Bone4_ComboBox;
        private System.Windows.Forms.ComboBox Bone3_ComboBox;
        private System.Windows.Forms.ComboBox Bone2_ComboBox;
        private System.Windows.Forms.ComboBox Bone1_ComboBox;
        private System.Windows.Forms.Button AddMeshButton;
    }
}
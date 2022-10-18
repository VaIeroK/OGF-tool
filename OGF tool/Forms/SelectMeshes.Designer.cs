namespace OGF_tool
{
    partial class SelectMeshes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectMeshes));
            this.MeshPanel = new System.Windows.Forms.Panel();
            this.AppyButton = new System.Windows.Forms.Button();
            this.MeshCheckBox = new System.Windows.Forms.CheckBox();
            this.MeshPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MeshPanel
            // 
            this.MeshPanel.Controls.Add(this.MeshCheckBox);
            this.MeshPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MeshPanel.Location = new System.Drawing.Point(0, 0);
            this.MeshPanel.Name = "MeshPanel";
            this.MeshPanel.Size = new System.Drawing.Size(539, 44);
            this.MeshPanel.TabIndex = 0;
            // 
            // AppyButton
            // 
            this.AppyButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AppyButton.Location = new System.Drawing.Point(0, 39);
            this.AppyButton.Name = "AppyButton";
            this.AppyButton.Size = new System.Drawing.Size(539, 30);
            this.AppyButton.TabIndex = 1;
            this.AppyButton.Text = "Apply";
            this.AppyButton.UseVisualStyleBackColor = true;
            this.AppyButton.Click += new System.EventHandler(this.AppyButton_Click);
            // 
            // MeshCheckBox
            // 
            this.MeshCheckBox.AutoSize = true;
            this.MeshCheckBox.Checked = true;
            this.MeshCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MeshCheckBox.Location = new System.Drawing.Point(13, 13);
            this.MeshCheckBox.Name = "MeshCheckBox";
            this.MeshCheckBox.Size = new System.Drawing.Size(55, 17);
            this.MeshCheckBox.TabIndex = 0;
            this.MeshCheckBox.Text = "Mesh:";
            this.MeshCheckBox.UseVisualStyleBackColor = true;
            // 
            // SelectMeshes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 69);
            this.Controls.Add(this.AppyButton);
            this.Controls.Add(this.MeshPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectMeshes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Meshes";
            this.MeshPanel.ResumeLayout(false);
            this.MeshPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MeshPanel;
        private System.Windows.Forms.CheckBox MeshCheckBox;
        private System.Windows.Forms.Button AppyButton;
    }
}
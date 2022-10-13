namespace OGF_tool
{
    partial class ImportParams
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportParams));
            this.TexturesChbx = new System.Windows.Forms.CheckBox();
            this.UserdataChbx = new System.Windows.Forms.CheckBox();
            this.LodPathChbx = new System.Windows.Forms.CheckBox();
            this.MotionRefsChbx = new System.Windows.Forms.CheckBox();
            this.MotionsChbx = new System.Windows.Forms.CheckBox();
            this.IKdataChbx = new System.Windows.Forms.CheckBox();
            this.RemoveChbx = new System.Windows.Forms.CheckBox();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TexturesChbx
            // 
            this.TexturesChbx.AutoSize = true;
            this.TexturesChbx.Location = new System.Drawing.Point(13, 13);
            this.TexturesChbx.Name = "TexturesChbx";
            this.TexturesChbx.Size = new System.Drawing.Size(130, 17);
            this.TexturesChbx.TabIndex = 0;
            this.TexturesChbx.Tag = "1";
            this.TexturesChbx.Text = "Textures and Shaders";
            this.TexturesChbx.UseVisualStyleBackColor = true;
            // 
            // UserdataChbx
            // 
            this.UserdataChbx.AutoSize = true;
            this.UserdataChbx.Location = new System.Drawing.Point(13, 83);
            this.UserdataChbx.Name = "UserdataChbx";
            this.UserdataChbx.Size = new System.Drawing.Size(69, 17);
            this.UserdataChbx.TabIndex = 1;
            this.UserdataChbx.Tag = "4";
            this.UserdataChbx.Text = "Userdata";
            this.UserdataChbx.UseVisualStyleBackColor = true;
            // 
            // LodPathChbx
            // 
            this.LodPathChbx.AutoSize = true;
            this.LodPathChbx.Location = new System.Drawing.Point(13, 107);
            this.LodPathChbx.Name = "LodPathChbx";
            this.LodPathChbx.Size = new System.Drawing.Size(68, 17);
            this.LodPathChbx.TabIndex = 2;
            this.LodPathChbx.Tag = "5";
            this.LodPathChbx.Text = "Lod path";
            this.LodPathChbx.UseVisualStyleBackColor = true;
            // 
            // MotionRefsChbx
            // 
            this.MotionRefsChbx.AutoSize = true;
            this.MotionRefsChbx.Location = new System.Drawing.Point(13, 36);
            this.MotionRefsChbx.Name = "MotionRefsChbx";
            this.MotionRefsChbx.Size = new System.Drawing.Size(83, 17);
            this.MotionRefsChbx.TabIndex = 3;
            this.MotionRefsChbx.Tag = "2";
            this.MotionRefsChbx.Text = "Motion Refs";
            this.MotionRefsChbx.UseVisualStyleBackColor = true;
            // 
            // MotionsChbx
            // 
            this.MotionsChbx.AutoSize = true;
            this.MotionsChbx.Location = new System.Drawing.Point(13, 59);
            this.MotionsChbx.Name = "MotionsChbx";
            this.MotionsChbx.Size = new System.Drawing.Size(63, 17);
            this.MotionsChbx.TabIndex = 4;
            this.MotionsChbx.Tag = "3";
            this.MotionsChbx.Text = "Motions";
            this.MotionsChbx.UseVisualStyleBackColor = true;
            // 
            // IKdataChbx
            // 
            this.IKdataChbx.AutoSize = true;
            this.IKdataChbx.Location = new System.Drawing.Point(13, 130);
            this.IKdataChbx.Name = "IKdataChbx";
            this.IKdataChbx.Size = new System.Drawing.Size(117, 17);
            this.IKdataChbx.TabIndex = 5;
            this.IKdataChbx.Tag = "5";
            this.IKdataChbx.Text = "Materials and Mass";
            this.IKdataChbx.UseVisualStyleBackColor = true;
            // 
            // RemoveChbx
            // 
            this.RemoveChbx.AutoSize = true;
            this.RemoveChbx.Location = new System.Drawing.Point(13, 163);
            this.RemoveChbx.Name = "RemoveChbx";
            this.RemoveChbx.Size = new System.Drawing.Size(145, 17);
            this.RemoveChbx.TabIndex = 6;
            this.RemoveChbx.Text = "Remove disabled params";
            this.RemoveChbx.UseVisualStyleBackColor = true;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ApplyButton.Location = new System.Drawing.Point(0, 186);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(277, 25);
            this.ApplyButton.TabIndex = 7;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // ImportParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 211);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.RemoveChbx);
            this.Controls.Add(this.IKdataChbx);
            this.Controls.Add(this.MotionsChbx);
            this.Controls.Add(this.MotionRefsChbx);
            this.Controls.Add(this.LodPathChbx);
            this.Controls.Add(this.UserdataChbx);
            this.Controls.Add(this.TexturesChbx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportParams";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Params to import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox TexturesChbx;
        private System.Windows.Forms.CheckBox UserdataChbx;
        private System.Windows.Forms.CheckBox LodPathChbx;
        private System.Windows.Forms.CheckBox MotionRefsChbx;
        private System.Windows.Forms.CheckBox MotionsChbx;
        private System.Windows.Forms.CheckBox IKdataChbx;
        private System.Windows.Forms.CheckBox RemoveChbx;
        private System.Windows.Forms.Button ApplyButton;
    }
}
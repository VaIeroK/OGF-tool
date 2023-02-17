namespace OGF_tool
{
    partial class Batch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Batch));
            this.label3 = new System.Windows.Forms.Label();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.TexturesReplaceButton = new System.Windows.Forms.Button();
            this.ShadersReplaceButton = new System.Windows.Forms.Button();
            this.FindPathButton = new System.Windows.Forms.Button();
            this.UserDataReplaceButton = new System.Windows.Forms.Button();
            this.MotionRefsReplaceButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.UserDataAddButton = new System.Windows.Forms.Button();
            this.MotionRefsAddButton = new System.Windows.Forms.Button();
            this.LodReplaceButton = new System.Windows.Forms.Button();
            this.LodAddButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LodDeleteButton = new System.Windows.Forms.Button();
            this.UserDataDeleteButton = new System.Windows.Forms.Button();
            this.MotionRefsDeleteButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Folder path:";
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(78, 180);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(400, 20);
            this.PathTextBox.TabIndex = 6;
            this.PathTextBox.TextChanged += new System.EventHandler(this.BoxTextChanged);
            // 
            // TexturesReplaceButton
            // 
            this.TexturesReplaceButton.Location = new System.Drawing.Point(6, 18);
            this.TexturesReplaceButton.Name = "TexturesReplaceButton";
            this.TexturesReplaceButton.Size = new System.Drawing.Size(153, 25);
            this.TexturesReplaceButton.TabIndex = 7;
            this.TexturesReplaceButton.Text = "Textures";
            this.TexturesReplaceButton.UseVisualStyleBackColor = true;
            this.TexturesReplaceButton.Click += new System.EventHandler(this.TexturesReplaceButton_Click);
            // 
            // ShadersReplaceButton
            // 
            this.ShadersReplaceButton.Location = new System.Drawing.Point(6, 49);
            this.ShadersReplaceButton.Name = "ShadersReplaceButton";
            this.ShadersReplaceButton.Size = new System.Drawing.Size(153, 25);
            this.ShadersReplaceButton.TabIndex = 8;
            this.ShadersReplaceButton.Text = "Shaders";
            this.ShadersReplaceButton.UseVisualStyleBackColor = true;
            this.ShadersReplaceButton.Click += new System.EventHandler(this.ShadersReplaceButton_Click);
            // 
            // FindPathButton
            // 
            this.FindPathButton.Location = new System.Drawing.Point(484, 179);
            this.FindPathButton.Name = "FindPathButton";
            this.FindPathButton.Size = new System.Drawing.Size(37, 23);
            this.FindPathButton.TabIndex = 9;
            this.FindPathButton.Text = "Find";
            this.FindPathButton.UseVisualStyleBackColor = true;
            this.FindPathButton.Click += new System.EventHandler(this.FindPathButton_Click);
            // 
            // UserDataReplaceButton
            // 
            this.UserDataReplaceButton.Location = new System.Drawing.Point(6, 80);
            this.UserDataReplaceButton.Name = "UserDataReplaceButton";
            this.UserDataReplaceButton.Size = new System.Drawing.Size(153, 25);
            this.UserDataReplaceButton.TabIndex = 10;
            this.UserDataReplaceButton.Text = "UserData";
            this.UserDataReplaceButton.UseVisualStyleBackColor = true;
            this.UserDataReplaceButton.Click += new System.EventHandler(this.UserDataReplaceButton_Click);
            // 
            // MotionRefsReplaceButton
            // 
            this.MotionRefsReplaceButton.Location = new System.Drawing.Point(6, 111);
            this.MotionRefsReplaceButton.Name = "MotionRefsReplaceButton";
            this.MotionRefsReplaceButton.Size = new System.Drawing.Size(153, 25);
            this.MotionRefsReplaceButton.TabIndex = 11;
            this.MotionRefsReplaceButton.Text = "MotionRefs";
            this.MotionRefsReplaceButton.UseVisualStyleBackColor = true;
            this.MotionRefsReplaceButton.Click += new System.EventHandler(this.MotionRefsReplaceButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LodReplaceButton);
            this.groupBox1.Controls.Add(this.UserDataReplaceButton);
            this.groupBox1.Controls.Add(this.MotionRefsReplaceButton);
            this.groupBox1.Controls.Add(this.TexturesReplaceButton);
            this.groupBox1.Controls.Add(this.ShadersReplaceButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 171);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Replace data";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LodAddButton);
            this.groupBox2.Controls.Add(this.UserDataAddButton);
            this.groupBox2.Controls.Add(this.MotionRefsAddButton);
            this.groupBox2.Location = new System.Drawing.Point(183, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 171);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add data";
            // 
            // UserDataAddButton
            // 
            this.UserDataAddButton.Location = new System.Drawing.Point(6, 18);
            this.UserDataAddButton.Name = "UserDataAddButton";
            this.UserDataAddButton.Size = new System.Drawing.Size(153, 25);
            this.UserDataAddButton.TabIndex = 10;
            this.UserDataAddButton.Text = "UserData";
            this.UserDataAddButton.UseVisualStyleBackColor = true;
            this.UserDataAddButton.Click += new System.EventHandler(this.UserDataAddButton_Click);
            // 
            // MotionRefsAddButton
            // 
            this.MotionRefsAddButton.Location = new System.Drawing.Point(6, 49);
            this.MotionRefsAddButton.Name = "MotionRefsAddButton";
            this.MotionRefsAddButton.Size = new System.Drawing.Size(153, 25);
            this.MotionRefsAddButton.TabIndex = 11;
            this.MotionRefsAddButton.Text = "MotionRefs";
            this.MotionRefsAddButton.UseVisualStyleBackColor = true;
            this.MotionRefsAddButton.Click += new System.EventHandler(this.MotionRefsAddButton_Click);
            // 
            // LodReplaceButton
            // 
            this.LodReplaceButton.Location = new System.Drawing.Point(6, 140);
            this.LodReplaceButton.Name = "LodReplaceButton";
            this.LodReplaceButton.Size = new System.Drawing.Size(153, 25);
            this.LodReplaceButton.TabIndex = 12;
            this.LodReplaceButton.Text = "Lod";
            this.LodReplaceButton.UseVisualStyleBackColor = true;
            this.LodReplaceButton.Click += new System.EventHandler(this.LodReplaceButton_Click);
            // 
            // LodAddButton
            // 
            this.LodAddButton.Location = new System.Drawing.Point(6, 80);
            this.LodAddButton.Name = "LodAddButton";
            this.LodAddButton.Size = new System.Drawing.Size(153, 25);
            this.LodAddButton.TabIndex = 13;
            this.LodAddButton.Text = "Lod";
            this.LodAddButton.UseVisualStyleBackColor = true;
            this.LodAddButton.Click += new System.EventHandler(this.LodAddButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.LodDeleteButton);
            this.groupBox3.Controls.Add(this.UserDataDeleteButton);
            this.groupBox3.Controls.Add(this.MotionRefsDeleteButton);
            this.groupBox3.Location = new System.Drawing.Point(354, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(165, 171);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Delete data";
            // 
            // LodDeleteButton
            // 
            this.LodDeleteButton.Location = new System.Drawing.Point(6, 80);
            this.LodDeleteButton.Name = "LodDeleteButton";
            this.LodDeleteButton.Size = new System.Drawing.Size(153, 25);
            this.LodDeleteButton.TabIndex = 13;
            this.LodDeleteButton.Text = "Lod";
            this.LodDeleteButton.UseVisualStyleBackColor = true;
            this.LodDeleteButton.Click += new System.EventHandler(this.LodDeleteButton_Click);
            // 
            // UserDataDeleteButton
            // 
            this.UserDataDeleteButton.Location = new System.Drawing.Point(6, 18);
            this.UserDataDeleteButton.Name = "UserDataDeleteButton";
            this.UserDataDeleteButton.Size = new System.Drawing.Size(153, 25);
            this.UserDataDeleteButton.TabIndex = 10;
            this.UserDataDeleteButton.Text = "UserData";
            this.UserDataDeleteButton.UseVisualStyleBackColor = true;
            this.UserDataDeleteButton.Click += new System.EventHandler(this.UserDataDeleteButton_Click);
            // 
            // MotionRefsDeleteButton
            // 
            this.MotionRefsDeleteButton.Location = new System.Drawing.Point(6, 49);
            this.MotionRefsDeleteButton.Name = "MotionRefsDeleteButton";
            this.MotionRefsDeleteButton.Size = new System.Drawing.Size(153, 25);
            this.MotionRefsDeleteButton.TabIndex = 11;
            this.MotionRefsDeleteButton.Text = "MotionRefs";
            this.MotionRefsDeleteButton.UseVisualStyleBackColor = true;
            this.MotionRefsDeleteButton.Click += new System.EventHandler(this.MotionRefsDeleteButton_Click);
            // 
            // Batch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 205);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.FindPathButton);
            this.Controls.Add(this.PathTextBox);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Batch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Tools";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PathTextBox;
        private System.Windows.Forms.Button TexturesReplaceButton;
        private System.Windows.Forms.Button ShadersReplaceButton;
        private System.Windows.Forms.Button FindPathButton;
        private System.Windows.Forms.Button UserDataReplaceButton;
        private System.Windows.Forms.Button MotionRefsReplaceButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button UserDataAddButton;
        private System.Windows.Forms.Button MotionRefsAddButton;
        private System.Windows.Forms.Button LodReplaceButton;
        private System.Windows.Forms.Button LodAddButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button LodDeleteButton;
        private System.Windows.Forms.Button UserDataDeleteButton;
        private System.Windows.Forms.Button MotionRefsDeleteButton;
    }
}
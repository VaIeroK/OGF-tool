namespace OGF_tool
{
    partial class MoveMesh
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoveMesh));
            this.ApplyButton = new System.Windows.Forms.Button();
            this.PositionZTextBox = new System.Windows.Forms.TextBox();
            this.PositionYTextBox = new System.Windows.Forms.TextBox();
            this.PositionXTextBox = new System.Windows.Forms.TextBox();
            this.PositionLabelEx = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ApplyButton
            // 
            this.ApplyButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ApplyButton.Location = new System.Drawing.Point(0, 44);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(489, 34);
            this.ApplyButton.TabIndex = 0;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // PositionZTextBox
            // 
            this.PositionZTextBox.Location = new System.Drawing.Point(353, 12);
            this.PositionZTextBox.Name = "PositionZTextBox";
            this.PositionZTextBox.Size = new System.Drawing.Size(126, 20);
            this.PositionZTextBox.TabIndex = 14;
            this.PositionZTextBox.Tag = "float";
            this.PositionZTextBox.TextChanged += new System.EventHandler(this.PosTextChanged);
            this.PositionZTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PosKeyDown);
            // 
            // PositionYTextBox
            // 
            this.PositionYTextBox.Location = new System.Drawing.Point(223, 12);
            this.PositionYTextBox.Name = "PositionYTextBox";
            this.PositionYTextBox.Size = new System.Drawing.Size(124, 20);
            this.PositionYTextBox.TabIndex = 13;
            this.PositionYTextBox.Tag = "float";
            this.PositionYTextBox.TextChanged += new System.EventHandler(this.PosTextChanged);
            this.PositionYTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PosKeyDown);
            // 
            // PositionXTextBox
            // 
            this.PositionXTextBox.Location = new System.Drawing.Point(93, 12);
            this.PositionXTextBox.Name = "PositionXTextBox";
            this.PositionXTextBox.Size = new System.Drawing.Size(124, 20);
            this.PositionXTextBox.TabIndex = 12;
            this.PositionXTextBox.Tag = "float";
            this.PositionXTextBox.TextChanged += new System.EventHandler(this.PosTextChanged);
            this.PositionXTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PosKeyDown);
            // 
            // PositionLabelEx
            // 
            this.PositionLabelEx.AutoSize = true;
            this.PositionLabelEx.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.PositionLabelEx.Location = new System.Drawing.Point(12, 15);
            this.PositionLabelEx.Name = "PositionLabelEx";
            this.PositionLabelEx.Size = new System.Drawing.Size(47, 13);
            this.PositionLabelEx.TabIndex = 18;
            this.PositionLabelEx.Text = "Position:";
            // 
            // MoveMesh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 78);
            this.Controls.Add(this.PositionLabelEx);
            this.Controls.Add(this.PositionZTextBox);
            this.Controls.Add(this.PositionYTextBox);
            this.Controls.Add(this.PositionXTextBox);
            this.Controls.Add(this.ApplyButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MoveMesh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mesh Offset";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.TextBox PositionZTextBox;
        private System.Windows.Forms.TextBox PositionYTextBox;
        private System.Windows.Forms.TextBox PositionXTextBox;
        private System.Windows.Forms.Label PositionLabelEx;
    }
}
namespace OGF_tool
{
    partial class ReplaceData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplaceData));
            this.StartButton = new System.Windows.Forms.Button();
            this.NewTextBox = new System.Windows.Forms.TextBox();
            this.ReplacerTextBox = new System.Windows.Forms.TextBox();
            this.Label_2 = new System.Windows.Forms.Label();
            this.Label_1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(340, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(89, 46);
            this.StartButton.TabIndex = 9;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // NewTextBox
            // 
            this.NewTextBox.Location = new System.Drawing.Point(84, 38);
            this.NewTextBox.Name = "NewTextBox";
            this.NewTextBox.Size = new System.Drawing.Size(250, 20);
            this.NewTextBox.TabIndex = 8;
            this.NewTextBox.TextChanged += new System.EventHandler(this.BoxTextChanged);
            // 
            // ReplacerTextBox
            // 
            this.ReplacerTextBox.Location = new System.Drawing.Point(84, 12);
            this.ReplacerTextBox.Name = "ReplacerTextBox";
            this.ReplacerTextBox.Size = new System.Drawing.Size(250, 20);
            this.ReplacerTextBox.TabIndex = 7;
            // 
            // Label_2
            // 
            this.Label_2.AutoSize = true;
            this.Label_2.Location = new System.Drawing.Point(8, 41);
            this.Label_2.Name = "Label_2";
            this.Label_2.Size = new System.Drawing.Size(72, 13);
            this.Label_2.TabIndex = 6;
            this.Label_2.Text = "Replace with:";
            // 
            // Label_1
            // 
            this.Label_1.AutoSize = true;
            this.Label_1.Location = new System.Drawing.Point(8, 15);
            this.Label_1.Name = "Label_1";
            this.Label_1.Size = new System.Drawing.Size(56, 13);
            this.Label_1.TabIndex = 5;
            this.Label_1.Text = "Find what:";
            // 
            // ReplaceData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 67);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.NewTextBox);
            this.Controls.Add(this.ReplacerTextBox);
            this.Controls.Add(this.Label_2);
            this.Controls.Add(this.Label_1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReplaceData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TextBoxReplacer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox NewTextBox;
        private System.Windows.Forms.TextBox ReplacerTextBox;
        private System.Windows.Forms.Label Label_2;
        private System.Windows.Forms.Label Label_1;
    }
}
namespace OGF_tool
{
    partial class AddDeleteData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddDeleteData));
            this.StartButton = new System.Windows.Forms.Button();
            this.DataTextBox = new System.Windows.Forms.RichTextBox();
            this.CreateChunkChbx = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(300, 10);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(89, 55);
            this.StartButton.TabIndex = 9;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // DataTextBox
            // 
            this.DataTextBox.Location = new System.Drawing.Point(12, 10);
            this.DataTextBox.Name = "DataTextBox";
            this.DataTextBox.Size = new System.Drawing.Size(282, 74);
            this.DataTextBox.TabIndex = 10;
            this.DataTextBox.Text = "";
            this.DataTextBox.TextChanged += new System.EventHandler(this.BoxTextChanged);
            // 
            // CreateChunkChbx
            // 
            this.CreateChunkChbx.AutoSize = true;
            this.CreateChunkChbx.Location = new System.Drawing.Point(300, 67);
            this.CreateChunkChbx.Name = "CreateChunkChbx";
            this.CreateChunkChbx.Size = new System.Drawing.Size(95, 17);
            this.CreateChunkChbx.TabIndex = 11;
            this.CreateChunkChbx.Text = "Create chunks";
            this.CreateChunkChbx.UseVisualStyleBackColor = true;
            // 
            // AddDeleteData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 96);
            this.Controls.Add(this.CreateChunkChbx);
            this.Controls.Add(this.DataTextBox);
            this.Controls.Add(this.StartButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddDeleteData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TextBoxAdd";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.RichTextBox DataTextBox;
        private System.Windows.Forms.CheckBox CreateChunkChbx;
    }
}
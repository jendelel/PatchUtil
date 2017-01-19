namespace PatchUtil
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.filesList = new System.Windows.Forms.ListBox();
            this.butPatchSelected = new System.Windows.Forms.Button();
            this.butPatchAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // filesList
            // 
            this.filesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesList.FormattingEnabled = true;
            this.filesList.HorizontalScrollbar = true;
            this.filesList.ItemHeight = 16;
            this.filesList.Location = new System.Drawing.Point(12, 12);
            this.filesList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.filesList.Name = "filesList";
            this.filesList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.filesList.Size = new System.Drawing.Size(339, 292);
            this.filesList.TabIndex = 0;
            this.filesList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.filesList_MouseClick);
            this.filesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.filesList_MouseDoubleClick);
            // 
            // butPatchSelected
            // 
            this.butPatchSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.butPatchSelected.Location = new System.Drawing.Point(12, 310);
            this.butPatchSelected.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.butPatchSelected.Name = "butPatchSelected";
            this.butPatchSelected.Size = new System.Drawing.Size(337, 37);
            this.butPatchSelected.TabIndex = 1;
            this.butPatchSelected.Text = "Patch selected files";
            this.butPatchSelected.UseVisualStyleBackColor = true;
            this.butPatchSelected.Click += new System.EventHandler(this.butPatchSelected_Click);
            // 
            // butPatchAll
            // 
            this.butPatchAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.butPatchAll.Location = new System.Drawing.Point(12, 353);
            this.butPatchAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.butPatchAll.Name = "butPatchAll";
            this.butPatchAll.Size = new System.Drawing.Size(337, 37);
            this.butPatchAll.TabIndex = 2;
            this.butPatchAll.Text = "Patch all files";
            this.butPatchAll.UseVisualStyleBackColor = true;
            this.butPatchAll.Click += new System.EventHandler(this.butPatchAll_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 398);
            this.Controls.Add(this.butPatchAll);
            this.Controls.Add(this.butPatchSelected);
            this.Controls.Add(this.filesList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "PatchUtil";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox filesList;
        private System.Windows.Forms.Button butPatchSelected;
        private System.Windows.Forms.Button butPatchAll;
    }
}


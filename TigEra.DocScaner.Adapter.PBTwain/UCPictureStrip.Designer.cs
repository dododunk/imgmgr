namespace TigEra.DocScaner.Adapter.PBTwain
{
    partial class UCPictureStrip
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_right = new System.Windows.Forms.Button();
            this.cb_left = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.cb_right);
            this.panel1.Controls.Add(this.cb_left);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 130);
            this.panel1.TabIndex = 0;
            // 
            // cb_right
            // 
            this.cb_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.cb_right.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cb_right.Location = new System.Drawing.Point(194, 0);
            this.cb_right.Name = "cb_right";
            this.cb_right.Size = new System.Drawing.Size(32, 130);
            this.cb_right.TabIndex = 1;
            this.cb_right.Text = "u";
            this.cb_right.UseVisualStyleBackColor = true;
            this.cb_right.Click += new System.EventHandler(this.cb_right_Click);
            // 
            // cb_left
            // 
            this.cb_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.cb_left.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.cb_left.Location = new System.Drawing.Point(0, 0);
            this.cb_left.Name = "cb_left";
            this.cb_left.Size = new System.Drawing.Size(32, 130);
            this.cb_left.TabIndex = 0;
            this.cb_left.Text = "t";
            this.cb_left.UseVisualStyleBackColor = true;
            this.cb_left.Click += new System.EventHandler(this.cb_left_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoSize = true;
            this.panel2.Location = new System.Drawing.Point(38, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 118);
            this.panel2.TabIndex = 2;
            this.panel2.Click += new System.EventHandler(this.cb_left_Click);
            // 
            // PictureStrip
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Name = "PictureStrip";
            this.Size = new System.Drawing.Size(229, 118);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FilmStripPanel_Paint);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.FilmStripPanel_Validating);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cb_right;
        private System.Windows.Forms.Button cb_left;
        public System.Windows.Forms.Panel panel2;
    }
}

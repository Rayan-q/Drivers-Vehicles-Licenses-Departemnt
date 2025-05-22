namespace DVLDManagement.Users_Management
{
    partial class frmUserDetails
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
            this.crtShowDetails1 = new DVLDManagement.People_Management.crtShowDetails();
            this.crtLoginInfo1 = new DVLDManagement.Users_Management.crtLoginInfo();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // crtShowDetails1
            // 
            this.crtShowDetails1.Location = new System.Drawing.Point(24, 21);
            this.crtShowDetails1.Name = "crtShowDetails1";
            this.crtShowDetails1.Size = new System.Drawing.Size(944, 311);
            this.crtShowDetails1.TabIndex = 0;
            // 
            // crtLoginInfo1
            // 
            this.crtLoginInfo1.Location = new System.Drawing.Point(24, 338);
            this.crtLoginInfo1.Name = "crtLoginInfo1";
            this.crtLoginInfo1.Size = new System.Drawing.Size(944, 82);
            this.crtLoginInfo1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Image = global::DVLDManagement.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(851, 436);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 41);
            this.btnClose.TabIndex = 72;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmUserDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 507);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.crtLoginInfo1);
            this.Controls.Add(this.crtShowDetails1);
            this.Name = "frmUserDetails";
            this.Text = "Users Info Details";
            this.Load += new System.EventHandler(this.frmUserDetails_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private People_Management.crtShowDetails crtShowDetails1;
        private crtLoginInfo crtLoginInfo1;
        private System.Windows.Forms.Button btnClose;
    }
}
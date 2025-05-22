namespace DVLDManagement.Manage_LDLApplications
{
    partial class frmApplicationDetails
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
            this.crtApplicationinfo1 = new DVLDManagement.Manage_LDLApplications.crtApplicationinfo();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbCurrentAction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // crtApplicationinfo1
            // 
            this.crtApplicationinfo1._PersonID = 0;
            this.crtApplicationinfo1.Location = new System.Drawing.Point(12, 130);
            this.crtApplicationinfo1.Name = "crtApplicationinfo1";
            this.crtApplicationinfo1.Size = new System.Drawing.Size(978, 503);
            this.crtApplicationinfo1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Image = global::DVLDManagement.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(873, 639);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 41);
            this.btnClose.TabIndex = 79;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbCurrentAction
            // 
            this.lbCurrentAction.AutoSize = true;
            this.lbCurrentAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.lbCurrentAction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbCurrentAction.Location = new System.Drawing.Point(326, 64);
            this.lbCurrentAction.Name = "lbCurrentAction";
            this.lbCurrentAction.Size = new System.Drawing.Size(350, 46);
            this.lbCurrentAction.TabIndex = 80;
            this.lbCurrentAction.Text = "Application Details";
            // 
            // frmApplicationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 692);
            this.Controls.Add(this.lbCurrentAction);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.crtApplicationinfo1);
            this.Name = "frmApplicationDetails";
            this.Text = "Application Details";
            this.Load += new System.EventHandler(this.frmApplicationDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private crtApplicationinfo crtApplicationinfo1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lbCurrentAction;
    }
}
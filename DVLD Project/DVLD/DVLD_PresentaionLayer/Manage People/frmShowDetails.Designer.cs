namespace DVLDManagement.People_Management
{
    partial class frmShowDetails
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
            this.lbCurrentAction = new System.Windows.Forms.Label();
            this.crtShowDetails1 = new DVLDManagement.People_Management.crtShowDetails();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbCurrentAction
            // 
            this.lbCurrentAction.AutoSize = true;
            this.lbCurrentAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.lbCurrentAction.ForeColor = System.Drawing.Color.Red;
            this.lbCurrentAction.Location = new System.Drawing.Point(345, 34);
            this.lbCurrentAction.Name = "lbCurrentAction";
            this.lbCurrentAction.Size = new System.Drawing.Size(381, 46);
            this.lbCurrentAction.TabIndex = 67;
            this.lbCurrentAction.Text = "Detailed Person Info";
            this.lbCurrentAction.Click += new System.EventHandler(this.lbCurrentAction_Click);
            // 
            // crtShowDetails1
            // 
            this.crtShowDetails1.Location = new System.Drawing.Point(43, 91);
            this.crtShowDetails1.Name = "crtShowDetails1";
            this.crtShowDetails1.Size = new System.Drawing.Size(944, 311);
            this.crtShowDetails1.TabIndex = 68;
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Image = global::DVLDManagement.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(870, 408);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 41);
            this.btnClose.TabIndex = 69;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmShowDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 461);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.crtShowDetails1);
            this.Controls.Add(this.lbCurrentAction);
            this.Name = "frmShowDetails";
            this.Text = "frmShowDetails";
            this.Load += new System.EventHandler(this.frmShowDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbCurrentAction;
        private crtShowDetails crtShowDetails1;
        private System.Windows.Forms.Button btnClose;
    }
}
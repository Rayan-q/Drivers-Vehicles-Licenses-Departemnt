namespace DVLDManagement.People_Management
{
    partial class frmAddEditPerson
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
            this.crtPersonCard1 = new DVLDManagement.crtPersonCard();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // crtPersonCard1
            // 
            this.crtPersonCard1.Location = new System.Drawing.Point(1, 12);
            this.crtPersonCard1.Name = "crtPersonCard1";
            this.crtPersonCard1.Size = new System.Drawing.Size(970, 440);
            this.crtPersonCard1.TabIndex = 0;
            this.crtPersonCard1.OnActionComplete += new System.Action(this.crtPersonCard1_OnActionComplete);
            this.crtPersonCard1.Load += new System.EventHandler(this.crtPersonCard1_Load);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Image = global::DVLDManagement.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(462, 394);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 41);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmAddEditPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 465);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.crtPersonCard1);
            this.Name = "frmAddEditPerson";
            this.Text = "frmAddPerson";
            this.Load += new System.EventHandler(this.frmAddPerson_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private crtPersonCard crtPersonCard1;
        private System.Windows.Forms.Button btnClose;
    }
}
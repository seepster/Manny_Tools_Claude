namespace Manny_Tools_Claude
{
    partial class ExistingSizesForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.listExistingSizes = new System.Windows.Forms.ListBox();
            this.lblNoSizes = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(282, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Existing Sizes for Parent Code";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(14, 44);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(364, 15);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "The following sizes already exist for this Parent Code. Review and continue.";
            // 
            // listExistingSizes
            // 
            this.listExistingSizes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listExistingSizes.FormattingEnabled = true;
            this.listExistingSizes.ItemHeight = 15;
            this.listExistingSizes.Location = new System.Drawing.Point(14, 71);
            this.listExistingSizes.Name = "listExistingSizes";
            this.listExistingSizes.Size = new System.Drawing.Size(456, 199);
            this.listExistingSizes.TabIndex = 2;
            // 
            // lblNoSizes
            // 
            this.lblNoSizes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoSizes.BackColor = System.Drawing.SystemColors.Info;
            this.lblNoSizes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNoSizes.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lblNoSizes.Location = new System.Drawing.Point(14, 120);
            this.lblNoSizes.Name = "lblNoSizes";
            this.lblNoSizes.Padding = new System.Windows.Forms.Padding(10);
            this.lblNoSizes.Size = new System.Drawing.Size(456, 40);
            this.lblNoSizes.TabIndex = 3;
            this.lblNoSizes.Text = "No existing sizes found for this Parent Code.";
            this.lblNoSizes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoSizes.Visible = false;
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContinue.Location = new System.Drawing.Point(370, 291);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(100, 30);
            this.btnContinue.TabIndex = 4;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(264, 291);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ExistingSizesForm
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 336);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lblNoSizes);
            this.Controls.Add(this.listExistingSizes);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblTitle);
            this.MinimumSize = new System.Drawing.Size(500, 375);
            this.Name = "ExistingSizesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Existing Sizes";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ListBox listExistingSizes;
        private System.Windows.Forms.Label lblNoSizes;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnCancel;
    }
}
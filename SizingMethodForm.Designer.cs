namespace Manny_Tools_Claude
{
    partial class SizingMethodForm
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
            this.radioButtonNumbered = new System.Windows.Forms.RadioButton();
            this.radioButtonDescriptive = new System.Windows.Forms.RadioButton();
            this.radioButtonBaby = new System.Windows.Forms.RadioButton();
            this.radioButtonDouble = new System.Windows.Forms.RadioButton();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDescription1 = new System.Windows.Forms.Label();
            this.lblDescription2 = new System.Windows.Forms.Label();
            this.lblDescription3 = new System.Windows.Forms.Label();
            this.lblDescription4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(192, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Select Sizing Method";
            // 
            // radioButtonNumbered
            // 
            this.radioButtonNumbered.AutoSize = true;
            this.radioButtonNumbered.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radioButtonNumbered.Location = new System.Drawing.Point(17, 53);
            this.radioButtonNumbered.Name = "radioButtonNumbered";
            this.radioButtonNumbered.Size = new System.Drawing.Size(148, 23);
            this.radioButtonNumbered.TabIndex = 1;
            this.radioButtonNumbered.TabStop = true;
            this.radioButtonNumbered.Text = "1. Numbered sizing";
            this.radioButtonNumbered.UseVisualStyleBackColor = true;
            this.radioButtonNumbered.CheckedChanged += new System.EventHandler(this.radioButtonNumbered_CheckedChanged);
            // 
            // radioButtonDescriptive
            // 
            this.radioButtonDescriptive.AutoSize = true;
            this.radioButtonDescriptive.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radioButtonDescriptive.Location = new System.Drawing.Point(17, 113);
            this.radioButtonDescriptive.Name = "radioButtonDescriptive";
            this.radioButtonDescriptive.Size = new System.Drawing.Size(153, 23);
            this.radioButtonDescriptive.TabIndex = 3;
            this.radioButtonDescriptive.TabStop = true;
            this.radioButtonDescriptive.Text = "2. Descriptive sizing";
            this.radioButtonDescriptive.UseVisualStyleBackColor = true;
            this.radioButtonDescriptive.CheckedChanged += new System.EventHandler(this.radioButtonDescriptive_CheckedChanged);
            // 
            // radioButtonBaby
            // 
            this.radioButtonBaby.AutoSize = true;
            this.radioButtonBaby.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radioButtonBaby.Location = new System.Drawing.Point(17, 173);
            this.radioButtonBaby.Name = "radioButtonBaby";
            this.radioButtonBaby.Size = new System.Drawing.Size(114, 23);
            this.radioButtonBaby.TabIndex = 5;
            this.radioButtonBaby.TabStop = true;
            this.radioButtonBaby.Text = "3. Baby Sizing";
            this.radioButtonBaby.UseVisualStyleBackColor = true;
            this.radioButtonBaby.CheckedChanged += new System.EventHandler(this.radioButtonBaby_CheckedChanged);
            // 
            // radioButtonDouble
            // 
            this.radioButtonDouble.AutoSize = true;
            this.radioButtonDouble.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.radioButtonDouble.Location = new System.Drawing.Point(17, 233);
            this.radioButtonDouble.Name = "radioButtonDouble";
            this.radioButtonDouble.Size = new System.Drawing.Size(130, 23);
            this.radioButtonDouble.TabIndex = 7;
            this.radioButtonDouble.TabStop = true;
            this.radioButtonDouble.Text = "4. Double Sizing";
            this.radioButtonDouble.UseVisualStyleBackColor = true;
            this.radioButtonDouble.CheckedChanged += new System.EventHandler(this.radioButtonDouble_CheckedChanged);
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(220, 290);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(100, 30);
            this.btnContinue.TabIndex = 9;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(114, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDescription1
            // 
            this.lblDescription1.AutoSize = true;
            this.lblDescription1.Location = new System.Drawing.Point(40, 79);
            this.lblDescription1.Name = "lblDescription1";
            this.lblDescription1.Size = new System.Drawing.Size(236, 15);
            this.lblDescription1.TabIndex = 2;
            this.lblDescription1.Text = "e.g., Size 4, Size 6, Size 7 (numerical range)";
            // 
            // lblDescription2
            // 
            this.lblDescription2.AutoSize = true;
            this.lblDescription2.Location = new System.Drawing.Point(40, 139);
            this.lblDescription2.Name = "lblDescription2";
            this.lblDescription2.Size = new System.Drawing.Size(221, 15);
            this.lblDescription2.TabIndex = 4;
            this.lblDescription2.Text = "e.g., Extra Small, Small, Medium, Large";
            // 
            // lblDescription3
            // 
            this.lblDescription3.AutoSize = true;
            this.lblDescription3.Location = new System.Drawing.Point(40, 199);
            this.lblDescription3.Name = "lblDescription3";
            this.lblDescription3.Size = new System.Drawing.Size(214, 15);
            this.lblDescription3.TabIndex = 6;
            this.lblDescription3.Text = "e.g., 0-3 months, 3-6 months, 1-2 years";
            // 
            // lblDescription4
            // 
            this.lblDescription4.AutoSize = true;
            this.lblDescription4.Location = new System.Drawing.Point(40, 259);
            this.lblDescription4.Name = "lblDescription4";
            this.lblDescription4.Size = new System.Drawing.Size(249, 15);
            this.lblDescription4.TabIndex = 8;
            this.lblDescription4.Text = "e.g., 32/30, 34/32 (Waist/Inseam for pants)";
            // 
            // SizingMethodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 332);
            this.Controls.Add(this.lblDescription4);
            this.Controls.Add(this.lblDescription3);
            this.Controls.Add(this.lblDescription2);
            this.Controls.Add(this.lblDescription1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.radioButtonDouble);
            this.Controls.Add(this.radioButtonBaby);
            this.Controls.Add(this.radioButtonDescriptive);
            this.Controls.Add(this.radioButtonNumbered);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SizingMethodForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Sizing Method";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.RadioButton radioButtonNumbered;
        private System.Windows.Forms.RadioButton radioButtonDescriptive;
        private System.Windows.Forms.RadioButton radioButtonBaby;
        private System.Windows.Forms.RadioButton radioButtonDouble;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDescription1;
        private System.Windows.Forms.Label lblDescription2;
        private System.Windows.Forms.Label lblDescription3;
        private System.Windows.Forms.Label lblDescription4;
    }
}
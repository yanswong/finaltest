namespace VSLDtest.TestForms
{
    partial class FormLD_Options
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.cbWireless = new System.Windows.Forms.CheckBox();
            this.cbIO = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(671, 31);
            this.label2.TabIndex = 5;
            this.label2.Text = "Choose Leak Detector Options from checkbox below: -";
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(370, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 41);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(240, 217);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 41);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cbWireless
            // 
            this.cbWireless.AutoSize = true;
            this.cbWireless.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbWireless.Location = new System.Drawing.Point(292, 104);
            this.cbWireless.Name = "cbWireless";
            this.cbWireless.Size = new System.Drawing.Size(166, 29);
            this.cbWireless.TabIndex = 8;
            this.cbWireless.Text = "Wireless Opt";
            this.cbWireless.UseVisualStyleBackColor = true;
            this.cbWireless.CheckedChanged += new System.EventHandler(this.cbWireless_CheckedChanged);
            // 
            // cbIO
            // 
            this.cbIO.AutoSize = true;
            this.cbIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIO.Location = new System.Drawing.Point(292, 148);
            this.cbIO.Name = "cbIO";
            this.cbIO.Size = new System.Drawing.Size(137, 29);
            this.cbIO.TabIndex = 9;
            this.cbIO.Text = "I/O Option";
            this.cbIO.UseVisualStyleBackColor = true;
            this.cbIO.CheckedChanged += new System.EventHandler(this.cbIO_CheckedChanged);
            // 
            // FormLD_Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 291);
            this.Controls.Add(this.cbIO);
            this.Controls.Add(this.cbWireless);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Name = "FormLD_Options";
            this.Text = "FormLD_Options";
            this.Load += new System.EventHandler(this.FormLD_Options_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox cbWireless;
        private System.Windows.Forms.CheckBox cbIO;
    }
}
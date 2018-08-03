namespace CustomFormLibrary
{
    partial class Test_Failed
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
            this.button_abort = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.test_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_error = new System.Windows.Forms.Label();
            this.label_testpoint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_abort
            // 
            this.button_abort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_abort.Location = new System.Drawing.Point(180, 269);
            this.button_abort.Name = "button_abort";
            this.button_abort.Size = new System.Drawing.Size(97, 39);
            this.button_abort.TabIndex = 1;
            this.button_abort.Text = "Abort";
            this.button_abort.UseVisualStyleBackColor = true;
            this.button_abort.Click += new System.EventHandler(this.button_abort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(456, 48);
            this.label1.TabIndex = 2;
            this.label1.Text = "The testing procedure is discontinued due to an error\r\nat test point:";
            // 
            // test_label
            // 
            this.test_label.AutoEllipsis = true;
            this.test_label.AutoSize = true;
            this.test_label.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.test_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.test_label.ForeColor = System.Drawing.Color.Red;
            this.test_label.Location = new System.Drawing.Point(11, 84);
            this.test_label.Name = "test_label";
            this.test_label.Size = new System.Drawing.Size(137, 29);
            this.test_label.TabIndex = 3;
            this.test_label.Text = "Test Label";
            this.test_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(408, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "To abort the entire test procedure, press \'Abort\' ";
            // 
            // label_error
            // 
            this.label_error.AutoSize = true;
            this.label_error.BackColor = System.Drawing.Color.White;
            this.label_error.Cursor = System.Windows.Forms.Cursors.Default;
            this.label_error.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_error.Location = new System.Drawing.Point(12, 192);
            this.label_error.Name = "label_error";
            this.label_error.Size = new System.Drawing.Size(97, 16);
            this.label_error.TabIndex = 5;
            this.label_error.Text = "Error Message";
            // 
            // label_testpoint
            // 
            this.label_testpoint.AutoSize = true;
            this.label_testpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_testpoint.Location = new System.Drawing.Point(12, 118);
            this.label_testpoint.Name = "label_testpoint";
            this.label_testpoint.Size = new System.Drawing.Size(103, 24);
            this.label_testpoint.TabIndex = 6;
            this.label_testpoint.Text = "Test Point: ";
            // 
            // Test_Failed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.button_abort;
            this.ClientSize = new System.Drawing.Size(475, 320);
            this.ControlBox = false;
            this.Controls.Add(this.label_testpoint);
            this.Controls.Add(this.label_error);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.test_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_abort);
            this.Name = "Test_Failed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TEST FAILED";
            this.Load += new System.EventHandler(this.Test_Failed_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_abort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label test_label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_error;
        private System.Windows.Forms.Label label_testpoint;
    }
}
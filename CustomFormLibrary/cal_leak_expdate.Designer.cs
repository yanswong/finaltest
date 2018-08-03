namespace CustomFormLibrary
{
    partial class cal_leak_expdate
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_prompt = new System.Windows.Forms.Label();
            this.year_input = new System.Windows.Forms.TextBox();
            this.month_input = new System.Windows.Forms.TextBox();
            this.day_input = new System.Windows.Forms.TextBox();
            this.btn_confirm = new System.Windows.Forms.Button();
            this.btn_abort = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_prompt);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(552, 120);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description:";
            // 
            // label_prompt
            // 
            this.label_prompt.AutoSize = true;
            this.label_prompt.BackColor = System.Drawing.SystemColors.Control;
            this.label_prompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_prompt.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label_prompt.Location = new System.Drawing.Point(6, 31);
            this.label_prompt.Name = "label_prompt";
            this.label_prompt.Size = new System.Drawing.Size(539, 62);
            this.label_prompt.TabIndex = 0;
            this.label_prompt.Text = "Enter the standard leak expiration date from\r\nthe label of the leak in the UUT.";
            // 
            // year_input
            // 
            this.year_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.year_input.Location = new System.Drawing.Point(330, 180);
            this.year_input.MaxLength = 4;
            this.year_input.Name = "year_input";
            this.year_input.Size = new System.Drawing.Size(75, 38);
            this.year_input.TabIndex = 2;
            this.year_input.Text = "yyyy";
            this.year_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.year_input.TextChanged += new System.EventHandler(this.year_input_TextChanged);
            // 
            // month_input
            // 
            this.month_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.month_input.Location = new System.Drawing.Point(168, 180);
            this.month_input.MaxLength = 2;
            this.month_input.Name = "month_input";
            this.month_input.Size = new System.Drawing.Size(75, 38);
            this.month_input.TabIndex = 0;
            this.month_input.Text = "mm";
            this.month_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.month_input.TextChanged += new System.EventHandler(this.month_input_TextChanged);
            // 
            // day_input
            // 
            this.day_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.day_input.Location = new System.Drawing.Point(249, 180);
            this.day_input.MaxLength = 2;
            this.day_input.Name = "day_input";
            this.day_input.Size = new System.Drawing.Size(75, 38);
            this.day_input.TabIndex = 1;
            this.day_input.Text = "dd";
            this.day_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.day_input.TextChanged += new System.EventHandler(this.day_input_TextChanged);
            // 
            // btn_confirm
            // 
            this.btn_confirm.Location = new System.Drawing.Point(12, 175);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(144, 51);
            this.btn_confirm.TabIndex = 3;
            this.btn_confirm.Text = "CONFIRM";
            this.btn_confirm.UseVisualStyleBackColor = true;
            this.btn_confirm.Click += new System.EventHandler(this.btn_confirm_Click);
            // 
            // btn_abort
            // 
            this.btn_abort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_abort.Location = new System.Drawing.Point(420, 176);
            this.btn_abort.Name = "btn_abort";
            this.btn_abort.Size = new System.Drawing.Size(144, 53);
            this.btn_abort.TabIndex = 4;
            this.btn_abort.Text = "ABORT";
            this.btn_abort.UseVisualStyleBackColor = true;
            // 
            // cal_leak_expdate
            // 
            this.AcceptButton = this.btn_confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_abort;
            this.ClientSize = new System.Drawing.Size(580, 240);
            this.ControlBox = false;
            this.Controls.Add(this.year_input);
            this.Controls.Add(this.month_input);
            this.Controls.Add(this.day_input);
            this.Controls.Add(this.btn_confirm);
            this.Controls.Add(this.btn_abort);
            this.Controls.Add(this.groupBox1);
            this.Name = "cal_leak_expdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stdleak Expiration Date";
            this.Load += new System.EventHandler(this.cal_leak_expdate_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_prompt;
        private System.Windows.Forms.TextBox year_input;
        private System.Windows.Forms.TextBox month_input;
        private System.Windows.Forms.TextBox day_input;
        private System.Windows.Forms.Button btn_confirm;
        private System.Windows.Forms.Button btn_abort;
    }
}
namespace CustomFormLibrary
{
    partial class cal_leak_tempfactor
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
            this.label_prompt = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_abort = new System.Windows.Forms.Button();
            this.button_confirm = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.decimal_input = new System.Windows.Forms.TextBox();
            this.num_input = new System.Windows.Forms.TextBox();
            this.combo_sign = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_prompt
            // 
            this.label_prompt.AutoSize = true;
            this.label_prompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_prompt.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label_prompt.Location = new System.Drawing.Point(6, 31);
            this.label_prompt.Name = "label_prompt";
            this.label_prompt.Size = new System.Drawing.Size(650, 62);
            this.label_prompt.TabIndex = 0;
            this.label_prompt.Text = "Enter the standard leak temperature correction factor\r\nfrom the label.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_prompt);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(662, 109);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description:";
            // 
            // button_abort
            // 
            this.button_abort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_abort.Location = new System.Drawing.Point(548, 150);
            this.button_abort.Name = "button_abort";
            this.button_abort.Size = new System.Drawing.Size(126, 51);
            this.button_abort.TabIndex = 4;
            this.button_abort.Text = "ABORT";
            this.button_abort.UseVisualStyleBackColor = true;
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(12, 149);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(126, 51);
            this.button_confirm.TabIndex = 3;
            this.button_confirm.Text = "CONFIRM";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(314, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 31);
            this.label2.TabIndex = 5;
            this.label2.Text = ".";
            // 
            // decimal_input
            // 
            this.decimal_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.decimal_input.Location = new System.Drawing.Point(338, 154);
            this.decimal_input.MaxLength = 1;
            this.decimal_input.Name = "decimal_input";
            this.decimal_input.Size = new System.Drawing.Size(55, 38);
            this.decimal_input.TabIndex = 2;
            this.decimal_input.Text = "X";
            this.decimal_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.decimal_input.TextChanged += new System.EventHandler(this.decimal_input_TextChanged);
            // 
            // num_input
            // 
            this.num_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_input.Location = new System.Drawing.Point(256, 154);
            this.num_input.MaxLength = 1;
            this.num_input.Name = "num_input";
            this.num_input.Size = new System.Drawing.Size(55, 38);
            this.num_input.TabIndex = 1;
            this.num_input.Text = "X";
            this.num_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_input.TextChanged += new System.EventHandler(this.num_input_TextChanged);
            // 
            // combo_sign
            // 
            this.combo_sign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_sign.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_sign.FormattingEnabled = true;
            this.combo_sign.Location = new System.Drawing.Point(192, 153);
            this.combo_sign.Name = "combo_sign";
            this.combo_sign.Size = new System.Drawing.Size(53, 39);
            this.combo_sign.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(398, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 31);
            this.label1.TabIndex = 19;
            this.label1.Text = "Celsius";
            // 
            // cal_leak_tempfactor
            // 
            this.AcceptButton = this.button_confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_abort;
            this.ClientSize = new System.Drawing.Size(690, 216);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.combo_sign);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_abort);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.decimal_input);
            this.Controls.Add(this.num_input);
            this.Name = "cal_leak_tempfactor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stdleak temperature Correction Factor";
            this.Load += new System.EventHandler(this.cal_leak_tempfactor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_prompt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_abort;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox decimal_input;
        private System.Windows.Forms.TextBox num_input;
        private System.Windows.Forms.ComboBox combo_sign;
        private System.Windows.Forms.Label label1;
    }
}
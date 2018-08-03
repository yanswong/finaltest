namespace CustomFormLibrary
{
    partial class cal_leak_prompt
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
            this.label1 = new System.Windows.Forms.Label();
            this.button_confirm = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.exp_value = new System.Windows.Forms.TextBox();
            this.num_input = new System.Windows.Forms.TextBox();
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
            this.label_prompt.Size = new System.Drawing.Size(621, 62);
            this.label_prompt.TabIndex = 0;
            this.label_prompt.Text = "Enter the value of the standard leak installed in the\r\nUUT.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_prompt);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(642, 119);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Description:";
            // 
            // button_abort
            // 
            this.button_abort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.button_abort.Location = new System.Drawing.Point(522, 172);
            this.button_abort.Name = "button_abort";
            this.button_abort.Size = new System.Drawing.Size(135, 54);
            this.button_abort.TabIndex = 3;
            this.button_abort.Text = "ABORT";
            this.button_abort.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(363, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 31);
            this.label1.TabIndex = 11;
            this.label1.Text = "Std .cc/s";
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(15, 172);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(134, 54);
            this.button_confirm.TabIndex = 2;
            this.button_confirm.Text = "CONFIRM";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(245, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 31);
            this.label2.TabIndex = 12;
            this.label2.Text = "E -";
            // 
            // exp_value
            // 
            this.exp_value.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exp_value.Location = new System.Drawing.Point(297, 182);
            this.exp_value.MaxLength = 2;
            this.exp_value.Name = "exp_value";
            this.exp_value.Size = new System.Drawing.Size(55, 38);
            this.exp_value.TabIndex = 1;
            this.exp_value.Text = "XX";
            this.exp_value.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.exp_value.TextChanged += new System.EventHandler(this.exp_value_TextChanged);
            // 
            // num_input
            // 
            this.num_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_input.Location = new System.Drawing.Point(183, 182);
            this.num_input.MaxLength = 3;
            this.num_input.Name = "num_input";
            this.num_input.Size = new System.Drawing.Size(55, 38);
            this.num_input.TabIndex = 0;
            this.num_input.Text = "X.X";
            this.num_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.num_input.TextChanged += new System.EventHandler(this.num_input_TextChanged);
            // 
            // cal_leak_prompt
            // 
            this.AcceptButton = this.button_confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_abort;
            this.ClientSize = new System.Drawing.Size(672, 237);
            this.ControlBox = false;
            this.Controls.Add(this.button_abort);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.num_input);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.exp_value);
            this.Name = "cal_leak_prompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stdleak Rate (Std .cc/s)";
            this.Load += new System.EventHandler(this.cal_leak_prompt_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_prompt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_abort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox exp_value;
        private System.Windows.Forms.TextBox num_input;
    }
}
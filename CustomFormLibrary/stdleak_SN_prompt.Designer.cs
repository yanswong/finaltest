namespace CustomFormLibrary
{
    partial class stdleak_SN_prompt
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
            this.text_input = new System.Windows.Forms.TextBox();
            this.button_abort = new System.Windows.Forms.Button();
            this.button_confirm = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_prompt
            // 
            this.label_prompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_prompt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_prompt.Location = new System.Drawing.Point(91, 32);
            this.label_prompt.Name = "label_prompt";
            this.label_prompt.Size = new System.Drawing.Size(401, 76);
            this.label_prompt.TabIndex = 0;
            this.label_prompt.Text = "\r\nEnter the stdleak serial number.";
            // 
            // text_input
            // 
            this.text_input.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.text_input.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_input.Location = new System.Drawing.Point(124, 197);
            this.text_input.Name = "text_input";
            this.text_input.Size = new System.Drawing.Size(369, 38);
            this.text_input.TabIndex = 0;
            this.text_input.Text = "ENTER HERE.....";
            this.text_input.TextChanged += new System.EventHandler(this.text_input_TextChanged);
            // 
            // button_abort
            // 
            this.button_abort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_abort.Location = new System.Drawing.Point(507, 192);
            this.button_abort.Name = "button_abort";
            this.button_abort.Size = new System.Drawing.Size(103, 50);
            this.button_abort.TabIndex = 2;
            this.button_abort.Text = "ABORT";
            this.button_abort.UseVisualStyleBackColor = true;
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(12, 192);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(98, 50);
            this.button_confirm.TabIndex = 1;
            this.button_confirm.Text = "CONFIRM";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_prompt);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 145);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DESCRIPTION:";
            // 
            // stdleak_SN_prompt
            // 
            this.AcceptButton = this.button_confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_abort;
            this.ClientSize = new System.Drawing.Size(621, 250);
            this.ControlBox = false;
            this.Controls.Add(this.text_input);
            this.Controls.Add(this.button_abort);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.groupBox1);
            this.Name = "stdleak_SN_prompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stdleak Serial Number";
            this.Load += new System.EventHandler(this.stdleak_SN_prompt_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_prompt;
        private System.Windows.Forms.TextBox text_input;
        private System.Windows.Forms.Button button_abort;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
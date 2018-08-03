namespace CustomFormLibrary
{
    partial class userprompt
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
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_abort = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_prompt
            // 
            this.label_prompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_prompt.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_prompt.Location = new System.Drawing.Point(6, 27);
            this.label_prompt.Name = "label_prompt";
            this.label_prompt.Size = new System.Drawing.Size(767, 251);
            this.label_prompt.TabIndex = 0;
            this.label_prompt.Text = "Label Here";
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(12, 315);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(122, 48);
            this.button_confirm.TabIndex = 2;
            this.button_confirm.Text = "OK";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_abort
            // 
            this.button_abort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_abort.Location = new System.Drawing.Point(669, 315);
            this.button_abort.Name = "button_abort";
            this.button_abort.Size = new System.Drawing.Size(122, 48);
            this.button_abort.TabIndex = 3;
            this.button_abort.Text = "ABORT";
            this.button_abort.UseVisualStyleBackColor = true;
            this.button_abort.Click += new System.EventHandler(this.button_abort_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_prompt);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(779, 285);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DESCRIPTION:";
            // 
            // userprompt
            // 
            this.AcceptButton = this.button_confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_abort;
            this.ClientSize = new System.Drawing.Size(804, 376);
            this.ControlBox = false;
            this.Controls.Add(this.button_abort);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.groupBox1);
            this.Name = "userprompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Uer Prompt";
            this.Load += new System.EventHandler(this.userprompt_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_prompt;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_abort;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
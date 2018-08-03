namespace CustomFormLibrary
{
    partial class StartupForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.fe = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.text_serial = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.text_spec = new System.Windows.Forms.TextBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_apply = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkbox_Wireless = new System.Windows.Forms.CheckBox();
            this.checkbox_IOboard = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(44, 59);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(18, 50);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(42, 21);
            this.comboBox2.TabIndex = 0;
            // 
            // fe
            // 
            this.fe.Location = new System.Drawing.Point(9, 14);
            this.fe.Name = "fe";
            this.fe.Size = new System.Drawing.Size(200, 100);
            this.fe.TabIndex = 0;
            this.fe.TabStop = false;
            this.fe.Text = "Model Number:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.text_serial);
            this.groupBox2.Location = new System.Drawing.Point(9, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Serial Number:";
            // 
            // text_serial
            // 
            this.text_serial.Location = new System.Drawing.Point(35, 44);
            this.text_serial.Name = "text_serial";
            this.text_serial.Size = new System.Drawing.Size(121, 20);
            this.text_serial.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.text_spec);
            this.groupBox1.Location = new System.Drawing.Point(9, 226);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spectrometer Serial #:";
            // 
            // text_spec
            // 
            this.text_spec.Location = new System.Drawing.Point(35, 44);
            this.text_spec.Name = "text_spec";
            this.text_spec.Size = new System.Drawing.Size(121, 20);
            this.text_spec.TabIndex = 0;
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(250, 387);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(96, 44);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(250, 329);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(96, 44);
            this.button_apply.TabIndex = 4;
            this.button_apply.Text = "Apply";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.checkbox_Wireless);
            this.groupBox3.Controls.Add(this.comboBox2);
            this.groupBox3.Controls.Add(this.checkbox_IOboard);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(9, 332);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Option(s):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Slot No.";
            // 
            // checkbox_Wireless
            // 
            this.checkbox_Wireless.AutoSize = true;
            this.checkbox_Wireless.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkbox_Wireless.Location = new System.Drawing.Point(92, 66);
            this.checkbox_Wireless.Name = "checkbox_Wireless";
            this.checkbox_Wireless.Size = new System.Drawing.Size(106, 17);
            this.checkbox_Wireless.TabIndex = 7;
            this.checkbox_Wireless.Text = "Wireless Remote";
            this.checkbox_Wireless.UseVisualStyleBackColor = true;
            // 
            // checkbox_IOboard
            // 
            this.checkbox_IOboard.AutoSize = true;
            this.checkbox_IOboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkbox_IOboard.Location = new System.Drawing.Point(92, 35);
            this.checkbox_IOboard.Name = "checkbox_IOboard";
            this.checkbox_IOboard.Size = new System.Drawing.Size(73, 17);
            this.checkbox_IOboard.TabIndex = 6;
            this.checkbox_IOboard.Text = "I/O Board";
            this.checkbox_IOboard.UseVisualStyleBackColor = true;
            // 
            // StartupForm
            // 
            this.AcceptButton = this.button_apply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(360, 439);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.fe);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_apply);
            this.Controls.Add(this.groupBox3);
            this.Name = "StartupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartupForm";
            this.Load += new System.EventHandler(this.StartupForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox fe;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox text_serial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox text_spec;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkbox_Wireless;
        private System.Windows.Forms.CheckBox checkbox_IOboard;
    }
}
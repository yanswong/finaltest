using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomFormLibrary
{
    public partial class stdleak_SN_prompt : Form
    {
        private static string std_serialnum;

        public static string Std_serialnum
        {
            get { return std_serialnum; }

            set { std_serialnum = value; }
        }

        public stdleak_SN_prompt()
        {
            InitializeComponent();
        }

        private void stdleak_SN_prompt_Load(object sender, EventArgs e)
        {
            button_confirm.Enabled = false;
        }

        private void text_input_TextChanged(object sender, EventArgs e)
        {
            button_confirm.Enabled = true;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            Std_serialnum = text_input.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}

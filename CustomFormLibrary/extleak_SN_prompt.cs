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
    public partial class extleak_SN_prompt : Form
    {
        private static string extleak_SN;

        public static string Extleak_SN
        {
            get { return extleak_SN; }
            set { extleak_SN = value; }
        }

        public extleak_SN_prompt()
        {
            InitializeComponent();
        }

        private void extleak_SN_prompt_Load(object sender, EventArgs e)
        {
            button_confirm.Enabled = false;
        }

        private void text_input_TextChanged(object sender, EventArgs e)
        {
            button_confirm.Enabled = true;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            Extleak_SN = text_input.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}

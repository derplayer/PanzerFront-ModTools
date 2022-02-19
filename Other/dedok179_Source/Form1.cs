using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Panzer_Front_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dialogPS1.InitialDirectory = Environment.CurrentDirectory;
            dialogDC.InitialDirectory = Environment.CurrentDirectory;
            dialogPVR.InitialDirectory = Environment.CurrentDirectory;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (checkPS1.Checked == true)
            {
                if (dialogPS1.ShowDialog() == DialogResult.Cancel)
                    return;
                Tex.TKD3_tex_All(dialogPS1.FileName);
            }

            else
            {
                if (dialogPVR.ShowDialog() == DialogResult.Cancel)
                    return;
                Tex.getDCTexture(dialogPVR.FileName);
            }
        }

        private void expModels_Click(object sender, EventArgs e)
        {
            Model.onPS1 = checkPS1.Checked;
            Model.onDC = checkDC.Checked;

            if (checkPS1.Checked == true)
            {
                if (dialogPS1.ShowDialog() == DialogResult.Cancel)
                    return;
                Model.exportPS1(dialogPS1.FileName);
            }

            else
            {
                if (dialogDC.ShowDialog() == DialogResult.Cancel)
                    return;
                Model.exportDC(dialogDC.FileName);
            }
        }
    }
}

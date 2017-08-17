using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace TextEditorCSharp
{
    public partial class Form1 : Form
    {
        string[] fileAsArray;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            //tb.Size = Form1.ActiveForm.Size;
            tb.Width = Form1.ActiveForm.Width - 19;
            tb.Height = Form1.ActiveForm.Height - 30;
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            tb.Width = Form1.ActiveForm.Width - 19;
            tb.Height = Form1.ActiveForm.Height - 30;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Text (*.txt)|*.txt";
            if (System.Windows.Forms.DialogResult.OK == openfile.ShowDialog())
            {
                string myfile = openfile.FileName;
                tb.Text = File.ReadAllText(myfile);
                fileAsArray = File.ReadAllLines(myfile);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Text (*.txt)|*.txt";
            if (System.Windows.Forms.DialogResult.OK == savefile.ShowDialog())
            {
                string writefile = savefile.FileName;
                File.WriteAllText(writefile, tb.Text);
            }
        }
    }
}

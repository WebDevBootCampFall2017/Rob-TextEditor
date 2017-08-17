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
        string[] fileAsArray; string myfile;

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
                try
                {
                    myfile = openfile.FileName;
                    tb.Text = File.ReadAllText(myfile);
                    fileAsArray = File.ReadAllLines(myfile);
                    MessageBox.Show("File Opened From: " + myfile);
                }
                catch(Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(myfile, tb.Text);
                MessageBox.Show("File Written to: " + myfile);
            }
            catch(Exception)
            {
                MessageBox.Show("Can't save to unknown file path, try 'Open' or 'Save as'");
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Text (*.txt)|*.txt";
            if (System.Windows.Forms.DialogResult.OK == savefile.ShowDialog())
            {
                try
                {
                    myfile = savefile.FileName;
                    File.WriteAllText(myfile, tb.Text);
                    MessageBox.Show("File Written to: " + myfile);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }
    }
}

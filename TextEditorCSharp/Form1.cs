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

namespace TextEditorCSharp
{
    public partial class Form1 : Form
    {        
        //make a string array in case we want to use the file data as an array (unused)
        //make a string for the file path, to be utilized in any function
        string[] fileAsArray; string filePath;

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        public Form1()
        {
            //Initialize form and set word wrap, and black font color, as checked by default
            InitializeComponent();
            wordWrapToolStripMenuItem.Checked = true;
            blackToolStripMenuItem.Checked = true;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {            
            //whenever you resize a form set the text box to match the width, and the height to
            //match the height of the form minus the height of the menustrip
            tb.Width = Form1.ActiveForm.Width - 16;
            tb.Height = Form1.ActiveForm.Height - menuStrip1.Height - 42;
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //Unused code to be removed
            //tb.Width = Form1.ActiveForm.Width - 19;
            //tb.Height = Form1.ActiveForm.Height - 30;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create new instance of class OpenFileDialog and set it's paramaters
            OpenFileDialog openfile = new OpenFileDialog();
            //Set filter to only allow users to open text files
            openfile.Filter = "Text (*.txt)|*.txt";
            if (System.Windows.Forms.DialogResult.OK == openfile.ShowDialog())
            {
                //if user clicks ok try to open file
                try
                {
                    //set file path as string equal to file name of the instantiated class
                    filePath = openfile.FileName;
                    //read all text from file path as a single string
                    //and write it to the text box
                    tb.Text = File.ReadAllText(filePath);
                    //save the file data as an array (unused)
                    fileAsArray = File.ReadAllLines(filePath);
                    //show where we opened the file from
                    MessageBox.Show("File Opened From: " + filePath);
                    Form1.ActiveForm.Text = filePath;
                }
                //if anything goes wrong, catch the error, prevent a crash
                catch(Exception error)
                {
                    //tell the user the exact error message
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //write text in text box to previously chosen file path
                File.WriteAllText(filePath, tb.Text);
                //tell user where we wrote the file
                MessageBox.Show("File Written to: " + filePath);
            }
            //attempt to guess extremely specific error exception
            //just in case, tell user exact error
            //works both ways :)
            catch(Exception error)
            {
                MessageBox.Show("[Can't save to unknown file path, try 'Save As']\n" + error);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //same as before but with save instead of open
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Text (*.txt)|*.txt";
            if (System.Windows.Forms.DialogResult.OK == savefile.ShowDialog())
            {
                //try to save file at path chosen in save file dialog
                try
                {
                    filePath = savefile.FileName;
                    File.WriteAllText(filePath, tb.Text);
                    MessageBox.Show("File Written to: " + filePath);
                    Form1.ActiveForm.Text = filePath;
                }
                //throw error message for every exception
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string findWhat = Prompt.ShowDialog("Find what?", "Find");
            int findIndex = tb.Text.IndexOf(findWhat);
            MessageBox.Show("word found at index " + findIndex);
        }

        private void andReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string replaceWhat = Prompt.ShowDialog("Replace what?", "Find & Replace");
            string withWhat = Prompt.ShowDialog("With what?", "Find & Replace");
            string grabText = tb.Text;
            string newText = grabText.Replace(replaceWhat, withWhat);
            tb.Text = newText;
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if word wrap is checked
            if(wordWrapToolStripMenuItem.Checked == true)
            {
                //uncheck if previously checked and set word wrap off
                wordWrapToolStripMenuItem.Checked = false;
                tb.WordWrap = false;
                tb.ScrollBars = ScrollBars.Both;
            }
            else
            {
                //check if previously unchecked and set word wrap on
                wordWrapToolStripMenuItem.Checked = true;
                tb.WordWrap = true;
                tb.ScrollBars = ScrollBars.Vertical;
            }
        }      


        private void wordWrapToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            /*if (wordWrapToolStripMenuItem.Checked == true)
            {                
                tb.ScrollBars = ScrollBars.Vertical;
            }
            else
            {
                tb.ScrollBars = ScrollBars.Both;
            }*/
        }
    }
}

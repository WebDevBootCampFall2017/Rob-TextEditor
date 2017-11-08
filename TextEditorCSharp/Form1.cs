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
        /*string[] fileAsArray;*/ string filePath; string safeFilePath; string chosenColor; float fontSize; string appName = "Let's Get Textual! Text Editor";
        bool changed; bool saved;
        /*List<FoundIndex> fi;

        class FoundIndex
        {
            public string value;
            public int index;
            //public List<FoundIndex> fi;
        }*/

        

        public static class Prompt
        {
            //method show dialog where you can customize values of text and caption to be displayed
            public static string ShowDialog(string text, string caption)
            {
                //Constructor for a new form where we are showing everything
                Form prompt = new Form()
                {
                    //Alternate form of class attribute assignment
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 500 };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;
                //Shortcut if-then statement to check if user pressed ok
                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : "";
            }

            public static bool ShowOkCancel(string text, string caption, string okTxt, string noTxt)
            {
                //Constructor for a new form where we are showing everything
                Form prompt = new Form()
                {
                    //Alternate form of class attribute assignment
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 500 };
                Button okButton = new Button() { Text = okTxt, Left = 250, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                Button cancelButton = new Button() { Text = noTxt, Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.Cancel };
                okButton.Click += (sender, e) => { prompt.Close(); };
                cancelButton.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(okButton);
                prompt.Controls.Add(cancelButton);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = okButton;
                prompt.CancelButton = cancelButton;
                //Shortcut if-then statement to check if user pressed ok
                return prompt.ShowDialog() == DialogResult.OK ? true : false;
            }
        }

        void ChangeTheme(int themeNum)
        {
            switch (themeNum)
            {
                //Default theme
                case 0:
                    toolStripMenuItem2.Checked = true;
                    toolStripMenuItem3.Checked = false;
                    toolStripMenuItem4.Checked = false;
                    menuStrip1.BackColor = SystemColors.Control;
                    menuStrip1.ForeColor = Color.Black;

                    rtb.ForeColor = Color.Black;
                    rtb.BackColor = Color.White;
                    //Form1.ActiveForm.BackColor = menuStrip1.BackColor;
                    break;
                //Dark theme
                case 1:
                    toolStripMenuItem3.Checked = true;
                    toolStripMenuItem2.Checked = false;
                    toolStripMenuItem4.Checked = false;
                    menuStrip1.BackColor = SystemColors.ControlDarkDark;
                    menuStrip1.ForeColor = Color.WhiteSmoke;

                    rtb.ForeColor = Color.WhiteSmoke;
                    rtb.BackColor = SystemColors.ControlDarkDark;
                    //Form1.ActiveForm.BackColor = menuStrip1.BackColor;
                    break;
                //Fire theme
                case 2:
                    toolStripMenuItem4.Checked = true;
                    toolStripMenuItem2.Checked = false;
                    toolStripMenuItem3.Checked = false;
                    menuStrip1.BackColor = Color.Firebrick;
                    menuStrip1.ForeColor = Color.LightYellow;

                    rtb.ForeColor = Color.Yellow;
                    rtb.BackColor = Color.OrangeRed;
                    //Form1.ActiveForm.BackColor = menuStrip1.BackColor;
                    break;
            }
            //If custom font color...(ORIGINAL)
            /*if (chosenColor != null)
            {//Ask if the user wants to keep it
                string keepColor = Prompt.ShowDialog("Would you like to keep your custom font color?", "Keep font color?").ToUpper();

                if (keepColor == "YES")
                {//User wants to keep his custom font color
                    rtb.ForeColor = Color.FromName(chosenColor);
                }
                else
                {//User doesn't want to keep custom font color, trash custom color reference
                    chosenColor = null;
                }
            }*/
            //If custom font color...
            if (chosenColor != null)
            {//Keep the custom font color
                rtb.ForeColor = Color.FromName(chosenColor);
            }
        }

        public Form1()
        {
            //Initialize form and set word wrap, and black font color, as checked by default
            InitializeComponent();
            //trying to fix default forecolor bug
            //rtb.ForeColor = Color.FromName("Black");
            wordWrapToolStripMenuItem.Checked = true;
            toolStripMenuItem2.Checked = true;
            //Shortcut Keys
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S | Keys.Shift;
            //findToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            everyInstanceToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.H;
            chooseInstanceToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.H;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (changed == true & saved == false)//If user has made a change but not saved...
            {//ask if they want to:
                var confirm = Prompt.ShowOkCancel("You have made changes to the text, save your " +
                    "file before opening a new one?", "Save Changes?", "Yes", "No");
                switch (confirm)
                {
                    case true://Save changes and open new file
                        saveToolStripMenuItem_Click(sender, e);
                        openToolStripMenuItem_Click(sender, e);
                        break;
                    case false://Scrap changes and open new file
                        MessageBox.Show("OK! Your funeral!");
                        changed = false; saved = false;
                        openToolStripMenuItem_Click(sender, e);
                        break;
                }
            }
            else//Else just go ahead and open a file
            {
                //create new instance of class OpenFileDialog and set it's paramaters
                OpenFileDialog openfile = new OpenFileDialog();
                //Set filter to only allow users to open text files
                openfile.Filter = "Text (*.txt)|*.txt";
                if (System.Windows.Forms.DialogResult.OK == openfile.ShowDialog())//if user clicks ok try to open file
                {                    
                    try
                    {
                        //set file path as string equal to file name of the instantiated class
                        filePath = openfile.FileName;
                        safeFilePath = openfile.SafeFileName;
                        //read all text from file path as a single string
                        //and write it to the text box
                        rtb.Text = File.ReadAllText(filePath);
                        //save the file data as an array (unused)
                        //fileAsArray = File.ReadAllLines(filePath);
                        //show where we opened the file from
                        MessageBox.Show("File Opened From: " + filePath);
                        //Show file name on title bar
                        //Also keep application name on title bar
                        Form1.ActiveForm.Text = String.Format("({0}) {1}", safeFilePath, appName);
                        changed = false; saved = false;
                    }
                    //if anything goes wrong, catch the error, prevent a crash
                    catch (Exception error)
                    {
                        //tell the user the exact error message
                        MessageBox.Show(error.Message);
                    }
                }
            }
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (filePath != null)//Only attempt if we know the path of the file
            {
                try
                {                   
                    File.WriteAllText(filePath, rtb.Text);//write text in text box to previously chosen file path
                    MessageBox.Show("File Written to: " + filePath);//tell user where we wrote the file
                    Form1.ActiveForm.Text = String.Format("({0}) {1}", safeFilePath, appName);//Show file name in title bar
                    changed = false; saved = true;
                }
                //attempt to guess extremely specific error exception
                //tell user exact error
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else//Else make them do save as function
            {
                MessageBox.Show("Where should I save this file?");
                saveAsToolStripMenuItem_Click(sender, e);
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
                    safeFilePath = Path.GetFileName(filePath);
                    File.WriteAllText(filePath, rtb.Text);
                    MessageBox.Show("File Written to: " + filePath);
                    Form1.ActiveForm.Text = String.Format("({0}) {1}", safeFilePath, appName);
                    changed = false; saved = true;
                    rtb.Focus();
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
            string findWhat = Prompt.ShowDialog("Insert a string of characters to find in the text", "Find");
            int findIndex = rtb.Text.IndexOf(findWhat);
            //Index of a word not found will be set to -1, check for it
            if (findIndex != -1)
            {
                //MessageBox.Show("Found at index " + findIndex);
                rtb.Focus();
                rtb.Select(findIndex, findWhat.Length);
            }
            else
            {
                MessageBox.Show("Not found in entire text file!\n" +
                    "Remember the find function is 'case-sensitive'!\n" +
                    "Try checking your capitalizations!");
            }
        }

        private void andReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void chooseInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
           /* try
            {
                string replaceWhat = Prompt.ShowDialog("Replace what? (Case-Sensitive)", "Find & Replace");
                string withWhat = Prompt.ShowDialog("With what? (Case-Sensitive)", "Find & Replace");
                var grabtext = rtb.Text;
                int[] indexArray;
                //Check if the text contains what we're looking for before we do anything
                //fi.Clear;
                if (grabtext.Contains(replaceWhat))
                {
                    int foundIndex = grabtext.IndexOf(replaceWhat);
                    fi.Add(index foundIndex)
                    
                }
                else
                {
                    MessageBox.Show("Could not find what you are looking for in the text!\n" +
                "Remember the find function is 'case-sensitive'!\n" +
                "Try checking your capitalizations!");
                }
            }
            catch (ArgumentException error)
            {
                MessageBox.Show("You failed to provide text to replace!\n" + error.Message);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }*/
        }

        private void everyInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string replaceWhat = Prompt.ShowDialog("Replace what?", "Find & Replace");
            string withWhat = Prompt.ShowDialog("With what?", "Find & Replace");
            string grabText = rtb.Text;
            if (grabText.Contains(replaceWhat))
            {
                string newText = grabText.Replace(replaceWhat, withWhat);
                rtb.Text = newText;
                MessageBox.Show("You have replaced every instance of '" + replaceWhat + "' in the textbox with '" + withWhat + "' !");
            }
            else
            {
                MessageBox.Show("Could not find what you were looking for in the text!\n" +
                "Remember the find function is 'case-sensitive'!\n" +
                "Try checking your capitalizations!");
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if word wrap is checked
            if (wordWrapToolStripMenuItem.Checked == true)
            {
                //uncheck if previously checked and set word wrap off
                wordWrapToolStripMenuItem.Checked = false;
                rtb.WordWrap = false;
            }
            else
            {
                //check if previously unchecked and set word wrap on
                wordWrapToolStripMenuItem.Checked = true;
                rtb.WordWrap = true;
            }
        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(appName + " is a unique and fun text editor for editing and saving your text files on the fly!\n" +
                "Original Author: Robert Tripp Ross IV\n" +
                "V0.5\n" +
                "Course Name: Web Development And Coding Bootcamp");
            rtb.Focus();
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Ask for a font color
            string askColor = Prompt.ShowDialog("Pick a color:", "Change Font Color");
            //Only change font color if user provides a new one
            //if (Color.FromName(askColor) != rtb.ForeColor)
            if (askColor != rtb.ForeColor.ToString())
            {
                rtb.ForeColor = Color.FromName(askColor);
                chosenColor = askColor;
            }
            else
            {
                MessageBox.Show("The color specified is already chosen");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //whenever you resize a form set the text box to match the width, and the height to
            //match the height of the form minus the height of the menustrip
            //Removed code to use docking for efficiency
            /*rtb.Width = Form1.ActiveForm.Width - 16;
            rtb.Height = Form1.ActiveForm.Height - menuStrip1.Height - 42;*/
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string fontSizeString = Prompt.ShowDialog("Enter a size:", "Font Size Selection");
                fontSize = float.Parse(fontSizeString);
                rtb.Font = new Font(rtb.Font.FontFamily, fontSize, rtb.Font.Style);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Font = new Font(rtb.Font, FontStyle.Bold);
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Font = new Font(rtb.Font, FontStyle.Underline);
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Font = new Font(rtb.Font, FontStyle.Italic);
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog1 = new FontDialog();
            fontDialog1.ShowColor = true;

            fontDialog1.Font = rtb.Font;
            fontDialog1.Color = rtb.ForeColor;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                //Get the font and style chosen in font dialog and assign to text box
                rtb.Font = fontDialog1.Font;
                //If not changing the color, don't set custom string

                if (rtb.ForeColor != fontDialog1.Color)
                {
                    //Change the font color only if different
                    rtb.ForeColor = fontDialog1.Color;
                    //Set font color to string
                    chosenColor = fontDialog1.Color.ToKnownColor().ToString();
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ChangeTheme(0);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ChangeTheme(1);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ChangeTheme(2);
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {/*
            if (e.Alt & e.KeyValue == (int)'F')
            {
                MessageBox.Show("HELLO");
            }*/
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Process myProcess;
        Boolean Flag;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            richTextBox1.Visible = false;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            myProcess = Process.Start(@"Monitor.exe");
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            richTextBox1.Visible = false;
            Flag = true;
            
        }

        private void End_Click(object sender, EventArgs e)
        {

            richTextBox1.Visible = true;
            button1.Enabled = true;

            try
            {
                // Close process by sending a close message to its main window.
                myProcess.CloseMainWindow();
                // Free resources associated with process.
                while (!myProcess.HasExited)
                {
                    Thread.Sleep(100);
                }       
            }
            catch (Exception Exp)
            {
               myProcess.Close();
            }

            Flag = false;
            button2.Enabled = false;
            richTextBox1.Clear();
            File_Read();
           
        }

        private void Close_Click(object sender, EventArgs e)
        {
            try
            {
                if (Flag.Equals(true))
                { // Close process by sending a close message to its main window.
                    myProcess.CloseMainWindow();
                    // Free resources associated with process.
                    myProcess.Close();
                }

                Form1.ActiveForm.Close();
            }
            catch(Exception exp)
            {
                Form1.ActiveForm.Close();
            }

        }

        private void File_Read()
        {
            string fileName = @"%UserProfile%\Documents\Result.txt";
             fileName = Environment.ExpandEnvironmentVariables(fileName);

             while (!File.Exists(fileName))
            {
                Thread.Sleep(500);
            }

             string[] lines = System.IO.File.ReadAllLines(fileName);

            foreach (string line in lines)
            {                
                // Use a tab to indent each line of the file.
                richTextBox1.AppendText(line);
                richTextBox1.AppendText("\n");
                

            }



        }


    }
}

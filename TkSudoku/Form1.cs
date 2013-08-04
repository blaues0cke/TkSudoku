using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace TkSudoku
{
    

    public partial class Form1 : Form
    {
        public bool TkSudokuGameState = false;
        public Thread TkSudokuTimeThread;
        public long TkSudokuGameStartTime;

        public Form1()
        {
            InitializeComponent();
        }


        public void TkSudokuDisableAll()
        {
            for (int y = 1; y <= 9; y++)
            {
                for (int x = 1; x <= 9; x++)
                {
                    groupBox1.Controls["TkSudokuField_" + y + "_" + x].Text = "5";
                    groupBox1.Controls["TkSudokuField_" + y + "_" + x].Enabled = false;
                    groupBox1.Controls["TkSudokuField_" + y + "_" + x].ForeColor = Color.FromArgb(0, 0, 0);
                    groupBox1.Controls["TkSudokuField_" + y + "_" + x].BackColor = Color.FromArgb(153, 153, 153);
                }
            }
        }
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern long GetTickCount();
        public void TkSudokuLoadFiles()
        {
            string[] Files = System.IO.Directory.GetFiles("levels");

            for (int i = 0; i < Files.Length; i++)
            {
                listBox1.Items.Add(Files[i].ToString().Remove(0, 7));
            }
         }



        private void TkSudokuFieldEnter(object sender, EventArgs e)
        {
            if (TkSudokuCheckBox2.Checked == true)
            {
                TkSudokuFieldHighlightByValue(Convert.ToInt16(((TextBox)sender).Text));
            }
            ((TextBox)sender).BackColor = Color.FromArgb(Convert.ToInt16(textBox1.Text), Convert.ToInt16(textBox2.Text), Convert.ToInt16(textBox3.Text));
            ((TextBox)sender).SelectAll();
        }

        private void TkSudokuFieldLeave(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Color.FromArgb(255, 255, 255);
        }

        private void TkSudokuFieldCheck(object sender, EventArgs e)
        {
            Regex TkSudokuCheck = new Regex("([0-9]{1})", RegexOptions.Singleline);
            if (!TkSudokuCheck.IsMatch(((TextBox)sender).Text)) {
                ((TextBox)sender).Text = "";
            }
            ((TextBox)sender).SelectAll();
        }

        private void TkSudokuClearFields()
        {
            for (int y = 1; y <= 9; y++)
            {
                for (int x = 1; x <= 9; x++)
                {
                    groupBox1.Controls["TkSudokuField_" + y + "_" + x].BackColor = Color.FromArgb(255, 255, 255);

                }
            }


        }

        private void TkSudokuFieldHighlightByValue(int TkSudokuHighlightValue)
        {
            for (int y = 1; y <= 9; y++)
            {
                for (int x = 1; x <= 9; x++)
                {
                    if (Convert.ToInt16(groupBox1.Controls["TkSudokuField_" + y + "_" + x].Text) == Convert.ToInt16(TkSudokuHighlightValue))
                    {
                        groupBox1.Controls["TkSudokuField_" + y + "_" + x].BackColor = Color.FromArgb(Convert.ToInt16(textBox6.Text), Convert.ToInt16(textBox5.Text), Convert.ToInt16(textBox4.Text));
                    }
                    else
                    {
                        groupBox1.Controls["TkSudokuField_" + y + "_" + x].BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            }
        }



        private void TkSudokuCheckBox2_Click(object sender, EventArgs e)
        {
            TkSudokuClearFields();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            TkSudokuClearFields();
            TkSudokuDisableAll();
            TkSudokuLoadFiles();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            TkSudokuLoadFiles();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Spiel starten")
            {
                TkSudokuStart();
            }
            else
            {
                TkSudokuEnd();
            }
         


        }


        public void TkSudokuStart()
        {
            TkSudokuStartTimeThread();
            TkSudokuGameStartTime = GetTickCount();
            button1.Text = "Spiel beenden";
        }
        public void TkSudokuEnd()
        {
            TkSudokuStopTimeThread();
            TkSudokuTimeLabel.Text = "Derzeit ist kein Spiel aktiv.";
            button1.Text = "Spiel starten";

        }
        public void TkSudokuStartTimeThread()
        {
            TkSudokuTimeThread = new Thread(new ThreadStart(TkSudokuRunTimeThread));
            TkSudokuTimeThread.Start();
        }
        public void TkSudokuStopTimeThread()
        {
            TkSudokuTimeThread.Abort();
        }
        public void TkSudokuRunTimeThread()
        {
            long gametime = GetTickCount() - TkSudokuGameStartTime;
            string temp = "";
            int seconds = (int)(gametime / 1000);
            int minutes = (int)(seconds / 60);
            seconds = (int)(seconds - (minutes * 60));
            int hours = (int)(minutes / 60);
            minutes = (int)(minutes - (hours * 60));
            int days = (int)(hours / 24);
            hours = (int)(hours - (days * 24));
            if (seconds != 0)
            {
                if (seconds == 1)
                {
                    temp += " " + seconds + " Sekunde";
                }
                else
                {
                    temp += " " + seconds + " Sekunden";
                }
            }


            TkSudokuTimeLabel.Text = temp;
            Thread.Sleep(1000);
            TkSudokuRunTimeThread();


        }


    }
}

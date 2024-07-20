﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AlarmClock
{
    public partial class SoundChoise : Form
    {
        public SoundChoise()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String extension = Path.GetExtension(openFileDialog1.FileName);
                
                if (extension != ".wav")
                {
                    MessageBox.Show("Это не wav! Выберите другой файл!");
                    Close();
                }

                textBox1.Text = Path.GetFileName(openFileDialog1.FileName);
                textBox2.Text = Path.GetFullPath(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string curPath = Directory.GetCurrentDirectory();

            if (textBox1.Text != "" || textBox2.Text != "")
            {
                if (File.Exists(curPath + "/Sound.wav"))
                {
                    File.Delete(curPath + "/Sound.wav");
                }

                File.Copy(textBox2.Text, curPath + "\\" + "Sound.wav");
                MessageBox.Show("Звук будильника установлен!");
                Close();
            }
            else Close();
        }
    }
}

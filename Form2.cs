using System;
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
                
                if (extension != ".mp3")
                {
                    MessageBox.Show("Это не mp3! Выберите другой файл!");
                    Close();
                }

                textBox1.Text = Path.GetFileName(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string curPath = Directory.GetCurrentDirectory();

            if (textBox1.Text != "")
            {
                if (File.Exists(curPath + "/Sound.mp3"))
                {
                    File.Delete(curPath + "/Sound.mp3");
                }

                File.Copy(Path.GetFullPath(openFileDialog1.FileName), curPath + "\\" + "Sound.mp3");
                MessageBox.Show("Звук будильника установлен!");
                Close();
            }
            else Close();
        }
    }
}

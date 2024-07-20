using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace AlarmClock
{
    public partial class AC : Form
    {

        SoundPlayer player = new SoundPlayer();
        public AC()
        {
            InitializeComponent();
            checkSound();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Visible = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (checkSound() == false)
            {
                Close();
            }

            timer1.Start();
            lblStatus.Text = "Будильник запущен!";
            btnStart.Enabled = false;
            btnStop.Visible = true;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Будильник выключен!";
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            player.Stop();  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            DateTime alarmTime = dateTimePicker1.Value;

            if (currentTime.Hour == alarmTime.Hour && currentTime.Minute == alarmTime.Minute && currentTime.Second == alarmTime.Second)
            {
                timer1.Stop();

                player.SoundLocation = @"Sound.mp3";
                player.PlayLooping();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            SoundChoise sc = new SoundChoise();
            sc.FormClosed += soundChoiseClosed;
            sc.Show();
        }

        private void soundChoiseClosed(object sender, FormClosedEventArgs e)
        {
            checkSound();
        }

        bool checkSound()
        {
            bool isHave = true;

            if (!File.Exists(@"Sound.mp3"))
            {
                btnStart.Enabled = false;
                MessageBox.Show("Звуковой файл не обнаружен! Добавьте его.");
                isHave = false;
            }
            else btnStart.Enabled = true;

            return isHave;
        }
    }
}

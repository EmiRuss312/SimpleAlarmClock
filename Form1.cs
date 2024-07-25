using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

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

            btnStop.Visible = true;
            btnStart.Visible = false;
            AddButton.Visible = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Будильник выключен!";
            btnStart.Visible = true;
            AddButton.Visible = true;
            btnStop.Visible = false;
            player.Stop();  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            DateTime alarmTime = dateTimePicker1.Value;

            if (currentTime.Hour == alarmTime.Hour && currentTime.Minute == alarmTime.Minute && currentTime.Second == alarmTime.Second)
            {
                timer1.Stop();

                player.SoundLocation = @"Sound.wav";
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

            if (!File.Exists(@"Sound.wav"))
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

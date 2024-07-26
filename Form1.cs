using NAudio.Wave;
using System;
using System.IO;
using System.Windows.Forms;

namespace AlarmClock
{
    public partial class AC : Form
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
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
            AddButton.Enabled = false;
            dateTimePicker1.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Будильник выключен!";
            btnStart.Visible = true;
            AddButton.Enabled = true;
            btnStop.Visible = false;
            dateTimePicker1.Enabled = true;

            waveOutDevice?.Stop();
            audioFileReader?.Dispose();
            waveOutDevice?.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            DateTime alarmTime = dateTimePicker1.Value;

            if (currentTime.Hour == alarmTime.Hour && currentTime.Minute == alarmTime.Minute && currentTime.Second == alarmTime.Second)
            {
                timer1.Stop();

                waveOutDevice = new WaveOut();
                audioFileReader = new AudioFileReader("Sound.mp3");
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.PlaybackStopped += onPlaybackStopped;
                waveOutDevice.Play();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            SoundChoise sc = new SoundChoise();
            sc.FormClosed += soundChoiseClosed;
            sc.Show();
        }

        private void onPlaybackStopped(object sender, StoppedEventArgs e)
        {
            audioFileReader.Position = 0;
            waveOutDevice.Play();
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

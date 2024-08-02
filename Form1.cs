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

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            AC_NotifyIcon.DoubleClick += (sender, e) => {
                this.Show();
            };
            AC_NotifyIcon.ContextMenuStrip = new ContextMenuStrip();
            AC_NotifyIcon.ContextMenuStrip.Items.Add("Закрыть приложение", null, ExitApplication);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStop.Visible = false;
        }

        private void AC_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
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
            btnLoadTimePreset.Enabled = false;
            btnSoundCheck.Enabled = false;

            saveTimePreset("alarmTime.txt");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Будильник выключен!";
            btnStart.Visible = true;
            AddButton.Enabled = true;
            dateTimePicker1.Enabled = true;
            btnLoadTimePreset.Enabled = true;
            btnSoundCheck.Enabled = true;
            btnStop.Visible = false;

            waveOutDevice?.Stop();
            audioFileReader?.Dispose();
            waveOutDevice?.Dispose();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            SoundChoise sc = new SoundChoise();
            sc.FormClosed += soundChoiseClosed;
            sc.Show();
        }

        private void btnLoadTimePreset_Click(object sender, EventArgs e)
        {
            loadTimePreset("alarmTime.txt");
        }

        private void btnSoundCheck_Click(object sender, EventArgs e)
        {
            if (waveOutDevice != null && waveOutDevice.PlaybackState == PlaybackState.Playing)
            {
                waveOutDevice.Stop();
                waveOutDevice.Dispose();
                waveOutDevice = null;
                audioFileReader.Dispose();
                audioFileReader = null;

                dateTimePicker1.Enabled = true;
                btnStart.Enabled = true;
                btnLoadTimePreset.Enabled = true;
                AddButton.Enabled = true;
            }
            else
            {
                waveOutDevice = new WaveOut();
                audioFileReader = new AudioFileReader("Sound.mp3");
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();

                dateTimePicker1.Enabled = false;
                btnStart.Enabled = false;
                btnLoadTimePreset.Enabled = false;
                AddButton.Enabled = false;
            }
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
                btnSoundCheck.Enabled = false;
                MessageBox.Show("Звуковой файл не обнаружен! Добавьте его.");
                isHave = false;
            }
            else
            {
                btnStart.Enabled = true;
                btnSoundCheck.Enabled = true;
            }

            return isHave;
        }

        private void saveTimePreset(string filePath)
        {
            DateTime selectedTime = dateTimePicker1.Value;
            string timeToSave = selectedTime.ToString("HH:mm:ss");

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(timeToSave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении времени: {ex.Message}");
            }
        }

        private void loadTimePreset(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string timeString;
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        timeString = reader.ReadLine();
                    }

                    DateTime savedTime = DateTime.ParseExact(timeString, "HH:mm:ss", null);

                    dateTimePicker1.Value = DateTime.Today.Add(savedTime.TimeOfDay);
                    MessageBox.Show("Время успешно загружено!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке времени: {ex.Message}");
                }
            }
            else MessageBox.Show("Пресет не обнаружен! Вы еще не запускали будильник!");
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

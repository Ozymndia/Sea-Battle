using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public class Time
    {
        public int elapsedTime;
        public MainForm gameForm;
        public Timer gameTimer;
        public Label timerLabel;
        public TimeSpan bestTimeSpan = TimeSpan.MaxValue;

        public Time(MainForm gameForm)
        {
            this.gameForm = gameForm;
            timerLabel = new Label();
            timerLabel.Location = new Point(gameForm.scoreBoard.scoreLabel.Location.X,
                gameForm.scoreBoard.scoreLabel.Location.Y + gameForm.scoreBoard.scoreLabel.Height + 10);
            elapsedTime = 0;
            timerLabel.Text = "00:00:00";
            timerLabel.AutoSize = true;
            AddTimer();
        }
        public void AddTimer()
        {
            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;
            gameForm.Controls.Add(timerLabel);
        }
        public void GameTimer_Tick(object sender, EventArgs e)
        {
            elapsedTime++;
            timerLabel.Text = FormatTime(elapsedTime);
        }

        public string FormatTime(int elapsedTime)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        public void ResetTimer()
        {
            elapsedTime = 0;
            timerLabel.Text = FormatTime(elapsedTime);
        }

        public void UpdateBestTime(TimeSpan newTime)
        {
            if (newTime < bestTimeSpan)
            {
                bestTimeSpan = newTime;
            }
        }
    }
}

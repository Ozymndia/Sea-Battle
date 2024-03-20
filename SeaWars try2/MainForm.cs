using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace SeaWars_try2
{
    public partial class MainForm : Form
    {
        private const string SaveFileName = "sea_wars_save.dat";
        private bool isFirstPlayerMove = true;

        private PlayerMap playerMap;
        private BotMap botMap;
        private Shooting shooting;
        private Bot bot;
        public ScoreBoard scoreBoard;
        private Time timer;
        private Label bestTimeLabel;

        public MainForm()
        {
            InitializeComponent();
            SetupWindow();
            
            playerMap = new PlayerMap(this);
            botMap = new BotMap(this);
            shooting = new Shooting(this, botMap);
            scoreBoard = new ScoreBoard(this);
            timer = new Time(this);

            Init();

        }
        public void Init()
        {
            bot = new Bot(botMap, playerMap);
            AddControls();
            AddRestartButton();
            AddSaveButton();
            AddResetButton();
            AddBestTimeLabel();
            LoadGame();

        }

        private void AddControls()
        {
            for (int i = 0; i < GameMap.Size; i++)
            {
                for (int j = 0; j < GameMap.Size; j++)
                {
                    Controls.Add(playerMap.buttons[i, j]);
                    Controls.Add(botMap.buttons[i, j]);
                }
            }
        }

        private void AddRestartButton()
        {
            Button restartButton = new Button();
            restartButton.Text = "Перезапуск";
            restartButton.Click += new EventHandler(RestartGame);
            restartButton.Location = new Point(10, GameMap.Size * GameMap.cellSize + 20);
            restartButton.Width = 100;
            restartButton.Height = 30;
            this.Controls.Add(restartButton);
        }
        private void AddSaveButton()
        {
            Button saveButton = new Button();
            saveButton.Text = "Сохранить";
            saveButton.Click += new EventHandler(SaveButton_Click);
            saveButton.Location = new Point(10, GameMap.Size * GameMap.cellSize + 60);
            saveButton.Width = 100;
            saveButton.Height = 30;
            this.Controls.Add(saveButton);
        }
        private void AddResetButton()
        {
            Button resetButton = new Button();
            resetButton.Text = "Сбросить";
            resetButton.Click += ResetButton_Click;
            resetButton.Location = new Point((ClientSize.Width - 120), GameMap.Size * GameMap.cellSize + 30);
            resetButton.Width = 100;
            resetButton.Height = 30;
            this.Controls.Add(resetButton);
        }
        private void AddBestTimeLabel()
        {
            bestTimeLabel = new Label();
            bestTimeLabel.Text = "Лучшее время: ";
            bestTimeLabel.AutoSize = true;
            bestTimeLabel.Location = new Point((ClientSize.Width - 160), GameMap.Size * GameMap.cellSize + 60);
            this.Controls.Add(bestTimeLabel);
        }


        private void SetupWindow()
        {
            AutoScaleDimensions = new SizeF(8, 15);
            AutoScaleMode = AutoScaleMode.Font;
            Font = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Point, 186);
            Margin = Padding.Empty;
            Text = "Морской бой";
            BackColor = Color.FromArgb(235, 235, 235);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;    
        }

        private void RestartGame(object sender, EventArgs e)
        {
            isFirstPlayerMove = true;
            playerMap.ClearAllShipsPlacement(playerMap.buttons);
            botMap.ClearAllShipsPlacement(botMap.buttons);
            playerMap.PlaceAllShips();
            botMap.PlaceAllShips();
            bot.ResetHitLocations();
            timer.gameTimer.Stop();
            timer.ResetTimer();
        }

        private void SaveGame()
        {
            try
            {
                using (FileStream fs = new FileStream(SaveFileName, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    GameSaveData saveData = new GameSaveData(scoreBoard.playerScore, scoreBoard.botScore, timer.bestTimeSpan);
                    formatter.Serialize(fs, saveData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения игры: " + ex.Message);
            }
        }

        private void LoadGame()
        {
            if (File.Exists(SaveFileName))
            {
                try
                {
                    using (FileStream fs = new FileStream(SaveFileName, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        GameSaveData saveData = (GameSaveData)formatter.Deserialize(fs);
                        scoreBoard.playerScore = saveData.PlayerScore;
                        scoreBoard.botScore = saveData.BotScore;
                        scoreBoard.scoreLabel.Text = $"{scoreBoard.playerScore}:{scoreBoard.botScore}";

                        timer.bestTimeSpan = saveData.BestTime;
                        if (timer.bestTimeSpan != TimeSpan.MaxValue)
                        {
                            bestTimeLabel.Text = "Лучшее время: " + timer.bestTimeSpan.ToString("mm\\:ss");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки игры: " + ex.Message);
                }
            }
        }

        private void ResetGameProgress()
        {
            scoreBoard.playerScore = 0;
            scoreBoard.botScore = 0;
            scoreBoard.scoreLabel.Text = $"{scoreBoard.playerScore}:{scoreBoard.botScore}";
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetGameProgress();
            timer.bestTimeSpan = TimeSpan.MaxValue;
            bestTimeLabel.Text = "";
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveGame();
        }

        public bool CheckIfAllShipsDestroyed(int[,] map)
        {
            for (int i = 0; i < GameMap.Size; i++)
            {
                for (int j = 0; j < GameMap.Size; j++)
                {
                    if (map[i, j] == 1)
                    { 
                        return false;
                    }
                }
            }
            return true;
        }
        public void OnPlayerCellClick(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            if (pressedButton.Enabled)
            {
                if (isFirstPlayerMove)
                {
                    timer.gameTimer.Start();
                    isFirstPlayerMove = false;
                }

                bool playerTurn = shooting.Shoot(pressedButton);
                if (CheckIfAllShipsDestroyed(botMap.field))
                {
                    timer.gameTimer.Stop();
                    timer.UpdateBestTime(TimeSpan.FromSeconds(timer.elapsedTime));
                    bestTimeLabel.Text = "Лучшее время: " + timer.bestTimeSpan.ToString("mm\\:ss");
                    MessageBox.Show("Поздравляем! Вы победили!");
                    scoreBoard.playerScore++;
                    scoreBoard.scoreLabel.Text = $"{scoreBoard.playerScore}:{scoreBoard.botScore}";
                    return;
                }

                while (!playerTurn)
                {
                    playerTurn = !bot.Shoot();
                    if (CheckIfAllShipsDestroyed(playerMap.field))
                    {
                        timer.gameTimer.Stop();
                        MessageBox.Show("Вы проиграли. Попробуйте ещё раз!");
                        scoreBoard.botScore++;
                        scoreBoard.scoreLabel.Text = $"{scoreBoard.playerScore}:{scoreBoard.botScore}";
                        return;
                    }
                }
                pressedButton.Enabled = false;
            }
        }
    }
}

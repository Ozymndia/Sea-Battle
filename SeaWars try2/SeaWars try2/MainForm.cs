using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeaWars_try2.Bot;


namespace SeaWars_try2
{
    public partial class MainForm : Form
    {
        public GameMap gameMap;
        PlayerMap playerMap;
        BotMap botMap;
        public Shooting shooting;
        public bool isPlaying;
        public BotMain bot;

        public MainForm()
        {
            InitializeComponent();
            SetupWindow();
            playerMap = new PlayerMap(this);
            botMap = new BotMap(this);
            shooting = new Shooting(this);
            Init();
        }


        public void Init()
        {
            isPlaying = false;
            bot = new BotMain(botMap, playerMap);
            AddControls();
            AddStartButton();
            AddResetButton();
        }

        private void AddControls()
        {
            for (int i = 0; i < GameMap.Size; i++)
            {
                for (int j = 0; j < GameMap.Size; j++)
                {
                    Controls.Add(playerMap.Buttons[i, j]);
                    Controls.Add(botMap.Buttons[i, j]);
                }
            }
        }

        private void AddStartButton()
        {
            Button startButton = new Button();
            startButton.Text = "Начать";
            startButton.Click += new EventHandler(StartGame);
            startButton.Location = new Point(0, GameMap.Size * GameMap.cellSize + 20);
            this.Controls.Add(startButton);
        }
        private void AddResetButton()
        {
            Button resetButton = new Button();
            resetButton.Text = "Перезапустить";
            resetButton.Click += new EventHandler(ResetGame);
            resetButton.Location = new Point(0, GameMap.Size * GameMap.cellSize + 45);
            this.Controls.Add(resetButton);
        }
        public void ResetGame(object sender, EventArgs e)
        {
            isPlaying = false;
            playerMap.ClearAllShipsPlacement(playerMap.buttons);
            playerMap.PlaceAllShips();
            for (int i = 0; i < GameMap.Size; i++)
            {
                for (int j = 0; j < GameMap.Size; j++)
                {
                    if (!(j == 0 || i == 0))
                    {
                        botMap.field[i, j] = 0;
                        botMap.Buttons[i, j].Enabled = true;
                        botMap.Buttons[i,j].BackColor = Color.White;
                        botMap.Buttons[i, j].Text = "";
                    }
                }
            }
        }

        private void SetupWindow()
        {
            AutoScaleDimensions = new SizeF(8, 19);
            AutoScaleMode = AutoScaleMode.Font;
            Font = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Point, 186);
            Margin = Padding.Empty;
            Text = "Морской бой";
            BackColor = Color.FromArgb(235, 235, 235);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;    
        }

        private void StartGame(object sender, EventArgs e)
        {
            isPlaying = true;
            bot.ConfigureShips();
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

            if (isPlaying)
            {
                if (pressedButton.Enabled)
                {
                    bool playerTurn = shooting.Shoot(botMap.field, pressedButton);
                    if (CheckIfAllShipsDestroyed(botMap.field))
                    {
                        MessageBox.Show("Поздравляем! Вы победили!");
                        //ResetGame();
                        return;
                    }
                    while (!playerTurn)
                    {
                        playerTurn = !bot.Shoot();

                        if (CheckIfAllShipsDestroyed(playerMap.field))
                        {
                            MessageBox.Show("Вы проиграли. Попробуйте ещё раз!");
                            //ResetGame();
                            return;
                        }
                    }
                    pressedButton.Enabled = false;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;

            this.Size = new System.Drawing.Size(Convert.ToInt32(0.5 * workingRectangle.Width),
                Convert.ToInt32(0.5 * workingRectangle.Height));

            this.Location = new System.Drawing.Point(0, 0);
        }
    }
}

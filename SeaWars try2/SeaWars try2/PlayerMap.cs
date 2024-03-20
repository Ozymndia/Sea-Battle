using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace SeaWars_try2
{
    public class PlayerMap : GameMap
    {
        private readonly MainForm gameForm;
        private readonly Ship ship;

        private readonly int[] shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

        public int LeftMargin = 10;
        public int TopMargin = 10;

        public int[,] field = new int[GameMap.Size, GameMap.Size];

        public PlayerMap(MainForm gameForm) : base(gameForm, "Карта игрока", new Point(GameMap.Size * GameMap.cellSize / 2, 
            GameMap.Size * GameMap.cellSize + 10))
        {
            this.gameForm = gameForm;
            this.ship = new Ship(this);

            PlaceAllShips();
        }
        protected override void Button_Click(object sender, EventArgs e)
        {

        }
        protected override int GetLeftMargin()
        {
            return LeftMargin;
        }

        protected override int GetTopMargin()
        {
            return TopMargin;
        }

        public void PlaceAllShips()
        {
            Random random = new Random();

            for (int i = 0; i < shipSizes.Length; i++)
            {
                int currentDecksNumber = shipSizes[i];
                bool shipPlaced = false;

                while (!shipPlaced)
                {
                    int startX = random.Next(1, GameMap.Size);
                    int startY = random.Next(1, GameMap.Size);

                    bool isVerticalOrientation = random.Next(0, 2) == 0;

                    if (ship.CanBePlaced(buttons[startY, startX], isVerticalOrientation, currentDecksNumber))
                    {
                        ship.Place(buttons[startY, startX], isVerticalOrientation, currentDecksNumber);
                        shipPlaced = true;
                    }
                }
            }
        }

        public void ClearAllShipsPlacement(Button[,] buttons)
        {
            for (int i = 0; i < GameMap.Size; i++)
            {
                for (int j = 0; j < GameMap.Size; j++)
                {
                    if (!(j == 0 || i == 0))
                    {
                        field[i, j] = 0;
                        buttons[i, j].Enabled = true;
                        buttons[i, j].BackColor = Color.White;
                        buttons[i, j].Text = "";
                    }
                }
            }
        }
    }
}

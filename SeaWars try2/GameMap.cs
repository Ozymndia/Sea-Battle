using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public abstract class GameMap
    {
        public const int Size = 11;
        public const int cellSize = 30;
        public const string alphabet = "АБВГДЕЖЗИК";

        public Button[,] buttons;
        public int[,] field = new int[Size, Size];

        protected Label mapLabel;

        private readonly MainForm gameForm;
        public readonly string labelName;
        private readonly Point labelLocation;
        private readonly int[] shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        private readonly Random _rng = new Random();
        public Ship ship;

        public GameMap(MainForm gameForm, string labelName, Color buttonColor, Point labelLocation)
        {
            buttons = new Button[Size, Size];
            CreateButtons();

            this.gameForm = gameForm;
            this.labelName = labelName;
            this.labelLocation = labelLocation;

            CreateLabels();

            this.ship = new Ship(this, buttonColor);

            PlaceAllShips();
        }

        
        private void CreateButtons()
        {
            
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    buttons[y, x] = new Button
                    {
                        Location = GetButtonLocation(x, y),
                        Size = new Size(cellSize, cellSize),
                        BackColor = Color.White,
                        Tag = new Point(x, y)
                    };
                    if (x == 0 || y == 0)
                    {
                        buttons[y, x].BackColor = Color.Gray;
                        if (y == 0 && x > 0)
                            buttons[y, x].Text = alphabet[x - 1].ToString();
                        if (x == 0 && y > 0)
                            buttons[y, x].Text = y.ToString();
                    }
                    else
                    {
                        buttons[y, x].Click += Button_Click;
                    }
                }
            }
        }

        private void CreateLabels()
        {
            mapLabel = new Label
            {
                Text = labelName,
                Location = labelLocation,
                AutoSize = true
            };
            gameForm.Controls.Add(mapLabel);
        }

        public void PlaceAllShips()
        {
            for (int i = 0; i < shipSizes.Length; i++)
            {
                int currentDecksNumber = shipSizes[i];
                bool shipPlaced = false;

                while (!shipPlaced)
                {
                    int startX = _rng.Next(1, Size);
                    int startY = _rng.Next(1, Size);

                    bool isVerticalOrientation = _rng.Next(0, 2) == 0;

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
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (!(j == 0 || i == 0))
                    {
                        field[i, j] = 0;
                        buttons[i, j].BackColor = Color.White;
                        buttons[i, j].Enabled = true;
                        buttons[i, j].Text = "";
                    }
                }
            }
        }
        public void MarkSurroundingCells(int row, int col)
        {
            bool isVerticalShip = false;

            if (field[row - 1, col] == -1 || (row != 10 && field[row + 1, col] == -1)) { isVerticalShip = true; }

            if (isVerticalShip)
            {
                for (int i = row; i > 0; i--)
                {
                    if (field[i, col] == -1)
                    {
                        MarkVerticalCell(i, col, -1);
                    }

                    if (field[i, col] == 0)
                    {
                        MarkVerticalCell(i, col, 0);

                        break;
                    }
                }

                for (int i = row; i < 11; i++)
                {
                    if (field[i, col] == -1)
                    {
                        MarkVerticalCell(i, col, -1);
                    }

                    if (field[i, col] == 0)
                    {
                        MarkVerticalCell(i, col, 0);

                        break;
                    }
                }
            }

            if (!isVerticalShip)
            {
                for (int i = col; i > 0; i--)
                {
                    if (field[row, i] == -1)
                    {
                        MarkHorizontalCell(row, i, -1);
                    }

                    if (field[row, i] == 0)
                    {
                        MarkHorizontalCell(row, i, 0);

                        break;
                    }
                }

                for (int i = col; i < 11; i++)
                {
                    if (field[row, i] == -1)
                    {
                        MarkHorizontalCell(row, i, -1);
                    }

                    if (field[row, i] == 0)
                    {
                        MarkHorizontalCell(row, i, 0);

                        break;
                    }
                }
            }
        }

        public bool CheckShipDestroyed(int row, int col)
        {
            bool isVerticalShip = false;

            if (field[row - 1, col] != 0 || (row != 10 && field[row + 1, col] != 0)) { isVerticalShip = true; }

            if (isVerticalShip)
            {
                for (int i = row; i > 0; i--)
                {
                    if (field[i, col] == 1)
                    {
                        return false;
                    }

                    if (field[i, col] == 0) { break; }
                }

                for (int i = row; i < 11; i++)
                {

                    if (field[i, col] == 1)
                    {
                        return false;
                    }

                    if (field[i, col] == 0) { break; }
                }
            }
            if (!isVerticalShip)
            {
                for (int i = col; i > 0; i--)
                {
                    if (field[row, i] == 1)
                    {
                        return false;
                    }

                    if (field[row, i] == 0) { break; }
                }

                for (int i = col; i < 11; i++)
                {
                    if (field[row, i] == 1)
                    {
                        return false;
                    }

                    if (field[row, i] == 0) { break; }
                }
            }

            return true;
        }

        private void MarkVerticalCell(int row, int col, int cellStatus)
        {

            if (col + 1 != 11)
            {
                buttons[row, col + 1].BackColor = Color.Black;
                buttons[row, col + 1].Enabled = false;
            }

            if (col - 1 != 0)
            {
                buttons[row, col - 1].BackColor = Color.Black;
                buttons[row, col - 1].Enabled = false;
            }

            if (cellStatus == 0)
            {
                buttons[row, col].BackColor = Color.Black;
                buttons[row, col].Enabled = false;
            }
        }

        private void MarkHorizontalCell(int row, int col, int cellStatus)
        {

            if (row + 1 != 11)
            {
                buttons[row + 1, col].BackColor = Color.Black;
                buttons[row + 1, col].Enabled = false;
            }

            if (row - 1 != 0)
            {
                buttons[row - 1, col].BackColor = Color.Black;
                buttons[row - 1, col].Enabled = false;
            }

            if (cellStatus == 0)
            {
                buttons[row, col].BackColor = Color.Black;
                buttons[row, col].Enabled = false;
            }
        }


        protected abstract void Button_Click(object sender, EventArgs e);

        protected Point GetButtonLocation(int x, int y)
        {
            return new Point(GetLeftMargin() + x * cellSize, GetTopMargin() + y * cellSize);
        }

        protected abstract int GetLeftMargin();
        protected abstract int GetTopMargin();
    }
}

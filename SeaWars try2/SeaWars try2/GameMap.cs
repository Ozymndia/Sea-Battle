using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public abstract class GameMap
    {
        public const int Size = 10;
        public const int cellSize = 30;
        public const string alphabet = "АБВГДЕЖЗИК";

        public Button[,] buttons;
        protected Label mapLabel;

        public GameMap(MainForm gameForm, string labelName, Point labelLocation)
        {
            buttons = new Button[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    buttons[i, j] = new Button();
                    buttons[i, j].Location = GetButtonLocation(j, i);
                    buttons[i, j].Size = new Size(cellSize, cellSize);
                    buttons[i, j].BackColor = Color.White;
                    buttons[i, j].Tag = new Point(j, i);
                    if (j == 0 || i == 0)
                    {
                        buttons[i, j].BackColor = Color.Gray;
                        if (i == 0 && j > 0)
                            buttons[i, j].Text = alphabet[j - 1].ToString();
                        if (j == 0 && i > 0)
                            buttons[i, j].Text = i.ToString();
                    }
                    else
                    {
                        buttons[i, j].Click += Button_Click;
                    }
                }
            }
            mapLabel = new Label();
            mapLabel.Text = labelName;
            mapLabel.Location = labelLocation;
            mapLabel.AutoSize = true;
            gameForm.Controls.Add(mapLabel);
        }
        protected abstract void Button_Click(object sender, EventArgs e);

        protected Point GetButtonLocation(int x, int y)
        {
            return new Point(GetLeftMargin() + x * cellSize, GetTopMargin() + y * cellSize);
        }

        protected abstract int GetLeftMargin();
        protected abstract int GetTopMargin();

        public Button[,] Buttons => buttons;
    }
}

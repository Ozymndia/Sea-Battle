using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public class Shooting
    {
        private readonly MainForm gameForm;

        public Shooting(MainForm gameForm)
        {
            this.gameForm = gameForm;
        }

        public bool Shoot(int[,] map, Button pressedButton)
        {
            bool hit = false;
            if (gameForm.isPlaying)
            {
                int delta = 0;
                if (pressedButton.Location.X > 320)
                    delta = 320;

                int row = pressedButton.Location.Y / GameMap.cellSize;
                int col = (pressedButton.Location.X - delta) / GameMap.cellSize;


                if (map[row, col] != 0)
                {
                    hit = true;
                    map[row, col] = 0;
                    pressedButton.BackColor = Color.Cyan;
                    pressedButton.Text = "X";
                }
                else
                {

                    hit = false;
                    pressedButton.BackColor = Color.Black;
                }

            }
            return hit;
        }
    }
}

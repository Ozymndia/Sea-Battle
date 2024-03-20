using SeaWars_try2.Bot;
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
    public class Ship
    {
        private readonly PlayerMap playerMap;

        public Ship(PlayerMap playerMap)
        {
            this.playerMap = playerMap;
        }

        public void Place(Button pressedButton, bool isVerticalOrientation, int decksNumber)
        {
            int startX = pressedButton.Location.X / GameMap.cellSize;
            int startY = pressedButton.Location.Y / GameMap.cellSize;


            for (int i = 0; i < decksNumber; i++)
            {
                if (isVerticalOrientation)
                {
                    playerMap.field[startY + i, startX] = 1;
                    playerMap.Buttons[startY + i, startX].BackColor = Color.Pink;
                }
                else
                {
                    playerMap.field[startY, startX + i] = 1;
                    playerMap.Buttons[startY, startX + i].BackColor = Color.Pink;
                }
            }
            pressedButton.Enabled = false;
        }

        public bool CanBePlaced(Button pressedButton, bool isVerticalOrientation, int decksNumber)
        {
            int startX = pressedButton.Location.X / GameMap.cellSize;
            int startY = pressedButton.Location.Y / GameMap.cellSize;

            if (isVerticalOrientation)
            {
                if (startY + decksNumber > GameMap.Size)
                    return false;

                for (int i = Math.Max(0, startY - 1); i < Math.Min(GameMap.Size, startY + decksNumber + 1); i++)
                {
                    if (startX > 0 && playerMap.field[i, startX - 1] == 1)
                        return false;
                    if (startX < GameMap.Size - 1 && playerMap.field[i, startX + 1] == 1)
                        return false;
                    if (playerMap.field[i, startX] == 1)
                        return false;
                }
            }
            else
            {
                if (startX + decksNumber > GameMap.Size)
                    return false;

                for (int i = Math.Max(0, startX - 1); i < Math.Min(GameMap.Size, startX + decksNumber + 1); i++)
                {
                    if (startY > 0 && playerMap.field[startY - 1, i] == 1)
                        return false;
                    if (startY < GameMap.Size - 1 && playerMap.field[startY + 1, i] == 1)
                        return false;
                    if (playerMap.field[startY, i] == 1)
                        return false;
                }
            }

            return true;
        }
    }
}



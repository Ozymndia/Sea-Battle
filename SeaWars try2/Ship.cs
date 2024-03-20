using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public class Ship
    {
        private readonly GameMap map;
        private readonly Color color;

        public Ship(GameMap map, Color color)
        {
            this.map = map;
            this.color = color;
        }
        


        public void Place(Button button, bool isVerticalOrientation, int decksNumber)
        {
            Point point = (Point)button.Tag;
            int startX = point.X;
            int startY = point.Y;

            for (int i = 0; i < decksNumber; i++)
            {
                if (isVerticalOrientation)
                {
                    map.field[startY + i, startX] = 1;
                    map.buttons[startY + i, startX].BackColor = color;
                }
                else
                {
                    map.field[startY, startX + i] = 1;
                    map.buttons[startY, startX + i].BackColor = color;
                }
            }
        }

        public bool CanBePlaced(Button pressedButton, bool isVerticalOrientation, int decksNumber)
        {
            Point point = (Point)pressedButton.Tag;
            int startX = point.X;
            int startY = point.Y;

            if (isVerticalOrientation)
            {
                if (startY + decksNumber > GameMap.Size)
                    return false;

                for (int i = Math.Max(0, startY - 1); i < Math.Min(GameMap.Size, startY + decksNumber + 1); i++)
                {
                    if (startX > 0 && map.field[i, startX - 1] == 1)
                        return false;
                    if (startX < GameMap.Size - 1 && map.field[i, startX + 1] == 1)
                        return false;
                    if (map.field[i, startX] == 1)
                        return false;
                }
            }
            else
            {
                if (startX + decksNumber > GameMap.Size)
                    return false;

                for (int i = Math.Max(0, startX - 1); i < Math.Min(GameMap.Size, startX + decksNumber + 1); i++)
                {
                    if (startY > 0 && map.field[startY - 1, i] == 1)
                        return false;
                    if (startY < GameMap.Size - 1 && map.field[startY + 1, i] == 1)
                        return false;
                    if (map.field[startY, i] == 1)
                        return false;
                }
            }
            return true;
        }

    }
}



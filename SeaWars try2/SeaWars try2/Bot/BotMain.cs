using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaWars_try2.Bot
{
    public class BotMain
    {
        public BotMap botMap;
        public PlayerMap playerMap;

        private int lastHitPosX = -1;
        private int lastHitPosY = -1;
        private bool lastShotHit = false;

        public int LastHitPosX
        {
            get { return lastHitPosX; }
        }

        public int LastHitPosY
        {
            get { return lastHitPosY; }
        }

        public BotMain(BotMap botMap, PlayerMap playerMap)
        {
            this.botMap = botMap;
            this.playerMap = playerMap;
        }

        public bool IsInsideMap(int i, int j)
        {
            if (i < 0 || j < 0 || i >= GameMap.Size || j >= GameMap.Size)
            {
                return false;
            }
            return true;
        }

        public bool IsEmpty(int i, int j, int length)
        {
            bool IsEmpty = true;

            for (int k = j; k < j + length; k++)
            {
                if (botMap.field[i, k] != 0)
                {
                    IsEmpty = false;
                    break;
                }
            }

            return IsEmpty;
        }
        public void ConfigureShips()
        {
            int lengthShip = 4;
            int cycleValue = 4;
            int shipsCount = 10;
            Random r = new Random();

            while (shipsCount > 0)
            {
                for (int i = 0; i < cycleValue / 4; i++)
                {
                    int posX, posY;
                    bool isHorizontal;
                    do
                    {
                        posX = r.Next(1, GameMap.Size);
                        posY = r.Next(1, GameMap.Size);
                        isHorizontal = r.Next(2) == 0; // 0 - горизонтально, 1 - вертикально
                    } while (!IsShipPlacementValid(botMap.field, posX, posY, lengthShip, isHorizontal));

                    if (isHorizontal)
                    {
                        for (int k = posY; k < posY + lengthShip; k++)
                        {
                            botMap.field[posX, k] = 1;
                        }
                    }
                    else
                    {
                        for (int k = posX; k < posX + lengthShip; k++)
                        {
                            botMap.field[k, posY] = 1;
                        }
                    }
                    shipsCount--;
                    if (shipsCount <= 0)
                        break;
                }
                cycleValue += 4;
                lengthShip--;
            }
        }

        public bool IsShipPlacementValid(int[,] map, int posX, int posY, int lengthShip, bool isHorizontal)
        {
            int endX = isHorizontal ? posX : posX + lengthShip - 1;
            int endY = isHorizontal ? posY + lengthShip - 1 : posY;

            // Check if the ship is inside the map
            if (!IsInsideMap(endX, endY))
            {
                return false;
            }

            // Check for adjacent cells
            for (int i = Math.Max(0, posX - 1); i <= Math.Min(GameMap.Size - 1, endX + 1); i++)
            {
                for (int j = Math.Max(0, posY - 1); j <= Math.Min(GameMap.Size - 1, endY + 1); j++)
                {
                    if (map[i, j] == 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public bool Shoot()
        {
            bool hit = false;
            Random r = new Random();

            if (lastShotHit)
            {
                // If the last shot was a hit, continue targeting in the same direction
                if (lastHitPosX != -1 && lastHitPosY != -1)
                {
                    int direction = r.Next(4); // 0: up, 1: down, 2: left, 3: right

                    switch (direction)
                    {
                        case 0: // up
                            lastHitPosX--;
                            break;
                        case 1: // down
                            lastHitPosX++;
                            break;
                        case 2: // left
                            lastHitPosY--;
                            break;
                        case 3: // right
                            lastHitPosY++;
                            break;
                    }
                }
            }
            else
            {
                // If the last shot missed or it's the first shot, shoot randomly
                lastHitPosX = r.Next(1, GameMap.Size);
                lastHitPosY = r.Next(1, GameMap.Size);
            }

            // Ensure the selected coordinates are within the valid range
            lastHitPosX = Math.Max(1, Math.Min(GameMap.Size - 1, lastHitPosX));
            lastHitPosY = Math.Max(1, Math.Min(GameMap.Size - 1, lastHitPosY));

            while (playerMap.buttons[lastHitPosX, lastHitPosY].BackColor == Color.Blue ||
                   playerMap.buttons[lastHitPosX, lastHitPosY].BackColor == Color.Black)
            {
                // If the selected coordinates have already been shot, choose new ones
                lastHitPosX = r.Next(1, GameMap.Size);
                lastHitPosY = r.Next(1, GameMap.Size);
            }

            if (playerMap.field[lastHitPosX, lastHitPosY] != 0)
            {
                hit = true;
                playerMap.field[lastHitPosX, lastHitPosY] = 0;
                playerMap.buttons[lastHitPosX, lastHitPosY].BackColor = Color.Cyan;
                playerMap.buttons[lastHitPosX, lastHitPosY].Text = "X";
                lastShotHit = true;
            }
            else
            {
                hit = false;
                playerMap.buttons[lastHitPosX, lastHitPosY].BackColor = Color.Black;
                lastShotHit = false;
            }

            return hit;
        }
    }
}

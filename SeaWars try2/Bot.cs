using System;
using System.Drawing;
using System.Collections.Generic;


namespace SeaWars_try2
{
    public class Bot
    {
        public BotMap map;
        public PlayerMap playerMap;

        public int firstHitRow = -1;
        public int firstHitCol = -1;

        public int shootPosRow = -1;
        public int shootPosCol = -1;

        private int hitCount = 0;

        public bool shipHit = false;
        private bool destroyingShip = false;
        private string shootDirection = "none";
        List<string> directions;

        public Bot(BotMap map, PlayerMap playerMap)
        {
            this.map = map;
            this.playerMap = playerMap;
        }
        
        private void GetRandomShootCoords()
        {
            Random rand = new Random();
            shootPosRow = rand.Next(1, GameMap.Size);
            shootPosCol = rand.Next(1, GameMap.Size);

            while (playerMap.buttons[shootPosRow, shootPosCol].BackColor == Color.Blue ||
                playerMap.buttons[shootPosRow, shootPosCol].BackColor == Color.Black)
            {
                shootPosRow = rand.Next(1, GameMap.Size);
                shootPosCol = rand.Next(1, GameMap.Size);
            }
        }

        private void GetShootCoords()
        {
            if (shootDirection == "none")
            {
                directions = new List<string> { "verticalUp", "verticalDown", "horizontalLeft", "horizontalRight" };
                Random rand = new Random();
                int currentDirection = rand.Next(4);

                switch (currentDirection)
                {
                    case 0:
                        shootDirection = "verticalUp";
                        break;
                    case 1:
                        shootDirection = "horizontalLeft";
                        break;
                    case 2:
                        shootDirection = "verticalDown";
                        break;
                    case 3:
                        shootDirection = "horizontalRight";
                        break;
                }
            }

            if (shootDirection == "verticalUp") { 
                SetShootUpCoords();
                return;
            }
            if (shootDirection == "verticalDown")
            {
                SetShootDownCoords();
                return;
            }
            if (shootDirection == "horizontalRight") { 
                SetShootRightCoords();
                return;
            }
            if (shootDirection == "horizontalLeft") { 
            
                SetShootLeftCoords();
                return;
            }
        }

        private void SetShootUpCoords()
        {
            if (shipHit)
            {
                if(shootPosRow == 1 && hitCount == 1)//верхняя граница
                {
                    GetRandomDirectionExceptOne("verticalUp");
                    GetShootCoords();
                    return;
                }
                if (shootPosRow != 1 && CellIsBlack(shootPosRow - 1, shootPosCol) && hitCount == 1)//наткнулись на черную клетку
                {
                    GetRandomDirectionExceptOne("verticalUp");
                    GetShootCoords();
                    return;
                }
                if(shootPosRow == 1 && CellIsKilled(shootPosRow + 1, shootPosCol))//наткнулись на вверхнюю границу, но есть больше 1 попадания
                {
                    shootDirection = "verticalDown";
                    shootPosRow = firstHitRow + 1;
                    return;
                }
                if(shootPosRow != 1 && shootPosRow != 10)//наткнулись на черную клетку, но есть больше 1 попадания
                {
                    if (CellIsBlack(shootPosRow - 1, shootPosCol))
                    {
                        if (CellIsKilled(shootPosRow + 1, shootPosCol))
                        {
                            shootDirection = "verticalDown";
                            shootPosRow = firstHitRow + 1;
                            return;
                        }
                    }
                }
                if(shootPosRow != 1 && !CellIsBlack(shootPosRow - 1, shootPosCol))//все свободно, можно стрелять
                {
                    shootPosRow--;
                    return;
                }
            }

            if (!shipHit)
            {
                if(shootPosRow == 1 && hitCount == 1) //попали на границу
                {
                    GetRandomDirectionExceptOne("verticalUp");
                    GetShootCoords();
                    return;
                }
                if(shootPosRow != 1 && CellIsBlack(shootPosRow - 1, shootPosCol) && hitCount == 1)//черная клетая, нельзя стрелять
                {
                    GetRandomDirectionExceptOne("verticalUp");
                    GetShootCoords();
                    return;
                }
                if(shootPosRow != 10)//угадали с направлением, но потом промах
                {
                    if (CellIsKilled(shootPosRow + 1, shootPosCol))
                    {
                        shootDirection = "verticalDown";
                        shootPosRow = firstHitRow + 1;
                        return;
                    }

                }
                if(shootPosRow != 1 && !CellIsBlack(shootPosRow - 1, shootPosCol))//все чисто, можно стрелять
                {
                    shootPosRow--;
                    return;
                }
            }
        }

        private void SetShootDownCoords()
        {
            if (shipHit)
            {
                if (shootPosRow == 10 && CellIsKilled(shootPosRow - 1, shootPosCol))//наткнулись на нижнюю границу, но есть больше 1 попадания
                {
                    shootDirection = "verticalUp";
                    shootPosRow = firstHitRow - 1;
                    return;
                }

                if (shootPosRow != 10 && shootPosRow != 1) //наткнулись на черную клетку, но есть больше 1 попадания
                {
                    if (CellIsBlack(shootPosRow + 1, shootPosCol))
                    {
                        if (CellIsKilled(shootPosRow - 1, shootPosCol))
                        {
                            shootDirection = "verticalUp";
                            shootPosRow = firstHitRow - 1;
                            return;
                        }
                    }
                }

                if (shootPosRow == 10) //наткнулись на нижнюю границу
                {
                    GetRandomDirectionExceptOne("verticalDown");
                    GetShootCoords();
                    return;
                }
                if (shootPosRow != 10 && CellIsBlack(shootPosRow + 1, shootPosCol)) //наткнулись на черную клетку
                {
                    GetRandomDirectionExceptOne("verticalDown");
                    GetShootCoords();
                    return;
                }
                if (shootPosRow != 10 && !CellIsBlack(shootPosRow + 1, shootPosCol)) //можно стрелять
                {
                    shootPosRow++;
                    return;
                }
            }

            if (!shipHit)
            {
                if (shootPosRow == 10 && hitCount == 1) //попали в границу, вниз нельзя
                {
                    GetRandomDirectionExceptOne("verticalDown");
                    GetShootCoords();
                    return;
                }
                if (shootPosRow != 10 && CellIsBlack(shootPosRow + 1, shootPosCol) && hitCount == 1) //внизу в черная клетка, туда нельзя
                {
                    GetRandomDirectionExceptOne("verticalDown");
                    GetShootCoords();
                    return;
                }
                if (shootPosRow != 1) //угадали с направлением, но потом промах
                {
                    if (CellIsKilled(shootPosRow - 1, shootPosCol))
                    {
                        shootDirection = "verticalUp";
                        shootPosRow = firstHitRow - 1;
                        return;
                    }
                }

                if (shootPosRow != 10 && !CellIsBlack(shootPosRow + 1, shootPosCol)) //все хорошо, стреляем
                {
                    shootPosRow++;
                    return;
                }
            }
        }
        private void SetShootRightCoords()
        {
            if (shipHit)
            {
                if (shootPosCol == 10 && CellIsKilled(shootPosRow, shootPosCol - 1)) //уперлись в правую границу, но есть больше 1 попадания
                {
                    shootDirection = "horizontalLeft";
                    shootPosCol = firstHitCol - 1;
                    return;
                }

                if (shootPosCol != 10 && shootPosCol != 1)
                {
                    if (CellIsBlack(shootPosRow, shootPosCol + 1))
                    {
                        if (CellIsKilled(shootPosRow, shootPosCol - 1))
                        {
                            shootDirection = "horizontalLeft";
                            shootPosCol = firstHitCol - 1;
                            return;
                        }
                    }
                }

                if (shootPosCol == 10)
                {
                    GetRandomDirectionExceptOne("horizontalRight");
                    GetShootCoords();
                    return;
                }
                if (shootPosCol != 10 && CellIsBlack(shootPosRow, shootPosCol + 1))
                {
                    GetRandomDirectionExceptOne("horizontalRight");
                    GetShootCoords();
                    return;
                }

                if (shootPosCol != 10 && !CellIsBlack(shootPosRow, shootPosCol + 1))
                {
                    shootPosCol++;
                    return;
                }
            }

            if (!shipHit)
            {
                if (shootPosCol == 10 && hitCount == 1)
                {
                    GetRandomDirectionExceptOne("horizontalRight");
                    GetShootCoords();
                    return;
                }
                if (shootPosCol != 10 && CellIsBlack(shootPosRow, shootPosCol + 1) && hitCount == 1)
                {
                    GetRandomDirectionExceptOne("horizontalRight");
                    GetShootCoords();
                    return;
                }
                if (shootPosCol != 1) //???
                {
                    if (CellIsKilled(shootPosRow, shootPosCol - 1))
                    {
                        shootDirection = "horizontalLeft";
                        shootPosCol = firstHitCol - 1;
                        return;
                    }
                }

                if (shootPosCol != 10 && !CellIsBlack(shootPosRow, shootPosCol + 1))
                {
                    shootPosCol++;
                    return;
                }
            }
            

        }

        private void SetShootLeftCoords()
        {
            if (shipHit)
            {
                if (shootPosCol == 1) //наткнулись на границу
                {
                    if (CellIsKilled(shootPosRow, shootPosCol + 1))
                    {
                        shootDirection = "horizontalRight";
                        shootPosCol = firstHitCol + 1;
                        return;
                    }
                }

                if (shootPosCol != 1 && shootPosCol != 10) //наткнулись на черную клетку
                {
                    if (CellIsBlack(shootPosRow, shootPosCol - 1))
                    {
                        if (CellIsKilled(shootPosRow, shootPosCol + 1))
                        {
                            shootDirection = "horizontalRight";
                            shootPosCol = firstHitCol + 1;
                            return;
                        }
                    }
                }

                if (shootPosCol == 1 && hitCount == 1) //нужно выбрать другое направление, т.к. слева граница
                {
                    GetRandomDirectionExceptOne("horizontalLeft");
                    GetShootCoords();
                    return;
                }
                if (shootPosCol != 1 && CellIsBlack(shootPosRow, shootPosCol - 1) && hitCount == 1) //нужно другое направление, так как слева черная клетка
                {
                    GetRandomDirectionExceptOne("horizontalLeft");
                    GetShootCoords();
                    return;
                }

                if (shootPosCol != 1 && !CellIsBlack(shootPosRow, shootPosCol - 1))
                {
                    shootPosCol--;
                    return;
                }
            }

            if (!shipHit)
            {
                if (shootPosCol == 1 && hitCount == 1)
                {
                    GetRandomDirectionExceptOne("horizontalLeft");
                    GetShootCoords();
                    return;
                }

                if (shootPosCol != 1 && CellIsBlack(shootPosRow, shootPosCol - 1) && hitCount == 1)
                {
                    GetRandomDirectionExceptOne("horizontalLeft");
                    GetShootCoords();
                    return;
                }

                if (shootPosCol != 10)
                {
                    if (CellIsKilled(shootPosRow, shootPosCol + 1))
                    {
                        shootDirection = "horizontalRight";
                        shootPosCol = firstHitCol + 1;
                        return;
                    }
                }

                if (shootPosCol != 1 && !CellIsBlack(shootPosRow, shootPosCol - 1))
                {
                    shootPosCol--;
                    return;
                }
            }  
        }

        private void GetRandomDirectionExceptOne(string exceptDirection)
        {
            if (!string.IsNullOrEmpty(exceptDirection))
            {
                directions.Remove(exceptDirection);
            }
            Random rand = new Random();
            int index = rand.Next(directions.Count);
            shootDirection = directions[index];
            shootPosRow = firstHitRow;
            shootPosCol = firstHitCol;
        }
        public bool Shoot()
        {
            if (!destroyingShip)
            {
                GetRandomShootCoords();
            }
            else {
                GetShootCoords();
            }

            if (playerMap.field[shootPosRow, shootPosCol] == 1)
            {
                shipHit = true;
                playerMap.field[shootPosRow, shootPosCol] = -1;
                playerMap.buttons[shootPosRow, shootPosCol].BackColor = Color.Blue;

                if (!playerMap.CheckShipDestroyed(shootPosRow, shootPosCol) && !destroyingShip)
                {
                    firstHitRow = shootPosRow;
                    firstHitCol = shootPosCol;
                    destroyingShip = true;
                }

                if (!playerMap.CheckShipDestroyed(shootPosRow, shootPosCol) && destroyingShip)
                {
                    hitCount++;
                }

                if (playerMap.CheckShipDestroyed(shootPosRow, shootPosCol))
                {
                    destroyingShip = false;
                    shootDirection = "none";
                    firstHitCol = -1;
                    firstHitRow = -1;
                    hitCount = 0;
                    playerMap.MarkSurroundingCells(shootPosRow, shootPosCol);
                }
            }
            else
            {
                shipHit = false;
                if (playerMap.buttons[shootPosRow, shootPosCol].BackColor != Color.Black)
                {
                    playerMap.buttons[shootPosRow, shootPosCol].BackColor = Color.Black;
                } else playerMap.buttons[shootPosRow, shootPosCol].BackColor = Color.Red;
                if (playerMap.field[shootPosRow, shootPosCol] == -1)
                {
                    playerMap.buttons[shootPosRow, shootPosCol].BackColor = Color.Red;
                }
                //playerMap.buttons[shootPosRow, shootPosCol].BackColor = Color.Black;
                if (destroyingShip && hitCount == 1)
                {
                    GetRandomDirectionExceptOne(shootDirection);
                }
            }

            return shipHit;
        }

        public void ResetHitLocations()
        {
            firstHitRow = -1;
            firstHitCol = -1;
            shootPosRow = -1;
            shootPosCol = -1;
            hitCount = 0;
            shipHit = false;
            destroyingShip = false;
            shootDirection = "none";

    }

        private bool CellIsBlack(int row, int col)
        {
            return playerMap.buttons[row, col].BackColor == Color.Black;
        }

        private bool CellIsKilled(int row, int col)
        {
            return playerMap.buttons[row, col].BackColor == Color.Blue;
        }
    }
}

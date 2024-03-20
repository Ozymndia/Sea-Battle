using System.Drawing;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public class Shooting
    {
        private readonly MainForm gameForm;
        private readonly BotMap botMap;

        public Shooting(MainForm gameForm, BotMap botMap)
        {
            this.gameForm = gameForm;
            this.botMap = botMap;
        }

        public bool Shoot(Button pressedButton)
        {
            Point point = (Point)pressedButton.Tag;

            int row = point.Y;
            int col = point.X;


            bool hit;
            if (botMap.field[row, col] != 0)
            {
                hit = true;
                botMap.field[row, col] = -1;
                pressedButton.BackColor = Color.Blue;
                if (botMap.CheckShipDestroyed(row, col))
                {
                    botMap.MarkSurroundingCells(row, col);
                }
            }
            else
            {

                hit = false;
                pressedButton.BackColor = Color.Black;
            }
            return hit;
        }


    }
}

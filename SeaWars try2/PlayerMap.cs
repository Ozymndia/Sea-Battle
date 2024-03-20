using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public class PlayerMap : GameMap
    {
        public int LeftMargin = 10;
        public int TopMargin = 10;

        public PlayerMap(MainForm gameForm) : base(gameForm, "Карта игрока", Color.Pink, new Point(GameMap.Size * GameMap.cellSize / 2,
            GameMap.Size * GameMap.cellSize + 10))
        {

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
    }
}

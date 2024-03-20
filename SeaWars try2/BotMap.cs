using System;
using System.Drawing;

namespace SeaWars_try2
{
    public class BotMap : GameMap
    {
        private readonly MainForm gameForm;

        public int LeftMargin = 350;
        public int TopMargin = 10;

        public BotMap(MainForm gameForm) : base(gameForm, "Карта противника", Color.White, new Point(320 + GameMap.Size * GameMap.cellSize / 2, 
            GameMap.Size * GameMap.cellSize + 10))
        {
            this.gameForm = gameForm;
        }
        protected override void Button_Click(object sender, EventArgs e)
        {
            gameForm.OnPlayerCellClick(sender, e);
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

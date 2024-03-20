using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public class BotMap : GameMap
    {
        private readonly MainForm gameForm;
        private readonly Shooting shooting;

        public int LeftMargin = 320;
        public int TopMargin = 10;

        public int[,] field = new int[GameMap.Size, GameMap.Size];

        public BotMap(MainForm gameForm) : base(gameForm, "Карта противника", new Point(320 + GameMap.Size * GameMap.cellSize / 2, 
            GameMap.Size * GameMap.cellSize + 10))
        {
            this.gameForm = gameForm;
            this.shooting = new Shooting(gameForm); 

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

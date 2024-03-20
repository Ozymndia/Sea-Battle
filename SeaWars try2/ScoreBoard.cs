using System.Drawing;
using System.Windows.Forms;

namespace SeaWars_try2
{
    public class ScoreBoard
    {
        public MainForm gameForm;
        public Label scoreLabel;

        private readonly Point labelLocation;

        public int playerScore = 0;
        public int botScore = 0;

        public ScoreBoard(MainForm gameForm)
        {
            this.gameForm = gameForm;
            labelLocation = new Point((gameForm.ClientSize.Width - 65) / 2, GameMap.Size * GameMap.cellSize + 30);
            CreateScoreLabel();
            scoreLabel.Text = $"{playerScore}:{botScore}";
        }
        public void CreateScoreLabel()
        {
            scoreLabel = new Label
            {
                Text = "",
                Location = labelLocation,
                AutoSize = true,
                Font = new Font("Calibri", 24, FontStyle.Regular, GraphicsUnit.Point, 186)

            };
            gameForm.Controls.Add(scoreLabel);
        }
    }
}

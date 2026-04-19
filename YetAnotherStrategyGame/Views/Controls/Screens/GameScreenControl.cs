using Model;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherStrategyGame.Views.Controls;
using Model;

namespace YetAnotherStrategyGame.Views.Controls.Screens
{
    [DesignerCategory("Code")]
    public partial class GameScreenControl : UserControl
    {
        private Game Game;
        private FieldControl FieldControl;
        private GameMenuControl GameMenuControl;

        public GameScreenControl(Game game)
        {
            Game = game;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Game.Start(11, 13);
            this.BackColor = Color.FromArgb(63, 77, 45);
            this.Dock = DockStyle.Fill;
            FieldControl = new FieldControl(Game, 80);
            GameMenuControl = new GameMenuControl(Game);
            GameMenuControl.Location = new Point(1040, 20);
            FieldControl.Location = new Point(80,20);
            Controls.Add(GameMenuControl);
            Controls.Add(FieldControl);
        }
    }
}

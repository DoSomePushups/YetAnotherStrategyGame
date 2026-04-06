using Model;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherStrategyGame.Views.Controls;

namespace YetAnotherStrategyGame.Views.Controls.Screens
{
    [DesignerCategory("Code")]
    public partial class GameScreenControl : UserControl
    {
        private Game Game;

        public GameScreenControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(190, 225, 150);
            this.Dock = DockStyle.Fill;
        }

        public void Configure(Game game)
        {
            Game = game;
        }
    }
}

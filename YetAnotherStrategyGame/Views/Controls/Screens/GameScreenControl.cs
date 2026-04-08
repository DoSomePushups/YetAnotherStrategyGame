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
        private FieldControl FieldControl;

        public GameScreenControl(Game game)
        {
            Game = game;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(190, 225, 150);
            this.Dock = DockStyle.Fill;
            FieldControl = new FieldControl(Game.GameField, 80);
            FieldControl.Location = new Point(0,0);
            Controls.Add(FieldControl);
            FieldControl.BringToFront();
        }
    }
}

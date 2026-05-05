using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Model;

namespace YetAnotherStrategyGame.Views.Controls
{
    [DesignerCategory("code")]
    public partial class Settings : UserControl
    {
        private Game Game;
        private Label Label;
        private Label AI;
        private CheckBox CheckBoxAI;
        private BackButton BackButton;

        public Settings(Game game)
        {
            Game = game;
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            Label = new()
            {
                Text = "Settings",
                Font = new Font("Arial", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            Size = new Size(1200, 800);
            Label.Location = new Point(470, 20);
            AI = new()
            {
                Text = "AI Mode",
                Font = new Font("Arial", 30F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            AI.Location = new Point(90, 145);
            CheckBoxAI = new();
            CheckBoxAI.Location = new Point(350, 165);
            CheckBoxAI.CheckedChanged += CheckedChangedAI;
            BackColor = Color.FromArgb(11, 166, 16);
            BackButton = new();
            BackButton.Location = new Point(450, 650);
            BackButton.Click += Close;
            Controls.Add(Label);
            Controls.Add(AI);
            Controls.Add(CheckBoxAI);
            Controls.Add(BackButton);
        }

        private void CheckedChangedAI(object sender, EventArgs e)
        {
            if (CheckBoxAI.Checked)
            {
                Game.ChangeAIMode(true);
            }
            else
            {
                Game.ChangeAIMode(false);
            }
        }

        private void Close(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}

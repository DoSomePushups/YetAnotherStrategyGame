using Model;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherStrategyGame.Views.Controls;

namespace YetAnotherStrategyGame.Views.Controls.Screens
{
    [DesignerCategory("Code")]
    public partial class MainScreenControl : UserControl
    {
        private Game Game;
        private Label TitleLabel;
        private PlayButton PlayButton;
        private SettingsButton SettingsButton;
        private ExitButton ExitButton;
        private Settings Settings;

        public MainScreenControl(Game game)
        {
            Game = game;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(190, 225, 150);
            this.Dock = DockStyle.Fill;

            TitleLabel = new Label()
            {
                Text = "Yet Another Strategy Game",
                Font = new Font("Arial", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            PlayButton = new PlayButton();
            SettingsButton = new SettingsButton();
            ExitButton = new ExitButton();

            // Привязка событий
            PlayButton.Click += PlayButton_Click;
            SettingsButton.Click += ShowSettings;
            ExitButton.Click += ExitButton_Click;

            Controls.Add(TitleLabel);
            Controls.Add(PlayButton);
            Controls.Add(SettingsButton);
            Controls.Add(ExitButton);
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Game?.ChangeGameState(GameState.PlayOptionScreen);
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            Settings = new(Game);
            Settings.Location = new Point(360, 197);
            Controls.Add(Settings);
            Settings.BringToFront();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Центрирование элементов при изменении размера окна
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var centerX = this.Width / 2;
            var startY = this.Height / 4;

            TitleLabel.Location = new Point(centerX - TitleLabel.Width / 2, 50);
            PlayButton.Location = new Point(centerX - PlayButton.Width / 2, startY);
            SettingsButton.Location = new Point(centerX - SettingsButton.Width / 2, startY + 80);
            ExitButton.Location = new Point(centerX - ExitButton.Width / 2, startY + 160);
        }
    }
}
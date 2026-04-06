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
        private Game _game;
        private Label titleLabel;
        private PlayButton playButton;
        private SettingsButton settingsButton;
        private ExitButton exitButton;

        public MainScreenControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.FromArgb(190, 225, 150);
            this.Dock = DockStyle.Fill;

            titleLabel = new Label()
            {
                Text = "Yet Another Strategy Game",
                Font = new Font("Arial", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            playButton = new PlayButton();
            settingsButton = new SettingsButton();
            exitButton = new ExitButton();

            // Привязка событий
            playButton.Click += PlayButton_Click;
            exitButton.Click += ExitButton_Click;
            // settingsButton пока ничего не делает

            this.Controls.Add(titleLabel);
            this.Controls.Add(playButton);
            this.Controls.Add(settingsButton);
            this.Controls.Add(exitButton);
        }

        public void Configure(Game game)
        {
            _game = game;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            _game?.ChangeGameState(GameState.PlayOptionScreen);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Центрирование элементов при изменении размера окна
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int centerX = this.Width / 2;
            int startY = this.Height / 4;

            titleLabel.Location = new Point(centerX - titleLabel.Width / 2, 50);
            playButton.Location = new Point(centerX - playButton.Width / 2, startY);
            settingsButton.Location = new Point(centerX - settingsButton.Width / 2, startY + 80);
            exitButton.Location = new Point(centerX - exitButton.Width / 2, startY + 160);
        }
    }
}
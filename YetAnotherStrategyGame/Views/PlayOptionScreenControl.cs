using ClassLibrary1;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherStrategyGame.Views.Controls;
using static System.Windows.Forms.AxHost;

namespace YetAnotherStrategyGame.Views
{
    [DesignerCategory("Code")]
    public partial class PlayOptionScreenControl : UserControl
    {
        private Game _game;
        private Label titleLabel;
        private FreePlayButton freePlayButton;
        private CampaignButton campaignButton;
        private BackButton backButton;

        public PlayOptionScreenControl()
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

            freePlayButton = new FreePlayButton();
            campaignButton = new CampaignButton();
            backButton = new BackButton();

            // Привязка событий
            backButton.Click += BackButton_Click;
            // Остальные пока ничего не делают

            this.Controls.Add(titleLabel);
            this.Controls.Add(freePlayButton);
            this.Controls.Add(campaignButton);
            this.Controls.Add(backButton);
        }

        public void Configure(Game game)
        {
            _game = game;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game?.ChangeGameState(GameState.MainScreen);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int centerX = this.Width / 2;
            int startY = this.Height / 4;

            titleLabel.Location = new Point(centerX - titleLabel.Width / 2, 50);
            freePlayButton.Location = new Point(centerX - freePlayButton.Width / 2, startY);
            campaignButton.Location = new Point(centerX - campaignButton.Width / 2, startY + 80);
            backButton.Location = new Point(centerX - backButton.Width / 2, startY + 160);
        }
    }
}
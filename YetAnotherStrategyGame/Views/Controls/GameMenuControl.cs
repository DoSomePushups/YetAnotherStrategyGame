using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Model;
using Svg;
using Svg.Pathing;

namespace YetAnotherStrategyGame.Views.Controls
{
    [DesignerCategory("Code")]
    public partial class GameMenuControl : UserControl
    {
        private Game Game;
        public GameMenuControl(Game game)
        {
            Game = game;
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            Size = new Size(832, 1040);
            var stats = new Stats();
            stats.Location = new Point(0, 0);
            var store = new Store();
            store.Location = new Point(62, 273);
            var exitButton = new ExitButton();
            exitButton.Location = new Point(this.Width / 2 - exitButton.Width / 2, this.Height - exitButton.Height);
            exitButton.Click += ExitButton_Click;
            Controls.Add(stats);
            Controls.Add(store);
            Controls.Add(exitButton);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Game?.ChangeGameState(GameState.MainScreen);
        }
    }

    public class Stats : Panel
    {
        private static string[] svgPaths = new string[]
        {
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "wheat.svg"),
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "gold.svg"),
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "clock.svg")
        };

        public Stats()
        {
            Size = new Size(832, 100);
            var food = new StatControl(svgPaths[0]);
            var gold = new StatControl(svgPaths[1]);
            var time = new StatControl(svgPaths[2]);
            food.Dock = DockStyle.Left;
            food.Width = this.Width / 3;
            time.Dock = DockStyle.Right;
            time.Width = this.Width / 3;
            gold.Dock = DockStyle.Fill;
            Controls.Add(gold);
            Controls.Add(food);
            Controls.Add(time);
        }
    }

    public class Store : Panel
    {
        private static string[] svgPaths = new string[] 
        {
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "farm.svg"),
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "SwordBarracks.svg"),
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "GoldMine.svg"),
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "CrossbowBarracks.svg"),
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "castle.svg"),
            Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "CannonFactory.svg")
        };
        public Store()
        {
            Size = new Size(708, 550);
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 2; j++)
                {
                    var storeItem = new StoreItemControl(svgPaths[2 * i + j]);
                    storeItem.Location = new Point(i * 264, j * (65 + storeItem.Height));
                    Controls.Add(storeItem);
                }
        }
    }

    public class StatControl : Panel
    {
        public PictureBox SvgHolder;
        public Label Quantity;

        public StatControl(string svgPath)
        {
            var svgDoc = SvgDocument.Open(svgPath);
            SvgHolder = new PictureBox();
            SvgHolder.Size = new Size(100, 100);
            SvgHolder.Location = new Point(0, 0);
            SvgHolder.Image = svgDoc.Draw(100, 100);
            Quantity = new Label();
            Quantity.Text = "000";
            Quantity.ForeColor = Color.White;
            Quantity.Font = new Font(Font.FontFamily, 36);
            Quantity.Size = new Size(150, 550);
            Quantity.Location = new Point(100, 0);
            Controls.Add(SvgHolder);
            Controls.Add(Quantity);
        }
    }

    public class StoreItemControl : Panel
    {
        public Button ActionButton;
        public Label PriceLabelFood;
        public Label PriceLabelGold;

        public StoreItemControl(string svgPath)
        {
            Size = new Size(180, 260); // Высота с учетом текста
            var svgDoc = SvgDocument.Open(svgPath);
            ActionButton = new Button
            {
                Size = new Size(180, 180),
                BackColor = Color.FromArgb(112, 177, 112),
                Location = new Point(0, 0),
                FlatStyle = FlatStyle.Flat,
                Image = svgDoc.Draw(160, 160)
            };

            var prices = new Panel
            {
                Size = new Size(180, 45),
                Location = new Point(0, 186)
            };

            PriceLabelFood = new Label
            {
                Text = "000",
                ForeColor = Color.White,
                Font = new Font(Font.FontFamily, 16),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            PriceLabelGold = new Label
            {
                Text = "000",
                ForeColor = Color.White,
                Font = new Font(Font.FontFamily, 16),
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight
            };

            prices.Controls.Add( PriceLabelFood );
            prices.Controls.Add ( PriceLabelGold );

            Controls.Add(ActionButton);
            Controls.Add(prices);
        }
    }
}

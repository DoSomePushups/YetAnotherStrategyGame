using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
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
        private Player Player;

        public GameMenuControl(Game game)
        {
            Game = game;
            Player = Game.Session.FirstPlayer;
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            Size = new Size(832, 1040);
            var stats = new Stats();
            stats.Food.Quantity.Text = Player.Food.ToString();
            stats.Gold.Quantity.Text = Player.Gold.ToString();
            Player.StatsChanged += (newGold, newFood) =>
            {
                stats.Food.Quantity.Text = Player.Food.ToString();
                stats.Gold.Quantity.Text = Player.Gold.ToString();
            };
            Game.Session.OnTick += () =>
                stats.Time.Quantity.Text = (Game.Session.Time / 10).ToString();
            stats.Location = new Point(0, 0);
            var store = new Store(Player);
            store.Location = new Point(62, 273);
            store.Items[0].PriceLabelFood.Quantity.Text = FarmInformation.CostFood.ToString();
            store.Items[0].PriceLabelGold.Quantity.Text = FarmInformation.CostGold.ToString();
            store.Items[1].PriceLabelFood.Quantity.Text = BarracksInformation.CostFood.ToString();
            store.Items[1].PriceLabelGold.Quantity.Text = BarracksInformation.CostGold.ToString();
            store.Items[2].PriceLabelFood.Quantity.Text = MineInformation.CostFood.ToString();
            store.Items[2].PriceLabelGold.Quantity.Text = MineInformation.CostGold.ToString();
            store.Items[3].PriceLabelFood.Quantity.Text = CrossbowWorkshopInformation.CostFood.ToString();
            store.Items[3].PriceLabelGold.Quantity.Text = CrossbowWorkshopInformation.CostGold.ToString();
            store.Items[4].PriceLabelFood.Quantity.Text = CastleInformation.CostFood.ToString();
            store.Items[4].PriceLabelGold.Quantity.Text = CastleInformation.CostGold.ToString();
            store.Items[5].PriceLabelFood.Quantity.Text = CannonFactoryInformation.CostFood.ToString();
            store.Items[5].PriceLabelGold.Quantity.Text = CannonFactoryInformation.CostGold.ToString();
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

        public class Stats : Panel
        {
            private static string[] svgPaths = new string[]
            {
                Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "wheat.svg"),
                Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "gold.svg"),
                Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "clock.svg")
            };

            public StatControl Food;
            public StatControl Gold;
            public StatControl Time;

            public Stats()
            {
                Size = new Size(832, 100);
                Food = new(svgPaths[0]);
                Gold = new(svgPaths[1]);
                Time = new(svgPaths[2]);
                Food.Dock = DockStyle.Left;
                Food.Width = this.Width / 3;
                Time.Dock = DockStyle.Right;
                Time.Width = this.Width / 3;
                Gold.Dock = DockStyle.Fill;
                Gold.Width = this.Width / 3;
                Controls.Add(Gold);
                Controls.Add(Food);
                Controls.Add(Time);
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

            public StoreItemControl[] Items;

            public Store(Player player)
            {
                Size = new Size(708, 560);
                Items = new StoreItemControl[6];
                for (var i = 0; i < 3; i++)
                    for (var j = 0; j < 2; j++)
                    {
                        var index = 2 * i + j;
                        Items[index] = new StoreItemControl(svgPaths[index]);
                        Items[index].ActionButton.Click += (s, e) =>
                        {
                            BuildingType? item = index switch
                            {
                                0 => BuildingType.Farm,
                                1 => BuildingType.Barracks,
                                2 => BuildingType.Mine,
                                3 => BuildingType.CrossbowWorkshop,
                                4 => BuildingType.Castle,
                                5 => BuildingType.CannonFactory,
                                _ => null
                            };
                            player.SelectStoreItem(item);
                        };
                        var item = Items[index];
                        item.Location = new Point(i * 264, j * (65 + item.Height));
                        Controls.Add(item);
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
            public StoreItemCost PriceLabelFood;
            public StoreItemCost PriceLabelGold;

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

                PriceLabelFood = new StoreItemCost(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "wheat.svg"))
                {
                    Dock = DockStyle.Left
                };

                PriceLabelGold = new StoreItemCost(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "gold.svg"))
                {
                    Dock = DockStyle.Right
                };

                prices.Controls.Add(PriceLabelFood);
                prices.Controls.Add(PriceLabelGold);

                Controls.Add(ActionButton);
                Controls.Add(prices);
            }
        }

        public class StoreItemCost : Panel
        {
            public PictureBox SvgHolder;
            public Label Quantity;

            public StoreItemCost(string svgPath)
            {
                Size = new Size(87, 45);
                var svgDoc = SvgDocument.Open(svgPath);
                SvgHolder = new PictureBox();
                SvgHolder.Size = new Size(45, 45);
                SvgHolder.Location = new Point(0, 0);
                SvgHolder.Image = svgDoc.Draw(45, 45);
                Quantity = new Label();
                Quantity.Text = "000";
                Quantity.ForeColor = Color.White;
                Quantity.Font = new Font(Font.FontFamily, 11);
                Quantity.Size = new Size(42, 45);
                Quantity.Location = new Point(45, 0);
                Quantity.TextAlign = ContentAlignment.MiddleCenter;
                Controls.Add(SvgHolder);
                Controls.Add(Quantity);
            }
        }
    }
}

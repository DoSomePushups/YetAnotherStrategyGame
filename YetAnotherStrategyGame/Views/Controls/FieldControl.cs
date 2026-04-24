using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Forms;
using Svg;
using Model;

namespace YetAnotherStrategyGame.Views.Controls
{
    [DesignerCategory("Code")]
    public partial class FieldControl : UserControl
    {
        private Game Game;
        private Field Field;
        private int CellSize;

        public FieldControl(Game game, int cellSize)
        {
            Game = game;
            Field = Game.Session.GameField;
            CellSize = cellSize;
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            this.Size = new Size(Field.Width * CellSize, Field.Height * CellSize);
            this.BackColor = Color.FromArgb(127, 179, 64);
            for (var i = 0; i < Field.Width; i++)
            {
                for (var j = 0; j < Field.Height; j++)
                {
                    var x = i;
                    var y = j;
                    var cellButton = new Button();
                    var cell = Field.Map[x, y];
                    cellButton.Location = new Point(i * CellSize, j * CellSize);
                    cellButton.Size = new Size(CellSize, CellSize);
                    cellButton.BackColor = Color.FromArgb(127, 179, 64);
                    cellButton.FlatStyle = FlatStyle.Flat;
                    DrawButtonSvg(cell, cellButton);
                    cellButton.Paint += (s, e) =>
                    {
                        if (cell.Entity != null)
                        {
                            var maxHP = GetMaxHpForEntity(cell.Entity);
                            var barWidth = 60;
                            var barHeight = 5;
                            var posX = (cellButton.Width - barWidth) / 2;
                            var posY = cellButton.Height - barHeight - 5;
                            var graphics = e.Graphics;
                            DrawHealthBar(graphics, cell.Entity.HP, maxHP, posX, posY, barWidth, barHeight);
                        }
                    };
                    cell.CellChanged += (updatedCell) =>
                    {
                        //Если из другого потока
                        if (cellButton.InvokeRequired)
                            cellButton.Invoke(new Action(() => DrawButtonSvg(updatedCell, cellButton)));
                        else
                            DrawButtonSvg(updatedCell, cellButton);
                    };
                    cellButton.Click += (sender, args) =>
                        Game.Session.FirstPlayer.Click(cell);
                    Controls.Add(cellButton);
                }
            }
        }

        private void DrawButtonSvg(Cell cell, Button cellButton)
        {
            EntityType type = cell.Entity switch
            {
                Farm => EntityType.Farm,
                Mine => EntityType.Mine,
                Castle => EntityType.Castle,
                Barracks => EntityType.Barracks,
                CrossbowWorkshop => EntityType.CrossbowWorkshop,
                CannonFactory => EntityType.CannonFactory,
                Human => EntityType.Human,
                Warrior => EntityType.Warrior,
                Crossbowman => EntityType.Crossbowman,
                Cannon => EntityType.Cannon,
                _ => EntityType.None
            };
            if (Game.SvgImages.TryGetValue(type, out var image))
            {
                cellButton.Image = image;
                if (cell.Entity.Owner.Team == Team.Second)
                    cellButton.BackColor = Color.FromArgb(100, 255, 0, 0);
            }
            else
            {
                cellButton.Image = null;
                cellButton.BackColor = Color.FromArgb(127, 179, 64);
            }
        }

        private int GetMaxHpForEntity(IEntity entity) => entity switch
        {
            Farm => FarmInformation.MaxHP,
            Mine => MineInformation.MaxHP,
            Castle => CastleInformation.MaxHP,
            Barracks => BarracksInformation.MaxHP,
            CrossbowWorkshop => CrossbowWorkshopInformation.MaxHP,
            CannonFactory => CannonFactoryInformation.MaxHP,
            Warrior => WarriorInformation.MaxHP,
            Crossbowman => CrossbowmanInformation.MaxHP,
            Cannon => CannonInformation.MaxHP,
            Human => HumanInformation.MaxHP,
            _ => 1
        };

        private void DrawHealthBar(Graphics g, int hp, int maxHp, int x, int y, int width, int height)
        {
            var percent = (double)hp / maxHp;
            var fillWidth = (int)(width * percent);

            using (SolidBrush redBrush = new SolidBrush(Color.Red))
                g.FillRectangle(redBrush, x, y, width, height);

            if (fillWidth > 0)
                using (SolidBrush greenBrush = new SolidBrush(Color.LimeGreen))
                    g.FillRectangle(greenBrush, x, y, fillWidth, height);

            using (Pen borderPen = new Pen(Color.Black, 1))
                g.DrawRectangle(borderPen, x, y, width, height);
        }

        //private void DrawUnitRange(Graphics g, int range, int x, int y)
        //{
        //    using (Pen borderPen = new Pen(Color.Blue, 2))
        //        g.DrawRectangle(borderPen, x - CellSize * range, y - CellSize * range, range * 2 + CellSize, range * 2 + CellSize);
        //}
    }
}

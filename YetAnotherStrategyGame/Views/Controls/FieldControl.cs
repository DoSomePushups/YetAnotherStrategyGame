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
                            var (maxHP, restTime) = GetEntityInfo(cell.Entity);
                            DrawEntityUI(e.Graphics, cell.Entity.HP, maxHP, cell.Entity.UnactionTime, restTime);
                        }
                    };
                    Game.Session.OnTick += () =>
                    {
                        if (cell.Entity != null)
                            cellButton.Invalidate();
                    };
                    cell.CellChanged += (updatedCell) =>
                    {
                        //Если из другого потока
                        if (cellButton.InvokeRequired)
                            cellButton.Invoke(new Action(() => DrawButtonSvg(updatedCell, cellButton)));
                        else
                            DrawButtonSvg(updatedCell, cellButton);
                    };
                    cellButton.MouseClick += (sender, args) =>
                    {
                        if (args.Button == MouseButtons.Left)
                            Game.Session.FirstPlayer.LeftClick(cell);
                        else
                            Game.Session.FirstPlayer.RightClick(cell);
                    };  
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

        private (int, int) GetEntityInfo(IEntity entity) => entity switch
        {
            Farm => (FarmInformation.MaxHP, FarmInformation.RestTime),
            Mine => (MineInformation.MaxHP, MineInformation.RestTime),
            Castle => (CastleInformation.MaxHP, CastleInformation.RestTime),
            Barracks => (BarracksInformation.MaxHP, BarracksInformation.RestTime),
            CrossbowWorkshop => (CrossbowWorkshopInformation.MaxHP, CrossbowWorkshopInformation.RestTime),
            CannonFactory => (CannonFactoryInformation.MaxHP, CannonFactoryInformation.RestTime),
            Warrior => (WarriorInformation.MaxHP, WarriorInformation.RestTime),
            Crossbowman => (CrossbowmanInformation.MaxHP, CrossbowmanInformation.RestTime),
            Cannon => (CannonInformation.MaxHP, CannonInformation.RestTime),
            Human => (HumanInformation.MaxHP, HumanInformation.RestTime),
            _ => throw new ArgumentException("Unknown entity")
        };

        private void DrawEntityUI(Graphics g, int hp, int maxHp, int unactionTime, int restTime)
        {
            var barWidth = 60;
            var barHeight = 5;
            var x = (CellSize - barWidth) / 2;
            var y = CellSize - barHeight - 5;
            var percent = (double)hp / maxHp;
            var fillWidth = (int)(barWidth * percent);
            using (SolidBrush redBrush = new SolidBrush(Color.Red))
                g.FillRectangle(redBrush, x, y, barWidth, barHeight);

            if (fillWidth > 0)
                using (SolidBrush greenBrush = new SolidBrush(Color.LimeGreen))
                    g.FillRectangle(greenBrush, x, y, fillWidth, barHeight);

            using (Pen borderPen = new Pen(Color.Black, 1))
                g.DrawRectangle(borderPen, x, y, barWidth, barHeight);

            if (unactionTime < restTime * 5)
                DrawBattery(g, unactionTime, restTime);
        }

        private void DrawBattery(Graphics g, int unactionTime, int restTime)
        {
            var width = 20;
            var height = 20;
            var x = 57;
            var y = 3;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var rect = new Rectangle(x, y, width, height);
            var startAngle = 0;
            var sweepAngle = (unactionTime / ((float)restTime * 5)) * 360f; ;

            using (var brush = new SolidBrush(Color.FromArgb(200, 255, 255, 255)))
                g.FillPie(brush, rect, startAngle, sweepAngle);
        }

        //private void DrawUnitRange(Graphics g, int range, int x, int y)
        //{
        //    using (Pen borderPen = new Pen(Color.Blue, 2))
        //        g.DrawRectangle(borderPen, x - CellSize * range, y - CellSize * range, range * 2 + CellSize, range * 2 + CellSize);
        //}
    }
}

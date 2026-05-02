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
        private Player Player;
        private int CellSize;

        private PictureBox FieldCanvas;

        public FieldControl(Game game, int cellSize)
        {
            Game = game;
            Field = Game.Session.GameField;
            Player = Game.Session.FirstPlayer;
            CellSize = cellSize;
            DoubleBuffered = true;
            InitializeComponent();
        }
        
        public void InitializeComponent()
        {
            this.Size = new Size(Field.Width * CellSize, Field.Height * CellSize);

            FieldCanvas = new PictureBox();
            FieldCanvas.Location = new Point(0, 0);
            FieldCanvas.Size = this.Size;
            FieldCanvas.BackColor = Color.FromArgb(127, 179, 64);
            FieldCanvas.Paint += MainCanvas_Paint;
            FieldCanvas.MouseDown += MainCanvas_MouseDown;
            Controls.Add(FieldCanvas);
            Game.Session.OnTick += () =>
            {
                if (FieldCanvas.InvokeRequired)
                    FieldCanvas.Invoke(new Action(FieldCanvas.Invalidate));
                else
                    FieldCanvas.Invalidate();
            };
            for (var gridX = 0; gridX < Field.Width; gridX++)
                for (var gridY = 0; gridY < Field.Height; gridY++)
                {
                    var cell = Field.Map[gridX, gridY];
                    var x = gridX;
                    var y = gridY;
                    cell.CellChanged += (updatedCell) =>
                    {
                        // Инвалидируется только квадрат изменившейся клетки для оптимизации
                        var invalidateAction = () => FieldCanvas.Invalidate(new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize));
                        if (FieldCanvas.InvokeRequired)
                            FieldCanvas.Invoke(invalidateAction);
                        else
                            invalidateAction();
                    };
                }
        }

        private void MainCanvas_MouseDown(object sender, MouseEventArgs args)
        {
            var gridX = args.X / CellSize;
            var gridY = args.Y / CellSize;
            if (gridX >= 0 && gridX < Field.Width && gridY >= 0 && gridY < Field.Height)
            {
                var clickedCell = Field.Map[gridX, gridY];
                //MessageBox.Show(clickedCell.X.ToString(), clickedCell.Y.ToString());
                if (args.Button == MouseButtons.Left)
                    Player.LeftClick(clickedCell);
                else if (args.Button == MouseButtons.Right)
                    Player.RightClick(clickedCell);
                else if (args.Button == MouseButtons.Middle)
                    Player.MiddleClick(clickedCell);
            }
        }

        private void MainCanvas_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            var selectedUnit = Player.SelectedUnit;
            for (var gridX = 0; gridX < Field.Width; gridX++)
            {
                for (var gridY = 0; gridY < Field.Height; gridY++)
                {
                    var cell = Field.Map[gridX, gridY];
                    var pixelX = gridX * CellSize;
                    var pixelY = gridY * CellSize;
                    var cellColor = Color.FromArgb(127, 179, 64);
                    if (cell.Entity != null && cell.Entity.Owner.Team == Team.Second)
                        cellColor = Color.FromArgb(100, 255, 0, 0);
                    using (var backgroundBrush = new SolidBrush(cellColor))
                        graphics.FillRectangle(backgroundBrush, pixelX, pixelY, CellSize, CellSize);
                    using (var gridPen = new Pen(Color.FromArgb(50, 0, 0, 0), 1))
                        graphics.DrawRectangle(gridPen, pixelX, pixelY, CellSize, CellSize);
                    var entityType = GetEntityType(cell.Entity);
                    if (entityType != EntityType.None && Game.SvgImages.TryGetValue(entityType, out var entityImage))
                        graphics.DrawImage(entityImage, pixelX, pixelY, CellSize, CellSize);
                    if (cell.Entity != null)
                    {
                        var (maxHP, restTime) = GetEntityInfo(cell.Entity);
                        DrawEntityUI(graphics, cell.Entity, maxHP, restTime, pixelX, pixelY);
                    }
                }
            }
            if (selectedUnit != null)
                DrawUnitRange(graphics, selectedUnit);
        }

        private void DrawUnitRange(Graphics graphics, IUnit selectedUnit)
        {
            const int borderWidth = 4;
            var attackRange = selectedUnit switch
            {
                Crossbowman => CrossbowmanInformation.Range,
                Cannon => CannonInformation.Range,
                _ => 1
            };
            var unitLocation = selectedUnit.Location;
            var rectangleX = (unitLocation.X - attackRange) * CellSize - borderWidth / 2;
            var rectangleY = (unitLocation.Y - attackRange) * CellSize - borderWidth / 2;
            var rectangleSize = (attackRange * 2 + 1) * CellSize + borderWidth;
            using (var radiusPen = new Pen(Color.Blue, borderWidth))
                graphics.DrawRectangle(radiusPen, rectangleX, rectangleY, rectangleSize, rectangleSize);
        }

        private EntityType GetEntityType(IEntity entity) => entity switch
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

        private void DrawEntityUI(Graphics graphics, IEntity entity, int maxHp, int restTime, int offsetX, int offsetY)
        {
            var currentHp = entity.HP;
            var currentUnactionTime = entity.UnactionTime;

            DrawHealthBar(graphics, currentHp, maxHp, offsetX, offsetY);

            if (currentUnactionTime < restTime * 5)
                DrawBattery(graphics, currentUnactionTime, restTime, offsetX, offsetY);

            if (entity is IRangedUnit rangedUnit)
                DrawAmount(graphics, rangedUnit.AmmoLeft, 2, offsetX, offsetY);
            else
            {
                if (entity is IProductionBuilding prodBuilding)
                    DrawAmount(graphics, prodBuilding.ItemAmount, 2, offsetX, offsetY);
                if (entity is IAmmunitionBuilding ammoBuilding)
                    DrawAmount(graphics, ammoBuilding.AmmoAmount, 28, offsetX, offsetY);
            }
        }

        private void DrawHealthBar(Graphics graphics, int currentHp, int maxHp, int offsetX, int offsetY)
        {
            var barWidth = 60;
            var barHeight = 5;
            var positionX = offsetX + (CellSize - barWidth) / 2;
            var positionY = offsetY + CellSize - barHeight - 5;
            var hpPercent = (double)currentHp / maxHp;
            var fillWidth = (int)(barWidth * hpPercent);
            using (SolidBrush redBrush = new SolidBrush(Color.Red))
                graphics.FillRectangle(redBrush, positionX, positionY, barWidth, barHeight);
            if (fillWidth > 0)
                using (SolidBrush greenBrush = new SolidBrush(Color.LimeGreen))
                    graphics.FillRectangle(greenBrush, positionX, positionY, fillWidth, barHeight);
            using (Pen borderPen = new Pen(Color.Black, 1))
                graphics.DrawRectangle(borderPen, positionX, positionY, barWidth, barHeight);
        }

        private void DrawBattery(Graphics graphics, int currentUnactionTime, int restTime, int offsetX, int offsetY)
        {
            var batteryWidth = 20;
            var batteryHeight = 20;
            var positionX = offsetX + 57;
            var positionY = offsetY + 3;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var batteryRectangle = new Rectangle(positionX, positionY, batteryWidth, batteryHeight);
            var startAngle = 0;
            var sweepAngle = (currentUnactionTime / ((float)restTime * 5)) * 360f;
            using (var whiteTransparentBrush = new SolidBrush(Color.FromArgb(200, 255, 255, 255)))
                graphics.FillPie(whiteTransparentBrush, batteryRectangle, startAngle, sweepAngle);
        }

        private void DrawAmount(Graphics graphics, int currentAmount, int textYOffset, int offsetX, int offsetY)
        {
            var textXOffset = 5;
            var positionX = offsetX + textXOffset;
            var positionY = offsetY + textYOffset;
            var amountFont = new Font("Arial", 16);
            using (var whiteTextBrush = new SolidBrush(Color.White))
                graphics.DrawString(currentAmount.ToString(), amountFont, whiteTextBrush, positionX, positionY);
        }
    }
}
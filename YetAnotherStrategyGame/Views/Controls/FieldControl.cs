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
                    cell.CellChanged += (updatedCell) =>
                    {
                        // Cлучай, если изменения придут из другого потока
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
            var svgName = cell.Entity switch
            {
                Farm => "farm.svg",
                Mine => "GoldMine.svg",
                Castle => "castle.svg",
                Barracks => "SwordBarracks.svg",
                CrossbowWorkshop => "CrossbowBarracks.svg",
                CannonFactory => "CannonFactory.svg",
                Human => "Human.svg",
                _ => null
            };
            var svgPath = svgName != null ? Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", svgName) : null;
            if (svgPath != null)
            {
                cellButton.Image = SvgDocument.Open(svgPath).Draw(80, 80);
                if (cell.Entity.Owner.Team == Team.Second)
                    cellButton.BackColor = Color.FromArgb(100, 255, 0, 0);
            }
        }
    }
}

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

        public FieldControl(Game Game, int cellSize)
        {
            Game = Game;
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
                    var svgPath = cell.Entity switch
                    {
                        Farm => Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "farm.svg"),
                        Mine => Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "GoldMine.svg"),
                        Castle => Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "castle.svg"),
                        Barracks => Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "SwordBarracks.svg"),
                        CrossbowWorkshop => Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "CrossbowBarracks.svg"),
                        CannonFactory => Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "CannonFactory.svg"),
                        _ => null
                    };
                    if (svgPath != null)
                    {
                        cellButton.Image = SvgDocument.Open(svgPath).Draw(80, 80);
                        if (cell.Entity.Owner.Team == Team.Second)
                            cellButton.BackColor = Color.FromArgb(100, 255, 0, 0);
                    }
                    cellButton.Click += (sender, args) =>
                    {
                        Game.Session.FirstPlayer.Click(cell);
                    };
                    Controls.Add(cellButton);
                }
            }
        }
    }
}

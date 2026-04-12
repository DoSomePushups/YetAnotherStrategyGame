using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Forms;
using Model;

namespace YetAnotherStrategyGame.Views.Controls
{
    [DesignerCategory("Code")]
    public partial class FieldControl : UserControl
    {
        private Field Field;
        private int CellSize;

        public FieldControl(Field field, int cellSize)
        {
            Field = field;
            CellSize = cellSize;
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            this.Size = new Size(Field.Width * CellSize, Field.Height * CellSize);
            this.BackColor = Color.White;
            for (var i = 0; i < Field.Width; i++)
            {
                for (var j = 0; j < Field.Height; j++)
                {
                    var x = i;
                    var y = j;
                    var cellButton = new Button();
                    cellButton.Location = new Point(i * CellSize, j * CellSize);
                    cellButton.Size = new Size(CellSize, CellSize);
                    cellButton.BackColor = Color.FromArgb(127, 179, 64);
                    cellButton.FlatStyle = FlatStyle.Flat;
                    cellButton.Click += (sender, args) =>
                    {
                        MessageBox.Show($"Кликнута кнопка [{x},{y}]");
                    };
                    Controls.Add(cellButton);
                }
            }
        }
    }
}

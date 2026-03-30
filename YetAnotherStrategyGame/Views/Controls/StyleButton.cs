using System.Drawing;
using System.Windows.Forms;

namespace YetAnotherStrategyGame.Views.Controls
{
    // Базовый класс для всех кнопок с общим дизайном
    public class StyledButton : Button
    {
        public StyledButton()
        {
            this.BackColor = Color.FromArgb(100, 195, 100);
            this.ForeColor = Color.White;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 1;
            this.FlatAppearance.BorderColor = Color.DarkGreen;
            this.Font = new Font("Arial", 20F, FontStyle.Bold);
            this.Size = new Size(300, 60);
            this.Cursor = Cursors.Hand;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YetAnotherStrategyGame
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var mainScreenPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightGreen,
                Visible = true
            };

            var playOptionScreenPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightBlue,
                Visible = false
            };

            Controls.Add(mainScreenPanel);
            Controls.Add(playOptionScreenPanel);
        }
    }
}

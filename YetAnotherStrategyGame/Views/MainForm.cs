using ClassLibrary1;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherStrategyGame.Views;
using static System.Windows.Forms.AxHost;

namespace YetAnotherStrategyGame
{
    [DesignerCategory("Code")]
    public class MainForm : Form
    {
        private Game _game;

        // Экземпляры наших экранов
        private MainScreenControl _mainScreen;
        private PlayOptionScreenControl _playOptionScreen;

        public MainForm()
        {
            // Теперь этот метод определен ниже в этом же файле
            InitializeComponent();

            _game = new Game();
            _game.StateChanged += Game_OnStateChanged;

            // Настраиваем экраны и показываем начальный
            ConfigureScreens();
            ShowMainScreen();
        }

        private void InitializeComponent()
        {
            // Базовые настройки окна (то, что раньше делал дизайнер)
            this.Text = "Yet Another Strategy Game";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(190, 225, 150);

            // Инициализируем контролы экранов
            _mainScreen = new MainScreenControl();
            _playOptionScreen = new PlayOptionScreenControl();

            // Добавляем их на форму
            this.Controls.Add(_mainScreen);
            this.Controls.Add(_playOptionScreen);
        }

        private void ConfigureScreens()
        {
            _mainScreen.Configure(_game);
            _playOptionScreen.Configure(_game);
        }

        private void Game_OnStateChanged(GameState state)
        {
            // Логика переключения экранов на основе состояния модели
            switch (state)
            {
                case GameState.MainScreen:
                    ShowMainScreen();
                    break;
                case GameState.PlayOptionScreen:
                    ShowPlayOptionScreen();
                    break;
            }
        }

        private void ShowMainScreen()
        {
            _playOptionScreen.Hide();
            _mainScreen.Show();
            _mainScreen.BringToFront();
        }

        private void ShowPlayOptionScreen()
        {
            _mainScreen.Hide();
            _playOptionScreen.Show();
            _playOptionScreen.BringToFront();
        }
    }
}
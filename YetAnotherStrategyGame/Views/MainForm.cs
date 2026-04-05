using ClassLibrary1;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherStrategyGame.Views.Controls.Screens;

namespace YetAnotherStrategyGame
{
    [DesignerCategory("Code")]
    public class MainForm : Form
    {
        private Game _game;

        // Экземпляры экранов
        private MainScreenControl _mainScreen;
        private PlayOptionScreenControl _playOptionScreen;

        public MainForm()
        {
            InitializeComponent();

            _game = new Game();
            _game.StateChanged += Game_OnStateChanged;

            // Настройка экранов и показ начального
            ConfigureScreens();
            ShowMainScreen();
        }

        private void InitializeComponent()
        {
            // Базовые настройки окна
            this.Text = "Yet Another Strategy Game";
            this.Size = new Size(1920, 1080);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(190, 225, 150);

            // Инициализация контролов экранов
            _mainScreen = new MainScreenControl();
            _playOptionScreen = new PlayOptionScreenControl();

            // Добавление на форму
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
            // Переключения экранов на основе состояния модели
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
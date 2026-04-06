using Model;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using YetAnotherStrategyGame.Views.Controls.Screens;

namespace YetAnotherStrategyGame
{
    [DesignerCategory("Code")]
    public class MainForm : Form
    {
        private Game Game;

        // Экземпляры экранов
        private MainScreenControl MainScreen;
        private PlayOptionScreenControl PlayOptionScreen;
        private GameScreenControl GameScreen;

        public MainForm()
        {
            InitializeComponent();

            Game = new Game();
            Game.StateChanged += Game_OnStateChanged;

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
            MainScreen = new MainScreenControl();
            PlayOptionScreen = new PlayOptionScreenControl();
            GameScreen = new GameScreenControl();

            // Добавление на форму
            this.Controls.Add(MainScreen);
            this.Controls.Add(PlayOptionScreen);
        }

        private void ConfigureScreens()
        {
            MainScreen.Configure(Game);
            PlayOptionScreen.Configure(Game);
            GameScreen.Configure(Game);
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
                case GameState.GameScreen:
                    ShowGameScreen();
                    break;

            }
        }

        private void ShowMainScreen()
        {
            PlayOptionScreen.Hide();
            GameScreen.Hide();
            MainScreen.Show();
            MainScreen.BringToFront();
        }

        private void ShowPlayOptionScreen()
        {
            MainScreen.Hide();
            GameScreen.Hide();
            PlayOptionScreen.Show();
            PlayOptionScreen.BringToFront();
        }

        private void ShowGameScreen()
        {
            MainScreen.Hide();
            PlayOptionScreen.Hide();
            GameScreen.Show();
            GameScreen.BringToFront();
        }
    }
}
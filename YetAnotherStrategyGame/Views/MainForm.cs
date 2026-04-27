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
            Game = new Game();
            Game.StateChanged += Game_OnStateChanged;

            InitializeComponent();

            ShowMainScreen();
        }

        private void InitializeComponent()
        {
            // Базовые настройки окна
            this.Text = "Yet Another Strategy Game";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            //this.TopMost = true;
            this.BackColor = Color.FromArgb(190, 225, 150);

            // Инициализация контролов экранов
            MainScreen = new MainScreenControl(Game);
            PlayOptionScreen = new PlayOptionScreenControl(Game);
            GameScreen = new GameScreenControl(Game);

            // Добавление на форму
            this.Controls.Add(MainScreen);
            this.Controls.Add(PlayOptionScreen);
            this.Controls.Add(GameScreen);
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
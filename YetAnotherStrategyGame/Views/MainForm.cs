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
        private GameScreenControl? GameScreen = null;

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
            Text = "Yet Another Strategy Game";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(190, 225, 150);

            // Инициализация контролов экранов
            MainScreen = new MainScreenControl(Game);
            PlayOptionScreen = new PlayOptionScreenControl(Game);

            // Добавление на форму
            Controls.Add(MainScreen);
            Controls.Add(PlayOptionScreen);
        }

        private void Game_OnStateChanged(GameState state)
        {
            if (state != GameState.GameScreen && GameScreen != null)
            {
                Game.End();
                GameScreen.Dispose();
                GameScreen = null;
            }

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
                    if (GameScreen == null)
                    {
                        GameScreen = new GameScreenControl(Game);
                        Controls.Add(GameScreen);
                    }
                    ShowGameScreen();
                    break;

            }
        }

        private void ShowMainScreen()
        {
            PlayOptionScreen.Hide();
            GameScreen?.Hide();
            MainScreen.Show();
            MainScreen.BringToFront();
        }

        private void ShowPlayOptionScreen()
        {
            MainScreen.Hide();
            GameScreen?.Hide();
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
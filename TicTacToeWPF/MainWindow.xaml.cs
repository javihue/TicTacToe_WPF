using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToeWPF
{
    public partial class MainWindow : Window
    {
        enum GameStatus
        {
            PLAY, WIN, LOSE, DRAW
        }

        private const string PLAYER_MARK = "X", PROGRAM_MARK = "O", EMPTY = " ";
        private GameStatus gameStatus;
        private List<Button> gameBoard;

        public MainWindow()
        {
            InitializeComponent();
            gameLabel.Text = "Let's Play";
            gameButton.Content = "Start Game?";
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            gameButton.Opacity = 0;
            gameButton.IsEnabled = false;
            InitializeBoard();
        }

        private void BoardGame_Click(object sender, RoutedEventArgs e)
        {
            Button selectedButton = (Button)sender;

            if (selectedButton.Content.Equals(EMPTY))
            {
                selectedButton.Content = PLAYER_MARK;
                selectedButton.IsEnabled = false;
                CheckForWin(PLAYER_MARK);
            }
            if (gameStatus == GameStatus.PLAY)
            {
                MakeProgramMove();
                CheckForWin(PROGRAM_MARK);
            }
        }

        private void InitializeBoard()
        {
            gameBoard = new List<Button>
            {
                Cell1, Cell2, Cell3, Cell4, Cell5, Cell6, Cell7, Cell8, Cell9
            };
            foreach (var cell in gameBoard)
            {
                cell.IsEnabled = true;
                cell.Content = EMPTY;
            }
            gameLabel.Text = "Let's Play";
            gameStatus = GameStatus.PLAY;
        }

        private void MakeProgramMove()
        {
            Button buttonChosen;
            bool moveIsMade = false;
            do
            {
                Random rd = new Random();
                int number = rd.Next(0, gameBoard.Count);
                buttonChosen = gameBoard.ElementAt(number);
                if (buttonChosen.Content.Equals(EMPTY))
                {
                    moveIsMade = true;
                }
            } while (!moveIsMade);
            buttonChosen.Content = PROGRAM_MARK;
            buttonChosen.IsEnabled = false;
        }

        private void CheckForWin(string mark)
        {
            CheckRowsFor(mark);
            CheckColumnsFor(mark);
            CheckDiagonalsFor(mark);
            if (gameBoard.TrueForAll(Button => !Button.Content.Equals(EMPTY)) && gameStatus == GameStatus.PLAY)
            {
                gameStatus = GameStatus.DRAW;
                gameLabel.Text = "It's a Draw!";
            }
            if (gameStatus != GameStatus.PLAY)
            {
                foreach (var cell in gameBoard)
                {
                    cell.IsEnabled = false;
                }
                gameButton.Opacity = 100;
                gameButton.IsEnabled = true;
            }
        }

        private void CheckRowsFor(string selectedMark)
        {
            if ((Cell1.Content.Equals(selectedMark) && Cell2.Content.Equals(selectedMark) && Cell3.Content.Equals(selectedMark))
             || (Cell4.Content.Equals(selectedMark) && Cell5.Content.Equals(selectedMark) && Cell6.Content.Equals(selectedMark))
             || (Cell7.Content.Equals(selectedMark) && Cell8.Content.Equals(selectedMark) && Cell9.Content.Equals(selectedMark)))
            {
                DeclareWinner(selectedMark);
            }
        }

        private void CheckColumnsFor(string selectedMark)
        {
            if ((Cell1.Content.Equals(selectedMark) && Cell4.Content.Equals(selectedMark) && Cell7.Content.Equals(selectedMark))
             || (Cell2.Content.Equals(selectedMark) && Cell5.Content.Equals(selectedMark) && Cell8.Content.Equals(selectedMark))
             || (Cell3.Content.Equals(selectedMark) && Cell6.Content.Equals(selectedMark) && Cell9.Content.Equals(selectedMark)))
            {
                DeclareWinner(selectedMark);
            }
        }

        private void CheckDiagonalsFor(string selectedMark)
        {
            if ((Cell1.Content.Equals(selectedMark) && Cell5.Content.Equals(selectedMark) && Cell9.Content.Equals(selectedMark))
                  ||(Cell3.Content.Equals(selectedMark) && Cell5.Content.Equals(selectedMark) && Cell7.Content.Equals(selectedMark)))
            {
                    DeclareWinner(selectedMark);
            }
        }

        private void DeclareWinner(string selectedMark)
        {
            if (selectedMark == PLAYER_MARK)
            {
                    gameStatus = GameStatus.WIN;
                    gameLabel.Text = "You WON!";
            }
            if (selectedMark == PROGRAM_MARK)
            { 
                    gameStatus = GameStatus.LOSE;
                    gameLabel.Text = "You Lost.";
            }
        }
    }
}

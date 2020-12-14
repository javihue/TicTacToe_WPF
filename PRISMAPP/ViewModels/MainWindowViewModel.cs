using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq;

namespace PRISMAPP.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }
        private string _prompt;
        public string Prompt
        {
            get { return _prompt; }
            set { SetProperty(ref _prompt, value); }
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        public Marker[] Markers { get; } = new Marker[9];
        private GameStatus _gameStatus;
        public GameStatus GameStatus
        {
            get { return _gameStatus; }
            set { SetProperty(ref _gameStatus, value); }
        }
        public DelegateCommand StartGameCommand { get; }
        public DelegateCommand<object> MarkerCommand { get; }
        private void Placemarker(object chosenMarker)
        {
            var location = Convert.ToInt32(chosenMarker);
            if (Markers[location] == Marker.Empty)
            {
                Markers[location] = Marker.Player1;
                CheckForWin(Marker.Player1);
                if (GameStatus == GameStatus.Play)
                {
                    MakeProgramMove();
                    CheckForWin(Marker.Player2);
                }
            }

            RaisePropertyChanged(nameof(Markers));
        }

        private void MakeProgramMove()
        {
            bool moveIsMade = false;
            Marker chosenMarker;
            do
            {
                Random rd = new Random();
                int number = rd.Next(0, Markers.Length);
                chosenMarker = Markers.ElementAt(number);
                if (chosenMarker.Equals(Marker.Empty))
                {
                    moveIsMade = true;
                    Markers[number] = Marker.Player2;
                }
            } while (!moveIsMade);
        }

        private void CheckForWin(Marker currentPlayer)
        {
            CheckRowsFor(currentPlayer);
            CheckColumnsFor(currentPlayer);
            CheckDiagonalsFor(currentPlayer);
            if (Markers.All(q => !q.Equals(Marker.Empty)) && GameStatus == GameStatus.Play)
            {
                GameStatus = GameStatus.Draw;
                Status = "It's a Draw!";
            }
            if (GameStatus != GameStatus.Play)
            {
            }
        }

        private bool CheckForMatch(Marker marker, params int[] locations)
        {
            return locations.All(q => Markers[q] == marker);
        }

        private void CheckDiagonalsFor(Marker currentPlayer)
        {
            if (CheckForMatch(currentPlayer, 0, 4, 8) || CheckForMatch(currentPlayer, 2, 4, 6))
            {
                DeclareWinner(currentPlayer);
            }
        }

        private void CheckColumnsFor(Marker currentPlayer)
        {
            if (CheckForMatch(currentPlayer, 0, 3, 6) || CheckForMatch(currentPlayer, 1, 4, 7) || CheckForMatch(currentPlayer, 2, 5, 8))
            {
                DeclareWinner(currentPlayer);
            }
        }

        private void CheckRowsFor(Marker currentPlayer)
        {

            if (CheckForMatch(currentPlayer, 0, 1, 2) || CheckForMatch(currentPlayer, 3, 4, 5) || CheckForMatch(currentPlayer, 6, 7, 8))
            {
                DeclareWinner(currentPlayer);
            }
        }

        private void DeclareWinner(Marker currentPlayer)
        {
            if (currentPlayer == Marker.Player1)
            {
                GameStatus = GameStatus.Win;
                Status = "You WON!";
            }
            if (currentPlayer == Marker.Player2)
            {
                GameStatus = GameStatus.Lose;
                Status = "You Lost.";
            }
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < Markers.Length; i++)
            {
                Markers[i] = Marker.Empty;
            }
            RaisePropertyChanged(nameof(Markers));
            Status = "Let's Play";
            GameStatus = GameStatus.Play;
        }

        private bool IsMarkerValid(object parameter)
        {
            return (GameStatus == GameStatus.Play && Markers[Convert.ToInt32(parameter)] == Marker.Empty);
        }

        public MainWindowViewModel()
        {
            Prompt = "Start Game?";
            Status = "Let's Play!";
            GameStatus = GameStatus.NotStarted;
            StartGameCommand = new DelegateCommand(() => { InitializeBoard(); });
            MarkerCommand = new DelegateCommand<object>(Placemarker, IsMarkerValid).ObservesProperty(() => GameStatus).ObservesProperty(() => Markers);
        }
    }
}

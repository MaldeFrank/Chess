using Chess.Models;

namespace Chess.Logic
{
    public class GameLogic
    {
        private Game Game;
        public BoardLogic BoardLogic { get; }
        private MoveSimulator MoveSimulator = new MoveSimulator();
        public GameLogic(Game game)
        {
            this.Game = game;
            this.BoardLogic = new BoardLogic(game.Board);

            Game.Board.ThreatTracker.KingCheckEvent += (sender, e) =>
            {
             Console.WriteLine("King is chess!");
             Game.ErrorKingChecked = true;
             bool chessmate = MoveSimulator.IsKingChessmate(e.Item1, Game.Board);
             if (chessmate)
             {
                 Game.Winner = (e.Item1 == Player.White) ? Player.Black : Player.White;
                 Console.WriteLine("Chessmate");
             }

             };
        }
        public void NextTurn()
        {
            if (Game.Winner != null) return; // Spillet er slut
            Game.CurrentTurn = Game.CurrentTurn == Player.White ? Player.Black : Player.White;
        }

        public void CheckWinner()
        {
            var whiteKingAlive = Game.Pieces.Any(p => p.Type == PieceType.King && p.Owner == Player.White && !p.IsCaptured);
            var blackKingAlive = Game.Pieces.Any(p => p.Type == PieceType.King && p.Owner == Player.Black && !p.IsCaptured);

            if (!whiteKingAlive) Game.Winner = Player.Black;
            if (!blackKingAlive) Game.Winner = Player.White;
        }
    }
}
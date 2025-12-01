using Chess.Logic;

namespace Chess.Models
{
    public class Game
    {
        public ChessBoard Board { get; set; } = new ChessBoard();
        public List<Piece> Pieces { get; set; } = new List<Piece>();

        public ChessPlayer Player1 { get; set; } = new ChessPlayer();
        public ChessPlayer Player2 { get; set; } = new ChessPlayer();

        public Player CurrentTurn { get; set; } = Player.White;
        public Player? Winner { get; set; } = null;

        private MoveSimulator MoveSimulator = new MoveSimulator();

        public bool ErrorKingChecked = false;

        public Game()
        {
            // Sæt enum kobling
            Player1.PlayerEnum = Player.White;
            Player2.PlayerEnum = Player.Black;

            InitializePieces();
            PlacePiecesOnBoard();
            Board.ThreatTracker.KingCheckEvent += (sender, e) =>
            {
                Console.WriteLine("King is chess!");
                ErrorKingChecked = true;
                bool chessmate = MoveSimulator.IsKingChessmate(e.Item1, this.Board);
                if (chessmate)
                {
                    Winner = (e.Item1 == Player.White) ? Player.Black : Player.White;
                    Console.WriteLine("Chessmate");
                }

            };
        }

        private void InitializePieces()
        {
            int idWhite = 0;
            // White pieces (rank 0 & 1)
            for (int file = 0; file < 8; file++)
            {
                idWhite++;
                Pieces.Add(new Piece(PieceType.Pawn, Player.White, file, 1, idWhite));
            }
            Pieces.Add(new Piece(PieceType.Rook, Player.White, 0, 0, 9));
            Pieces.Add(new Piece(PieceType.Knight, Player.White, 1, 0, 10));
            Pieces.Add(new Piece(PieceType.Bishop, Player.White, 2, 0, 11));
            Pieces.Add(new Piece(PieceType.Queen, Player.White, 3, 0, 12));
            Pieces.Add(new Piece(PieceType.King, Player.White, 4, 0, 13));
            Pieces.Add(new Piece(PieceType.Bishop, Player.White, 5, 0, 14));
            Pieces.Add(new Piece(PieceType.Knight, Player.White, 6, 0, 15));
            Pieces.Add(new Piece(PieceType.Rook, Player.White, 7, 0, 16));

            int idBlack = 16;
            // Black pieces (rank 6 & 7)
            for (int file = 0; file < 8; file++)
            {
                idBlack++;
                Pieces.Add(new Piece(PieceType.Pawn, Player.Black, file, 6, idBlack));
            }
            Pieces.Add(new Piece(PieceType.Rook, Player.Black, 0, 7, 25));
            Pieces.Add(new Piece(PieceType.Knight, Player.Black, 1, 7, 26));
            Pieces.Add(new Piece(PieceType.Bishop, Player.Black, 2, 7, 27));
            Pieces.Add(new Piece(PieceType.Queen, Player.Black, 3, 7, 28));
            Pieces.Add(new Piece(PieceType.King, Player.Black, 4, 7, 29));
            Pieces.Add(new Piece(PieceType.Bishop, Player.Black, 5, 7, 30));
            Pieces.Add(new Piece(PieceType.Knight, Player.Black, 6, 7, 31));
            Pieces.Add(new Piece(PieceType.Rook, Player.Black, 7, 7, 32));
        }

        private void PlacePiecesOnBoard()
        {
            foreach (Piece piece in Pieces)
            {
                Board.PlacePiece(piece);
            }
        }

        public void NextTurn()
        {
            if (Winner != null) return; // Spillet er slut
            CurrentTurn = CurrentTurn == Player.White ? Player.Black : Player.White;
        }

        public void CheckWinner()
        {
            var whiteKingAlive = Pieces.Any(p => p.Type == PieceType.King && p.Owner == Player.White && !p.IsCaptured);
            var blackKingAlive = Pieces.Any(p => p.Type == PieceType.King && p.Owner == Player.Black && !p.IsCaptured);

            if (!whiteKingAlive) Winner = Player.Black;
            if (!blackKingAlive) Winner = Player.White;
        }
    }
}

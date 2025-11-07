namespace Chess.Models
{
    public class Game
    {
        public ChessBoard Board { get; set; } = new ChessBoard();
        public List<Piece> Pieces { get; set; } = new List<Piece>();
        public Player CurrentTurn { get; set; } = Player.White; // Starter med White
        public Player? Winner { get; set; } = null;

        public Game()
        {
            InitializePieces();
            PlacePiecesOnBoard();
        }

        private void InitializePieces()
        {
            // White pieces (rank 0 & 1)
            for (int file = 0; file < 8; file++)
            {
                Pieces.Add(new Piece(PieceType.Pawn, Player.White, file, 1));
            }
            Pieces.Add(new Piece(PieceType.Rook, Player.White, 0, 0));
            Pieces.Add(new Piece(PieceType.Knight, Player.White, 1, 0));
            Pieces.Add(new Piece(PieceType.Bishop, Player.White, 2, 0));
            Pieces.Add(new Piece(PieceType.Queen, Player.White, 3, 0));
            Pieces.Add(new Piece(PieceType.King, Player.White, 4, 0));
            Pieces.Add(new Piece(PieceType.Bishop, Player.White, 5, 0));
            Pieces.Add(new Piece(PieceType.Knight, Player.White, 6, 0));
            Pieces.Add(new Piece(PieceType.Rook, Player.White, 7, 0));

            // Black pieces (rank 6 & 7)
            for (int file = 0; file < 8; file++)
            {
                Pieces.Add(new Piece(PieceType.Pawn, Player.Black, file, 6));
            }
            Pieces.Add(new Piece(PieceType.Rook, Player.Black, 0, 7));
            Pieces.Add(new Piece(PieceType.Knight, Player.Black, 1, 7));
            Pieces.Add(new Piece(PieceType.Bishop, Player.Black, 2, 7));
            Pieces.Add(new Piece(PieceType.Queen, Player.Black, 3, 7));
            Pieces.Add(new Piece(PieceType.King, Player.Black, 4, 7));
            Pieces.Add(new Piece(PieceType.Bishop, Player.Black, 5, 7));
            Pieces.Add(new Piece(PieceType.Knight, Player.Black, 6, 7));
            Pieces.Add(new Piece(PieceType.Rook, Player.Black, 7, 7));
        }

        private void PlacePiecesOnBoard()
        {
            foreach (var piece in Pieces)
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

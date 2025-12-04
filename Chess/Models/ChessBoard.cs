

using Chess.Logic;

namespace Chess.Models
{
    public class ChessBoard
    {
        public List<string> PossibleMoves = new List<string>();
        public Dictionary<string, Cell> BoardCells { get; set; } = new();
        public Cell? Selected { get; set; }
        public ThreatTracker ThreatTracker = new ThreatTracker();
        public Cell WhiteKingPos;
        public Cell BlackKingPos;
        
        
        public string[] CellIds { get; set; } = [
        "a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8",
        "b1", "b2", "b3", "b4", "b5", "b6", "b7", "b8",
        "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8",
        "d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8",
        "e1", "e2", "e3", "e4", "e5", "e6", "e7", "e8",
        "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8",
        "g1", "g2", "g3", "g4", "g5", "g6", "g7", "g8",
        "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8"];
        public ChessBoard()
        {
            InstantiateCells();
        }

        public void InstantiateCells()
        {
            for (int i = 0; i < CellIds.Length; i++)
            {
                char colChar = CellIds[i][1];
                int col = int.Parse(colChar.ToString());

                int row = (int)CellIds[i][0] - 96; //Casting char to ASCII

                string color = "#161717ff";
                if ((row + col) % 2 == 0) { color = "#ffffffff"; }
                BoardCells.Add(CellIds[i], new Cell(CellIds[i], color, col, row));
            }

        }


        public void PlacePiece(Piece piece)
        {
            string cellId = $"{(char)('a' + piece.File)}{piece.Rank + 1}";
            if (BoardCells.ContainsKey(cellId))
            {
                BoardCells[cellId].Occupant = piece;
                if (piece.Id == 13) { WhiteKingPos = BoardCells[cellId]; }
                if (piece.Id == 29) { BlackKingPos = BoardCells[cellId]; }
            }
        }

    }
}
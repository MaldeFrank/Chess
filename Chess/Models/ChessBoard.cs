

using System.Reflection;
using Chess.logic;
using Chess.Logic;

namespace Chess.Models
{
    public class ChessBoard
    {
        public List<string> PossibleMoves = new List<string>();
        public Dictionary<string, Cell> BoardCells { get; set; } = new();
        public Cell? Selected { get; set; }
        public ThreatTracker ThreatTracker = new ThreatTracker();

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
            }
        }

        public bool SelectField(string selected)
        {

            if (Selected != null) { Selected.IsSelected = false; } //Removes select from last
            Selected = BoardCells[selected];

            Selected.IsSelected = true; //Highligts the selected field

            //Highligts the possible move choices
            if (PossibleMoves.Count > 0) { PossibleMoves.ForEach((id) => BoardCells[id].IsHighlighted = false); } // Resets the possible moves
            Piece? piece = null;

            if (Selected.Occupant != null)
            {
                piece = Selected.Occupant;
                PossibleMoves = MoveRegistry.Generators[piece.Type].GenerateMoves(Selected, BoardCells, ThreatTracker); // Sets the new possible moves

                PossibleMoves.ForEach((id =>
                {
                    BoardCells[id].IsHighlighted = true;
                }));
            }
            ;
            return true;
        }


        //Action when player moves to cell
        public void MakeMove(Cell targetCell)
        {
            if (Selected?.Occupant == null) return; // Can it ever be null here?

            var movingPiece = Selected.Occupant;

            HandleSpecialMoveLogic(movingPiece, targetCell);

            targetCell.Occupant = movingPiece;
            Selected.Occupant = null;

            movingPiece.File = targetCell.Col;
            movingPiece.Rank = targetCell.Row;
            movingPiece.Moves++;

            ThreatTracker.UpdateAllThreats(BoardCells);

            ClearSelection();
        }

        private void ClearSelection()
        {
            if (Selected != null) Selected.IsSelected = false;
            Selected = null;
            PossibleMoves.ForEach(id => BoardCells[id].IsHighlighted = false);
            PossibleMoves.Clear();
        }

        private void HandleSpecialMoveLogic(Piece piece, Cell targetCell)
        {
            if (piece.Type == PieceType.Pawn)
            {
                CheckPassant(piece, targetCell);
                QueenSpawnCheck(piece, targetCell);
            }
        }

        //If piece type is pawn, it checks cell behind after move to check for passant.
        private void CheckPassant(Piece selectedPiece, Cell cell)
        {
            if (selectedPiece.Type == PieceType.Pawn)
            {
                try
                {
                    (int, int) movedTo = ChessBoardUtility.ToCoords([cell.Id])[0];
                    int checkPassant = selectedPiece.Owner == Player.White ? -1 : +1; //Check left is white, and right if black.

                    (int, int) passantLocation = (movedTo.Item1, movedTo.Item2 + checkPassant); //Gets the location of the passant
                    Cell passantCell = ChessBoardUtility.FindCellFromCoords(passantLocation, this.BoardCells);

                    if (passantCell?.Occupant != null && passantCell.Occupant.Type == PieceType.Pawn)
                    {
                        passantCell.Occupant = null;
                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine($"NUll reference: {ex.Message}");
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine($"Cell id not found: {ex.Message}");
                }
            }
        }

        private void QueenSpawnCheck(Piece piece, Cell targetCell)
        {
            int promotionRank = piece.Owner == Player.White ? 8 : 1;

            if (targetCell.Col == promotionRank)
            {
                piece.Type = PieceType.Queen;
            }
        }

        public void ValidateBoardCells()
        {
            foreach (var kvp in BoardCells)
            {
                var cell = kvp.Value;
                if (cell.Row < 0 || cell.Row > 8 || cell.Col < 0 || cell.Col > 8)
                    throw new ArgumentOutOfRangeException($"Cell {cell.Id} has invalid row or col");
            }
        }


        public void ValidatePiecePlacement(Piece piece)
        {
            if (piece.File < 0 || piece.File > 7 || piece.Rank < 0 || piece.Rank > 7)
                throw new ArgumentOutOfRangeException("Piece has invalid File or Rank");
        }

        public void Validate()
        {
            ValidateBoardCells();
            foreach (var cell in BoardCells.Values)
            {
                if (cell.Occupant != null)
                    ValidatePiecePlacement(cell.Occupant);
            }
        }


    }
}
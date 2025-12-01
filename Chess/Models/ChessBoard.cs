

using System.Reflection;
using Chess.Components.Pages;
using Chess.logic;
using Chess.Logic;
using Force.DeepCloner;

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
        public bool MakeMove(Cell targetCell)
        {
            if (Selected?.Occupant == null) return false; // Can it ever be null here?
            Piece movingPiece = Selected.Occupant;
            BoardSnapshot snapshot = new BoardSnapshot(this);

            HandleSpecialMoveLogic(movingPiece, targetCell);

            targetCell.Occupant = movingPiece;
            KingTracking(targetCell, movingPiece); //Tracking kings position for checks
            Selected.Occupant = null;


            movingPiece.File = targetCell.Col;
            movingPiece.Rank = targetCell.Row;
            movingPiece.Moves++;

            ThreatTracker.UpdateAllThreats(BoardCells);

            Player owner = movingPiece.Owner == Player.White ? Player.White : Player.Black;
            Cell kingPos = owner == Player.White ? WhiteKingPos : BlackKingPos;

            if (ThreatTracker.IskingChecked(owner, kingPos.Id))
            {
                snapshot.Restore(this);
                ThreatTracker.UpdateAllThreats(BoardCells);
                return false;
            }

            ClearSelection();
            return true;
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

        private void KingTracking(Cell targetCell, Piece movingPiece)
        {
            if (movingPiece.Type != PieceType.King) return;

            Player color = movingPiece.Owner;
            if (color == Player.White)
                WhiteKingPos = targetCell;
            else
                BlackKingPos = targetCell;
        }

        //If piece type is pawn, it checks cell behind after move to check for passant.
        private void CheckPassant(Piece movingPawn, Cell targetCell)
        {
            if (targetCell.Occupant != null) return; //Passant can only move to empty space

            if (movingPawn.Type == PieceType.Pawn)
            {
                try
                {
                    (int, int) movedTo = ChessBoardUtility.ToCoords([targetCell.Id])[0];

                    int offset = movingPawn.Owner == Player.White ? -1 : +1;

                    (int, int) passantLocation = (movedTo.Item1, movedTo.Item2 + offset);
                    Cell passantCell = ChessBoardUtility.FindCellFromCoords(passantLocation, this.BoardCells);

                    if (passantCell?.Occupant != null && passantCell.Occupant.Type == PieceType.Pawn)
                    {
                        if (passantCell.Occupant.Owner == movingPawn.Owner) return; //Makes sure the pawn behind is an enemy
                        passantCell.Occupant = null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Passant Error: {ex.Message}");
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
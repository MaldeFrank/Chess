using Chess.logic;
using Chess.Models;

namespace Chess.Logic
{
    public class BoardLogic
    {
        private ChessBoard ChessBoard;

        public BoardLogic(ChessBoard chessBoard)
        {
            this.ChessBoard = chessBoard;
        }

        public bool SelectField(string selected)
        {

            if (ChessBoard.Selected != null) { ChessBoard.Selected.IsSelected = false; } //Removes select from last
            ChessBoard.Selected = ChessBoard.BoardCells[selected];

            ChessBoard.Selected.IsSelected = true; //Highligts the selected field

            //Highligts the possible move choices
            if (ChessBoard.PossibleMoves.Count > 0) { ChessBoard.PossibleMoves.ForEach((id) => ChessBoard.BoardCells[id].IsHighlighted = false); } // Resets the possible moves
            Piece? piece = null;

            if (ChessBoard.Selected.Occupant != null)
            {
                piece = ChessBoard.Selected.Occupant;
                ChessBoard.PossibleMoves = MoveRegistry.Generators[piece.Type].GenerateMoves(ChessBoard.Selected, ChessBoard.BoardCells, ChessBoard.ThreatTracker); // Sets the new possible moves

                ChessBoard.PossibleMoves.ForEach((id =>
                {
                    ChessBoard.BoardCells[id].IsHighlighted = true;
                }));
            }
           ;
            return true;
        }


        //Action when player moves to cell
        public bool MakeMove(Cell targetCell)
        {
            if (ChessBoard.Selected?.Occupant == null) return false; // Can it ever be null here?
            Piece movingPiece = ChessBoard.Selected.Occupant;
            BoardSnapshot snapshot = new BoardSnapshot(ChessBoard);

            HandleSpecialMoveLogic(movingPiece, targetCell);

            targetCell.Occupant = movingPiece;
            KingTracking(targetCell, movingPiece); //Tracking kings position for checks
            ChessBoard.Selected.Occupant = null;


            movingPiece.File = targetCell.Col;
            movingPiece.Rank = targetCell.Row;
            movingPiece.Moves++;

            ChessBoard.ThreatTracker.UpdateAllThreats(ChessBoard.BoardCells);

            Player owner = movingPiece.Owner == Player.White ? Player.White : Player.Black;
            Cell kingPos = owner == Player.White ? ChessBoard.WhiteKingPos : ChessBoard.BlackKingPos;

            if (ChessBoard.ThreatTracker.IskingChecked(owner, kingPos.Id, snapshot, ChessBoard))
            {
                return false;
            }

            ClearSelection();
            return true;
        }


        private void ClearSelection()
        {
            if (ChessBoard.Selected != null) ChessBoard.Selected.IsSelected = false;
            ChessBoard.Selected = null;
            ChessBoard.PossibleMoves.ForEach(id => ChessBoard.BoardCells[id].IsHighlighted = false);
            ChessBoard.PossibleMoves.Clear();
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
                ChessBoard.WhiteKingPos = targetCell;
            else
                ChessBoard.BlackKingPos = targetCell;
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
                    Cell passantCell = ChessBoardUtility.FindCellFromCoords(passantLocation, ChessBoard.BoardCells);

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
            foreach (var kvp in ChessBoard.BoardCells)
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
            foreach (var cell in ChessBoard.BoardCells.Values)
            {
                if (cell.Occupant != null)
                    ValidatePiecePlacement(cell.Occupant);
            }
        }
    }
}
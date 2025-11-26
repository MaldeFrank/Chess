using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class KingMoveGenerator : MoveGenerator
    {
        protected override bool ShouldStopMove(int row, int col, Cell cell, List<(int, int)> moves)
        {
            string cellId = ChessBoardUtility.ToCellId([(row, col)])[0];
            var occupant = cell?.Occupant;
            if (occupant == null)
                return false;
                
            if (ThreatTracker.IsCellTargeted(cellId, occupant.Owner))
            {
                moves.Clear();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds)
        {
            int col = cell.Col; // 1–8
            int row = cell.Row; // 1–8

            var moves = new List<(int, int)>();

            const int maxSteps = 1;

            //Diagonal
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, +1, cell, cellIds).GetResults());
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, -1, cell, cellIds).GetResults());
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, -1, cell, cellIds).GetResults());
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, +1, cell, cellIds).GetResults());

            //Straight
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, 0, cell, cellIds).GetResults());
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, 0, cell, cellIds).GetResults());
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, 0, +1, cell, cellIds).GetResults());
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, 0, -1, cell, cellIds).GetResults());
            return moves;
        }
    }
}
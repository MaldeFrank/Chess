using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class RookMoveGenerator : MoveGenerator
    {
        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds)
        {
            int col = cell.Col; // 1–8
            int row = cell.Row; // 1–8

            var moves = new List<(int, int)>();

            const int maxSteps = 8;
            //straight
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, 0, cell, cellIds));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, 0, cell, cellIds));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, 0, +1, cell, cellIds));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, 0, -1, cell, cellIds));

            return moves;
        }
    }
}
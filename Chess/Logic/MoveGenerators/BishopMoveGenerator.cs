
using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class BishopMoveGenerator : MoveGenerator
    {
        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds)
        {
            int col = cell.Col; // 1–8
            int row = cell.Row; // 1–8

            var moves = new List<(int, int)>();

            const int maxSteps = 7;

            // 1. Diagonal Up-Right
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, +1, cell, cellIds));
            // 2. Diagonal Down-Left
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, -1, cell, cellIds));
            // 3. Diagonal Up-Left
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, -1, cell, cellIds));
            // 4. Diagonal Down-Right
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, +1, cell, cellIds));

            return moves;
        }
    }
}
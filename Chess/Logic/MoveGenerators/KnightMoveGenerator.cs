using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class KnightMoveGenerator : MoveGenerator
    {
        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells)
        {
            int col = cell.Col; // 1–8
            int row = cell.Row; // 1–8

            var moves = new List<(int, int)>();

            const int maxSteps = 1;


            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +2, +1));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +2, -1));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -2, +1));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -2, -1));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, +2));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, +2));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, -2));
            moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, -2));

            return moves;
        }
    }
}
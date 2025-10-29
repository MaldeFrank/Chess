
using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class BishopMoveGenerator : MoveGenerator
    {
        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells)
        {
            int col = cell.Col; // 1–8
            int row = cell.Row; // 1–8

            var moves = new List<(int, int)>();

            const int MaxDistance = 7;
            moves.AddRange(GetMovesUpRight(row, col, MaxDistance, occupiedCells));
            moves.AddRange(GetMovesDownLeft(row, col, MaxDistance, occupiedCells));
            moves.AddRange(GetMovesUpLeft(row, col, MaxDistance, occupiedCells));
            moves.AddRange(GetMovesDownRight(row, col, MaxDistance, occupiedCells));

            return moves;
        }
    }
}

using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class BishopMoveGenerator : MoveGenerator
    {
        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell)
        {
            int col = cell.Col; // 1–8
            int row = cell.Row; // 1–8

            var moves = new List<(int, int)>();

            const int MaxDistance = 7;
            moves.AddRange(GetMovesUpRight(row, col, MaxDistance));
            moves.AddRange(GetMovesDownLeft(row, col, MaxDistance));
            moves.AddRange(GetMovesUpLeft(row, col, MaxDistance));
            moves.AddRange(GetMovesDownRight(row, col, MaxDistance));

            return moves;
        }
    }
}
using System.Buffers;
using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class PawnMoveGenerator : MoveGenerator
    {
        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells)
        {
            int col = cell.Col; // 1–8
            int row = cell.Row; // 1–8

            var moves = new List<(int, int)>();

            const int maxSteps = 1;

            if (cell.Occupant.Owner == Player.White)
            {
                //Diagonal
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, +1));
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, +1));
                //Straight
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, 0, +1));
            }
            else
            {
                //Diagonal
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, -1));
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, -1));
                //Straight
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, 0, -1));
            }


            return moves;
        }
    }
}
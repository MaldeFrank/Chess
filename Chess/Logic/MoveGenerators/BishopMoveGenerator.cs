using Chess.Logic.Interfaces;
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

            // Diagonal opad-højre
            int r = row + 1;
            int c = col + 1;
            while (r <= 8 && c <= 8)
            {
                moves.Add((r, c));
                r++;
                c++;
            }

            // Diagonal nedad-venstre
            r = row - 1;
            c = col - 1;
            while (r >= 1 && c >= 1)
            {
                moves.Add((r, c));
                r--;
                c--;
            }

            return moves;

        }
    }
}
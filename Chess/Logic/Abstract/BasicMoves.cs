
namespace Chess.Logic.Abstract
{
    public class BasicMoves
    {

        private bool Collision(int row, int col, List<(int, int)> occupiedCells)
        {
            (int, int) potentialMove = (row, col);

            if (occupiedCells.Contains(potentialMove))
            {
                return true;
            }
            return false;
        }

        protected List<(int row, int col)> GetMovesInDirection(int startRow, int startCol, int maxSteps, List<(int, int)> occupiedCells, int rowDirec, int colDirec)
        {
            var moves = new List<(int, int)>();
            int r = startRow + rowDirec;
            int c = startCol + colDirec;
            int steps = 0;

            while (r >= 1 && r <= 8 && c >= 1 && c <= 8 && steps < maxSteps)
            {
                if (Collision(r, c, occupiedCells))
                {
                    break;
                };

                moves.Add((r, c));

                r += rowDirec;
                c += colDirec;
                steps++;
            }
            return moves;
        }
    }
}
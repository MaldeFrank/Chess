
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
        
        protected List<(int, int)> GetMovesUpRight(int startRow, int startCol, int maxSteps, List<(int, int)> occupiedCells) 
        {
            var moves = new List<(int, int)>();
            int r = startRow + 1;
            int c = startCol + 1;
            int steps = 0;

            while (r <= 8 && c <= 8 && steps < maxSteps)
            {
                if(Collision(r, c, occupiedCells)){ break; }; //If field is occupied
                moves.Add((r, c));
                r++;
                c++;
                steps++;
            }
            return moves;
        }

        protected List<(int, int)> GetMovesDownLeft(int startRow, int startCol, int maxSteps, List<(int, int)> occupiedCells)
        {
            var moves = new List<(int, int)>();
            int r = startRow - 1;
            int c = startCol - 1;
            int steps = 0;

            while (r >= 1 && c >= 1 && steps < maxSteps)
            {
               if(Collision(r, c, occupiedCells)){ break; }; //If field is occupied
                moves.Add((r, c));
                r--;
                c--;
                steps++;
            }
            return moves;
        }

        protected List<(int, int)> GetMovesUpLeft(int startRow, int startCol, int maxSteps, List<(int, int)> occupiedCells)
        {
            var moves = new List<(int, int)>();
            int r = startRow + 1;
            int c = startCol - 1;
            int steps = 0;

            while (r <= 8 && c >= 1 && steps < maxSteps)
            {
                if(Collision(r, c, occupiedCells)){ break; }; //If field is occupied
                moves.Add((r, c));
                r++;
                c--;
                steps++;
            }
            return moves;
        }

        protected List<(int, int)> GetMovesDownRight(int startRow, int startCol, int maxSteps, List<(int, int)> occupiedCells)
        {
            var moves = new List<(int, int)>();
            int r = startRow - 1;
            int c = startCol + 1;
            int steps = 0;

            while (r >= 1 && c <= 8 && steps < maxSteps)
            {
                if(Collision(r, c, occupiedCells)){ break; }; //If field is occupied
                moves.Add((r, c));
                r--;
                c++;
                steps++;
            }
            return moves;
        }
    }
}
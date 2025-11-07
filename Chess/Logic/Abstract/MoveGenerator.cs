using Chess.Models;


namespace Chess.Logic.Abstract
{
    public abstract class MoveGenerator
    {
        /// <summary>
        /// Generates all the possible moves for the selected cell
        /// </summary>
        /// <param name="cell">Selected cell</param>
        /// <param name="cellIds">All board cells</param>
        /// <returns>List of cellids in format "a1" for all possible moves</returns>
        public List<string> GenerateMoves(Cell cell, Dictionary<string, Cell> cellIds)
        {
            var piece = cell.Occupant;
            if (piece == null) return new List<string>();
            List<string> occupiedCells = ChessBoardUtility.FindOccupiedCells(cellIds);
            List<(int, int)> occupiedCellsCoords = ChessBoardUtility.ToCoords(occupiedCells);

            var baseMoves = GeneratePieceSpecificMoves(cell, occupiedCellsCoords);

            return ChessBoardUtility.ToCellId(baseMoves);
        }
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
                }
                moves.Add((r, c));

                r += rowDirec;
                c += colDirec;
                steps++;
            }
            return moves;
        }

        /// <summary>
        /// When implemented in a derived class, this method calculates all possible target 
        /// coordinates (moves) for the specific piece type located on the starting cell, 
        /// </summary>
        /// <param name="cell">The starting cell containing the piece whose moves are being generated.</param>
        /// <param name="occupiedCells">A list of all currently occupied coordinates on the board.</param>
        /// <returns>A list of (row, column) coordinates representing all valid cells the piece can move to.</returns>
        protected abstract List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells);
    }

}
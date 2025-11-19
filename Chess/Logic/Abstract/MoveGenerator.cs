using Chess.Models;

namespace Chess.Logic.Abstract
{
    public abstract class MoveGenerator
    {

        public List<string> GenerateMoves(Cell cell, Dictionary<string, Cell> cellIds)
        {
            return Generate(cell, cellIds);
        }

        /// <summary>
        /// Generates all the possible moves for the selected cell
        /// </summary>
        /// <param name="cell">Selected cell</param>
        /// <param name="cellIds">All board cells</param>
        /// <returns>List of cellids in format "a1" for all possible moves</returns>
        protected List<string> Generate(Cell cell, Dictionary<string, Cell> cellIds)
        {
            var piece = cell.Occupant;
            if (piece == null) return new List<string>();
            List<string> occupiedCells = ChessBoardUtility.FindOccupiedCells(cellIds);
            List<(int, int)> occupiedCellsCoords = ChessBoardUtility.ToCoords(occupiedCells);

            var baseMoves = GeneratePieceSpecificMoves(cell, occupiedCellsCoords, cellIds);

            return ChessBoardUtility.ToCellId(baseMoves);
        }

        protected virtual void HandleCollision(int row, int col, List<(int, int)> occupiedCells, List<(int, int)> moves, Cell cell, Dictionary<string, Cell> cellIds)
        {
            (int, int) potentialMove = (row, col);

            List<string> move = ChessBoardUtility.ToCellId([potentialMove]);
            if (cellIds[move[0]]?.Occupant?.Owner != cell?.Occupant?.Owner)
            {
                moves.Add(potentialMove);
            }
        }

        /// <summary>
        /// Calculates moves in given direction specified by row and column movement.
        /// </summary>
        /// <param name="startRow">Starting row of selected cell</param>
        /// <param name="startCol">Starting column of selected cell</param>
        /// <param name="maxSteps">Max steps piece can move</param>
        /// <param name="occupiedCells">All occupied cells</param>
        /// <param name="rowDirec">Row movement</param>
        /// <param name="colDirec">Column movement</param>
        /// <returns> A DirectionBuilder (row,column)</returns>
        protected DirectionMoveBuilder GetMovesInDirection(int startRow, int startCol, int maxSteps, List<(int, int)> occupiedCells, int rowDirec, int colDirec, Cell cell, Dictionary<string, Cell> cellIds)
        {
            var moves = new List<(int, int)>();
            int r = startRow + rowDirec;
            int c = startCol + colDirec;
            int steps = 0;
            
            bool collision = false;

            while (r >= 1 && r <= 8 && c >= 1 && c <= 8 && steps < maxSteps)
            {

                if (occupiedCells.Contains((r, c))) //collision
                { 
                    collision = true;
                    HandleCollision(r, c, occupiedCells, moves,cell, cellIds);
                    break;
                }
                moves.Add((r,c));
                r += rowDirec;
                c += colDirec;
                steps++;
            }
            return new DirectionMoveBuilder(moves, collision);
        }

        /// <summary>
        /// When implemented in a derived class, this method calculates all possible target 
        /// coordinates (moves) for the specific piece type located on the starting cell, 
        /// </summary>
        /// <param name="cell">The starting cell containing the piece whose moves are being generated.</param>
        /// <param name="occupiedCells">A list of all currently occupied coordinates on the board.</param>
        /// <returns>A list of (row, column) coordinates representing all valid cells the piece can move to.</returns>
        protected abstract List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds);

    }

}
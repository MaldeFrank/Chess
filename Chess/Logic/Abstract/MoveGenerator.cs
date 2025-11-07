using Chess.Models;

namespace Chess.Logic.Abstract
{
    public abstract class MoveGenerator : BasicMoves
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
            List<string> occupiedCells = findOccupiedCells(cellIds);
            List<(int, int)> occupiedCellsCoords = ToCoords(occupiedCells);

            var baseMoves = GeneratePieceSpecificMoves(cell, occupiedCellsCoords);

            return ToCellId(baseMoves);
        }

        /// <summary>
        ///  Converts a list of tuples (row, column) to cellids like "a1", "h8"
        /// </summary>
        /// <param name="ids">List of (row, column) </param>
        /// <returns></returns>
        private List<string> ToCellId(List<(int, int)> ids)
        {
            Dictionary<int, string> mappings = new Dictionary<int, string>();
            mappings.Add(1, "a");
            mappings.Add(2, "b");
            mappings.Add(3, "c");
            mappings.Add(4, "d");
            mappings.Add(5, "e");
            mappings.Add(6, "f");
            mappings.Add(7, "g");
            mappings.Add(8, "h");

            List<string> cellIds = new List<string>();

            ids.ForEach(id =>
            {
                string letter = mappings[id.Item1];
                string number = id.Item2.ToString();
                cellIds.Add(letter + number);
            });

            return cellIds;
        }

        /// <summary>
        ///  Finds all occupied cells
        /// </summary>
        /// <param name="cellIds">A list of cells and their id.</param>
        /// <returns> Returns all ids of occupied cells </returns>
        private List<string> findOccupiedCells(Dictionary<string, Cell> cellIds)
        {
            List<string> occupiedCells = new List<string>();
            foreach (KeyValuePair<string, Cell> pair in cellIds)
            {
                if (pair.Value.Occupant != null)
                {
                    occupiedCells.Add(pair.Key);
                }
            }
            return occupiedCells;
        }

        /// <summary>
        /// Converts a list of chess notations like "a1", "h8" into a set of (row, column) coordinates.
        /// </summary>
        /// <param name="cellIds">The list of chess notations (e.g., ["a1", "b2"]).</param>
        /// <returns> A list of (row, column) coordinates. </returns>
        private List<(int, int)> ToCoords(List<string> cellIds)
        {
            Dictionary<string, int> mappings = new Dictionary<string, int>();
            mappings.Add("a", 1);
            mappings.Add("b", 2);
            mappings.Add("c", 3);
            mappings.Add("d", 4);
            mappings.Add("e", 5);
            mappings.Add("f", 6);
            mappings.Add("g", 7);
            mappings.Add("h", 8);

            List<(int, int)> coords = new List<(int, int)>();

            cellIds.ForEach(cellId =>
            {
                string letterPart = cellId.Substring(0, 1).ToLower();
                string numberPart = cellId.Substring(1);

                int row = mappings[letterPart];
                int col = int.Parse(numberPart);

                coords.Add((row, col));
            });

            return coords;
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
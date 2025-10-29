using Chess.Models;

namespace Chess.Logic.Abstract
{
    public abstract class MoveGenerator : BasicMoves
    {
        public List<string> GenerateMoves(Cell cell, Dictionary<string, Cell> cellIds)
        {
            var piece = cell.Occupant;
            if (piece == null) return new List<string>();
            List<string> occupiedCells = findOccupiedCells(cellIds);
            List<(int, int)> occupiedCellsCoords = ToCoords(occupiedCells);

            var baseMoves = GeneratePieceSpecificMoves(cell, occupiedCellsCoords);

            return ToCellId(baseMoves);
        }

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

        protected abstract List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells);
    }

}
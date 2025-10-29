using Chess.Models;

namespace Chess.Logic.Abstract
{
    public abstract class MoveGenerator: BasicMoves
    {
        public List<string> GenerateMoves(Cell cell)
        {
            var piece = cell.Occupant;
            if (piece == null) return new List<string>();

            var baseMoves = GeneratePieceSpecificMoves(cell);

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
                string row = mappings[id.Item1];
                string col = id.Item2.ToString();
                cellIds.Add(row + col);
            });

            return cellIds;
        }


        protected abstract List<(int, int)> GeneratePieceSpecificMoves(Cell cell);
    }

}
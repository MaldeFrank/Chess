
using Chess.Models;

namespace Chess.Logic
{
    public class DirectionMoveBuilder
    {
        private List<(int row, int col)> PotentialMoves;
        private readonly bool Collision;
        private Dictionary<string, Cell> CellIds;

        public DirectionMoveBuilder(List<(int row, int col)> moves, bool collision, Dictionary<string, Cell> cellIds)
        {
            this.PotentialMoves = moves;
            this.Collision = collision;
            this.CellIds = cellIds;
        }

        public DirectionMoveBuilder HandleCollision(Action<List<(int row, int col)>, bool> moves)
        {
            moves(this.PotentialMoves, this.Collision);
            return this;
        }

         public DirectionMoveBuilder FromPosition(Action<List<(int row, int col)>, Dictionary<string, Cell>> moves)
        {
            moves(this.PotentialMoves, this.CellIds);
            return this;
        }

        public List<(int row, int col)> GetResults()
        {
            return PotentialMoves;
        }

    }
}
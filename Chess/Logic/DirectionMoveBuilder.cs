
namespace Chess.Logic
{
    public class DirectionMoveBuilder
    {
        private List<(int row, int col)> potentialMoves;
        private readonly bool collision;

        public DirectionMoveBuilder(List<(int row, int col)> moves, bool collision)
        {
            this.potentialMoves = moves;
            this.collision = collision;
        }

        public DirectionMoveBuilder HandleCollision(Action<List<(int row, int col)>, bool> moves)
        {
            moves(this.potentialMoves, this.collision);
            return this;
        }

        public List<(int row, int col)> GetResults()
        {
            return potentialMoves;
        }

    }
}
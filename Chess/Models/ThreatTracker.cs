
namespace Chess.Models
{
    public class ThreatTracker
    {
        private Dictionary<int, HashSet<string>> Threats = [];
        private Dictionary<int, Player> PieceColors = [];

        public void AddThreats(Piece piece, HashSet<string> moves)
        {
            Threats[piece.Id] = moves;
            PieceColors[piece.Id] = piece.Owner;
        }

        public void PieceDead(Piece piece)
        {
            Threats.Remove(piece.Id);
        }

        public bool IsCellTargeted(string cellId, Player playerWhoAsks)
        {
            Player enemyColor = (playerWhoAsks == Player.White) ? Player.Black : Player.White;

            return Threats
                .Where(item => PieceColors[item.Key] == enemyColor)
                .Any(item => item.Value.Contains(cellId));
        }
    }
}
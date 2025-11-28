
using Chess.logic;

namespace Chess.Models
{
    public class ThreatTracker
    {
        private Dictionary<int, HashSet<string>> Threats = [];
        private Dictionary<int, Player> PieceColors = [];

        public Player TargetPlayer { get; }
        public string KingPosition { get; set; }
        public delegate void KingCheck(Player target, string position);
        public event KingCheck KingCheckEvent;

        private void AddThreats(Piece piece, HashSet<string> moves)
        {
            Threats[piece.Id] = moves;
            PieceColors[piece.Id] = piece.Owner;
        }

        public bool IsCellTargeted(string cellId, Player playerWhoAsks)
        {
            Player enemyColor = (playerWhoAsks == Player.White) ? Player.Black : Player.White;

            return Threats
                .Where(item => PieceColors[item.Key] == enemyColor)
                .Any(item => item.Value.Contains(cellId));
        }

        public void UpdateAllThreats(Dictionary<string, Cell> BoardCells)
        {
            ClearAll();

            foreach (var cell in BoardCells.Values)
            {
                if (cell.Occupant != null)
                {
                    var piece = cell.Occupant;

                    var moves = MoveRegistry.Generators[piece.Type]
                                .GenerateMoves(cell, BoardCells, this);

                    AddThreats(piece, moves.ToHashSet());
                }
            }
        }

        public void ClearAll()
        {
            Threats.Clear();
            PieceColors.Clear();
        }
    }
}
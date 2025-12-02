
using Chess.logic;

namespace Chess.Models
{
    public class ThreatTracker
    {
        private Dictionary<int, HashSet<string>> Threats = [];
        private Dictionary<int, Player> PieceColors = [];
        public event EventHandler<(Player, string)> KingCheckEvent;
        public bool IsSimulation {get;set;} = false;

        public bool IskingChecked(Player player, string cell, BoardSnapshot snapshot, ChessBoard board)
        {
            if (IsCellTargeted(cell, player))
            {
                snapshot.Restore(board); //Restore board (King cant move there)
                board.ThreatTracker.UpdateAllThreats(board.BoardCells);
                if (!IsSimulation) KingCheckEvent?.Invoke(this, (player, cell));
                return true;
            }
            return false;
        }
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
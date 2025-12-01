using Force.DeepCloner;
using Chess.logic;
using Chess.Models;

namespace Chess.Logic
{
    public class MoveSimulator
    {
        private Dictionary<string, Cell> GetOccupantPlayerCells(Player player, ChessBoard board)
        {
            return board.BoardCells
                .Where(kvp => kvp.Value.Occupant?.Owner == player)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private bool MakeSimMoves(Player player, ChessBoard board)
        {
            ChessBoard BoardClone = board.DeepClone();
            var playerCells = GetOccupantPlayerCells(player, board);
            BoardClone.ThreatTracker.IsSimulation = true;

            foreach (var kvp in playerCells)
            {
                if (kvp.Value.Occupant == null) continue;

                Cell cellStart = kvp.Value;

                List<string> moves = MoveRegistry.Generators[cellStart.Occupant.Type]
                    .GenerateMoves(kvp.Value, BoardClone.BoardCells, BoardClone.ThreatTracker);

                foreach (string moveId in moves)
                {
                    BoardSnapshot simSnapshot = new BoardSnapshot(BoardClone);

                    BoardClone.Selected = cellStart;
                    BoardClone.Selected.IsSelected = true;

                    bool isSafe = BoardClone.MakeMove(BoardClone.BoardCells[moveId]);

                    simSnapshot.Restore(BoardClone);
                    BoardClone.ThreatTracker.UpdateAllThreats(BoardClone.BoardCells);

                    if (isSafe)
                    {
                        BoardClone.ThreatTracker.IsSimulation = false;
                        return false;
                    }
                }
            }

            BoardClone.ThreatTracker.IsSimulation = false;
            return true; // No safe moves found, king is checkmate
        }

        public bool IsKingChessmate(Player player, ChessBoard board)
        {
            return MakeSimMoves(player, board);
        }
    }
}
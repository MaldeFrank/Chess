using Force.DeepCloner;
using Chess.logic;
using Chess.Models;

namespace Chess.Logic
{
    public class MoveSimulator
    {
        private IEnumerable<string> GetOccupantPlayerCells(Player player, ChessBoard board)
        {
            return board.BoardCells
                .Where(kvp => kvp.Value.Occupant?.Owner == player)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                .Select(kvp => kvp.Key);
        }

        private bool MakeSimMoves(Player player, ChessBoard board)
        {
            ChessBoard BoardClone = board.DeepClone();
            var playerCellsIds = GetOccupantPlayerCells(player, BoardClone);
            BoardClone.ThreatTracker.IsSimulation = true;

            foreach (var cellId in playerCellsIds)
            {
                if (!BoardClone.BoardCells.TryGetValue(cellId, out var cellStart) || cellStart.Occupant == null)
                    continue;

                List<string> moves = MoveRegistry.Generators[cellStart.Occupant.Type]
                    .GenerateMoves(cellStart, BoardClone.BoardCells, BoardClone.ThreatTracker);

                foreach (string moveId in moves)
                {
                    BoardSnapshot simSnapshot = new BoardSnapshot(BoardClone);

                    BoardClone.Selected = BoardClone.BoardCells[cellId];
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
using Chess.Logic.Abstract;
using Chess.Models;
using static Chess.Logic.ChessBoardUtility;

namespace Chess.Logic
{
    public class PawnMoveGenerator : MoveGenerator
    {
        private const int MAX_STEPS = 1;

        private void GenerateDiagonalMoves(int rowDirec, int colDirec, Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds, List<(int, int)> moves)
        {
            int col = cell.Col;
            int row = cell.Row;

            moves.AddRange(GetMovesInDirection(row, col, MAX_STEPS, occupiedCells, rowDirec, colDirec, cell, cellIds)
            .HandleCollision((moves, collision) =>
            {
                if (!collision) moves.Clear();
            })
            .GetResults());
        }

        private void GenerateStraightMoves(int colDirec, Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds, List<(int, int)> moves, int extraStepPos)
        {
            int col = cell.Col;
            int row = cell.Row;

            moves.AddRange(GetMovesInDirection(row, col, MAX_STEPS, occupiedCells, 0, colDirec, cell, cellIds)
            .FromPosition((moves, cells) =>
            {
                if (cell.Col == extraStepPos) //If pawn is at start position
                {
                    if (moves.Any())
                    {
                        (int r1, int c1) lastStep = moves.Last();

                        (int r2, int c2) doubleStep = (lastStep.r1, lastStep.c1 + colDirec);

                        string cellId = ToCellId([doubleStep])[0];

                        if (cellIds.TryGetValue(cellId, out Cell moveTo) && moveTo.Occupant == null)
                        {
                            moves.Add(doubleStep);
                        }
                    }
                }
            })
            .HandleCollision((m, collision) =>
            {
                if (collision) m.Clear();
            })
            .GetResults());
        }

        private void GeneratePassantMove(int colDirec, Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds, List<(int, int)> moves, int passantPos)
        {
            int col = cell.Col;
            int row = cell.Row;

            moves.AddRange(GetMovesInDirection(row, col, MAX_STEPS, occupiedCells, 0, colDirec, cell, cellIds)
              .HandleCollision((m, collision) => { if (collision) m.Clear(); })
              .FromPosition((moves, cells) =>
              {
                  if (cell.Col == passantPos) //If pawn is at passant position
                  {
                      (int, int) passantOnePos = (cell.Row + 1, cell.Col);
                      (int, int) passantTwoPos = (cell.Row - 1, cell.Col);

                      string passantOneId = InRange(passantOnePos) ? ToCellId([passantOnePos])[0] : "";
                      string passantTwoId = InRange(passantTwoPos) ? ToCellId([passantTwoPos])[0] : "";

                      Piece? pieceOne = null;
                      Piece? pieceTwo = null;

                      pieceOne = passantOneId != "" ? cells[passantOneId].Occupant : null;
                      pieceTwo = passantTwoId != "" ? cells[passantTwoId].Occupant : null;

                      if (pieceOne != null && pieceOne.Owner != cell.Occupant?.Owner && pieceOne.Moves == 1)
                      {
                          moves.Add((passantOnePos.Item1, passantOnePos.Item2 + colDirec));
                      }

                      if (pieceTwo != null && pieceTwo.Owner != cell.Occupant?.Owner && pieceTwo.Moves == 1)
                      {
                          moves.Add((passantTwoPos.Item1, passantTwoPos.Item2 + colDirec));
                      }
                  }
              })
             .GetResults());
        }

        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds)
        {
            var moves = new List<(int, int)>();

            bool isWhite = cell?.Occupant?.Owner == Player.White;
            int colDirec = isWhite ? +1 : -1;
            int startCol = isWhite ? 2 : 7;
            int passantPos = isWhite ? 5 : 4;

            // 1. Diagonal
            GenerateDiagonalMoves(-1, colDirec, cell, occupiedCells, cellIds, moves);
            GenerateDiagonalMoves(+1, colDirec, cell, occupiedCells, cellIds, moves);

            // 2. straight
            GenerateStraightMoves(colDirec, cell, occupiedCells, cellIds, moves, startCol);

            GeneratePassantMove(colDirec, cell, occupiedCells, cellIds, moves, passantPos);

            return moves;
        }
    }
}
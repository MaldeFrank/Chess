using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class PawnMoveGenerator : MoveGenerator
    {

        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells, Dictionary<string, Cell> cellIds)
        {
            int col = cell.Col; // 1–8
            int row = cell.Row; // 1–8

            var moves = new List<(int, int)>();

            const int maxSteps = 1;

            if (cell?.Occupant?.Owner == Player.White)
            {
                //Diagonal
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, +1, cell, cellIds)
                .HandleCollision((m, collision) => { if (!collision) m.Clear(); })
                .GetResults());

                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, +1, cell, cellIds)
                .HandleCollision((m, collision) => { if (!collision) m.Clear(); })
                .GetResults());

                //Straight
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, 0, +1, cell, cellIds)
                .HandleCollision((m, collision) =>{if(collision) m.RemoveAt(m.Count-1);})
                .GetResults());
            }
            else
            {
                //Diagonal
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, +1, -1, cell, cellIds)
                .HandleCollision((m, collision) => { if (!collision) m.Clear(); })
                .GetResults());

                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, -1, -1, cell, cellIds)
                .HandleCollision((m, collision) => { if (!collision) m.Clear(); })
                .GetResults());

                //Straight
                moves.AddRange(GetMovesInDirection(row, col, maxSteps, occupiedCells, 0, -1, cell, cellIds)
                .HandleCollision((m, collision) =>{if(collision) m.RemoveAt(m.Count-1);})
                .GetResults());
            }


            return moves;
        }
    }
}
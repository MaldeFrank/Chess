using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class QueenMoveGenerator : MoveGenerator
    {
        protected override List<(int, int)> GeneratePieceSpecificMoves(Cell cell, List<(int, int)> occupiedCells)
        {
            throw new NotImplementedException();
        }
    }
}
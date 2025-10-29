using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class PawnMoveGenerator : MoveGenerator
    {
        protected override List<(int,int)> GeneratePieceSpecificMoves(Cell cell)
        {
            throw new NotImplementedException();
        }
    }
}
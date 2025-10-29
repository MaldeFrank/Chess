using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.Logic
{
    public class RookMoveGenerator : MoveGenerator
    {
        protected override List<(int,int)> GeneratePieceSpecificMoves(Cell cell)
        {
            throw new NotImplementedException();
        }
    }
}
using Chess.Logic;
using Chess.Logic.Abstract;
using Chess.Models;

namespace Chess.logic
{
     public static class MoveRegistry
    {
        public static readonly Dictionary<PieceType, MoveGenerator> Generators = new()
        {
            { PieceType.Pawn, new PawnMoveGenerator() },
            { PieceType.Knight, new KnightMoveGenerator() },
            { PieceType.Bishop, new BishopMoveGenerator() },
            { PieceType.Rook, new RookMoveGenerator() },
            { PieceType.Queen, new QueenMoveGenerator() },
            { PieceType.King, new KingMoveGenerator() }
        };
    }

}
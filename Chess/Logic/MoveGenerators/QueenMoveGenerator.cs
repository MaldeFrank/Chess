using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chess.Logic.Interfaces;
using Chess.Models;

namespace Chess.Logic
{
    public class QueenMoveGenerator : MoveGenerator
    {
        protected override List<(int,int)> GeneratePieceSpecificMoves(Cell cell)
        {
            throw new NotImplementedException();
        }
    }
}
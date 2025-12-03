using Chess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTests.Models
{
    [TestClass]
    public class PieceTests
    {
        [TestMethod]
        public void PositionTest()
        {
            var piece = new Piece(PieceType.Knight, Player.White, 0, 7);
            piece.ValidatePosition();

            var piece2 = new Piece(PieceType.Rook, Player.Black, -1, 8);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => piece2.ValidatePosition());

            var piece3 = new Piece(PieceType.Pawn, Player.White, 0, -1);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => piece3.ValidatePosition());
        }
    }
}
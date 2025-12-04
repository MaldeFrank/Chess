using Chess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tests
{
    [TestClass()]
    public class CellTests
    {
        [TestMethod]
        public void PositionTest()
        {
            var cell = new Cell("A1","white", 0, 0);
            cell.ValidatePosition();

            var cell2 = new Cell("H1", "black", -1, 0);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cell2.ValidatePosition());

            var cell3 = new Cell("A8", "white", 0, 8);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cell3.ValidatePosition());

            var cell4 = new Cell("I9", "black", 10, -4);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cell4.ValidatePosition());

            var cell5 = new Cell("D5", "white", 3, -1);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cell5.ValidatePosition());
        }

        [TestMethod]
        public void ValidateOccupantTest()
        {
            // Gyldig occupant
            var piece = new Piece(PieceType.Pawn, Player.White, 1, 2);
            var cell = new Cell("B3", "white", 1, 2) { Occupant = piece };
            cell.ValidateOccupant();

            // Ugyldig occupant
            var piece2 = new Piece(PieceType.Rook, Player.Black, 0, 0);
            var cell2 = new Cell("A1", "black", 1, 1) { Occupant = piece2 };
            Assert.ThrowsException<InvalidOperationException>(() => cell2.ValidateOccupant());
        }

        [TestMethod]
        public void ValidateTest()
        {
            var piece = new Piece(PieceType.Rook, Player.White, 0, 0);
            var cell = new Cell("A1", "white", 0, 0) { Occupant = piece };
            cell.Validate();

            var cell2 = new Cell("A9", "white", 0, 9);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cell2.Validate());

            var piece2 = new Piece(PieceType.Knight, Player.Black, 1, 1);
            var cell3 = new Cell("A1", "black", 0, 0) { Occupant = piece2 };
            Assert.ThrowsException<InvalidOperationException>(() => cell3.Validate());
        }
    }
}
using System.Collections.Generic;

namespace Chess.Models
{

    public enum PieceType { Pawn, Knight, Bishop, Rook, Queen, King }

    public class Piece
    {
        public PieceType Type { get; set; }
        public Player Owner { get; set; }
        public int File { get; set; }
        public int Rank { get; set; }
        public bool IsCaptured { get; set; }

        public Piece(PieceType type, Player owner, int file, int rank)
        {
            Type = type;
            Owner = owner;
            File = file;
            Rank = rank;
            IsCaptured = false;
        }

        // Unicode symbols for chess pieces
        public string UnicodeSymbol => Owner == Player.White
            ? WhiteSymbols[Type]
            : BlackSymbols[Type];

        private static readonly Dictionary<PieceType, string> WhiteSymbols = new()
    {
        { PieceType.King,   "\u2654" }, // ♔
        { PieceType.Queen,  "\u2655" }, // ♕
        { PieceType.Rook,   "\u2656" }, // ♖
        { PieceType.Bishop, "\u2657" }, // ♗
        { PieceType.Knight, "\u2658" }, // ♘
        { PieceType.Pawn,   "\u2659" }  // ♙
    };
        private static readonly Dictionary<PieceType, string> BlackSymbols = new()
    {
        { PieceType.King,   "\u265A" }, // ♚
        { PieceType.Queen,  "\u265B" }, // ♛
        { PieceType.Rook,   "\u265C" }, // ♜
        { PieceType.Bishop, "\u265D" }, // ♝
        { PieceType.Knight, "\u265E" }, // ♞
        { PieceType.Pawn,   "\u265F" }  // ♟
    };

        public void ValidatePosition()
        {
            if (File < 0 || File > 7)
                throw new ArgumentOutOfRangeException($"Invalid file: {File}. Must be between 0 and 7.");
            if (Rank < 0 || Rank > 7)
                throw new ArgumentOutOfRangeException($"Invalid rank: {Rank}. Must be between 0 and 7.");
        }
    }

}

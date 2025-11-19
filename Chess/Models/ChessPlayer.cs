namespace Chess.Models
{
    public class ChessPlayer
    {
        public string Name { get; set; } = String.Empty;
        public string Color { get; set; } = String.Empty;

        // Enum for at linke til spillets logik
        public Player PlayerEnum { get; set; }
    }
}
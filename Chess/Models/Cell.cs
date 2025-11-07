

namespace Chess.Models
{

    public class Cell
    {
        public string Id { get; set; }
        public Piece? Occupant { get; set; }
        //public string Width { get; } = "150px";
        //public string Height { get; } = "150px";
        public int Row { get; set; }
        public int Col { get; set; }
        public Boolean IsSelected { get; set; }
        public string Color { get; set; }

        public Cell(string id, string color, int col, int row)
        {
            Id = id;
            Color = color;
            Col = col;
            Row = row;
        }

        public void ValidatePosition()
        {
            if (Col < 0 || Col > 7)
                throw new ArgumentOutOfRangeException(nameof(Col), $"Column {Col} is out of range 0-7");

            if (Row < 0 || Row > 7)
                throw new ArgumentOutOfRangeException(nameof(Row), $"Row {Row} is out of range 0-7");
        }

        public void ValidateOccupant()
        {
            if (Occupant != null)
            {
                if (Occupant.File != Col || Occupant.Rank != Row)
                    throw new InvalidOperationException($"Occupant {Occupant.Type} position does not match cell {Id}");
            }
        }

        public void Validate()
        {
            ValidatePosition();
            ValidateOccupant();
        }

    }
}
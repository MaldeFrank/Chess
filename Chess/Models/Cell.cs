namespace BoardInfo
{

    public class Cell
    {
        public string Id { get; set; }
        public Piece? Occupant { get; set; }
        public string Width { get; } = "150px";
        public string Height { get; } = "150px";
        public int Row { get; set; }
        public int Col { get; set; }
        public Boolean IsSelected { get; set; }
        public string Color { get; set; }

        public Cell(string id, string color, int col, int row)
        {
            this.Id = id;
            this.Color = color;
            this.Col = col;
            this.Row = row;
        }
    }
}
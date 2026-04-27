namespace Minesweeper
{
    public interface ICell
    {
        /// <summary>
        /// The current state of the cell. Setting this wlil update the UI.
        /// </summary>
        CellState State { get; set; }
        
        int Row { get; set; }

        int Column { get; set; }

        int Value { get; set; }

        /// <summary>
        /// True if there's a mine on this cell, otherwise false.
        /// </summary>
        bool HasMine { get; set; }
    }
}
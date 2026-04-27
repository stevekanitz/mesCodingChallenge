namespace Minesweeper
{
    /// <summary>
    /// Provides event data when a minefield cell is clicked.
    /// </summary>
    public class CellClickedEventArgs : EventArgs
    {
        public CellClickedEventArgs(ICell cell, MouseButtons buttons)
        {
            this.Cell = cell;
            this.Buttons = buttons;
        }
        /// <summary>
        /// The minefield cell that was clicked.
        /// </summary>
        public ICell Cell { get; }
        /// <summary>
        /// The mouse button that was pressed.
        /// </summary>
        public MouseButtons Buttons { get; }
    }
}

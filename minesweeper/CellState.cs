namespace Minesweeper
{
    /// <summary>
    /// Represents the state of a minefield cell.
    /// </summary>
    public enum CellState
    {
        /// <summary>
        /// The cell state is unknown and not flagged.
        /// </summary>
        Unknown,
        /// <summary>
        /// The cell has 0 adjacent mines.
        /// </summary>
        Zero,
        /// <summary>
        /// The cell has 1 adjacent mine.
        /// </summary>
        One,
        /// <summary>
        /// The cell has 2 adjacent mines.
        /// </summary>
        Two,
        /// <summary>
        /// The cell has 3 adjacent mines.
        /// </summary>
        Three,
        /// <summary>
        /// The cell has 4 adjacent mines.
        /// </summary>
        Four,
        /// <summary>
        /// The cell has 5 adjacent mines.
        /// </summary>
        Five,
        /// <summary>
        /// The cell has 6 adjacent mines.
        /// </summary>
        Six,
        /// <summary>
        /// The cell has 7 adjacent mines.
        /// </summary>
        Seven,
        /// <summary>
        /// The cell has 8 adjacent mines.
        /// </summary>
        Eight,
        /// <summary>
        /// The cell is flagged as suspect for having a mine.
        /// </summary>
        Flagged,
        /// <summary>
        /// The cell was flagged when there was no mine.
        /// </summary>
        Misflagged,
        /// <summary>
        /// The cell has an exploded mine (game over).
        /// </summary>
        Exploded,
        /// <summary>
        /// The cell had a mine but was unflagged (post-game reveal).
        /// </summary>
        Unflagged
    }
}

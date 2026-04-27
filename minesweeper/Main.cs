namespace Minesweeper
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles when the reset button is clicked.
        /// </summary>
        /// <param name="sender">The reset button.</param>
        /// <param name="e">Event args.</param>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            // TODO: Reset the mine field for a new game.
            // Access the mine field control via the MineField property like this:
            MineField.Cells[0, 0].State = CellState.Unknown;
        }

        /// <summary>
        /// Handles when a minefield cell is clicked.
        /// </summary>
        /// <param name="sender">The minefield.</param>
        /// <param name="e">Cell clicked event args.</param>
        private void MineField_CellClicked(object sender, CellClickedEventArgs e)
        {
            // TODO: Evaluate the cell state, mine status, and mouse button clicked to update the minefield.
        }
    }
}

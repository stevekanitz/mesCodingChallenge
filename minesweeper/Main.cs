using System.CodeDom.Compiler;

namespace Minesweeper
{
    public partial class Main : Form
    {
        private bool firstLeftClickDone = false;
        private bool gameDone = false;
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
            //MineField.Cells[0, 0].State = CellState.Unknown;
            foreach (var cell in MineField.Cells)
            {
                cell.State = CellState.Unknown;
                cell.HasMine = false;
            }
            firstLeftClickDone = false;
            gameDone = false;
        }

        /// <summary>
        /// Handles when a minefield cell is clicked.
        /// </summary>
        /// <param name="sender">The minefield.</param>
        /// <param name="e">Cell clicked event args.</param>
        private void MineField_CellClicked(object sender, CellClickedEventArgs e)
        {
            // TODO: Evaluate the cell state, mine status, and mouse button clicked to update the minefield.
            if (gameDone)
            {
                return;
            }

            if (e.Buttons == MouseButtons.Right)
            {
                RightMouseClick(e.Cell);
            }

            if (e.Buttons == MouseButtons.Left)
            {
                LeftMouseclick(e.Cell);
            }
        }

        private void RightMouseClick(ICell cell)
        {
            if (cell.State == CellState.Unknown)
                {
                    cell.State = CellState.Flagged;
                }
            else if (cell.State == CellState.Flagged)
                {
                    cell.State = CellState.Unknown;
                }
        }

        private void LeftMouseclick (ICell cell)
        {
            if (!firstLeftClickDone)
            {
                GenerateMines(cell);
                GenerateNumbers();
                ShowResults(cell);
                firstLeftClickDone = true;
                return;
            }

            if (cell.State != CellState.Unknown && cell.State != CellState.Flagged)
            {
                return;
            } 

            ShowResults(cell);

        }

        private void GenerateMines(ICell cell)
        {
            int clickedCellRow = cell.Row;
            int clickedCellCol = cell.Column;

            Random random = new Random();
            int minesPlaced = 0;

            while (minesPlaced < 10)
            {
                int mineRow = random.Next(9);
                int mineColumn = random.Next(9);

                if (Math.Abs(mineRow - clickedCellRow) <= 1 && Math.Abs(mineColumn - clickedCellCol) <= 1)
                continue;

                if (!MineField.Cells[mineColumn, mineRow].HasMine)
                {
                    MineField.Cells[mineColumn, mineRow].HasMine = true;
                    minesPlaced++;
                }   
            }
        }

        private void GenerateNumbers()
        {
            foreach (var cell in MineField.Cells)
            {
                if (cell.HasMine)
                    continue;

                int count = 0;

                for (int dy = -1; dy <= 1; dy++)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;

                        int nearRow = cell.Row + dy;
                        int nearColumn = cell.Column + dx;

                        if (nearRow < 0 || nearRow >= 9 || nearColumn < 0 || nearColumn >= 9)
                            continue;

                        if (MineField.Cells[nearColumn, nearRow].HasMine)
                            count++;
                    }
                }

                cell.Value = count;
            }
        }

        private void ShowResults(ICell cell)
        {
            if (cell.State != CellState.Unknown)
                return;

            if (cell.HasMine)
            {
                cell.State = CellState.Exploded;
                foreach (var cellLeft in MineField.Cells)
                {
                    if (cellLeft.HasMine && cellLeft.State == CellState.Unknown)
                    {
                        cellLeft.State = CellState.Unflagged;
                    }
                    else if (!cellLeft.HasMine && cellLeft.State == CellState.Flagged)
                    {
                        cellLeft.State = CellState.Misflagged;
                    }
                    else if (cellLeft.State == CellState.Unknown)
                    {
                        cellLeft.State = (CellState)(cell.Value + 1);
                    }
                }

                gameDone = true;
                return;
            }

            cell.State = (CellState)(cell.Value + 1);

            if (cell.Value > 0)
                return;

            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    int nearRow = cell.Row + dy;
                    int nearColumn = cell.Column + dx;

                    if (nearRow < 0 || nearRow >= 9 || nearColumn < 0 || nearColumn >= 9)
                        continue;

                    ShowResults(MineField.Cells[nearColumn, nearRow]);
                }
            }
            bool gameWon = MineField.Cells.Cast<ICell>().Where(c => c.State == CellState.Unknown || c.State == CellState.Flagged).All(c => c.HasMine);
            if (gameWon)
            {
                foreach (var cellLeft in MineField.Cells)
                {
                    if (cellLeft.HasMine && (cellLeft.State == CellState.Unknown || cellLeft.State == CellState.Flagged))
                    {
                        cellLeft.State = CellState.Unflagged;
                    }
                }
                gameDone = true;
            }

        }
    }
}

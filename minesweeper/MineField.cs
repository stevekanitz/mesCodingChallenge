using System.Resources;

namespace Minesweeper
{
    /// <summary>
    /// The mine field control.
    /// </summary>
    public partial class MineField : Control
    {
        private static readonly Dictionary<CellState, Image> _cellImages;

        private readonly Cell[,] _cells = new Cell[9, 9];

        /// <summary>
        /// Represents a 9 x 9 2D array of cells making up the mine field.
        /// </summary>
        public ICell[,] Cells => _cells;

        /// <summary>
        /// Raised when an unswept or flagged cell is clicked.
        /// The event handler must determine whether the user is sweeping, flagging, or unflagging.
        /// </summary>
        public event EventHandler<CellClickedEventArgs> CellClicked;

        static MineField()
        {
            var rm = new ResourceManager(typeof(MineField));
            _cellImages = Enum.GetValues<CellState>().ToDictionary(cs => cs, cs => (Image)rm.GetObject(cs.ToString())!);
        }

        public MineField()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackgroundImage = new Bitmap(9 * 16, 9 * 16);
            this.BackgroundImageLayout = ImageLayout.None;
            this.MouseDown += MineField_MouseDown;
            this.MouseMove += MineField_MouseMove;
            this.MouseUp += MineField_MouseUp;

            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    var cell = new Cell
                    {
                        Location = new Point(x * 16, y * 16),
                        Row = y,
                        Column = x

                    };
                    cell.StateChanged += (s, e) => RedrawCell((Cell)s!);
                    Cells[x, y] = cell;
                    RedrawCell(cell);
                }
            }
        }

        private void MineField_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.X < 0 || e.X >= this.Width || e.Y < 0 || e.Y >= this.Height)
            {
                for (var x = 0; x < 9; x++)
                {
                    for (var y = 0; y < 9; y++)
                    {
                        var cell = _cells[x, y];
                        if (cell.Sweeping)
                        {
                            cell.Sweeping = false;
                            RedrawCell(cell);
                        }
                    }
                }
                return;
            }

            try
            {
                var currentCell = _cells[e.X / 16, e.Y / 16];
                if (currentCell.State == CellState.Unknown || currentCell.State == CellState.Flagged)
                {
                    currentCell.Sweeping = false;
                    RedrawCell(currentCell);
                    CellClicked?.Invoke(this, new CellClickedEventArgs(currentCell, e.Button));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void MineField_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.X < 0 || e.X >= this.Width || e.Y < 0 || e.Y >= this.Height)
            {
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    var currentCell = _cells[e.X / 16, e.Y / 16];

                    for (var x = 0; x < 9; x++)
                    {
                        for (var y = 0; y < 9; y++)
                        {
                            var cell = _cells[x, y];
                            if (cell != currentCell && cell.Sweeping)
                            {
                                cell.Sweeping = false;
                                RedrawCell(cell);
                            }
                        }
                    }
                    if (!currentCell.Sweeping)
                    {
                        currentCell.Sweeping = true;
                        RedrawCell(currentCell);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void MineField_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            try
            {
                var cell = _cells[e.X / 16, e.Y / 16];
                if (cell.State == CellState.Unknown)
                {
                    cell.Sweeping = true;
                    using var g = Graphics.FromImage(this.BackgroundImage!);
                    RedrawCell(cell);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void RedrawCell(Cell cell)
        {
            using var g = Graphics.FromImage(this.BackgroundImage!);
            g.DrawImage(cell.Sweeping ? _cellImages[CellState.Zero] : _cellImages[cell.State], cell.Location);
            Invalidate();
        }
    }
}

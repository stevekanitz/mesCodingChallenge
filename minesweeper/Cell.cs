namespace Minesweeper
{
    public class Cell : ICell
    {
        private CellState _state;

        public event EventHandler? StateChanged;

        public Point Location { get; set; }

        public CellState State
        {
            get => _state;
            set
            {
                _state = value;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool HasMine { get; set; }

        public bool Sweeping { get; set; }
    }
}

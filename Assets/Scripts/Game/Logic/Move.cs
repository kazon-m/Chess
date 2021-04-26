namespace Game.Logic
{
    public class Move
    {
        public delegate void MoveCallback();

        private MoveCallback _callback;
        private MoveCallback _callbackToReset;

        public Move(Square square, bool harmMove = true)
        {
            Square = square;
            _callback = null;
            IsHarmMove = harmMove;
        }

        public Square Square { get; }

        public bool IsHarmMove { get; }

        public void RegisterCallback(MoveCallback callback) => _callback = callback;

        public void RegisterCallbackToReset(MoveCallback callback) => _callbackToReset = callback;

        public void RunCallbackReset() => _callbackToReset?.Invoke();

        public void RunCallback() => _callback?.Invoke();
    }
}
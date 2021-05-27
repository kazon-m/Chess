namespace Components
{
    public struct PieceComponent
    {
        public enum PieceType
        {
            Pawn,
            King,
            Queen,
            Knight,
            Bishop,
            Rook
        }

        public PieceType Type;

        public int PieceValue;
    }
}
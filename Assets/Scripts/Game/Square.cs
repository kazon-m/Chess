using Game.Behaviour;
using Game.Pieces;

namespace Game
{
    public class Square
    {
        public Square(Square square)
        {
            X = square.X;
            Y = square.Y;
            Piece = null;
        }

        public Square() { }

        public SquareBehaviour Behaviour => Board.Instance.SquareBehaviourMatrix[Y, X];

        public int X { get; private set; }

        public int Y { get; private set; }

        public Piece Piece { get; private set; }

        public void SetNewPiece(Piece piece)
        {
            if(piece != null)
            {
                if(Piece != null && !Piece.Propietary.Equal(piece.Propietary)) Piece.Destroy();
            }

            Piece = piece;
            if(piece != null) Piece.SetNewSquare(this);
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equal(object obj) =>
            obj is Square otherSquare && otherSquare.X == X && otherSquare.Y == Y;
    }
}
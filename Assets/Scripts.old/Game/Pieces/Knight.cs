using System.Collections.Generic;
using Game.Behaviour;
using Game.Logic;

namespace Game.Pieces
{
    public class Knight : Piece
    {
        public Knight(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(
            behaviour, pieceValue, values)
        {
        }

        public Knight() { }

        public override Piece Copy(Square copySquare)
        {
            var knight = new Knight();
            copySquare.SetNewPiece(knight);
            knight.pieceValue = pieceValue;
            knight.values = values;
            knight.PossibleMoves = new List<Move>();
            return knight;
        }

        private void EvaluatePosition(Square[,] board, int x, int y)
        {
            if(x >= 8) return;
            if(x < 0) return;
            if(y >= 8) return;
            if(y < 0) return;

            if(board[y, x].Piece == null || !board[y, x].Piece.Propietary.Equal(Propietary))
                PossibleMoves.Add(new Move(board[y, x]));
        }

        public override void Evaluate(Square[,] b = null)
        {
            var board = b ?? Board.Instance.SquareMatrix;
            PossibleMoves.Clear();

            var x = ActualSquare.X;
            var y = ActualSquare.Y;

            EvaluatePosition(board, x + 2, y + 1);
            EvaluatePosition(board, x + 2, y - 1);
            EvaluatePosition(board, x - 2, y + 1);
            EvaluatePosition(board, x - 2, y - 1);
            EvaluatePosition(board, x + 1, y + 2);
            EvaluatePosition(board, x - 1, y + 2);
            EvaluatePosition(board, x - 1, y - 2);
            EvaluatePosition(board, x + 1, y - 2);
        }
    }
}
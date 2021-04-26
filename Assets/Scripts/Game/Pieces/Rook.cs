using System.Collections.Generic;
using Game.Behaviour;
using Game.Logic;

namespace Game.Pieces
{
    public class Rook : Piece
    {
        public Rook(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(
            behaviour, pieceValue, values)
        {
        }

        public Rook() { }

        public bool IsMoved { get; private set; }

        public override void Move(SquareBehaviour destiny)
        {
            base.Move(destiny);
            IsMoved = true;
        }

        public override Piece Copy(Square copySquare)
        {
            var rook = new Rook();
            copySquare.SetNewPiece(rook);
            rook.PieceValue = PieceValue;
            rook.Values = Values;
            rook.IsMoved = IsMoved;
            rook.PossibleMoves = new List<Move>();
            return rook;
        }

        private void EvaluateBoardInPosition(Square[,] board, int x, int y, out bool flag)
        {
            flag = true;
            if(x >= 8) return;
            if(x < 0) return;
            if(y >= 8) return;
            if(y < 0) return;
            if(board[y, x].Piece == null) PossibleMoves.Add(new Move(board[y, x]));
            else
            {
                flag = false;
                if(!board[y, x].Piece.Propietary.Equal(Propietary)) PossibleMoves.Add(new Move(board[y, x]));
            }
        }

        public override void Evaluate(Square[,] b = null)
        {
            var board = b ?? Board.Instance.SquareMatrix;
            PossibleMoves.Clear();
            var actualX = ActualSquare.X;
            var actualY = ActualSquare.Y;
            bool right = true, left = true, up = true, down = true;
            for(var i = 1; i < 8; i++)
            {
                if(right) EvaluateBoardInPosition(board, actualX + i, actualY, out right);
                if(left) EvaluateBoardInPosition(board, actualX - i, actualY, out left);
                if(up) EvaluateBoardInPosition(board, actualX, actualY + i, out up);
                if(down) EvaluateBoardInPosition(board, actualX, actualY - i, out down);
            }
        }
    }
}
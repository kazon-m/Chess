using System.Collections.Generic;
using Game.Behaviour;
using Game.Logic;

namespace Game.Pieces
{
    public class Queen : Piece
    {
        public Queen(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(
            behaviour, pieceValue, values)
        {
        }

        public Queen() { }

        public Queen(int pieceValue, SquareTableValues values, Square square, Player propietary)
        {
            square.SetNewPiece(this);
            PieceValue = pieceValue;
            Values = values;
            SetPropietary(propietary);
            PossibleMoves = new List<Move>();
        }

        public override Piece Copy(Square copySquare)
        {
            var queen = new Queen();
            copySquare.SetNewPiece(queen);
            queen.PieceValue = PieceValue;
            queen.Values = Values;
            queen.PossibleMoves = new List<Move>();
            return queen;
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
            bool rightUp = true, rightDown = true, leftUp = true, leftDown = true;

            for(var i = 1; i < 8; i++)
            {
                if(rightUp) EvaluateBoardInPosition(board, actualX + i, actualY + i, out rightUp);
                if(rightDown) EvaluateBoardInPosition(board, actualX + i, actualY - i, out rightDown);
                if(leftDown) EvaluateBoardInPosition(board, actualX - i, actualY - i, out leftDown);
                if(leftUp) EvaluateBoardInPosition(board, actualX - i, actualY + i, out leftUp);

                if(right) EvaluateBoardInPosition(board, actualX + i, actualY, out right);
                if(left) EvaluateBoardInPosition(board, actualX - i, actualY, out left);
                if(up) EvaluateBoardInPosition(board, actualX, actualY + i, out up);
                if(down) EvaluateBoardInPosition(board, actualX, actualY - i, out down);
            }
        }
    }
}
using System.Collections.Generic;
using Game.Behaviour;
using Game.Logic;

namespace Game.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(
            behaviour, pieceValue, values)
        {
        }

        public Bishop() { }

        public override Piece Copy(Square copySquare)
        {
            var bishop = new Bishop();
            copySquare.SetNewPiece(bishop);
            bishop.PieceValue = PieceValue;
            bishop.Values = Values;
            bishop.PossibleMoves = new List<Move>();
            return bishop;
        }

        private void EvaluateBoardInPosition(Square[,] board, bool checkRight, bool checkUp, int x, int y,
                                             out bool flag)
        {
            flag = true;
            if(checkRight && x >= 8) return;
            if(!checkRight && x < 0) return;
            if(checkUp && y >= 8) return;
            if(!checkUp && y < 0) return;
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
            bool rightUp = true, rightDown = true, leftUp = true, leftDown = true;
            for(var i = 1; i < 8; i++)
            {
                if(rightUp) EvaluateBoardInPosition(board, true, true, actualX + i, actualY + i, out rightUp);
                if(rightDown)
                    EvaluateBoardInPosition(board, true, false, actualX + i, actualY - i, out rightDown);
                if(leftDown)
                    EvaluateBoardInPosition(board, false, false, actualX - i, actualY - i, out leftDown);
                if(leftUp) EvaluateBoardInPosition(board, false, true, actualX - i, actualY + i, out leftUp);
            }
        }
    }
}
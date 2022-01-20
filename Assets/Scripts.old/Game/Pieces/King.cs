using System.Collections.Generic;
using Game.Behaviour;
using Game.Logic;

namespace Game.Pieces
{
    public class King : Piece
    {
        private bool _isMoved;

        public King(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(
            behaviour, pieceValue, values)
        {
        }

        public King() { }

        public override void Move(SquareBehaviour destiny)
        {
            base.Move(destiny);
            _isMoved = true;
        }

        public override Piece Copy(Square copySquare)
        {
            var king = new King();
            copySquare.SetNewPiece(king);
            king.pieceValue = pieceValue;
            king.values = values;
            king._isMoved = _isMoved;
            king.PossibleMoves = new List<Move>();
            return king;
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

            const int i = 1;

            if(rightUp) EvaluateBoardInPosition(board, actualX + i, actualY + i, out rightUp);
            if(rightDown) EvaluateBoardInPosition(board, actualX + i, actualY - i, out rightDown);
            if(leftDown) EvaluateBoardInPosition(board, actualX - i, actualY - i, out leftDown);
            if(leftUp) EvaluateBoardInPosition(board, actualX - i, actualY + i, out leftUp);

            if(right) EvaluateBoardInPosition(board, actualX + i, actualY, out right);
            if(left) EvaluateBoardInPosition(board, actualX - i, actualY, out left);
            if(up) EvaluateBoardInPosition(board, actualX, actualY + i, out up);
            if(down) EvaluateBoardInPosition(board, actualX, actualY - i, out down);
        }

        public void CastlingRound(Square[,] b, Player otherPlayer)
        {
            var canDoCastling = true;
            var board = b ?? Board.Instance.SquareMatrix;


            if(!_isMoved && otherPlayer != null && !otherPlayer.CheckIfSquareIsInMoves(ActualSquare))
            {
                for(var i = ActualSquare.X + 1; i < 7; i++)
                {
                    if(board[ActualSquare.Y, i].Piece == null
                    && !otherPlayer.CheckIfSquareIsInMoves(board[ActualSquare.Y, i])) continue;
                    canDoCastling = false;
                    break;
                }


                if(!_isMoved
                && canDoCastling
                && board[ActualSquare.Y, 7].Piece != null
                && board[ActualSquare.Y, 7].Piece is Rook)
                {
                    if(board[ActualSquare.Y, 7].Piece is Rook rook && !rook.IsMoved)
                    {
                        var move = new Move(board[ActualSquare.Y, ActualSquare.X + 2]);
                        var y = ActualSquare.Y;
                        var x = ActualSquare.X + 1;
                        var previousX = rook.ActualSquare.X;
                        var previousY = rook.ActualSquare.Y;
                        move.RegisterCallback(() =>
                        {
                            if(b != null) rook.Move(board[y, x]);
                            else rook.Move(board[y, x].Behaviour);
                        });
                        move.RegisterCallbackToReset(() => { rook.Move(board[previousY, previousX]); });
                        PossibleMoves.Add(move);
                    }
                }


                canDoCastling = true;

                for(var i = ActualSquare.X - 1; i > 0; i--)
                {
                    if(board[ActualSquare.Y, i].Piece != null
                    || otherPlayer.CheckIfSquareIsInMoves(board[ActualSquare.Y, i]))
                    {
                        canDoCastling = false;
                        break;
                    }
                }

                if(!_isMoved
                && canDoCastling
                && board[ActualSquare.Y, 0].Piece != null
                && board[ActualSquare.Y, 0].Piece is Rook)
                {
                    if(board[ActualSquare.Y, 0].Piece is Rook rook && !rook.IsMoved)
                    {
                        var move = new Move(board[ActualSquare.Y, ActualSquare.X - 2]);
                        var y = ActualSquare.Y;
                        var x = ActualSquare.X - 1;
                        var previousX = rook.ActualSquare.X;
                        var previousY = rook.ActualSquare.Y;
                        move.RegisterCallback(() =>
                        {
                            if(b != null) rook.Move(board[y, x]);
                            else rook.Move(board[y, x].Behaviour);
                        });
                        move.RegisterCallbackToReset(() => { rook.Move(board[previousY, previousX]); });


                        PossibleMoves.Add(move);
                    }
                }
            }
        }
    }
}
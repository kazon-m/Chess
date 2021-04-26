using System.Collections.Generic;
using Game.Behaviour;
using Game.Controller;
using Game.Logic;

namespace Game.Pieces
{
    public class Pawn : Piece
    {
        private bool _evaluatedYet;
        private bool _justMovedTwo;
        private bool _positionInitial = true;
        private int _yDirection = -1;

        public Pawn(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(
            behaviour, pieceValue, values)
        {
        }

        public Pawn() { }

        public override Piece Copy(Square copySquare)
        {
            var pawn = new Pawn();
            copySquare.SetNewPiece(pawn);
            pawn.PieceValue = PieceValue;
            pawn.Values = Values;
            pawn._evaluatedYet = _evaluatedYet;
            pawn._yDirection = _yDirection;
            pawn._justMovedTwo = _justMovedTwo;
            pawn._positionInitial = _positionInitial;
            pawn.PossibleMoves = new List<Move>();
            return pawn;
        }

        public void ResetJustMovedTwo() => _justMovedTwo = false;

        private void EvaluateFrontMoves(Square[,] board)
        {
            if(ActualSquare.Y + _yDirection < 8
            && ActualSquare.Y + _yDirection >= 0
            && board[ActualSquare.Y + _yDirection, ActualSquare.X].Piece == null)
            {
                PossibleMoves.Add(new Move(board[ActualSquare.Y + _yDirection, ActualSquare.X], false));
                CheckPromotionOnLastMove();
                if((_yDirection == 1 && ActualSquare.Y == 1 || _yDirection == -1 && ActualSquare.Y == 6)
                && ActualSquare.Y + _yDirection * 2 < 8
                && ActualSquare.Y + _yDirection * 2 >= 0
                && board[ActualSquare.Y + _yDirection * 2, ActualSquare.X].Piece == null)
                {
                    PossibleMoves.Add(
                        new Move(board[ActualSquare.Y + _yDirection * 2, ActualSquare.X], false));
                    PossibleMoves[PossibleMoves.Count - 1].RegisterCallback(() => { _justMovedTwo = true; });
                    PossibleMoves[PossibleMoves.Count - 1]
                       .RegisterCallbackToReset(() => { _justMovedTwo = false; });
                }
            }
        }

        private void EvaluateAttackPawn(Square[,] board)
        {
            if(ActualSquare.Y + _yDirection < 8
            && ActualSquare.Y + _yDirection >= 0
            && ActualSquare.X + 1 < 8
            && board[ActualSquare.Y + _yDirection, ActualSquare.X + 1].Piece != null
            && !board[ActualSquare.Y + _yDirection, ActualSquare.X + 1].Piece.Propietary.Equal(Propietary))
            {
                PossibleMoves.Add(new Move(board[ActualSquare.Y + _yDirection, ActualSquare.X + 1]));
                CheckPromotionOnLastMove();
            }

            if(ActualSquare.Y + _yDirection < 8
            && ActualSquare.Y + _yDirection >= 0
            && ActualSquare.X - 1 >= 0
            && board[ActualSquare.Y + _yDirection, ActualSquare.X - 1].Piece != null
            && !board[ActualSquare.Y + _yDirection, ActualSquare.X - 1].Piece.Propietary.Equal(Propietary))
            {
                PossibleMoves.Add(new Move(board[ActualSquare.Y + _yDirection, ActualSquare.X - 1]));
                CheckPromotionOnLastMove();
            }
        }

        private void EvaluateEnPassant(Square[,] board, int modX)
        {
            if(ActualSquare.X + modX >= 0
            && ActualSquare.X + modX < 8
            && board[ActualSquare.Y, ActualSquare.X + modX].Piece != null
            && !board[ActualSquare.Y, ActualSquare.X + modX].Piece.Propietary.Equal(Propietary)
            && board[ActualSquare.Y, ActualSquare.X + modX].Piece is Pawn)
            {
                var otherPawn = board[ActualSquare.Y, ActualSquare.X + modX].Piece as Pawn;
                if(otherPawn._justMovedTwo)
                {
                    PossibleMoves.Add(new Move(board[ActualSquare.Y + _yDirection, ActualSquare.X + modX]));
                    var previousSquare = otherPawn.ActualSquare;
                    var previousPropietary = otherPawn.Propietary;
                    PossibleMoves[PossibleMoves.Count - 1].RegisterCallback(() =>
                    {
                        otherPawn.ActualSquare.SetNewPiece(null);
                        otherPawn.Destroy();
                    });
                    PossibleMoves[PossibleMoves.Count - 1].RegisterCallbackToReset(() =>
                    {
                        previousSquare.SetNewPiece(otherPawn);
                        previousPropietary.AddPiece(otherPawn);
                    });
                }
            }
        }

        public override void Evaluate(Square[,] b = null)
        {
            if(!_evaluatedYet)
            {
                _evaluatedYet = true;
                _yDirection = ActualSquare.Y == 1 ? 1 : -1;
            }

            var board = b ?? Board.Instance.SquareMatrix;
            PossibleMoves.Clear();


            EvaluateFrontMoves(board);
            EvaluateAttackPawn(board);
            EvaluateEnPassant(board, 1);
            EvaluateEnPassant(board, -1);
        }

        private void CheckPromotionOnLastMove()
        {
            if(PossibleMoves[PossibleMoves.Count - 1].Square.Y == 0
            || PossibleMoves[PossibleMoves.Count - 1].Square.Y == 7)
            {
                var number = Propietary.ReturnNumberPiece(this);
                PossibleMoves[PossibleMoves.Count - 1].RegisterCallback(() =>
                {
                    if(Behaviour != null)
                    {
                        GameController.Instance.InstantiatePiece(PieceType.Queen, ActualSquare.Behaviour,
                                                                 Propietary);
                        Destroy();
                    }
                    else
                    {
                        GameController.Instance.InstantiateQueen(ActualSquare, Propietary);
                        Destroy();
                    }
                });

                PossibleMoves[PossibleMoves.Count - 1].RegisterCallbackToReset(() =>
                {
                    Propietary.DestroyLastPiece();
                    Propietary.AddPieceInNumber(this, number);
                });
            }
        }

        public override void Move(SquareBehaviour destiny)
        {
            base.Move(destiny);
            _positionInitial = false;
        }
    }
}
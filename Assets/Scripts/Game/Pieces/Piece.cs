using System.Collections.Generic;
using Game.Behaviour;
using Game.Logic;
using UnityEngine;

namespace Game.Pieces
{
    public class Piece
    {
        protected readonly PieceBehaviour Behaviour;
        protected int PieceValue;

        protected SquareTableValues Values;

        public Piece() { }

        public Piece(PieceBehaviour behaviour, int pieceValue, SquareTableValues values)
        {
            Behaviour = behaviour;
            PieceValue = pieceValue;
            Values = values;
            PossibleMoves = new List<Move>();
        }

        public Square ActualSquare { get; private set; }
        public List<Move> PossibleMoves { get; protected set; }

        protected int TotalPieceValue { get; private set; }

        public Player Propietary { get; private set; }

        public virtual Piece Copy(Square copySquare) => null;

        public virtual void Evaluate(Square[,] board = null) { }

        public int EvaluateBoardScore()
        {
            TotalPieceValue = PieceValue
                            + Values.SquareValues[ActualSquare.X,
                                                  Mathf.Abs(ActualSquare.Y - Propietary.ModSquareTableValue)];
            return TotalPieceValue;
        }

        public virtual void Move(Square destiny)
        {
            ActualSquare.SetNewPiece(null);
            destiny.SetNewPiece(this);
        }

        public virtual void Move(SquareBehaviour destiny)
        {
            Move(destiny.Square);
            if(Behaviour != null)
            {
                /*Behaviour.transform.DOMove(
                    new Vector3(destiny.transform.position.x, Behaviour.transform.position.y,
                                destiny.transform.position.z), 1);*/
            }
        }

        public void Destroy()
        {
            if(Propietary != null)
            {
                Propietary.DestroyPiece(this);
                if(Behaviour != null) Object.Destroy(Behaviour.gameObject);
            }
        }

        public void SetNewSquare(Square square) => ActualSquare = square;

        public void SetPropietary(Player player)
        {
            Propietary = player;
            Propietary.AddPiece(this);
        }

        public int ActualValue() => PieceValue + Values.SquareValues[ActualSquare.X, ActualSquare.Y];

        public void RemoveMove(Square value)
        {
            for(var i = 0; i < PossibleMoves.Count; i++)
            {
                if(PossibleMoves[i].Square.Equal(value))
                {
                    PossibleMoves.RemoveAt(i);
                    break;
                }
            }
        }

        public void RemoveMove(int value) => PossibleMoves.RemoveAt(value);
    }
}
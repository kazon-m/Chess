using System.Collections.Generic;
using System.Linq;
using Game.Behaviour;
using Game.Controller;
using Game.Logic;
using UnityEngine;
using Util;

namespace Game
{
    public class Board : Singleton<Board>
    {
        [SerializeField]
        private SquareBehaviour _squarePrefab;

        private SquareBehaviour _activeSquare;

        private List<Move> _possibleMovesActive;

        public Square[,] SquareMatrix { get; private set; }

        public SquareBehaviour[,] SquareBehaviourMatrix { get; private set; }

        protected void Awake()
        {
            SquareMatrix = new Square[8, 8];
            SquareBehaviourMatrix = new SquareBehaviour[8, 8];

            var isWhite = true;
            for(var y = 0; y < 8; y++)
            {
                for(var x = 0; x < 8; x++)
                {
                    SquareBehaviourMatrix[y, x] = Instantiate(_squarePrefab, transform);
                    SquareBehaviourMatrix[y, x].Color = isWhite ? Color.white : Color.black;
                    SquareBehaviourMatrix[y, x].ResetColor();

                    SquareMatrix[y, x] = SquareBehaviourMatrix[y, x].Square;
                    SquareMatrix[y, x].SetPosition(x, y);

                    isWhite = !isWhite;
                }

                isWhite = !isWhite;
            }
        }

        public void SquareClicked(SquareBehaviour square)
        {
            if(square.Square.Piece != null
            && square.Square.Piece.Propietary.Equal(GameController.Instance.ActualPlayer)
            && (_activeSquare == null
             || _activeSquare.Square.Piece.Propietary.Equal(square.Square.Piece.Propietary))
            && square.Square.Piece.Propietary.Type == Player.PlayerType.Human)
            {
                ResetMoves();
                _activeSquare = square;
                _possibleMovesActive = _activeSquare.Square.Piece.PossibleMoves;
                foreach(var move in _possibleMovesActive) move.Square.Behaviour.MarkPossible();
            }

            else if(_activeSquare != null && square.BlockColor)
            {
                _activeSquare.Square.Piece.Move(square);
                GetMoveFromPossibles(square.Square).RunCallback();
                _activeSquare = null;
                ResetMoves();
                GameController.Instance.ChangeTurns();
                GameController.Instance.EvaluatePlayers();
            }
        }

        private Move GetMoveFromPossibles(Square square) =>
            _possibleMovesActive.FirstOrDefault(move => move.Square.Equal(square));

        private void ResetMoves()
        {
            if(_possibleMovesActive == null) return;
            foreach(var move in _possibleMovesActive) move.Square.Behaviour.ResetColor();
        }

        public Square[,] GenerateBoardCopy()
        {
            var copy = new Square[8, 8];

            for(var i = 0; i < 8; i++)
            {
                for(var j = 0; j < 8; j++) copy[i, j] = new Square(SquareMatrix[i, j]);
            }

            return copy;
        }

        public Square[,] GenerateBoardCopy(Square[,] board)
        {
            var copy = new Square[8, 8];

            for(var i = 0; i < 8; i++)
            {
                for(var j = 0; j < 8; j++) copy[i, j] = new Square(board[i, j]);
            }

            return copy;
        }
    }
}
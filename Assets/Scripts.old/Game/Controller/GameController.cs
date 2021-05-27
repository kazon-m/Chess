using Game.Behaviour;
using Game.Pieces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Game.Controller
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField]
        private PieceBehaviour kingPrefab, queenPrefab, rookPrefab, bishopPrefab, knightPrefab, pawnPrefab;

        [SerializeField]
        private int queenValue;

        [SerializeField]
        private SquareTableValues queenSquareTableValues;

        private MiniMax _miniMax;

        private Player _otherPlayer;

        private Player _playerOne, _playerTwo;

        public Player ActualPlayer { get; private set; }

        public void Reset() => SceneManager.LoadScene(0);

        private void Start()
        {
            _playerOne = new Player(Player.PlayerType.Human, 7);
            _playerTwo = new Player(Player.PlayerType.AI);
            _miniMax = new MiniMax();

            InstantiatePlayerOnePieces();
            InstantiatePlayerTwoPieces();

            ActualPlayer = _playerOne;
            _otherPlayer = _playerTwo;

            EvaluatePlayers();
        }

        private void InstantiatePlayerOnePieces()
        {
            var board = Board.Instance.SquareBehaviourMatrix;

            for(var i = 0; i < 8; i++) InstantiatePiece(PieceType.Pawn, board[1, i], _playerOne);
            InstantiatePiece(PieceType.Rook, board[0, 0], _playerOne);
            InstantiatePiece(PieceType.Rook, board[0, 7], _playerOne);

            InstantiatePiece(PieceType.Knight, board[0, 1], _playerOne);
            InstantiatePiece(PieceType.Knight, board[0, 6], _playerOne);

            InstantiatePiece(PieceType.Bishop, board[0, 2], _playerOne);
            InstantiatePiece(PieceType.Bishop, board[0, 5], _playerOne);

            InstantiatePiece(PieceType.Queen, board[0, 3], _playerOne);
            InstantiatePiece(PieceType.King, board[0, 4], _playerOne);
        }

        private void InstantiatePlayerTwoPieces()
        {
            var board = Board.Instance.SquareBehaviourMatrix;
            for(var i = 0; i < 8; i++) InstantiatePiece(PieceType.Pawn, board[6, i], _playerTwo);
            InstantiatePiece(PieceType.Rook, board[7, 0], _playerTwo);
            InstantiatePiece(PieceType.Rook, board[7, 7], _playerTwo);

            InstantiatePiece(PieceType.Knight, board[7, 1], _playerTwo);
            InstantiatePiece(PieceType.Knight, board[7, 6], _playerTwo);

            InstantiatePiece(PieceType.Bishop, board[7, 2], _playerTwo);
            InstantiatePiece(PieceType.Bishop, board[7, 5], _playerTwo);

            InstantiatePiece(PieceType.Queen, board[7, 3], _playerTwo);
            InstantiatePiece(PieceType.King, board[7, 4], _playerTwo);
        }

        public void InstantiatePiece(PieceType type, SquareBehaviour squareBehaviour, Player player)
        {
            PieceBehaviour prefab = null;
            switch(type)
            {
                case PieceType.Pawn:
                    prefab = pawnPrefab;
                    break;
                case PieceType.Queen:
                    prefab = queenPrefab;
                    break;
                case PieceType.Rook:
                    prefab = rookPrefab;
                    break;
                case PieceType.Knight:
                    prefab = knightPrefab;
                    break;
                case PieceType.King:
                    prefab = kingPrefab;
                    break;
                case PieceType.Bishop:
                    prefab = bishopPrefab;
                    break;
            }

            var piece = Instantiate(prefab, squareBehaviour.transform);
            squareBehaviour.Square.SetNewPiece(piece.Piece);
            piece.Piece.SetPropietary(player);
        }

        public void InstantiateQueen(Square square, Player player)
        {
            var piece = new Queen(queenValue, queenSquareTableValues, square, player);
        }

        public void ChangeTurns()
        {
            if(ActualPlayer == _playerOne)
            {
                ActualPlayer = _playerTwo;
                _otherPlayer = _playerOne;
            }
            else
            {
                ActualPlayer = _playerOne;
                _otherPlayer = _playerTwo;
            }
        }

        public void EvaluatePlayers()
        {
            ActualPlayer.Evaluate();
            _otherPlayer.Evaluate();
            ActualPlayer.EvaluateCastling(null, _otherPlayer);
            _otherPlayer.EvaluateCastling(null, ActualPlayer);

            ActualPlayer.EvaluateCheckOffMoves(_otherPlayer);
            _otherPlayer.ResetPawnsState();

            if(!ActualPlayer.CheckIfCheckMate())
            {
                if(ActualPlayer.Type == Player.PlayerType.AI)
                    _miniMax.RunMiniMax(ActualPlayer, _otherPlayer, Board.Instance.SquareMatrix);
            }
        }
    }

    public enum PieceType
    {
        Pawn,
        King,
        Queen,
        Knight,
        Bishop,
        Rook
    }
}
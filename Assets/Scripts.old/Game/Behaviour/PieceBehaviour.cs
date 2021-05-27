using Game.Controller;
using Game.Pieces;
using UnityEngine;

namespace Game.Behaviour
{
    public class PieceBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PieceType type;

        [SerializeField]
        private int pieceValue;

        [SerializeField]
        private SquareTableValues values;

        [SerializeField]
        private Sprite spriteWhite, spriteBlack;

        public Piece Piece { get; private set; }

        private void Awake()
        {
            values.Init();
            switch(type)
            {
                case PieceType.Bishop:
                    Piece = new Bishop(this, pieceValue, values);
                    break;
                case PieceType.King:
                    Piece = new King(this, pieceValue, values);
                    break;

                case PieceType.Knight:
                    Piece = new Knight(this, pieceValue, values);
                    break;

                case PieceType.Pawn:
                    Piece = new Pawn(this, pieceValue, values);
                    break;

                case PieceType.Queen:
                    Piece = new Queen(this, pieceValue, values);
                    break;

                case PieceType.Rook:
                    Piece = new Rook(this, pieceValue, values);
                    break;
            }
        }
    }
}
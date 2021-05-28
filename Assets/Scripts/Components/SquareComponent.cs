using Components.Chess;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public struct SquareComponent
    {
        private Color _hoverColor, _clickedColor, _possibleColor;

        public Color Color;
        private Image _image;

        public Vector2Int Position;

        public PieceComponent Piece;
    }
}
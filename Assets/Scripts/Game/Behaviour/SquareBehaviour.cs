using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviour
{
    [RequireComponent(typeof(Image))]
    public class SquareBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Color _hoverColor, _clickedColor, _possibleColor;

        public Color Color;
        private Image _image;

        public Square Square { get; private set; }

        public bool BlockColor { get; private set; }

        private void Awake()
        {
            Square = new Square();
            _image = GetComponent<Image>();
        }

        private void OnMouseDown()
        {
            if(!BlockColor) _image.color = _clickedColor;
            Board.Instance.SquareClicked(this);
        }

        private void OnMouseEnter()
        {
            if(!BlockColor) _image.color = _hoverColor;
        }

        private void OnMouseExit()
        {
            if(!BlockColor) ResetColor();
        }

        public void ResetColor()
        {
            _image.color = Color;
            BlockColor = false;
        }

        public void MarkPossible()
        {
            BlockColor = true;
            _image.color = _possibleColor;
        }
    }
}
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    [RequireComponent(typeof(Image))]
    public class ChessView : MonoBehaviour
    {
        public EcsEntity entity;

        public Image Image { get; private set; }

        private void Awake() => Image = GetComponent<Image>();
    }
}
using Components;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class SquareView : MonoBehaviour
    {
        private Button _button;

        public EcsEntity entity;

        public Image Image { get; private set; }

        private void Awake()
        {
            Image = GetComponent<Image>();
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                if(!entity.IsNull() && !entity.Has<ClickedComponent>()) entity.Get<ClickedComponent>();
            });
        }

        private void OnDestroy() => _button.onClick.RemoveAllListeners();
    }
}
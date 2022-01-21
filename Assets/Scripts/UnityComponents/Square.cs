using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class Square : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => EcsEvents.RegisterOnSquareClickEvent(GetComponent<Image>()));
        }

        private void OnDestroy() => _button.onClick.RemoveAllListeners();
    }
}
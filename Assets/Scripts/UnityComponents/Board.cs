using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class Board : MonoBehaviour
    {
        private void Awake() => EcsEvents.RegisterOnCreateBoardEvent(gameObject);
    }
}
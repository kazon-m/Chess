using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class Board : MonoBehaviour
    {
        private void Start() => EcsEvents.RegisterOnCreateBoardEvent(gameObject);
    }
}
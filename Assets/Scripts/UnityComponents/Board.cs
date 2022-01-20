using Helpers;
using UnityEngine;

namespace UnityComponents
{
    public class Board : MonoBehaviour
    {
        private void Start() => EcsEvents.RegisterOnCreateBoardEvent(gameObject);
    }
}
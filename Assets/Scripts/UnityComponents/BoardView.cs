using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class BoardView : MonoBehaviour
    {
        public EcsEntity entity;
    }
}
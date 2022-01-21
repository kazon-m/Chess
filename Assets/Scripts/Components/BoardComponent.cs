using Data.Enums;
using UnityEngine;

namespace Components
{
    public struct BoardComponent
    {
        public SquareComponent[,] matrix;
        public GameObject component;
        public TeamType move;
    }
}
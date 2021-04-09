using UnityEngine;

namespace Game.Chessmen
{
    public abstract class AbstractChess : MonoBehaviour
    {
        public bool IsWhite;
        
        public Vector2Int Position;

        public abstract bool[,] PossibleMove();
    }
}
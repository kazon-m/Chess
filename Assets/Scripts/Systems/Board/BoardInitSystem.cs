using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Board
{
    public class BoardInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;

        public void Init()
        {
            var isWhite = true;
            for(var x = 0; x < 8; x++)
            {
                for(var y = 0; y < 8; y++)
                {
                    var squareEntity = _world.NewEntity();
                    ref var square = ref squareEntity.Get<SquareComponent>();
                    
                    square.Position = new Vector2Int(x, y);
                    square.IsWhite = isWhite;
                    
                    isWhite = !isWhite;
                }

                isWhite = !isWhite;
            }
        }
    }
}
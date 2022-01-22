using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.Init
{
    public class SquareInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;

        public void Init()
        {
            var isWhite = true;

            for(var x = 0; x < 8; x++)
            {
                for(var y = 0; y < 8; y++)
                {
                    var entity = _world.NewEntity();
                    ref var square = ref entity.Get<SquareComponent>();
                    ref var squarePosition = ref entity.Get<PositionComponent>();

                    square.isWhite = isWhite;
                    squarePosition.value = new Vector2Int(x, y);

                    isWhite = !isWhite;
                }

                isWhite = !isWhite;
            }
        }
    }
}
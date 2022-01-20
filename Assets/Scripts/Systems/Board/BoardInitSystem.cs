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
            var boardEntity = _world.NewEntity();
            ref var board = ref boardEntity.Get<BoardComponent>();
            board.matrix = new SquareComponent[8, 8];

            var isWhite = true;

            for(var x = 0; x < 8; x++)
            {
                for(var y = 0; y < 8; y++)
                {
                    var squareEntity = _world.NewEntity();
                    ref var square = ref squareEntity.Get<SquareComponent>();

                    square.position = new Vector2Int(x, y);
                    square.isWhite = isWhite;

                    board.matrix[x, y] = square;

                    isWhite = !isWhite;
                }

                isWhite = !isWhite;
            }
        }
    }
}
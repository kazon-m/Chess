using Components;
using Data.Enums;
using Leopotam.Ecs;

namespace Systems.Init
{
    public class BoardInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;

        public void Init()
        {
            var entity = _world.NewEntity();
            ref var board = ref entity.Get<BoardComponent>();

            board.move = TeamType.White;
        }
    }
}
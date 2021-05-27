using Components;
using Leopotam.Ecs;

namespace Scenarios
{
    public class GameScenario : IEcsInitSystem
    {
        private EcsWorld _world = null;

        public void Init()
        {
            var player = _world.NewEntity();
            ref var playerComponent = ref player.Get<PlayerComponent>();
            ref var pieceComponent = ref player.Get<PieceComponent>();
            ref var movableComponent = ref player.Get<MovableComponent>();

            var square = _world.NewEntity();
            ref var squareComponent = ref square.Get<SquareComponent>();
        }
    }
}
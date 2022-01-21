using Components.Chess;
using Data.Enums;
using Leopotam.Ecs;

namespace Systems
{
    public class PlayerSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;
        
        public void Init()
        {
            var aiEntity = _world.NewEntity();
            ref var ai = ref aiEntity.Get<PlayerComponent>();
            ai.type = PlayerType.AI;
            ai.team = TeamType.Black;
            
            var playerEntity = _world.NewEntity();
            ref var player = ref playerEntity.Get<PlayerComponent>();
            player.type = PlayerType.Player;
            player.team = TeamType.White;
        }
    }
}
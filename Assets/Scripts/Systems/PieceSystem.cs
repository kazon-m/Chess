using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class PieceSystem : IEcsSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<PieceComponent> _filter = null;
    }
}
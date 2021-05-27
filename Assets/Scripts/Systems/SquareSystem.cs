using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class SquareSystem : IEcsSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<SquareComponent> _filter = null;
    }
}
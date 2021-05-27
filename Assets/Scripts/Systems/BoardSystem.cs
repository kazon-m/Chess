using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class BoardSystem : IEcsSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<BoardComponent> _filter = null;
    }
}
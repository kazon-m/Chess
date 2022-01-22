using Components;
using Components.Events;
using Leopotam.Ecs;

namespace Systems.View
{
    public class BoardViewSystem : IEcsRunSystem
    {
        private readonly EcsFilter<OnCreateBoardEvent> _createBoardFilter = null;
        private readonly EcsFilter<BoardComponent>.Exclude<ObjectComponent> _boardFilter = null;

        public void Run()
        {
            if(_createBoardFilter.IsEmpty() || _boardFilter.IsEmpty()) return;

            _boardFilter.GetEntity(0).Get<ObjectComponent>().value = _createBoardFilter.Get1(0).board;
        }
    }
}
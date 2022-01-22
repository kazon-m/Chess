using Components;
using Components.View;
using Leopotam.Ecs;
using UnityComponents;
using UnityEngine;

namespace Systems.View
{
    public class BoardViewSystem : IEcsRunSystem
    {
        private readonly EcsFilter<BoardComponent>.Exclude<BoardViewComponent> _boardFilter = null;

        public void Run()
        {
            foreach(var i in _boardFilter)
            {
                var board = Object.FindObjectOfType<BoardView>();

                if(board != null)
                {
                    ref var entity = ref _boardFilter.GetEntity(i);
                    ref var boardView = ref entity.Get<BoardViewComponent>();

                    boardView.value = Object.FindObjectOfType<BoardView>();
                    boardView.value.entity = entity;
                }
            }
        }
    }
}
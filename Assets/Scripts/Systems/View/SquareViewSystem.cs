using Components;
using Components.View;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.View
{
    public class SquareViewSystem : IEcsRunSystem
    {
        private readonly ChessPreset _chessPreset = null;
        private readonly EcsFilter<BoardViewComponent> _boardFilter = null;
        private readonly EcsFilter<SquareComponent>.Exclude<SquareViewComponent> _squareFilter = null;

        public void Run()
        {
            if(_boardFilter.IsEmpty() || _squareFilter.IsEmpty()) return;

            ref var boardView = ref _boardFilter.Get1(0);

            foreach(var i in _squareFilter)
            {
                ref var entity = ref _squareFilter.GetEntity(i);
                ref var square = ref _squareFilter.Get1(i);
                ref var squareView = ref entity.Get<SquareViewComponent>();
                
                squareView.value = Object.Instantiate(_chessPreset.SquarePrefab, boardView.value.transform);
                squareView.value.Image.color = square.isWhite ? _chessPreset.SquareWhiteColor : _chessPreset.SquareBlackColor;
                squareView.value.entity = entity;
            }
        }
    }
}
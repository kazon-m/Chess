using Components;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.View
{
    public class SquareViewSystem : IEcsRunSystem
    {
        private readonly ChessPreset _chessPreset = null;
        private readonly EcsFilter<BoardComponent, ObjectComponent> _boardFilter = null;
        private readonly EcsFilter<SquareComponent>.Exclude<ViewComponent> _squareFilter = null;

        public void Run()
        {
            if(_boardFilter.IsEmpty() || _squareFilter.IsEmpty()) return;

            foreach(var i in _squareFilter)
            {
                ref var squareView = ref _squareFilter.GetEntity(i).Get<ViewComponent>();

                squareView.value = Object.Instantiate(_chessPreset.SquarePrefab, _boardFilter.Get2(0).value.transform);
                squareView.value.color = _squareFilter.Get1(i).isWhite ? _chessPreset.SquareWhiteColor : _chessPreset.SquareBlackColor;
            }
        }
    }
}
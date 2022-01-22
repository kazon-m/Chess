using Components;
using Data;
using Data.Enums;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.View
{
    public class ChessViewSystem : IEcsRunSystem
    {
        private readonly ChessPreset _chessPreset = null;
        private readonly EcsFilter<SquareComponent, PositionComponent, ViewComponent> _squareFilter = null;
        private readonly EcsFilter<ChessComponent, PositionComponent>.Exclude<ViewComponent> _chessFilter = null;

        public void Run()
        {
            if(_chessFilter.IsEmpty() || _squareFilter.IsEmpty()) return;

            foreach(var i in _chessFilter)
            {
                foreach(var j in _squareFilter)
                {
                    if(_squareFilter.Get2(j).value == _chessFilter.Get2(i).value)
                    {
                        ref var chess = ref _chessFilter.Get1(i);
                        ref var chessView = ref _chessFilter.GetEntity(i).Get<ViewComponent>();

                        var chessItem = _chessPreset.GetChessItemByType(chess.type);
                        chessView.value = Object.Instantiate(_chessPreset.ChessPrefab, _squareFilter.Get3(j).value.transform);
                        chessView.value.sprite = chess.team == TeamType.White ? chessItem.spriteWhite : chessItem.spriteBlack;
                    }
                }
            }
        }
    }
}
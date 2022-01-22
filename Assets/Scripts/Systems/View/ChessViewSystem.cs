using Components;
using Components.View;
using Data;
using Data.Enums;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems.View
{
    public class ChessViewSystem : IEcsRunSystem
    {
        private readonly ChessPreset _chessPreset = null;
        private readonly EcsFilter<ChessComponent, SquareViewComponent>.Exclude<ChessViewComponent> _chessFilter = null;

        public void Run()
        {
            foreach(var i in _chessFilter)
            {
                ref var entity = ref _chessFilter.GetEntity(i);
                ref var chess = ref _chessFilter.Get1(i);
                ref var squareView = ref _chessFilter.Get2(i);
                ref var chessView = ref entity.Get<ChessViewComponent>();

                var chessItem = _chessPreset.GetChessItemByType(chess.type);
                chessView.value = Object.Instantiate(_chessPreset.ChessPrefab, squareView.value.transform);
                chessView.value.Image.sprite = chess.team == TeamType.White ? chessItem.spriteWhite : chessItem.spriteBlack;
                chessView.value.entity = entity;
            }
        }
    }
}
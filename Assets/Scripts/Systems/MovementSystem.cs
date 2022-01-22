using Components;
using Components.View;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private readonly ChessPreset _chessPreset = null;
        private readonly EcsFilter<BoardComponent> _boardFilter = null;
        private readonly EcsFilter<PlayerComponent> _playerFilter = null;
        private readonly EcsFilter<SquareViewComponent, ClickedComponent>.Exclude<ChessComponent, ChessViewComponent> _chessClickedFilter = null;
        private readonly EcsFilter<SquareComponent, SquareViewComponent, ChessComponent, ChessViewComponent, SelectedComponent> _chessSelectedFilter = null;

        public void Run()
        {
            if(_chessClickedFilter.IsEmpty() || _chessSelectedFilter.IsEmpty()) return;

            ref var board = ref _boardFilter.Get1(0);

            foreach(var i in _playerFilter)
            {
                ref var player = ref _playerFilter.Get1(i);

                if(board.move != player.team)
                {
                    ref var clickedEntity = ref _chessClickedFilter.GetEntity(0);
                    ref var clickedSquareView = ref _chessClickedFilter.Get1(0);

                    ref var selectedEntity = ref _chessSelectedFilter.GetEntity(0);
                    ref var selectedSquare = ref _chessSelectedFilter.Get1(0);
                    ref var selectedSquareView = ref _chessSelectedFilter.Get2(0);
                    ref var selectedChess = ref _chessSelectedFilter.Get3(0);
                    ref var selectedChessView = ref _chessSelectedFilter.Get4(0);

                    selectedSquareView.value.Image.color = selectedSquare.isWhite ? _chessPreset.SquareWhiteColor : _chessPreset.SquareBlackColor;

                    selectedChessView.value.entity = clickedEntity;
                    selectedChessView.value.transform.SetParent(clickedSquareView.value.transform);
                    selectedChessView.value.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        
                    clickedEntity.Replace(selectedChess);
                    clickedEntity.Replace(selectedChessView);
                    clickedEntity.Del<ClickedComponent>();

                    selectedEntity.Del<SelectedComponent>();
                    selectedEntity.Del<ChessComponent>();
                    selectedEntity.Del<ChessViewComponent>();
                    break;
                }
            }
        }
    }
}
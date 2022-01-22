using Components;
using Components.View;
using Data;
using Leopotam.Ecs;

namespace Systems
{
    public class SquareSelectSystem : IEcsRunSystem
    {
        private readonly ChessPreset _chessPreset = null;
        private readonly EcsFilter<BoardComponent> _boardFilter = null;
        private readonly EcsFilter<PlayerComponent> _playerFilter = null;
        private readonly EcsFilter<SquareViewComponent, ClickedComponent> _squareClickedFilter = null;
        private readonly EcsFilter<SquareComponent, SquareViewComponent, SelectedComponent> _squareSelectedFilter = null;

        public void Run()
        {
            if(_squareClickedFilter.IsEmpty()) return;
            
            ref var board = ref _boardFilter.Get1(0);

            foreach(var i in _playerFilter)
            {
                ref var player = ref _playerFilter.Get1(i);

                if(board.move != player.team)
                {
                    foreach(var s in _squareSelectedFilter)
                    {
                        ref var entity = ref _squareSelectedFilter.GetEntity(s);
                        ref var square = ref _squareSelectedFilter.Get1(s);
                        ref var squareView = ref _squareSelectedFilter.Get2(s);

                        squareView.value.Image.color = square.isWhite ? _chessPreset.SquareWhiteColor : _chessPreset.SquareBlackColor;
                        entity.Del<SelectedComponent>();
                    }

                    foreach(var s in _squareClickedFilter)
                    {
                        ref var entity = ref _squareClickedFilter.GetEntity(s);
                        ref var squareView = ref _squareClickedFilter.Get1(s);
                        
                        squareView.value.Image.color = _chessPreset.SquareClickedColor;
                        entity.Get<SelectedComponent>();
                        entity.Del<ClickedComponent>();
                    }

                    break;
                }
            }
        }
    }
}
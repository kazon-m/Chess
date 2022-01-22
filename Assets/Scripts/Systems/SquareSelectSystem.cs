using Components;
using Components.Events;
using Data;
using Leopotam.Ecs;

namespace Systems
{
    public class SquareSelectSystem : IEcsRunSystem
    {
        private readonly ChessPreset _chessPreset = null;
        private readonly EcsFilter<BoardComponent> _boardFilter = null;
        private readonly EcsFilter<PlayerComponent> _playerFilter = null;
        private readonly EcsFilter<OnSquareClickEvent> _squareClickFilter = null;
        private readonly EcsFilter<SquareComponent, ViewComponent> _squareFilter = null;
        private readonly EcsFilter<SquareComponent, ViewComponent, SelectedComponent> _squareSelectedFilter = null;

        public void Run()
        {
            if(_squareClickFilter.IsEmpty()) return;

            foreach(var i in _playerFilter)
            {
                ref var player = ref _playerFilter.Get1(i);

                if(_boardFilter.Get1(0).move != player.team)
                {
                    if(!_squareSelectedFilter.IsEmpty())
                    {
                        _squareSelectedFilter.GetEntity(0).Del<SelectedComponent>();
                        _squareSelectedFilter.Get2(0).value.color = _squareSelectedFilter.Get1(0).isWhite ? _chessPreset.SquareWhiteColor : _chessPreset.SquareBlackColor;
                    }

                    foreach(var j in _squareFilter)
                    {
                        ref var squareView = ref _squareFilter.Get2(j);

                        if(squareView.value == _squareClickFilter.Get1(0).square)
                        {
                            _squareFilter.GetEntity(j).Get<SelectedComponent>();
                            squareView.value.color = _chessPreset.SquareClickedColor;
                            break;
                        }
                    }

                    break;
                }
            }
        }
    }
}
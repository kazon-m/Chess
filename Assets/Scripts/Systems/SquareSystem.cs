using Components;
using Components.Chess;
using Components.Events;
using Data;
using Leopotam.Ecs;

namespace Systems
{
    public class SquareSystem : IEcsRunSystem
    {
        private readonly EcsFilter<OnSquareClickEvent> _squareClickFilter = null;
        private readonly EcsFilter<PlayerComponent> _playerFilter = null;
        private readonly EcsFilter<SquareComponent> _squareFilter = null;
        private readonly EcsFilter<BoardComponent> _boardFilter = null;
        private ChessPreset _chessPreset;

        public void Run()
        {
            foreach(var i in _squareClickFilter)
            {
                ref var squareClicked = ref _squareClickFilter.Get1(i).square;

                ref var board = ref _boardFilter.Get1(0);

                foreach(var j in _playerFilter)
                {
                    ref var player = ref _playerFilter.Get1(j);

                    if(board.move == player.team)
                    {
                        if(player.square.isSelected)
                        {
                            if(player.square.component == squareClicked) break;
                            
                            player.square.isSelected = false;
                            player.square.component.color = player.square.isWhite ? _chessPreset.SquareWhiteColor : _chessPreset.SquareBlackColor;
                        }

                        foreach(var s in _squareFilter)
                        {
                            ref var square = ref _squareFilter.Get1(s);

                            if(square.component == squareClicked)
                            {
                                player.square = square;
                                player.square.isSelected = true;
                                player.square.component.color = _chessPreset.SquareClickedColor;
                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}
using Components;
using Data;
using Data.Enums;
using Leopotam.Ecs;

namespace Systems.Init
{
    public class ChessInitSystem : IEcsInitSystem
    {
        private readonly EcsFilter<SquareComponent, PositionComponent> _squareFilter = null;
        private readonly ChessPreset _chessPreset = null;

        public void Init()
        {
            foreach(var i in _squareFilter)
            {
                ref var entity = ref _squareFilter.GetEntity(i);
                ref var squarePosition = ref _squareFilter.Get2(i);

                foreach(var chessItem in _chessPreset.ChessList)
                {
                    if(chessItem.whitePositions.Contains(squarePosition.value))
                    {
                        ref var chess = ref entity.Get<ChessComponent>();

                        chess.type = chessItem.type;
                        chess.team = TeamType.White;
                    }
                    else if(chessItem.blackPositions.Contains(squarePosition.value))
                    {
                        ref var chess = ref entity.Get<ChessComponent>();

                        chess.type = chessItem.type;
                        chess.team = TeamType.Black;
                    }
                }
            }
        }
    }
}
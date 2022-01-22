using Components;
using Data;
using Data.Enums;
using Leopotam.Ecs;

namespace Systems.Init
{
    public class ChessInitSystem : IEcsInitSystem
    {
        private readonly ChessPreset _chessPreset = null;
        private readonly EcsWorld _world = null;

        public void Init()
        {
            foreach(var chessItem in _chessPreset.ChessList)
            {
                foreach(var position in chessItem.whitePositions)
                {
                    var chessEntity = _world.NewEntity();
                    ref var chess = ref chessEntity.Get<ChessComponent>();
                    ref var chessPosition = ref chessEntity.Get<PositionComponent>();

                    chess.type = chessItem.type;
                    chess.team = TeamType.White;
                    chessPosition.value = position;
                }

                foreach(var position in chessItem.blackPositions)
                {
                    var chessEntity = _world.NewEntity();
                    ref var chess = ref chessEntity.Get<ChessComponent>();
                    ref var chessPosition = ref chessEntity.Get<PositionComponent>();

                    chess.type = chessItem.type;
                    chess.team = TeamType.Black;
                    chessPosition.value = position;
                }
            }
        }
    }
}
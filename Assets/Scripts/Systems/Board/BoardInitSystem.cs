using Components;
using Leopotam.Ecs;

namespace Systems.Board
{
    public class BoardInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = default;
        private readonly EcsFilter<BoardComponent> _filter = default;
        
        public void Init()
        {
            foreach(var i in _filter)
            {
                var isWhite = true;
                for(var y = 0; y < 8; y++)
                {
                    for(var x = 0; x < 8; x++)
                    {
                        isWhite = !isWhite;
                    }

                    isWhite = !isWhite;
                }
            }
        }
    }
}
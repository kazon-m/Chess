using Components.Events;
using Leopotam.Ecs;
using UnityEngine;

namespace Helpers
{
    public static class EcsEvents
    {
        public static EcsWorld world;

        public static EcsSystems OneFrameEvents(this EcsSystems ecsSystems)
        {
            ecsSystems.OneFrame<OnCreateBoardEvent>();
            return ecsSystems;
        }

        public static void RegisterOnCreateBoardEvent(GameObject board)
        {
            var eventEntity = world.NewEntity();
            ref var eventComponent = ref eventEntity.Get<OnCreateBoardEvent>();
            eventComponent.board = board;
        }
    }
}
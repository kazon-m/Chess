using Components.Events;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers
{
    public static class EcsEvents
    {
        public static EcsWorld world;

        public static EcsSystems OneFrameEvents(this EcsSystems ecsSystems)
        {
            ecsSystems.OneFrame<OnCreateBoardEvent>();
            ecsSystems.OneFrame<OnSquareClickEvent>();
            return ecsSystems;
        }

        public static void RegisterOnCreateBoardEvent(GameObject board)
        {
            var eventEntity = world.NewEntity();
            ref var eventComponent = ref eventEntity.Get<OnCreateBoardEvent>();
            eventComponent.board = board;
        }

        public static void RegisterOnSquareClickEvent(Image square)
        {
            var eventEntity = world.NewEntity();
            ref var eventComponent = ref eventEntity.Get<OnSquareClickEvent>();
            eventComponent.square = square;
        }
    }
}
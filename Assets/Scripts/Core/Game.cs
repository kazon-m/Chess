using Leopotam.Ecs;
using Scenarios;
using UnityEngine;

namespace Core
{
    public class Game : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;

        private void Start()
        {
            Application.targetFrameRate = 60;

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _systems.Add(new GameScenario()).Init();
        }

        private void Update() => _systems.Run();

        private void OnDestroy()
        {
            _systems.Destroy();
            _world.Destroy();
        }
    }
}
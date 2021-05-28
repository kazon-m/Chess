using Systems.Board;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private GridLayoutGroup _boardGrid;
        
        private EcsWorld _world;
        private EcsSystems _systems;

        private void Start()
        {
            Application.targetFrameRate = 60;

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            #if UNITY_EDITOR
            EcsWorldObserver.Create(_world);
            EcsSystemsObserver.Create(_systems);
            #endif

            _systems.Add(new BoardInitSystem()).Init();
        }

        private void Update() => _systems?.Run();

        private void OnDestroy()
        {
            if(_systems != null)
            {
                _systems.Destroy();
                _world.Destroy();
                _systems = null;
                _world = null;
            }
        }
    }
}
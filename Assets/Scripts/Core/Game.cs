﻿using Data;
using Data.Models;
using Helpers;
using Leopotam.Ecs;
using Systems;
using Systems.Board;
using UI.Controllers;
using UnityEngine;
#if UNITY_EDITOR
using Leopotam.Ecs.UnityIntegration;
#endif

namespace Core
{
    [RequireComponent(typeof(LevelManager))]
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private Transform _ui;

        [SerializeField]
        private ChessPreset _chessPreset;

        private LevelManager _levelManager;

        private Menu _menu;
        private EcsSystems _systems;

        private EcsWorld _world;

        private void Awake() => _levelManager = GetComponent<LevelManager>();

        private void Start()
        {
            Application.targetFrameRate = 60;

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            EcsEvents.world = _world;

            #if UNITY_EDITOR
            EcsWorldObserver.Create(_world);
            EcsSystemsObserver.Create(_systems);
            #endif

            InitializeUI();
            InitializeSystems();

            _systems.OneFrameEvents().Inject(_levelManager).Inject(_chessPreset).Init();

            _menu.Get<TransitionController>().Show();

            // Планируем действие на завершение загрузки уровня.
            _levelManager.OnLoaded(() =>
            {
                _menu.Get<TransitionController>().Hide();
                _menu.Get<LobbyController>().Show();
            });

            // Определяем текущий уровень для загрузки.
            var saveBox = SavesLoader.Load();

            // Если индекс уровня в сохранении больше последнего, то загружаем случайный уровень.
            if(saveBox.levelIndex >= _levelManager.TotalLevels - 1) _levelManager.LoadRandom();
            else _levelManager.LoadLevel(saveBox.levelIndex); // Загружаем текущий уровень.
        }

        private void Update() => _systems?.Run();

        private void OnDestroy()
        {
            if(_systems == null) return;

            EcsEvents.world = null;
            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
        }

        private void InitializeSystems() { _systems.Add(new BoardInitSystem()); }

        private void InitializeUI()
        {
            _menu = new Menu(_ui, _systems);
            _menu.Add(new LobbyController());
            _menu.Add(new TransitionController());
            _menu.Add(new SettingsController());
            _systems.Inject(_menu);
        }
    }
}
using System.Collections.Generic;
using Leopotam.Ecs;
using UI;
using UnityEngine;

namespace Systems
{
    public class Menu
    {
        private readonly List<IController> _controllers;

        private readonly EcsSystems _systems;

        public Menu(Transform ui, EcsSystems systems)
        {
            UI = ui;
            _systems = systems;
            _controllers = new List<IController>();
        }

        public Transform UI { get; }

        public T Add<T>(T controller) where T : IController
        {
            _systems.Add(controller);
            _controllers.Add(controller);
            return controller;
        }

        public T Get<T>() where T : IController
        {
            var index = _controllers.FindIndex(x => x is T);
            if(index >= 0 && index < _controllers.Count) return (T) _controllers[index];
            return default;
        }

        public bool Contains<T>() where T : IController
        {
            var index = _controllers.FindIndex(x => x is T);
            return index >= 0 && index < _controllers.Count;
        }
    }
}
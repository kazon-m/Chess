using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Core
{
    public class LevelLoader : MonoBehaviour
    {
        public delegate void LevelLoaderCallback();

        public delegate void LevelLoaderDelegate(string aSceneName, float aProgress);

        [Header("Loading Settings")]
        public ThreadPriority loadThreadPriority;

        public float delayAfterSceneLoading = 0.25f;

        [HideInInspector]
        public List<string> scenesHistory = new List<string>();

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private LevelLoaderCallback _beginLoadingCallback;
        private LevelLoaderCallback _beginUnloadingCallback;
        private LevelLoaderCallback _finishLoadingCallback;
        private LevelLoaderCallback _finishUnloadingCallback;
        private string _nextScene;

        private AsyncOperation _operation = new AsyncOperation();
        private float _progress;

        /// <summary>
        ///     Определяет имя текущей загруженной сцены.
        /// </summary>
        public string CurrentScene { get; private set; }

        /// <summary>
        ///     Определяет загружена ли в данный момент времени какая-либо сцена.
        /// </summary>
        public bool HasScene { get; private set; }

        private void UnloadedHandler(string aSceneName, float aProgress)
        {
            EventFinishUnloading -= UnloadedHandler;
            LoadScene(_nextScene);
        }

        /// <summary>
        ///     Событие возникающее когда начинается процесс загрузки уровня.
        /// </summary>
        public event LevelLoaderDelegate EventBeginLoading;

        /// <summary>
        ///     Событие возникающее во время процесса уровня.
        /// </summary>
        public event LevelLoaderDelegate EventProcessLoading;

        /// <summary>
        ///     Событие возникающе когда процесс загрузки уровня завершен.
        /// </summary>
        public event LevelLoaderDelegate EventFinishLoading;

        /// <summary>
        ///     Событие возникающее когда начинается процесс выгрузки уровня.
        /// </summary>
        public event LevelLoaderDelegate EventBeginUnloading;

        /// <summary>
        ///     Событие возникающее во время процесса выгрузки уровня.
        /// </summary>
        public event LevelLoaderDelegate EventProcessUnloading;

        /// <summary>
        ///     Событие возникающее когда процесс выгрузки уровня завершен.
        /// </summary>
        public event LevelLoaderDelegate EventFinishUnloading;

        /// <summary>
        ///     Выгружает последнюю загруженную сцену и загружает новую.
        /// </summary>
        /// <param name="aSceneName">Имя сцены которую нужно загрузить после выгрузки текущей.</param>
        public LevelLoader UnloadCurrentAndLoad(string aSceneName)
        {
            _nextScene = aSceneName;
            EventFinishUnloading += UnloadedHandler;
            UnloadScene(CurrentScene);
            return this;
        }

        /// <summary>
        ///     Загружает новую сцену по имени.
        /// </summary>
        /// <param name="aSceneName">Имя сцены которую нужно загрузить.</param>
        public LevelLoader LoadScene(string aSceneName)
        {
            StartCoroutine(AsyncLoading(aSceneName));
            return this;
        }

        /// <summary>
        ///     Выгружает сцену по имени.
        /// </summary>
        /// <param name="aSceneName">Имя сцены которую нужно выгрузить.</param>
        public LevelLoader UnloadScene(string aSceneName)
        {
            StartCoroutine(AsyncUnloading(aSceneName));
            return this;
        }

        /// <summary>
        ///     Устанавливает одноразовый обратный вызов на начало загрузки сцены.
        /// </summary>
        /// <param name="aCallback">Обратный вызов.</param>
        public LevelLoader OnBeginLoading(LevelLoaderCallback aCallback)
        {
            _beginLoadingCallback = aCallback;
            return this;
        }

        /// <summary>
        ///     Устанавливает одноразовый обратный вызов на завершение загрузки сцены.
        /// </summary>
        /// <param name="aCallback">Обратный вызов.</param>
        public LevelLoader OnFinishLoading(LevelLoaderCallback aCallback)
        {
            _finishLoadingCallback = aCallback;
            return this;
        }

        /// <summary>
        ///     Устанавливает одноразовый обратный вызов на начало выгрузки сцены.
        /// </summary>
        /// <param name="aCallback">Обратный вызов.</param>
        public LevelLoader OnBeginUnloading(LevelLoaderCallback aCallback)
        {
            _beginUnloadingCallback = aCallback;
            return this;
        }

        /// <summary>
        ///     Устанавливает одноразовый обратный вызов на завершение выгрузки сцены.
        /// </summary>
        /// <param name="aCallback">Обратный вызов.</param>
        public LevelLoader OnFinishUnloading(LevelLoaderCallback aCallback)
        {
            _beginUnloadingCallback = aCallback;
            return this;
        }

        private void AddToHistory(string aSceneName)
        {
            scenesHistory.Add(aSceneName);
            if(scenesHistory.Count > 10) scenesHistory.Remove(scenesHistory[0]);
        }

        private IEnumerator AsyncLoading(string aSceneName)
        {
            Debug.Log($"[LevelLoader]: Start loading `{aSceneName}` scene.");

            _stopwatch.Reset();
            _stopwatch.Start();

            _progress = 0.0f;

            _beginLoadingCallback?.Invoke();
            _beginLoadingCallback = null;
            EventBeginLoading?.Invoke(aSceneName, _progress);

            Application.backgroundLoadingPriority = loadThreadPriority;
            _operation = SceneManager.LoadSceneAsync(aSceneName, LoadSceneMode.Additive);
            _operation.allowSceneActivation = false;

            while(!_operation.isDone)
            {
                if(!Mathf.Approximately(_operation.progress, _progress))
                {
                    _progress = _operation.progress;
                    EventProcessLoading?.Invoke(aSceneName, _progress);
                }

                // Check if the load has finished
                if(_operation.progress >= 0.9f) _operation.allowSceneActivation = true;

                yield return null;
            }

            yield return new WaitForSeconds(delayAfterSceneLoading);

            _progress = 1.0f;
            EventProcessLoading?.Invoke(aSceneName, _progress);

            CurrentScene = aSceneName;
            HasScene = true;

            AddToHistory(aSceneName);
            _stopwatch.Stop();

            Debug.Log($"[LevelLoader]: Scene {aSceneName} loaded in {_stopwatch.Elapsed.TotalMilliseconds} ms.");

            EventFinishLoading?.Invoke(CurrentScene, _progress);
            _finishLoadingCallback?.Invoke();
            _finishLoadingCallback = null;
        }

        private IEnumerator AsyncUnloading(string aSceneName)
        {
            Debug.Log($"[LevelLoader]: Start unloading `{aSceneName}` scene.");

            _stopwatch.Reset();
            _stopwatch.Start();

            yield return new WaitForSeconds(0.1f);

            _progress = 0.0f;
            _beginUnloadingCallback?.Invoke();
            _beginUnloadingCallback = null;
            EventBeginUnloading?.Invoke(aSceneName, _progress);

            Application.backgroundLoadingPriority = loadThreadPriority;

            _operation = SceneManager.UnloadSceneAsync(aSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            _operation.allowSceneActivation = false;

            while(!_operation.isDone)
            {
                if(!Mathf.Approximately(_operation.progress, _progress))
                {
                    _progress = _operation.progress;
                    EventProcessUnloading?.Invoke(aSceneName, _progress);
                }

                if(_operation.progress >= 0.9f) _operation.allowSceneActivation = true;

                yield return null;
            }

            _progress = 1.0f;
            EventProcessUnloading?.Invoke(aSceneName, _progress);

            HasScene = false;
            CurrentScene = string.Empty;

            _stopwatch.Stop();

            Debug.Log($"[LevelLoader]: Scene `{aSceneName}` unloaded in {_stopwatch.Elapsed.TotalMilliseconds}ms.");

            EventFinishUnloading?.Invoke(scenesHistory[scenesHistory.Count - 1], _progress);
            _finishUnloadingCallback?.Invoke();
            _finishUnloadingCallback = null;
        }
    }
}
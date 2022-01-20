using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    [RequireComponent(typeof(LevelLoader))]
    public class LevelManager : MonoBehaviour
    {
        public delegate void LevelManagerDelegate();

        /// <summary>
        ///     Набор уровней для загрузки/выгрузки.
        /// </summary>
        public LevelsPreset levelsPreset;

        /// <summary>
        ///     Устанавливает загруженную сцену активной (применяет настройки скайбоксов
        ///     и освещения из загруженной сцены).
        /// </summary>
        public bool setLoadedSceneAsActive;

        private LevelManagerDelegate _loadedCallback;
        private LevelLoader _loader;

        private RandomLevelItem[] _randomLevels;
        private LevelManagerDelegate _unloadedCallback;

        /// <summary>
        ///     Определяет последнюю загруженную сцену.
        /// </summary>
        public string LastScene => _loader.scenesHistory.Count > 0 ? _loader.scenesHistory[_loader.scenesHistory.Count - 1] : string.Empty;

        /// <summary>
        ///     Список последних загруженных сцен.
        /// </summary>
        public List<string> ScenesHistory => _loader.scenesHistory;

        /// <summary>
        ///     Определяет загружен ли какой-нибудь уровень.
        /// </summary>
        public bool IsLevelLoaded => _loader != null && _loader.HasScene;

        /// <summary>
        ///     Определяет общее количество уровней в текущем наборе.
        /// </summary>
        /// <value></value>
        public int TotalLevels
        {
            get
            {
                if(levelsPreset == null) Debug.LogError("LevelsPreset is not set!");
                return levelsPreset.levels.Count;
            }
        }

        private void Awake()
        {
            _loader = GetComponent<LevelLoader>();
            if(_loader == null) Debug.LogError("LevelManager requires LevelLoader script on the same GameObject!");
        }

        /// <summary>
        ///     Событие возникающее когда процесс загрузки уровня завершен.
        /// </summary>
        public event LevelManagerDelegate EventFinishLoading;

        /// <summary>
        ///     Выбирает и загружает случайный уровень из LevelPreset из тех уровней
        ///     которые имеют флаг isRandom.
        /// </summary>
        public LevelManager LoadRandom()
        {
            _randomLevels ??= InitializeRandomLevels();
            return LoadLevel(SelectRandomLevel());
        }

        /// <summary>
        ///     Загружает уровень по индексу.
        /// </summary>
        /// <param name="aLevelIndex">Индекс уровня который нужно загрузить.</param>
        public LevelManager LoadLevel(int aLevelIndex)
        {
            if(levelsPreset == null) Debug.LogError("LevelsPreset is not set!");
            if(aLevelIndex >= 0 && aLevelIndex < levelsPreset.levels.Count) LoadLevel(levelsPreset.levels[aLevelIndex].sceneName);
            else Debug.LogWarning($"[LevelManager]: index of level out of bounds ({aLevelIndex}).");
            return this;
        }

        /// <summary>
        ///     Загружает уровень по имени сцены.
        /// </summary>
        /// <param name="aLevelName">Имя сцены которую нужно загрузить.</param>
        public LevelManager LoadLevel(string aLevelName)
        {
            _loader.LoadScene(aLevelName).OnFinishLoading(() =>
            {
                if(setLoadedSceneAsActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(aLevelName));

                EventFinishLoading?.Invoke();
                _loadedCallback?.Invoke();
                _loadedCallback = null;
            });
            return this;
        }

        /// <summary>
        ///     Выгружает текущий уровень, выбирает и загружает случайный уровень
        ///     из LevelPreset из тех уровней которые имеют флаг RND.
        /// </summary>
        public LevelManager UnloadCurrentAndLoadRandom()
        {
            _randomLevels ??= InitializeRandomLevels();
            return UnloadCurrentAndLoad(SelectRandomLevel());
        }

        /// <summary>
        ///     Выгружает уровень по имени сцены.
        /// </summary>
        /// <param name="aLevelName">Имя сцены которую нужно выгрузить.</param>
        public LevelManager UnloadLevel(string aLevelName)
        {
            _loader.UnloadScene(aLevelName).OnBeginUnloading(() =>
            {
                _unloadedCallback?.Invoke();
                _unloadedCallback = null;
            });
            return this;
        }

        /// <summary>
        ///     Выгружает текущий уровень и загружает следующий по индексу.
        /// </summary>
        /// <param name="aLevelIndex">Индекс уровня который нужно загрузить.</param>
        public LevelManager UnloadCurrentAndLoad(int aLevelIndex)
        {
            if(levelsPreset == null) Debug.LogError("LevelsPreset is not set!");
            if(aLevelIndex >= 0 && aLevelIndex < levelsPreset.levels.Count) UnloadCurrentAndLoad(levelsPreset.levels[aLevelIndex].sceneName);
            else Debug.LogWarning($"[LevelManager]: Index of level out of bounds ({aLevelIndex}).");
            return this;
        }

        /// <summary>
        ///     Выгружает текущий уровень и загружает следующий по имени сцены.
        /// </summary>
        /// <param name="aLevelName">Имя сцены которую нужно загрузить.</param>
        public LevelManager UnloadCurrentAndLoad(string aLevelName)
        {
            _loader.UnloadCurrentAndLoad(aLevelName).OnBeginUnloading(() =>
            {
                _unloadedCallback?.Invoke();
                _unloadedCallback = null;
            }).OnFinishLoading(() =>
            {
                if(setLoadedSceneAsActive) SceneManager.SetActiveScene(SceneManager.GetSceneByName(aLevelName));

                EventFinishLoading?.Invoke();
                _loadedCallback?.Invoke();
                _loadedCallback = null;
            });
            return this;
        }

        /// <summary>
        ///     Устанавливает одноразовый обратный вызов на событие завершения загрузки.
        /// </summary>
        /// <param name="aCallback">Обратный вызов.</param>
        public LevelManager OnLoaded(LevelManagerDelegate aCallback)
        {
            _loadedCallback = aCallback;
            return this;
        }

        /// <summary>
        ///     Устанавливает одноразовый обратный вызов на событие завершения выгрузки.
        /// </summary>
        /// <param name="aCallback">Обратный вызов.</param>
        public LevelManager OnUnloaded(LevelManagerDelegate aCallback)
        {
            _unloadedCallback = aCallback;
            return this;
        }

        private RandomLevelItem[] InitializeRandomLevels()
        {
            if(levelsPreset == null) Debug.LogError("LevelsPreset is not set!");

            var count = 0;
            for(int i = 0, n = levelsPreset.levels.Count; i < n; i++)
                if(levelsPreset.levels[i].isRandom)
                    count++;

            var j = 0;
            var result = new RandomLevelItem[count];
            for(int i = 0, n = levelsPreset.levels.Count; i < n; i++)
            {
                if(levelsPreset.levels[i].isRandom)
                {
                    result[j] = new RandomLevelItem { levelIndex = i, probability = 1.0f };
                    j++;
                }
            }

            return result;
        }

        private int SelectRandomLevel()
        {
            if(_randomLevels.Length == 0)
            {
                // Если уровни для случайного выбора отсуствуют.
                return 0;
            }

            // 1. Складываем веротяность выпадения всех уровней.
            var length = 0.0f;
            for(int i = 0, n = _randomLevels.Length; i < n; i++) length += _randomLevels[i].probability;

            // 2. Выбираем случайный уровень.
            var rnd = Random.Range(0.0f, length);
            var cur = 0.0f;
            var index = -1;
            for(int i = 0, n = _randomLevels.Length; i < n; i++)
            {
                if(rnd >= cur && rnd <= cur + _randomLevels[i].probability)
                {
                    index = i;
                    break;
                }

                cur += _randomLevels[i].probability;
            }

            // 3. Увеличиваем вероятность появления для всех уровней.
            RandomLevelItem item;
            for(int i = 0, n = _randomLevels.Length; i < n; i++)
            {
                item = _randomLevels[i];
                item.probability += 0.05f;
                _randomLevels[i] = item;
            }

            // 4. Обнуляем шансы выпадения для выбранного уровня.
            item = _randomLevels[index];
            item.probability = 0.0f;
            _randomLevels[index] = item;

            return _randomLevels[index].levelIndex;
        }

        public struct RandomLevelItem
        {
            public int levelIndex;
            public float probability;
        }
    }
}
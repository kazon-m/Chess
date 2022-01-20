using UnityEngine;

namespace Data.Models
{
    /// <summary>
    ///     Данный класс реализует загрузку и сохранение данных для модели SaveData.
    /// </summary>
    public static class SavesLoader
    {
        /// <summary>
        ///     Создает модель данных для сохранения и загружает данные в нее.
        /// </summary>
        /// <returns>Загруженная или созданная модель данных.</returns>
        public static SaveData Load() => JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SavedData")) ?? new SaveData();

        /// <summary>
        ///     Записывает модель данных в сохранение.
        /// </summary>
        public static void Save(SaveData saveData)
        {
            PlayerPrefs.SetString("SavedData", JsonUtility.ToJson(saveData));
            PlayerPrefs.Save();
        }
    }
}
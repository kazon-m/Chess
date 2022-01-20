namespace Data.Models
{
    /// <summary>
    ///     Это модель данных для базового сохранения. Модель данных представляет собой набор полей
    ///     которые могут быть сохранены или загружены из PlayerPrefs.
    ///     Для загрузки/сохранения данных используйте класс SavesLoader.
    ///     Вы можете добавить сюда любое количество необходимых дополнительных полей.
    /// </summary>
    public class SaveData
    {
        public int coins = 0;
        public int displayLevelIndex = 1;
        public bool enableSounds = true;
        public bool enableVibrations = true;
        public int levelIndex = 0;
    }
}
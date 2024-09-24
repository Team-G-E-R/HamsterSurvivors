
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        //Глобал
        public int gold = 0;
        public string globalStats;
        public string upgradeLevels;

        //Игрок
        public float playerMaxHealth = 100;
        private float _playerHealthRegen;
        public float PlayerArmor;
        public float _playerMoveSpeedModifire = 1;

        //Оружие
        public float ActiveItemsCooldownMulti = 1;
        public float ActiveItemsProjSpeedMulti = 1;

        //Другое
        public float PlayerExpirienseModifire;

        // ...



        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}

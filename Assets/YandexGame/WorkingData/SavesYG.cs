
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
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public int coins = 0;

        // Tiers 0 1 2 3 etc
        public int engine = 0;
        public int speed = 0;
        public int capacity = 0;

        // Max tier that player got
        public int maxEngine = 0;
        public int maxSpeed = 0;
        public int maxCapacity = 0;

        public int leaderboardScore = 0;


        // Skibidi
        public bool isGotSkibidi = false; 
        public bool isEnableSkibidi = false;
        public int skibidiAdCount = 0;


        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}

using SaveData;

namespace Services.SaveProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(GameSaveData gameSaveData);
    }

    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(GameSaveData gameSaveData);
    }
}
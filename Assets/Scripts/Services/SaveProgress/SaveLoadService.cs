using System.IO;
using Factories;
using SaveData;
using UnityEngine;

namespace Services.SaveProgress
{
    public class SaveLoadService
    {
        private readonly string _saveFilePath = Application.persistentDataPath + "/gameSaveData.json";

        private GameSaveService _gameSaveService;
        private AllFactories _allFactories;

        public SaveLoadService(GameSaveService gameSaveService, AllFactories allFactories)
        {
            _gameSaveService = gameSaveService;
            _allFactories = allFactories;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _allFactories.ProgressWriters)
                progressWriter.UpdateProgress(_gameSaveService.GameSaveData);

            File.WriteAllText(_saveFilePath, _gameSaveService.GameSaveData.ToJson());
            Debug.Log(_saveFilePath);
        }

        public void ReadProgress()
        {
            GameSaveData gameSaveData;
            
            if (File.Exists(_saveFilePath))
            {
                string content = File.ReadAllText(_saveFilePath);
                
                gameSaveData = content.ToDeserialized<GameSaveData>();
            }
            else
                gameSaveData = new GameSaveData();
            
            _gameSaveService.GameSaveData = gameSaveData;
        }

        public GameSaveData LoadProgress()
        {
            ReadProgress();
            
            foreach (ISavedProgressReader progressReader in _allFactories.ProgressReaders)
                progressReader.LoadProgress(_gameSaveService.GameSaveData);

            return _gameSaveService.GameSaveData;
        }
    }
}
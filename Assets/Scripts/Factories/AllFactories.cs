using System.Collections.Generic;
using CharacterComponents;
using Services.SaveProgress;
using UI.HUD;
using UnityEngine;
using EventProvider = Services.EventProviderComponents.EventProvider;

namespace Factories
{
    public class AllFactories
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();
        
        private readonly PlayerFactory _playerFactory;
        private readonly HudFactory _hudFactory;

        public AllFactories(PlayerFactory playerFactory, HudFactory hudFactory)
        {
            _playerFactory = playerFactory;
            _hudFactory = hudFactory;
        }

        public CharacterView GetPlayerView(Vector3 position, Quaternion rotation, Transform parent, EventProvider eventProvider)
        {
            CharacterView playerView = _playerFactory.GetPlayerView(position, rotation, parent, eventProvider);

            RegisterProgressWatchers(playerView.gameObject);

            return playerView;
        }
        
        public HudView GetHudView(Transform parent, EventProvider eventProvider, SaveLoadService saveLoadService)
        {
            HudView hudView = _hudFactory.GetHudView(parent, eventProvider, saveLoadService);

            RegisterProgressWatchers(hudView.gameObject);

            return hudView;
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
            
            ProgressReaders.Add(progressReader);
        }
    }
}
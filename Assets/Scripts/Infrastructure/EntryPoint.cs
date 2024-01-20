using System.Collections;
using Factories;
using SaveData;
using Services;
using Services.SaveProgress;
using UnityEngine;
using EventProvider = Services.EventProviderComponents.EventProvider;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private MonoService monoService;
        
        private Game _game;
        private EventProvider _eventProvider;
        private AllFactories _allFactories;
        private SaveLoadService _saveLoadService;
        private GameSaveService _gameSaveService;
        
        private void Start() => 
            StartCoroutine(Initialization());

        private IEnumerator Initialization()
        {
            _eventProvider = new EventProvider();
            
            PlayerFactory playerFactory = new PlayerFactory(monoService);
            HudFactory hudFactory = new HudFactory();
            _allFactories = new AllFactories(playerFactory, hudFactory);

            _gameSaveService = new GameSaveService();
            _saveLoadService = new SaveLoadService(_gameSaveService, _allFactories);
            _saveLoadService.ReadProgress();
            
            _game = new Game(canvas, spawnPoint, _eventProvider, _allFactories, _saveLoadService);
            
            yield return Activation();
        }
        
        private IEnumerator Activation()
        {
            _game.Activate();
            
            yield break;
        }
    }
}
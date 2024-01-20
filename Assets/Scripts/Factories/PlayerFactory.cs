using CharacterComponents;
using Services;
using UnityEngine;
using EventProvider = Services.EventProviderComponents.EventProvider;
using Object = UnityEngine.Object;

namespace Factories
{
    public class PlayerFactory
    {
        private const string CharacterPrefabPath = "Prefabs/Character/Character";
        
        private readonly MonoService _monoService;

        public PlayerFactory(MonoService monoService) => 
            _monoService = monoService;

        public CharacterView GetPlayerView(Vector3 position, Quaternion rotation, Transform parent,
            EventProvider eventProvider)
        {
            CharacterView character = Object.Instantiate(Resources.Load<CharacterView>(CharacterPrefabPath), 
                position, rotation, parent);
            character.Initialize(eventProvider, _monoService);

            return character;
        }
    }
}
using Services.SaveProgress;
using UI.HUD;
using UnityEngine;
using EventProvider = Services.EventProviderComponents.EventProvider;

namespace Factories
{
    public class HudFactory
    {
        private const string HUDPrefabPath = "Prefabs/UI/HUD/HudView";
        
        public HudView GetHudView(Transform parent, EventProvider eventProvider, SaveLoadService saveLoadService)
        {
            HudView hudView = Object.Instantiate(Resources.Load<HudView>(HUDPrefabPath), parent);
            hudView.Initialize(eventProvider, saveLoadService);

            return hudView;
        }
    }
}
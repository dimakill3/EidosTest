using Enums;
using Events;
using Services.SaveProgress;
using UnityEngine;
using UnityEngine.UI;
using EventProvider = Services.EventProviderComponents.EventProvider;

namespace UI.HUD
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private ToggleGroup aimModeToggleGroup;
        [SerializeField] private Toggle aimModeToggle;
        [SerializeField] private Toggle freeModeToggle;
        [SerializeField] private Toggle avoidModeToggle;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;

        private EventProvider _eventProvider;
        private SaveLoadService _saveLoadService;
        
        public void Initialize(EventProvider eventProvider, SaveLoadService saveLoadService)
        {
            _eventProvider = eventProvider;
            _saveLoadService = saveLoadService;
        }

        public void Enable()
        {
            aimModeToggle.onValueChanged.AddListener((x) =>
            {
                if (x) 
                    ChangeAimMode(AimModeEnum.Aim);
            });
            
            freeModeToggle.onValueChanged.AddListener((x) => {
                if (x) 
                    ChangeAimMode(AimModeEnum.Free);
            });
            
            avoidModeToggle.onValueChanged.AddListener((x) => {
                if (x) 
                    ChangeAimMode(AimModeEnum.Avoid);
            });
            
            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
            
            _eventProvider.Subscribe<AimModeChangedEvent>(OnAimModeChanged);
        }

        public void Disable()
        {
            aimModeToggle.onValueChanged.RemoveAllListeners();
            freeModeToggle.onValueChanged.RemoveAllListeners();
            avoidModeToggle.onValueChanged.RemoveAllListeners();
            
            _eventProvider.UnSubscribe<AimModeChangedEvent>(OnAimModeChanged);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                aimModeToggle.onValueChanged.Invoke(!aimModeToggle.isOn);
            
            if (Input.GetKeyDown(KeyCode.W))
                freeModeToggle.onValueChanged.Invoke(!freeModeToggle.isOn);
            
            if (Input.GetKeyDown(KeyCode.E))
                avoidModeToggle.onValueChanged.Invoke(!avoidModeToggle.isOn);
            
            if (Input.GetKeyDown(KeyCode.S))
                Save();
            
            if (Input.GetKeyDown(KeyCode.L))
                Load();
        }

        private void Save() => 
            _saveLoadService.SaveProgress();

        private void Load() => 
            _saveLoadService.LoadProgress();

        private void ChangeAimMode(AimModeEnum aim) => 
            _eventProvider.Invoke(new ChangeAimModeEvent(aim));

        private void OnAimModeChanged(AimModeChangedEvent aimModeChangedEvent)
        {
            switch (aimModeChangedEvent.AimModeEnum)
            {
                case AimModeEnum.None:
                    aimModeToggleGroup.SetAllTogglesOff();
                    break;
                case AimModeEnum.Aim:
                    aimModeToggle.isOn = true;
                    break;
                case AimModeEnum.Free:
                    freeModeToggle.isOn = true;
                    break;
                case AimModeEnum.Avoid:
                    avoidModeToggle.isOn = true;
                    break;
            }
        }
    }
}
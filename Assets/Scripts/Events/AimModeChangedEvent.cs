using Enums;

namespace Events
{
    public struct AimModeChangedEvent : IEvent
    {
        private AimModeEnum _aimModeEnum;

        public AimModeEnum AimModeEnum => _aimModeEnum;

        public AimModeChangedEvent(AimModeEnum aimModeEnum) => 
            _aimModeEnum = aimModeEnum;
    }
}
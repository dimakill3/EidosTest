using Enums;

namespace Events
{
    public struct ChangeAimModeEvent : IEvent
    {
        private AimModeEnum _aimModeEnum;

        public AimModeEnum AimModeEnum => _aimModeEnum;

        public ChangeAimModeEvent(AimModeEnum aimModeEnum) => 
            _aimModeEnum = aimModeEnum;
    }
}
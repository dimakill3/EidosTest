using System;
using Enums;

namespace SaveData
{
    [Serializable]
    public class GameSaveData
    {
        public Vector3Data AimPosition;
        public AimModeEnum AimModeEnum;

        public GameSaveData(Vector3Data aimPosition, AimModeEnum aimModeEnum)
        {
            AimPosition = aimPosition;
            AimModeEnum = aimModeEnum;
        }

        public GameSaveData()
        {
            AimPosition = new Vector3Data();
            AimModeEnum = AimModeEnum.None;
        }
    }
}
using UnityEngine;

namespace SaveData
{
    public static class DataExtensions
    {
        public static Vector3Data ToVector3Data(this Vector3 vector) => 
            new(vector.x, vector.y, vector.z);
        
        public static Vector3 ToUnityVector3(this Vector3Data vector) =>
            new(vector.X, vector.Y, vector.Z);
        
        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);
        
        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);
    }
}
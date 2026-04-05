using System;
using UnityEngine;

namespace ByNorth.Core.VillageSystem
{
    [Serializable]
    public class CreateData<T> where T: ScriptableObject
    {
        [Tooltip("만들 데이터")] public T data = null;
        [Tooltip("만들 수")] public int count = 0;
        
        public CreateData(T data, int count) { this.data = data; this.count = count; }
    }
}
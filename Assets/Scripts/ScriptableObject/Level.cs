using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Level Setting", menuName = "Settings/Level", order = 0)]
public class Level : ScriptableObject
{
    [SerializeField] private SpawnSettings[] _spawn;
    public SpawnSettings[] SpawnSetting => _spawn;
    
    [Serializable]
    public record SpawnSettings
    {
        public GameObject Prefab;
        public Transform SpawnTransform;
    }
}

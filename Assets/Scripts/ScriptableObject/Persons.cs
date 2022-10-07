using UnityEngine;

[CreateAssetMenu(fileName = "New Person Setting", menuName = "Settings/Person", order = 0)]
public class Persons : ScriptableObject
{
    [SerializeField] private float _firerate;
    [SerializeField] private float _speed;
    [SerializeField] private Color _colorSkin;
    
    public float Firerate => _firerate;
    public float Speed => _speed;
    public Color ColorSkin => _colorSkin;
}
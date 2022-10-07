﻿using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] private Transform _firePosition;
    [SerializeField] private Persons _setting;
    public Transform Fireposition => _firePosition;
    public Persons Setting => _setting;
    
    protected void Init()
    {
        var sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.color = _setting.ColorSkin;
    }
}
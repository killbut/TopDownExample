using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WallsTrigger : MonoBehaviour
{
    public event Action OnWallsEnter;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer==LayerMask.NameToLayer("Walls"))
            Debug.Log("HUI");
    }
}
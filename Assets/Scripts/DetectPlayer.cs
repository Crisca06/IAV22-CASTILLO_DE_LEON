using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private GameObject target;
    public MovableEnemy movEnemy;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target) 
            movEnemy.setFollow(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target) movEnemy.setFollow(false);
    }
}

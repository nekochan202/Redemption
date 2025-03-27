using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void LateUpdate()
    {
        Vector3 temp = transform.position;
        temp.x = player.position.x;
        temp.y = player.position.y;

        transform.position = temp;
    }
}
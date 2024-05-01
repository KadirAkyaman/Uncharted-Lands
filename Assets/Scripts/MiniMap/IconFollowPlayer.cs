using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class IconFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x,transform.position.y,player.position.z);
            transform.rotation = Quaternion.Euler(90, 0, -player.rotation.eulerAngles.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform player;
    //public Transform playerFollowCamera;

    private float height;

    // Start is called before the first frame update
    void Start()
    {
        height = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0 , height , 0);

        transform.rotation = Quaternion.Euler(90, 0, -player.rotation.eulerAngles.y);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetMotion : MonoBehaviour
{
    public Transform cameraTrans;
    public Transform playerRootTrans;
    private Vector3 cameraPos = Vector3.zero;
    private Vector3 playerPos = Vector3.zero;
    private float r;

    private float localHeight;
    void Start()
    {
        localHeight = transform.position.y;
        r = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                            new Vector2(playerRootTrans.position.x, playerRootTrans.position.z));

    }
    void FixedUpdate()
    {
        cameraPos = new Vector3(cameraTrans.position.x , localHeight,  cameraTrans.position.z);
        playerPos = new Vector3(playerRootTrans.position.x , localHeight, playerRootTrans.position.z);

        Vector3 playerToCamera = cameraPos - playerPos;
        Vector3 playerToCameraDerction = playerToCamera.normalized * r;

        float angle = Mathf.Acos(r / playerToCamera.magnitude) * Mathf.Rad2Deg;

        Vector3 leftTangent = playerPos + Quaternion.Euler(0, -angle, 0) * playerToCameraDerction;
        //Vector3 rightTangent = playerPos + Quaternion.Euler(0, angle, 0) * playerToCameraDerction;

        transform.localPosition = new Vector3(leftTangent.x, playerRootTrans.position.y + localHeight, leftTangent.z);
    }


}

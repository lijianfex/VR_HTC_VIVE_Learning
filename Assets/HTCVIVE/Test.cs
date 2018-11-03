using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Test : MonoBehaviour
{
    public SteamVR_Skeleton_JointIndexEnum fingerJointHover = SteamVR_Skeleton_JointIndexEnum.indexTip;
    public new GameObject gameObject;


    public Vector3 oldPos;
    public Vector3 newPos;

    public Vector3 currentDir;
    public Vector3 newDir;

    Transform renderModel;

    void Start()
    {
        renderModel = Player.instance.rightHand.gameObject.transform;
        oldPos = renderModel.position;
    }


    void Update()
    {
        currentDir = transform.forward;
        transform.position += currentDir * Time.deltaTime * 4f;

        newPos = renderModel.position;

        if (newPos != oldPos)
        {
            newDir = (newPos - oldPos).normalized;
            oldPos = newPos;
        }

        newDir = Vector3.Lerp(currentDir, newDir, 100 * Time.deltaTime);

        gameObject.transform.localRotation = Quaternion.FromToRotation(currentDir, newDir);



    }
}

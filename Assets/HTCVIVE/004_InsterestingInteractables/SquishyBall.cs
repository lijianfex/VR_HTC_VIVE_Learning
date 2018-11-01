using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class SquishyBall : MonoBehaviour
{

    public Interactable interactable; //交互组件

    public new SkinnedMeshRenderer renderer; //模型的SkinnedMeshRenderer ,用来控制变形

    public bool afftectMaterial;  //是否变形后，调整材质

    [SteamVR_DefaultAction("Squeeze")]
    public SteamVR_Action_Single gripSqueeze; //握的手势操作

    [SteamVR_DefaultAction("Squeeze")]
    public SteamVR_Action_Single pinchSqueeze; //捏的手势操作

    public TextMesh textMesh; //用于显示状态的 3D 文本

    private float attachTime; //抓取的时长

    private new Rigidbody rigidbody; //刚体


    void Start()
    {
        if (interactable == null)
            interactable = GetComponent<Interactable>();
        if (renderer == null)
            renderer = GetComponent<SkinnedMeshRenderer>();
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        textMesh.text = "无手悬停！";
    }


    void Update()
    {
        float grip = 0;
        float pinch = 0;
        if (interactable.attachedToHand)
        {
            grip = gripSqueeze.GetAxis(interactable.attachedToHand.handType);
            pinch = pinchSqueeze.GetAxis(interactable.attachedToHand.handType);
        }

        renderer.SetBlendShapeWeight(0, Mathf.Lerp(renderer.GetBlendShapeWeight(0), grip * 150, Time.deltaTime * 10));
        if (renderer.sharedMesh.blendShapeCount > 1)
        {
            renderer.SetBlendShapeWeight(1, Mathf.Lerp(renderer.GetBlendShapeWeight(1), pinch * 200, Time.deltaTime * 10));
        }

        if (afftectMaterial)
        {
            renderer.material.SetFloat("_Deform", Mathf.Pow(grip * 1.5f, 0.5f));
            if (renderer.material.HasProperty("_PinchDeform"))
            {
                renderer.material.SetFloat("_PinchDeform", Mathf.Pow(pinch * 2.0f, 0.5f));
            }
        }
    }

    private void OnHandHoverBegin(Hand hand)
    {
        textMesh.text = "弹性球正被: " + hand.name + " 悬停！";
        
    }


    private void OnHandHoverEnd(Hand hand)
    {
        textMesh.text = "无手悬停！";        
    }

    //-------------------------------------------------
    // 当可交互物体刚被手抓取附着时，被调用一次
    //-------------------------------------------------
    private void OnAttachedToHand(Hand hand)
    {
        textMesh.text = "弹性球附着在：" + hand.name;
        attachTime = Time.time;
    }


    //-------------------------------------------------
    // 当可交互物体刚被手释放分离时，被调用一次
    //-------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {
        textMesh.text = "弹性球从 " + hand.name + " 分离！";
    }



    //-------------------------------------------------
    //当可交互物体刚被手一直抓取附着时，被每帧调用
    //-------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        textMesh.text = "弹性球附着在：" + hand.name + "\n附着时间: " + (Time.time - attachTime).ToString("F2");
    }
}

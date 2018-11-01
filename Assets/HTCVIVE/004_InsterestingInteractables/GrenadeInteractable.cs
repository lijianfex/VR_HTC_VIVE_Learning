using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class GrenadeInteractable : MonoBehaviour
{
    public GameObject explodePartPrefab; //爆炸生成物 预制体

    public int explodeCount = 10; //爆炸物的数量

    public float minMagnitudeToExplode = 1f; //最小的爆炸冲量强度

    private Interactable interactable; //用来判断是否还在手上

    public TextMesh textMesh; //用于显示状态的 3D 文本

    private float attachTime; //抓取的时长

    private void Start()
    {
        interactable = this.GetComponent<Interactable>();
        textMesh.text = "无手悬停！";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (interactable != null && interactable.attachedToHand != null) //防止在手上发生爆炸
            return;

        if (collision.impulse.magnitude > minMagnitudeToExplode) // 碰撞冲量强度 大于 最小爆炸强度才产生爆炸物
        {
            for (int explodeIndex = 0; explodeIndex < explodeCount; explodeIndex++)
            {
                GameObject explodePart = (GameObject)GameObject.Instantiate(explodePartPrefab, this.transform.position, this.transform.rotation);
                explodePart.GetComponentInChildren<MeshRenderer>().material.SetColor("_TintColor", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
            }

            Destroy(this.gameObject); //销毁手雷
        }
    }

    private void OnHandHoverBegin(Hand hand)
    {
        textMesh.text = "手雷正被: " + hand.name + " 悬停！";
       

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
        textMesh.text = "手雷附着在：" + hand.name;
        attachTime = Time.time;
    }


    //-------------------------------------------------
    // 当可交互物体刚被手释放分离时，被调用一次
    //-------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {
        textMesh.text = "手雷从 " + hand.name + " 分离！";    
    }



    //-------------------------------------------------
    //当可交互物体刚被手一直抓取附着时，被每帧调用
    //-------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        textMesh.text = "手雷附着在：" + hand.name + "\n附着时间: " + (Time.time - attachTime).ToString("F2");
    }
}

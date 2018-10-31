
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// 简单的交互
/// </summary>
public class SampleInteractable : MonoBehaviour {

    public TextMesh textMesh;//显示文字，输出状态

    private Vector3 oldPosition; //记录原来的位置，在分离时回到原来的位置。
    private Quaternion oldRotation;//记录原来的旋转，在分离时回到原来的旋转角度

    private float attachTime; //被抓取附着的时间

    //附着的类型标志
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);

    //可交互对象
    private Interactable interactable;

    //-------------------------------------------------
    void Awake()
    {
        //textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = "无手柄悬停！";

        interactable = this.GetComponent<Interactable>();
    }



    //-------------------------------------------------
    // 当手开始悬停在可交互物体上，被调用一次
    //-------------------------------------------------
    private void OnHandHoverBegin(Hand hand)
    {
        textMesh.text = "正在悬停的手柄: " + hand.name;
    }


    //-------------------------------------------------
    // 当手结束悬停在可交互物体上，被调用一次
    //-------------------------------------------------
    private void OnHandHoverEnd(Hand hand)
    {
        textMesh.text = "无手柄悬停！";
    }


    //-------------------------------------------------
    // 当手一直悬停在可交互物体上，被每帧调用
    //-------------------------------------------------
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting(); //获取手柄进行抓取的方式

        bool isGrabEnding = hand.IsGrabEnding(this.gameObject); //获取手柄是否结束抓取

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            // 保存位置/旋转，当分离时，以便可以恢复它。
            oldPosition = transform.position;
            oldRotation = transform.rotation;

           
            //锁定该交互物，避免再悬停到其他物体上，保证一次悬停锁定一个可交互的物体
            hand.HoverLock(interactable);

            // 附加该物体到手上
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
        }
        else if (isGrabEnding)
        {
            // 从手上分离该物体
            hand.DetachObject(gameObject);

            // 解除对该物体的悬停锁定
            hand.HoverUnlock(interactable);

            // 恢复分离的物体到原来的位置
            transform.position = oldPosition;
            transform.rotation = oldRotation;
        }
    }



    //-------------------------------------------------
    // 当可交互物体刚被手抓取附着时，被调用一次
    //-------------------------------------------------
    private void OnAttachedToHand(Hand hand)
    {
        textMesh.text = "球体附着在：" + hand.name;
        attachTime = Time.time;
    }


    //-------------------------------------------------
    // 当可交互物体刚被手释放分离时，被调用一次
    //-------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {
        textMesh.text = "球体从 " + hand.name+" 分离！";
    }



    //-------------------------------------------------
    //当可交互物体刚被手一直抓取附着时，被每帧调用
    //-------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
        textMesh.text = "球体附着在：" + hand.name + "\n附着时间: " + (Time.time - attachTime).ToString("F2");
    }


    //-------------------------------------------------
    // 当附加的GameObject成为主要附加对象时调用调用一次(就是说附加了很多物体到附加物栈中，到该物体成为栈顶时，调用一次)
    //-------------------------------------------------
    private void OnHandFocusAcquired(Hand hand)
    {
    }


    //-------------------------------------------------
    // 当这个主要附加GameObject不再是栈顶对象时调用一次
    //-------------------------------------------------
    private void OnHandFocusLost(Hand hand)
    {
    }

}

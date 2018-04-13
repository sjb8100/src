using UnityEngine;
using System.Collections;
using System;

public class SinglenNoticeLable : MonoBehaviour
{

    UILabel lable;
    public int lableWidth = 0;
    bool moving = false;
    Transform trans = null;
    float speed = 0f;
    bool showed = false;
    Action<SinglenNoticeLable> onMoveEnd;
    void Awake()
    {
        trans = transform;
        lable = GetComponent<UILabel>();
    }

    void Start()
    {

    }

    public void SetDelegate(Action<SinglenNoticeLable> onMoveEnd)
    {
        this.onMoveEnd = onMoveEnd;
    }

    public void SetContent(string content)
    {
        lable.text = content;
        lableWidth = lable.width;
    }

    public void StartMove(float speed)
    {
        moving = true;
        this.speed = speed;
    }

    void Update()
    {
        if (!moving)
        {
            return;
        }
        Vector3 pos = trans.localPosition;
        pos.x -= Time.deltaTime * speed;
        trans.localPosition = pos;
        if (!showed && lable.isVisible)
        {
            showed = true;
        }

        if (showed && !lable.isVisible)
        {
            if (onMoveEnd != null)
            {
                onMoveEnd(this);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// (带过度的)转向
/// </summary>
class RotateDelayTo : MonoBehaviour
{
    public float speed = 40;
    /// <summary>
    /// 本次转向的最终朝向
    /// </summary>
    private Vector3 finalDestDir;
    public Vector3 curLookAtPos;

    void Awake()
    {
        this.enabled = false;
    }

    /// <summary>
    /// 实体看向某个点（忽略目标点与自身的y方向差值）
    /// </summary>
    /// <param name="vector3"></param>
    public void LookAt(Vector3 lookAtPos)
    {
        if (lookAtPos == curLookAtPos) return;
        curLookAtPos = lookAtPos;
        finalDestDir = lookAtPos - this.transform.position;
        if (finalDestDir == Vector3.zero) return;

        finalDestDir.Normalize();

        this.enabled = true;
    }

    void Update()
    {
        if (finalDestDir == Vector3.zero)
        {
            this.enabled = false;
            return;
        }

        var dir = Vector3.RotateTowards(this.gameObject.transform.forward, finalDestDir, speed * Mathf.Deg2Rad, 0f);
        this.transform.rotation = Quaternion.LookRotation(dir);
    }
}

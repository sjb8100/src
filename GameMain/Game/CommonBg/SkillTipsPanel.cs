using System;
using System.Collections.Generic;
using UnityEngine;

partial class SkillTipsPanel : UIPanelBase
{
    public class SkillTipsInfo
    {
        public string icon;
        public string name;
        public string des;
        /// <summary>
        /// 0则开放
        /// </summary>
        public uint openLevel = 0;
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data is SkillTipsInfo )
        {
            SkillTipsInfo skilldata = data as SkillTipsInfo;
            if (skilldata != null)
            {
                m_sprite_icon.spriteName = skilldata.icon;
                m_label_name.text = skilldata.name;
                m_label_result.text = skilldata.des;

                if (skilldata.openLevel > 0)
                {
                    m_label_unlockLevel.text = string.Format("{0}级且领悟上一个技能后解锁",skilldata.openLevel);
                }
                else
                {
                    m_label_unlockLevel.text = "";
                }
            }
        }
    }
    /// <summary>
    /// 初始化父节点 和相对 父节点的偏移
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="relativeOffset"></param>
    public void InitParentTransform( Transform parent , Vector2 relativeOffset)
    {
 
        Vector3 vec = transform.worldToLocalMatrix.MultiplyPoint( parent.position );
        int height = 360; //Screen.height/2;
        int bgHeight = m_sprite_bg.height/2;
        float y = vec.y + m_sprite_bg.height / 2 + relativeOffset.y / 2;
        float offsety = 0;
        if(y+bgHeight > height)
        {
            offsety = height - (y + bgHeight);
        }
        y = y + offsety;
        Vector3 locPos = new Vector3( vec.x - m_sprite_bg.width / 2 - relativeOffset.x / 2 , y , vec.z );

        m_sprite_bg.transform.localPosition = locPos;
    }
    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }
}
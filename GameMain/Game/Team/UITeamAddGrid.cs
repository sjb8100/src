using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UITeamAddGrid : UIGridBase
{

    GameObject m_spBg;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_spBg = this.transform.Find("bg").gameObject;

        UIEventListener.Get(m_spBg).onPress = OnClickBg;
    }

    void OnClickBg(GameObject go, bool state)
    {
        if (m_spBg != null)
        {
            if (state)
            {
                m_spBg.gameObject.SetActive(true);
            }
            else
            {
                m_spBg.gameObject.SetActive(false);
            }
        }
    }
}


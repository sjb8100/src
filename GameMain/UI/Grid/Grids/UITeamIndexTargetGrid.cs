using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


    class UITeamIndexTargetGrid : UIGridBase
    {

        UILabel m_lblName;

        GameObject m_goSelect;

        GameObject m_goSelectIcon;

        uint m_activityId;

        public uint Id
        {
            get 
            {
                return m_activityId;
            }
        }

        #region overridemethod

        protected override void OnAwake()
        {
            base.OnAwake();

            m_lblName = this.transform.Find("Target/Target_label").GetComponent<UILabel>();
            m_goSelect = this.transform.Find("Target/ChoseMark").gameObject;
            m_goSelectIcon = this.transform.Find("Target/Icon/IconChose").gameObject;
        }

        public override void SetGridData(object data)
        {
            base.SetGridData(data);
            m_activityId = (uint)data;
        }

        #endregion

        public void SetName(string name)
        {
            if (m_lblName != null)
            {
                m_lblName.text = name;
            }
        }

        public void SetSelect(bool select)
        {
            if (m_goSelect != null && m_goSelectIcon!= null && m_goSelect.activeSelf != select)
            {
                m_goSelect.SetActive(select);
                m_goSelectIcon.SetActive(select);
            }
        }


    }


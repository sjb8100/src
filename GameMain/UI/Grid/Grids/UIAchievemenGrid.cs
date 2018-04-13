using UnityEngine;
using table;
using GameCmd;
using System;
using System.Collections.Generic;
public class UIItemRewardData 
{
    public uint itemID;
    public uint num;
    public ItemSerialize data;
    public string name = "";
    public bool blockVisible;
    public bool hasGot;
    public bool GodWeapenType;
    public uint DataID;
    public bool showAdditional =false;
    public string additionalIconName ="";
}
class UIAchievemenGrid : UIGridBase
{
    UILabel name;
    UILabel description;
    UISlider process;
    UILabel processNum;
    UIButton canReceive;
    UIButton goToDo;
    UILabel point;
    GameObject received;

    UIToggle toggle;
    Transform rewardRoot;

    AchieveData achieveData = null;

    Transform m_trans_UIItemRewardGrid;
    UIGridCreatorBase m_ctor;
    public int index
    {
        set;
        get;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("TittleContent/AchievementName").GetComponent<UILabel>();
        rewardRoot = CacheTransform.Find("TittleContent/RewardRoot");
        point = CacheTransform.Find("Bg/ICON/Label").GetComponent<UILabel>();
        description = CacheTransform.Find("Describe").GetComponent<UILabel>();
        process = CacheTransform.Find("AchievementSlider").GetComponent<UISlider>();
        processNum = CacheTransform.Find("AchievementSlider/Label").GetComponent<UILabel>();
        canReceive = CacheTransform.Find("Status/Btn_Receive").GetComponent<UIButton>();
        UIEventListener.Get(canReceive.gameObject).onClick = OnBtnCanReceiveClick;
        goToDo = CacheTransform.Find("Status/Btn_Go").GetComponent<UIButton>();
        UIEventListener.Get(goToDo.gameObject).onClick = OnBtnGoClick;
        received = CacheTransform.Find("Status/Received").gameObject;
        received.SetActive(false);
        toggle = CacheTransform.Find("Toggle").GetComponent<UIToggle>();
        m_trans_UIItemRewardGrid = CacheTransform.Find("UIItemRewardGrid");

        AddCreator(rewardRoot);
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data != null  && data is int)
        {
            index = (int)data;
        }
   
    }
    public void UpdateItemInfo(AchieveData achieveData)
    {
        this.achieveData = achieveData;
        if (achieveData == null)
        {
            return;
        }
        AchievementDataBase achieveDataBase = DataManager.Manager<AchievementManager>().GetAchievementTableData(achieveData.id);
        if (achieveDataBase != null)
        {
            name.text = achieveDataBase.name;
            description.text = achieveDataBase.condition;
            process.value = 0;
            processNum.text = string.Format("{0}/{1}", achieveData.progress, achieveDataBase.aggregate);
            process.value = (achieveDataBase.aggregate <= 0) ? 0 : (achieveData.progress * 1.0f / achieveDataBase.aggregate);
            point.text = achieveDataBase.get_point.ToString();
            switch (achieveData.status)
            {
                case (uint)AchieveStatus.AchieveStatus_CanReceive:
                    ControlStatus(false, true, false);
                    break;
                case (uint)AchieveStatus.AchieveStatus_HaveReceive:
                    ControlStatus(false, false, true);
                    break;
                default:
                    ControlStatus(true, false, false);
                    break;
            }


            if (rewardRoot !=  null)
            {
                ParseReward(achieveDataBase.items);
            }
        }
    }
 
    void ParseReward(string str) 
    {
        string[] itemsList = str.Split(';');
        list.Clear();
        for (int i = 0; i < itemsList.Length; i ++ )
        {
            string[] item = itemsList[i].Split('_');
            if (item.Length == 2)
           {
                uint itemID = 0;
                uint num= 0;
                if (uint.TryParse(item[0], out itemID) && uint.TryParse(item[1], out num))
               {
                   list.Add(new UIItemRewardData() 
                   {
                       itemID = itemID,
                       num = num,
                   });
               }
           }
        }
        m_ctor.CreateGrids(list.Count);
     }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;

    List<UIItemRewardData> list = new List<UIItemRewardData>();
    void AddCreator(Transform parent) 
    {
        if (parent != null)
       {
           m_ctor = parent.GetComponent<UIGridCreatorBase>();
           if (m_ctor == null)
            {
                m_ctor = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
           m_ctor.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
           m_ctor.gridWidth = 90;
           m_ctor.gridHeight = 90;
           m_ctor.RefreshCheck();
           m_ctor.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData,null);
       }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if(index < list.Count)
                {
                    UIItemRewardData data = list[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false, false);
                }                            
            }
        }
    }



    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
        if (achieveData != null)
        {
            achieveData = null;
        }
    }
    void ControlStatus(bool show1, bool show2, bool show3)
    {
        goToDo.gameObject.SetActive(show1);
        canReceive.gameObject.SetActive(show2);
        received.SetActive(show3);
    }

    void OnBtnCanReceiveClick(GameObject go)
    {
        if (achieveData != null && achieveData.status == (uint)AchieveStatus.AchieveStatus_CanReceive)
        {
            DataManager.Manager<AchievementManager>().ReqGetAchieveReward(achieveData.id);
        }
    }

    void OnBtnGoClick(GameObject go)
    {
        ExecuteGoto();
    }

    void ExecuteGoto()
     {
         if (null == achieveData)
         {
             return;
         }
         AchievementDataBase achieveDataBase = DataManager.Manager<AchievementManager>().GetAchievementTableData(achieveData.id);
         if (null == achieveDataBase)
         {
             return;
         }
         if (achieveDataBase.jumpID > 0)
         {
             ItemManager.DoJump(achieveDataBase.jumpID);
         }
        else if (achieveDataBase.taskID > 0)
         {
             Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
             new Client.stDoingTask() { taskid = achieveDataBase.taskID });
         }
     }

    public void SetSelect(bool value) 
    {
        if (null != toggle)
        {
            toggle.value = value;
        }
    }
}

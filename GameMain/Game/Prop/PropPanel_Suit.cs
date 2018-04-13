
//*************************************************************************
//	创建日期:	2017/2/20 星期一 16:23:23
//	文件名称:	PropPanel_Suit
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	时装处理
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using table;
using UnityEngine;
enum SuitEventDispatch
{
    Refresh,//刷新数据
}
partial class PropPanel
{
    //时装模式
    private GameCmd.EquipSuitType m_em_fashionMode = GameCmd.EquipSuitType.Clothes_Type;
    //格子生成器
    private UIGridCreatorBase m_fashionCreator = null;
    //时装数据
    private List<uint> m_list_fashionDatas = new List<uint>();
    //宠物renderobj
    private IRenerTextureObj m_petRTObj = null;
    bool m_bInitSuit = false;

    void InitSuitUI()
    {
        InitSuitBtnScorll();
        if (m_bInitSuit)
        {
            SetNoneGridSelect();
            return;
        }
        if (null != m_trans_FashionScrollView)
        {
            m_fashionCreator = m_trans_FashionScrollView.GetComponent<UIGridCreatorBase>();
            if (null == m_fashionCreator)
            {
                m_fashionCreator = m_trans_FashionScrollView.gameObject.AddComponent<UIGridCreatorBase>();
            }

            GameObject obj = UIManager.GetResGameObj(GridID.Uifashiongrid);
            m_fashionCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_fashionCreator.gridContentOffset = new Vector2(-148, -161f);
            m_fashionCreator.gridWidth = 150;
            m_fashionCreator.gridHeight = 182;
            m_fashionCreator.rowcolumLimit = 3;
            m_fashionCreator.RefreshCheck();
            m_fashionCreator.Initialize<UIFashionGrid>(obj, OnUpdateGridData, OnGridUIEventDlg);
            //m_fashionCreator.Init();
            CreateFashionGridUI();

            m_bInitSuit = true;
        }


    }
    Dictionary<string, SuitBtnItem> suitDic = new Dictionary<string, SuitBtnItem>();
    //按钮名字是key  类型是value
    Dictionary<string, int> m_typeDic = new Dictionary<string, int>();

    void InitSuitBtnScorll()
    {
        List<string> typeList = GameTableManager.Instance.GetGlobalConfigKeyList("SuitTypeOpen");
        m_grid_suitgrid.sorting = UIGrid.Sorting.Alphabetic;
        for (int i = 0; i < typeList.Count; i++)
        {
            string key = typeList[i];
            List<string> strList = GameTableManager.Instance.GetGlobalConfigList<string>("SuitTypeOpen", key);
            if (strList.Count != 5)
            {
                Engine.Utility.Log.Error("全局配置suittypeopen不符合长度 5");
                continue;
            }
            string suitBtnName = strList[3];
            if (!m_typeDic.ContainsKey(suitBtnName))
            {
                int type = 0;
                if (int.TryParse(key, out type))
                {
                    m_typeDic.Add(suitBtnName, type);
                }
            }
            bool bOpen = strList[0] == "1" ? true : false;
            Transform suitBtnTrans = m_grid_suitgrid.transform.Find(suitBtnName);
            if (suitBtnTrans == null)
            {
                GameObject go = NGUITools.AddChild(m_grid_suitgrid.gameObject, m_trans_SuitBtnItem.gameObject);
                if (go != null)
                {
                    go.SetActive(true);
                    suitBtnTrans = go.transform;
                    SuitBtnItem si = go.AddComponent<SuitBtnItem>();
                    if (si != null)
                    {
                        if (!suitDic.ContainsKey(suitBtnName))
                        {
                            suitDic.Add(suitBtnName, si);
                        }
                        si.InitSuitBtnItem(bOpen, strList[1], strList[4], strList[2], (name) =>
                            {
                                if (m_typeDic.ContainsKey(name))
                                {
                                    int suitType = m_typeDic[name];

                                 
                                    OnClickSuitBtn(suitType);

                                }

                            });
                        go.name = suitBtnName;

                    }
                }
            }
            UIButton btn = suitBtnTrans.Find("suitItem").GetComponent<UIButton>();
            if (btn != null)
            {
                //btn.isEnabled = bOpen;
                if (suitBtnName.Equals("1"))
                {
                    btn.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

    }
    List<SuitDataBase> GetPetSuitData()
    {
        List<SuitDataBase> resultList = new List<SuitDataBase>();
        List<ClientSuitData> sortList = m_suitDataManager.GetSortListData();
        for (int i = 0; i < sortList.Count; i++)
        {
            ClientSuitData cd = sortList[i];
            SuitDataBase db = GameTableManager.Instance.GetTableItem<SuitDataBase>(cd.suitBaseID, 1);
            if (db != null)
            {
                if (db.type == (uint)GameCmd.EquipSuitType.Magic_Pet_Type)
                {
                    resultList.Add(db);
                }
            }
        }
        return resultList;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseID">等于0时为第一次初始化</param>
    /// <returns></returns>
    int GetPetResID(int baseID = 0)
    {
        int resID = 0;
        List<SuitDataBase> petList = GetPetSuitData();
        SuitDataBase petData = null;
        if (baseID == 0)
        {
            if (petList.Count == 0)
            {
                return resID;
            }

            petData = petList[0];
            PetDataManager pm = DataManager.Manager<PetDataManager>();
            if (pm != null)
            {
                if (pm.CurFightingPet != 0)
                {
                    IPet pet = pm.GetPetByThisID(pm.CurFightingPet);
                    if (pet != null)
                    {
                        uint petBaseID = pet.PetBaseID;
                        PetDataBase db = GameTableManager.Instance.GetTableItem<PetDataBase>(petBaseID);
                        if (db != null)
                        {
                            resID = (int)db.modelID;
                        }
                    }
                }
                else
                {
                    resID = (int)petData.resid;
                }
            }
        }
        else
        {
            for (int i = 0; i < petList.Count; i++)
            {
                if (baseID == petList[i].base_id)
                {
                    petData = petList[i];
                    break;
                }
            }
            if (petData == null)
            {
                return resID;
            }

            resID = (int)petData.resid;
        }
        return resID;
    }

    void CreatePetRenderTexture(int resID)
    {
        if (m_petRTObj != null)
        {
            m_petRTObj.Release();
            m_petRTObj = null;
        }
        m_petRTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj(resID, 750);
        if (null == m_petRTObj)
        {
            return;
        }
        ModeDiplayDataBase modelDisp = GameTableManager.Instance.GetTableItem<ModeDiplayDataBase>((uint)resID);
        if (modelDisp == null)
        {
            Engine.Utility.Log.Error("模型ID为{0}的模型展示数据为空", resID);
            return;
        }
        m_petRTObj.SetDisplayCamera(modelDisp.pos750, modelDisp.rotate750, modelDisp.Modelrotation);
        m_petRTObj.PlayModelAni(Client.EntityAction.Stand);
        UIRenderTexture rt = m__modelTexture.GetComponent<UIRenderTexture>();
        if (null == rt)
        {
            rt = m__modelTexture.gameObject.AddComponent<UIRenderTexture>();
        }
        if (null != rt)
        {
            rt.SetDepth(0);
            rt.Initialize(m_petRTObj, m_petRTObj.YAngle, new UnityEngine.Vector2(750, 750));
        }

    }
    void ShowPlayerRenderTex(bool bShow)
    {
        if (m_RTObj == null)
        {
            Engine.Utility.Log.Error("rt obj is null");
            return;
        }
        if (m_RTObj != null)
        {
            m_RTObj.Enable(bShow);
            if (!bShow)
            {
                return;
            }
        }
        if (null != m__modelTexture)
        {
            UIRenderTexture rt = m__modelTexture.GetComponent<UIRenderTexture>();
            if (null == rt)
            {
                rt = m__modelTexture.gameObject.AddComponent<UIRenderTexture>();
            }
            if (null != rt)
            {
                rt.SetDepth(0);
                rt.Initialize(m_RTObj, m_RTObj.YAngle, new Vector2(700f, 700f), () =>
                {
                    DataManager.Manager<UIPanelManager>().HidePanel(PanelID.FashionTips);
                });
            }
            SetRenderTexPos(m_RTObj);
        }
        else
        {
            m_RTObj.Release();
            m_RTObj = null;
        }

    }
    void ShowPetRenderTex(bool bShow)
    {
        if (m_petRTObj == null)
        {
           // Engine.Utility.Log.Error("pet rt obj is null");
            return;
        }
        if (m_petRTObj != null)
        {
            m_petRTObj.Enable(bShow);
            if (!bShow)
            {
                return;
            }
        }
        if (null != m__modelTexture)
        {
            UIRenderTexture rt = m__modelTexture.GetComponent<UIRenderTexture>();
            if (null == rt)
            {
                rt = m__modelTexture.gameObject.AddComponent<UIRenderTexture>();
            }
            if (null != rt)
            {
                rt.SetDepth(0);
                rt.Initialize(m_petRTObj, m_petRTObj.YAngle, new Vector2(750, 750), () =>
                {
                    DataManager.Manager<UIPanelManager>().HidePanel(PanelID.FashionTips);
                });
            }
            SetRenderTexPos(m_petRTObj);
        }
        else
        {
            m_petRTObj.Release();
            m_petRTObj = null;
        }
    }
    void SetRenderTexPos(IRenerTextureObj obj)
    {
//         SuitDataBase curData = m_suitDataManager.CurSuitDataBase;
//         if (curData != null)
//         {
//             obj.SetModelScale(curData.modelScale);
//             obj.SetCamera(new Vector3(0, 1f, 0f), new Vector3(0, 45, 0), curData.renderOffset);
//         }
//         if (curData != null)
//         {
//             m__modelTexture.transform.localPosition = new Vector3(curData.modeloffsetX, curData.modeloffsetY, 0);
//         }
    }


    void ResetPlayerObj()
    {
        if (m_RTObj != null)
        {
            m_RTObj.Release();
            m_RTObj = null;
        }
        CreatePlayerView();
    }
    void ResetObj()
    {
        if (m_em_fashionMode == GameCmd.EquipSuitType.Magic_Pet_Type)
        {
            InitPetRenderObj();
        }
        else
        {

            ResetPlayerObj();
        }
        ShowRenderTex();
        SetNoneGridSelect();
        HideFashiontips();
    }
    void HideFashiontips()
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.FashionTips);
    }
    void InitPetRenderObj()
    {
        List<ClientSuitData> petsuitList = m_suitDataManager.GetSortListData();
        int baseID = 0;
        for (int i = 0; i < petsuitList.Count; i++)
        {
            ClientSuitData data = petsuitList[i];
            if (data.suitState == SuitState.Equip)
            {
                baseID = (int)data.suitBaseID;
                break;
            }
        }
        int resID = GetPetResID(baseID);
        CreatePetRenderTexture(resID);
    }
    void ShowRenderTex()
    {
        bool bShowPet = m_em_fashionMode == GameCmd.EquipSuitType.Magic_Pet_Type ? true : false;
        ShowPlayerRenderTex(!bShowPet);
        ShowPetRenderTex(bShowPet);
        PlayShowAni();
    }
    void PlayShowAni()
    {
        if (m_RTObj != null)
        {
            m_RTObj.PlayModelAni(EntityAction.Show.ToString());
        }
        //if(m_petRTObj != null)
        //{
        //    m_petRTObj.PlayModelAni(EntityAction.Show.ToString());
        //}
    }
    #region btns
    void onClick_Btn_reset_Btn(GameObject caster)
    {
        ResetObj();
    }

    void OnClickSuitBtn(int type)
    {
        SetPartBtnHighLight(type);
        m_em_fashionMode = (GameCmd.EquipSuitType)type;
        OnChangePos();
    }

    void SetNoneGridSelect()
    {
        if (m_fashionCreator == null)
        {
            return;
        }
        int nCount = m_fashionCreator.ActiveCount;
        for (int i = 0; i < nCount; i++)
        {
            UIFashionGrid temp = m_fashionCreator.GetGrid<UIFashionGrid>(i);
            if (temp != null)
            {
                temp.SetSelect(false);
            }
        }

    }
    void OnChangePos()
    {
        RefreshFashionGrid();
        m_suitDataManager.SetSuitDataOnChangePos();
        RefreshRenderObj();
    }
    void RefreshRenderObj()
    {
        if (m_em_fashionMode == GameCmd.EquipSuitType.Magic_Pet_Type)
        {
            InitPetRenderObj();
        }
        ShowRenderTex();
    }
    void RefreshFashionGrid()
    {

        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.FashionTips);
        m_suitDataManager.CurSuitType = m_em_fashionMode;
        int count = m_suitDataManager.GetSortListData().Count;
        if (m_fashionCreator != null)
        {
            m_fashionCreator.CreateGrids(count);

            SetNoneGridSelect();
        }

    }
    void ChangeRenderObj(uint baseID, int suitType)
    {
        if (m_RTObj != null)
        {
            m_RTObj.ChangeSuit(m_suitDataManager.GetPosBySuitType((uint)suitType), (int)baseID);
        }
    }
    #endregion
    /// <summary>
    /// 创建时装格子UI
    /// </summary>
    private void CreateFashionGridUI()
    {
        if (null != m_fashionCreator)
        {
            m_fashionCreator.CreateGrids(m_suitDataManager.GetSortListData().Count);

        }
    }
    void SuitEventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)GameEventID.ENTITYSYSTEM_CHANGERENDEROBJ)
        {
            stRefreshRenderObj data = (stRefreshRenderObj)param;
            if (m_em_fashionMode == GameCmd.EquipSuitType.Magic_Pet_Type)
            {
                int resID = GetPetResID((int)data.suitID);
                CreatePetRenderTexture(resID);
            }
            else
            {
                ChangeRenderObj(data.suitID, (int)data.suitType);
            }
            ShowRenderTex();
        }
    }
    void ShowFashion(int part ,uint suitID)
    {
        OnClickSuitBtn(part);
        int resID = GetPetResID((int)suitID);
        CreatePetRenderTexture(resID);
        ShowRenderTex();
        SetGridHilight(suitID);
    }
    void SetPartBtnHighLight(int part)
    {
        List<string> strList = GameTableManager.Instance.GetGlobalConfigList<string>("SuitTypeOpen", part.ToString());
        if(strList == null)
        {
            return;
        }
        string btnName = "";
        if(strList.Count >= 4)
        {
            btnName = strList[3];
        }
        var iter = suitDic.GetEnumerator();
        while(iter.MoveNext())
        {
            var item = iter.Current;
            if(item.Key == btnName.ToString())
            {
                item.Value.SetSprHightLight(true);
            }
            else
            {
                item.Value.SetSprHightLight(false);
            }
        }
    }
    void SetGridHilight(uint suitID)
    {
        if (m_fashionCreator == null)
        {
            return;
        }
        int nCount = m_fashionCreator.ActiveCount;
        for (int i = 0; i < nCount; i++)
        {
            UIFashionGrid temp = m_fashionCreator.GetGrid<UIFashionGrid>(i);
            if (temp != null)
            {
                if(temp.SuitBaseID == suitID)
                {
                    temp.SetSelect(true);
                }
                else
                {
                    temp.SetSelect(false);
                }
            }
        }
    }
    void onClick_BtnStatus_Btn(GameObject caster)
    {

    }

    void onClick_FashionTipsClose_Btn(GameObject caster)
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class DownloadPanel : UIPanelBase
{
    class RewardData
    {
        public uint id;
        public uint num;
    }
    List<RewardData> m_lst_Reward = new List<RewardData>();
    private void OnUpdateRewardGrid(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid data = grid as UIItemRewardGrid;
            if (index < m_lst_Reward.Count)
            {
                RewardData param = m_lst_Reward[index];
                if (param != null)
                {
                    data.SetGridData(param.id, param.num, false, false);
                    if (index < 3)
                    {
                        data.AddEffect(true, 52018);
                    }
                }
            }

        }
    }
    private void ShowRewardItem()
    {
        m_ctor_itemRoot.RefreshCheck();
        m_ctor_itemRoot.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateRewardGrid, null);

        List<string> keyList = GameTableManager.Instance.GetGlobalConfigKeyList("DownLoadGifBag");

        if (keyList.Count > 0)
        {
            if (m_lst_Reward == null)
            {
                m_lst_Reward = new List<RewardData>();
            }
            else
            {
                m_lst_Reward.Clear();
            }
            for (int i = 0; i < keyList.Count; i++)
            {
                uint itemNum = GameTableManager.Instance.GetGlobalConfig<uint>("DownLoadGifBag", keyList[i]);

                RewardData msg = new RewardData()
                {
                    id = uint.Parse(keyList[i]),
                    num = itemNum,
                };
                m_lst_Reward.Add(msg);
            }
            m_ctor_itemRoot.CreateGrids(m_lst_Reward.Count);
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        ShowRewardItem();

        if (KDownloadInstance.Instance().IsWF())
        {
            if (Application.isEditor == false)
            {
                KDownloadInstance.DownloadState state = KDownloadInstance.Instance().GetDownloadState();
                if (state != KDownloadInstance.DownloadState.STOP &&
                    state != KDownloadInstance.DownloadState.COMPLETE &&
                    KDownloadInstance.Instance().IsSmallPackage())
                {
                    KDownloadInstance.Instance().StartDownload();
                }

            }
        }
        else
        {
            // 提示下载
            KDownloadInstance.DownloadState state = KDownloadInstance.Instance().GetDownloadState();
            if (state != KDownloadInstance.DownloadState.STOP &&
                state != KDownloadInstance.DownloadState.COMPLETE &&
                state != KDownloadInstance.DownloadState.ERROR &&
                state != KDownloadInstance.DownloadState.DOWNLOAD &&
                KDownloadInstance.Instance().IsSmallPackage())
            {
                /// 这是修复,不是准备
                Action agree = delegate
                {
                    // 设置回调
                    KDownloadInstance.Instance().StartDownload();
                };

                Action refuse = delegate
                {

                };

                string des = string.Format("您当前处于3G,4G环境下是否继续下载.");
                // 提示下载.
                TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, des, agree, refuse, null, "提示", "确定", "取消", 1);
            }

        }
        Ref_Reward();
        Ref_TotalLenght();
        Ref_DownloadBtnState();
    }


    protected override void OnHide()
    {
        base.OnHide();
    }

    // 开始下载
    void onClick_Btn_Continue_Btn(GameObject caster)
    {
        KDownloadInstance.Instance().StartDownload();
        Ref_DownloadBtnState();

    }
    void onClick_Btn_Pause_Btn(GameObject caster)
    {
        KDownloadInstance.Instance().StopDownload();
        Ref_DownloadBtnState();
    }

    void onClick_Colsebtn_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.DownloadPanel);
    }

    // 奖励领取
    void onClick_Btn_takeReward_Btn(GameObject caster)
    {
        KDownloadInstance.DownloadState state = KDownloadInstance.Instance().GetDownloadState();
        if (state == KDownloadInstance.DownloadState.COMPLETE)
        {
            GameCmd.stCompleteDownLoadDataUserCmd_C cmd = new GameCmd.stCompleteDownLoadDataUserCmd_C();
            NetService.Instance.Send(cmd);

            //KHttpDown.Instance().m_bTakeReward = true;

            KDownloadInstance.Instance().SetTakeReward(true);

            // 奖励按钮更新
            Ref_Reward();
            Ref_DownloadBtnState();

            MainPanel mainPanel = DataManager.Manager<UIPanelManager>().GetPanel<MainPanel>(PanelID.MainPanel);
            if (mainPanel != null)
            {
                mainPanel.HideHttpDown();
            }
        }


    }


    void Ref_TotalLenght()
    {
        int nLenght = KDownloadInstance.Instance().GetTotalLength() / 1024 / 1024;

        string text = string.Format("继续体验新的内容需要下载资源扩展包,下载将消耗{0}M流量(wifi环境下自动下载)", nLenght);

        m_label_label3.text = text;

    }

    // 奖励按钮更新
    void Ref_Reward()
    {
        // 领取过奖励
        if (KDownloadInstance.Instance().GetTakeReward())
        {
            m_btn_btn_takeReward.gameObject.SetActive(false);
            m_sprite_labelReward.gameObject.SetActive(true);
        }
        else
        {
            m_btn_btn_takeReward.gameObject.SetActive(true);
            m_sprite_labelReward.gameObject.SetActive(false);
        }

        KDownloadInstance.DownloadState state = KDownloadInstance.Instance().GetDownloadState();
        if (state != KDownloadInstance.DownloadState.COMPLETE)
        {
            m_btn_btn_takeReward.isEnabled = false;
            m_label_Name_hui.gameObject.SetActive(true);
            m_label_Name.gameObject.SetActive(false);
        }
        else
        {
            m_btn_btn_takeReward.isEnabled = true;
            m_label_Name_hui.gameObject.SetActive(false);
            m_label_Name.gameObject.SetActive(true);
        }

    }

    // 下载按钮显示更新
    void Ref_DownloadBtnState()
    {
        KDownloadInstance.DownloadState state = KDownloadInstance.Instance().GetDownloadState();
        if (state == KDownloadInstance.DownloadState.ERROR ||
            state == KDownloadInstance.DownloadState.NULL ||
            state == KDownloadInstance.DownloadState.STOP)
        {
            // 非下载中
            m_btn_btn_Continue.isEnabled = true;
            m_btn_btn_Continue.gameObject.SetActive(true);

            m_btn_btn_Pause.isEnabled = false;
            m_btn_btn_Pause.gameObject.SetActive(false);
        }
        else if (state == KDownloadInstance.DownloadState.COMPLETE)
        {
            // 下载完成
            m_btn_btn_Pause.isEnabled = false;
            m_btn_btn_Continue.isEnabled = false;
            // 文字修改
            m_label_Label.text = "下载完成";
            m_label_chuanzhancd.text = "下载完成";


        }
        else// 下载中...
        {

            m_btn_btn_Pause.isEnabled = true;
            m_btn_btn_Pause.gameObject.SetActive(true);

            m_btn_btn_Continue.isEnabled = false;
            m_btn_btn_Continue.gameObject.SetActive(false);
        }

    }


    string[] downText = new string[3] { "下载中.", "下载中..", "下载中..." };
    int nIndex = 0;
    float m_fLastTextIndex = 0f;
    void Ref_Progress()
    {
        float fProgress = KDownloadInstance.Instance().GetProgress();
        // 进度条更新
        m_slider_percentBar.value = fProgress;

        string strState = "";
        KDownloadInstance.DownloadState state = KDownloadInstance.Instance().GetDownloadState();
        switch (state)
        {
            case KDownloadInstance.DownloadState.DOWNLOAD:

                if (m_fLastTextIndex < Time.time)
                {
                    m_fLastTextIndex = Time.time + 1f;

                    strState = downText[nIndex++];
                    if (nIndex >= 3)
                    {
                        nIndex = 0;
                    }
                }
                else
                {
                    strState = downText[nIndex];
                }

                break;

            case KDownloadInstance.DownloadState.STOP:

                strState = "暂停中";
                break;

            case KDownloadInstance.DownloadState.COMPLETE:

                strState = "已完成";
                break;

        }

        // 文字的更新
        m_label_Percent.text = string.Format("(资源更新进度:{0}%{1})", (int)(fProgress * 100), strState);


    }
    //ui界面更新
    void RefUI()
    {
        // 进度的更新
        Ref_Progress();

        Ref_TotalLenght();

        Ref_DownloadBtnState();

        Ref_Reward();
    }

    void Update()
    {
        RefUI();
    }
}

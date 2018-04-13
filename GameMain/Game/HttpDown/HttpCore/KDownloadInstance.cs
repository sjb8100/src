using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Engine.Utility;
using Client;

public class KLoadConfigFileHelp
{
    //GlobalConst.PackageConfigFile
    public static bool LoadPackageConfigFile(string strConfigFile, ref bool bSmallPackage)
    {
        byte[] configBytes;
        string strIniFile = "";
        if (Application.isEditor)
        {
            strIniFile = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strConfigFile, FileUtils.UnityPathType.UnityPath_CustomPath);
        }
        else
        {
            //该配置不做更新
            strIniFile = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strConfigFile, FileUtils.UnityPathType.UnityPath_StreamAsset);
        }
        if (!FileUtils.Instance().ReadFile(strIniFile, out configBytes))
        {
            return false;
        }

        byte[] desBytes;
        if (!Engine.Utility.Security.EncryptUtil.Decode(ref configBytes, out desBytes))
        {
            configBytes = null;
            return false;
        }
        configBytes = null;
        string configJsonStr = System.Text.UTF8Encoding.UTF8.GetString(desBytes);
        desBytes = null;
        Engine.JsonNode jsonRoot = Engine.RareJson.ParseJson(configJsonStr);
        if (jsonRoot == null)
        {
            Engine.Utility.Log.Error("GlobalConfig LoadPackageConfigFile 解析{0}文件失败!", configJsonStr);
            return false;
        }


        try
        {
            bSmallPackage = ((int)jsonRoot["smallPackage"] == 1) ? true : false;
        }
        catch
        {
            bSmallPackage = false;
        }

        return true;
    }
    public static bool LoadConfigFile(string strConfigFile, ref string strHttpSmallPackagerRUL)
    {
        byte[] configBytes;
        string strIniFile = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strConfigFile, FileUtils.UnityPathType.UnityPath_CustomPath);
        if (!FileUtils.Instance().ReadFile(strIniFile, out configBytes))
        {
            strIniFile = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strConfigFile, FileUtils.UnityPathType.UnityPath_StreamAsset);
            if (!FileUtils.Instance().ReadFile(strIniFile, out configBytes))
            {
                return false;
            }
        }
        byte[] desBytes;
        if (!Engine.Utility.Security.EncryptUtil.Decode(ref configBytes, out desBytes))
        {
            configBytes = null;
            return false;
        }
        configBytes = null;
        string configJsonStr = System.Text.UTF8Encoding.UTF8.GetString(desBytes);
        desBytes = null;
        Engine.JsonNode jsonRoot = Engine.RareJson.ParseJson(configJsonStr);
        if (jsonRoot == null)
        {
            Engine.Utility.Log.Error("GlobalConfig LoadConfigFile 解析{0}文件失败!", configJsonStr);
            return false;
        }
        Engine.JsonObject jsonObj = null;
        /******************SystemConfig开始*****************/
        jsonObj = (Engine.JsonObject)jsonRoot["SystemConfig"];
        if (null != jsonObj)
        {
            //SDKLogin = ((int)jsonObj["SdkLogin"] == 1) ? true : false;
            ////是否开启版本检测
            //verCheck = ((int)jsonObj["vercheck"] == 1) ? true : false;
            //// 是否测试版本
            //alphaVer = ((int)jsonObj["alphaver"] == 1) ? true : false;
            //// Log等级
            //logLevel = (int)jsonObj["loglevel"];
            //Test = ((int)jsonObj["test"] == 1) ? true : false;
            //TestServerId = (uint)jsonObj["testserverid"];
            //FPS = ((int)jsonObj["fps"] == 1) ? true : false;
            //LogServerLevel = (int)jsonObj["logseverlevel"];
        }

        /******************SdkConfig开始*****************/
        Engine.JsonArray jsonArray = (Engine.JsonArray)jsonRoot["SDKConfig"];
        if (null != jsonArray)
        {
            Engine.JsonObject tempJsonObj = null;
            string platform = "";
            bool match = false;
            for (int i = 0, max = jsonArray.Count; i < max; i++)
            {
                tempJsonObj = (Engine.JsonObject)jsonArray[i];
                platform = tempJsonObj["platform"];
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (platform.Equals("android"))
                    {
                        match = true;
                    }

                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    if (platform.Equals("ios"))
                    {
                        match = true;
                    }
                }
                else
                {
                    if (platform.Equals("windows"))
                    {
                        match = true;
                    }
                }

                if (match)
                {
                    //loginUrl = tempJsonObj["loginurl"];
                    //GameID = (int)tempJsonObj["gameid"];
                    //QuestionnaireURL = tempJsonObj["questionnaireurl"];
                    //DNS = tempJsonObj["dns"];
                    //strRomotePlatformUpdataUrl = tempJsonObj["fullupdataurl"];

                    try
                    {
                        string httpSmallPackagerul = tempJsonObj["smallpackurl"];
                        strHttpSmallPackagerRUL = httpSmallPackagerul;
                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        strHttpSmallPackagerRUL = string.Empty;
                    }

                    //正式数据
                    //FormalstrRomoteVersionFileUrl = tempJsonObj["verurl"];
                    //FormalAreaServerUrl = tempJsonObj["AreaServerurl"];
                    //FormalNoticeUrl = tempJsonObj["noticeurl"];

                    ////审核数据
                    //Reviewverurl = tempJsonObj["Reviewverurl"];
                    //ReviewAreaServerurl = tempJsonObj["ReviewAreaServerurl"];
                    //Reviewnoticeurl = tempJsonObj["Reviewnoticeurl"];

                    break;
                }
            }
        }

        strHttpSmallPackagerRUL = string.Empty;
        return true;
    }
}


// 就一个文件就这么玩
class KDownloadInstance
{
    private static KDownloadInstance s_Instance = null;
    public static KDownloadInstance Instance()
    {
        if (s_Instance == null)
        {
            s_Instance = new KDownloadInstance();
        }
        return s_Instance;
    }


    public string m_strURL = string.Empty;
    public string m_strSaveFile = string.Empty;

    private float m_fProgress = 0f;
    private int m_nTotalLength = 0;
    public bool m_bSmallPackage = false;

    // 奖励是否领取过
    private bool m_bTakeReward = false;

    public enum DownloadState
    {
        NULL,
        DOWNLOAD, // 下载中.包涵连接状态.
        STOP,
        COMPLETE,
        ERROR,
    }
    private DownloadState m_State = DownloadState.NULL;

    public float GetProgress()
    {
        return m_fProgress;
    }

    public int GetTotalLength()
    {
        return m_nTotalLength;
    }
    public DownloadState GetDownloadState()
    {
        return m_State;
    }

    public bool GetTakeReward()
    {
        return m_bTakeReward;
    }
    public void SetTakeReward(bool bReward)
    {
        m_bTakeReward = bReward;
    }

    //wifi
    public bool IsWF()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) //wifi
        {
            return true;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) // 3g 4g
        {
            return false;
        }
        return false;
    }

    void InitConfig()
    {
        //try
        //{
        //    m_strURL = GlobalConfig.Instance().httpSmallPackagerul;
        //}
        //catch(Exception e)
        //{
        //    return;
        //}

        KLoadConfigFileHelp.LoadConfigFile(GlobalConst.MainConfigFile, ref m_strURL);
        if (m_strURL == string.Empty)
            return;


        string strFileName = System.IO.Path.GetFileName(m_strURL);

        if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android)
        {
            m_strSaveFile = GlobalConst.localFilePath + "assets/" + strFileName;
        }
        else if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
        {
            m_strSaveFile = GlobalConst.localFilePath + strFileName;
        }
        else
        {
            m_strSaveFile = "c://map.zip";
        }
    }

    void InitPackageConfig()
    {
        //KLoadConfigFileHelp.LoadConfigFile
        KLoadConfigFileHelp.LoadPackageConfigFile(GlobalConst.PackageConfigFile, ref m_bSmallPackage);
    }

    public KDownloadInstance()
    {
        //m_strURL = "http://update.jzry.zqgame.com/androidmicro/resources/map.zip";
        //m_strSaveFile = "c://map.zip";

        RegisterEvent();
        InitConfig();
        InitPackageConfig();

        if (!IsSmallPackage())
        {
            m_State = DownloadState.COMPLETE;
        }

        InitHttpDownloadData();

    }


    public static int HTTPDOWNLOAD_LENGHT = 291 * 1024 * 1024;// 先这里写以后用http获取
    void InitHttpDownloadData()
    {
        if (IsSmallPackage())
        {
            string strPath = m_strSaveFile + ".fd";
            if (File.Exists(strPath))
            {
                FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);
                m_fProgress = (float)fs.Length / (float)HTTPDOWNLOAD_LENGHT;
                fs.Close();
            }
            m_nTotalLength = HTTPDOWNLOAD_LENGHT;
        }
        else
        {
            m_fProgress = 1f;
            m_nTotalLength = HTTPDOWNLOAD_LENGHT;
        }

    }

    private void RegisterEvent()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SHOWHTTPDOWNUI, GlobalEventHandler);

    }

    private void UnRegisterEvent()
    {
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SHOWHTTPDOWNUI, GlobalEventHandler);
    }


    public void GlobalEventHandler(int eventid, object data)
    {
        switch (eventid)
        {
            case (int)GameEventID.ENTITYSYSTEM_LEVELUP:
            case (int)GameEventID.SYSTEM_GAME_READY:
                //人物等级达到18级,是小包且是wifi环境下直接开始下载 
                IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
                if (mainPlayer != null &&
                    m_State != DownloadState.COMPLETE &&
                    m_State != DownloadState.DOWNLOAD &&
                    m_State != DownloadState.ERROR &&
                    m_State != DownloadState.STOP)
                {
                    int nLevel = MainPlayerHelper.GetPlayerLevel();

                    if (nLevel >= 18) //等级
                    {
                        if (IsSmallPackage()) //小包
                        {
                            if (IsWF()) //wifi
                            {
                                if (Application.isEditor == false)
                                {
                                    StartDownload();
                                }
                            }
                        }
                    }

                }

                break;
            case (int)GameEventID.SHOWHTTPDOWNUI:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                break;
        }


    }

    public bool IsSmallPackage()
    {
        bool bResult = true;
        try
        {
            bResult = m_bSmallPackage;
        }
        catch
        {
            bResult = false; // 不是小包
        }

        if (bResult == true)// 小包安装过的也算整包.
        {
            string strFullPackage = "IsFullPackage.txt";
            string strUnZipFileSuccess = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strFullPackage, Engine.Utility.FileUtils.UnityPathType.UnityPath_CustomPath);
            if (File.Exists(strUnZipFileSuccess))
            {
                bResult = false;
            }
        }

        return bResult;

    }

    private void OnTotalLength(int length)
    {
        m_nTotalLength = length;
    }

    private void SaveUnZipFileSuccess()
    {
        // 写文件表示拷贝ok.
        string strFullPackage = "IsFullPackage.txt";
        string strCopyFile = Engine.Utility.FileUtils.Instance().FullPathFileName(ref strFullPackage, FileUtils.UnityPathType.UnityPath_CustomPath);
        Engine.Utility.FileUtils.Instance().WriteStreamFile(ref strCopyFile, "ok");

    }

    private string GetUnZipPath()
    {
        string strResPath;
        if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android)
        {
            strResPath = GlobalConst.localFilePath + "assets/";
        }
        else if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
        {
            strResPath = GlobalConst.localFilePath;
        }
        else
        {
            strResPath = GlobalConst.localFilePath;
        }
        return strResPath;
    }

    private void OnComplete(string path)
    {
        // 下载完成解压资源
        m_State = DownloadState.COMPLETE;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                string strResPath = GetUnZipPath();
                //Engine.ThreadHelper.RunOnMainThread(() =>
                //{
                string strLog = string.Format("解压文件{0},到{1}", m_strSaveFile, strResPath);
                UnityEngine.Debug.Log(strLog);
                bool bResult = ZipHelper.UnZip(m_strSaveFile, strResPath);
                if (bResult == true)
                {
                    // 记录下
                    SaveUnZipFileSuccess();
                    // 删除文件
                    File.Delete(m_strSaveFile);

                }
                else
                {
                    Debug.Log("解压资源文件失败");
                }
                //});

            });
            t.Start();
        }

    }

    private void OnProgress(float progress)
    {
        //textProgress1.text = "进度:" + (progress * 100).ToString("0.00") + "%";
        m_fProgress = progress;
    }

    public void OnError(string url)
    {
        // 就一个文件就这么玩
        // 继续下载.
        StartDownload();
    }

    bool CkeckNetworking()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StartDownload()
    {
        // 网络不通跳过http下载
        if (CkeckNetworking() == false)
            return;

        KDownloadHandler handler = KDownlaodFile.Instance().StartDownload(m_strURL, m_strSaveFile);
        if (handler == null)
            return;

        m_State = DownloadState.DOWNLOAD;

        handler.RegisteProgressBack(OnProgress);
        handler.RegisteReceiveTotalLengthBack(OnTotalLength);
        handler.RegisteCompleteBack(OnComplete);
    }

    public void StopDownload()
    {
        //handler在这里释放.
        m_State = DownloadState.STOP;
        KDownlaodFile.Instance().StopDownload(m_strURL);
    }

    public void Update()
    {
        KDownlaodFile.Instance().Update();
    }
    public void Quit()
    {
        UnRegisterEvent();
        KDownlaodFile.Instance().Quit();
    }


}


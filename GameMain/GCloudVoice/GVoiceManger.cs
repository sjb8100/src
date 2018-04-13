//*************************************************************************
//	创建日期:	2017-2-21 11:13
//	文件名称:	GVoiceManger.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	游戏语音 GVoice
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using gcloud_voice;

public enum JoinRoomEnum
{
    None,
    Team,
    Nation,
    FM,

}
public class GVoiceManger :SingletonMono<GVoiceManger>
{
    private IGCloudVoice m_voiceengine = null;
    string VoiceDataPath
    {
        get
        {
            if (Application.isEditor)
            {
                return Application.streamingAssetsPath + "/hxllvoice";
            }
            return Application.persistentDataPath + "/hxllvoice";
        }
    }

    private string m_filePath = "";
    public static AndroidJavaObject Current()
    {
        if (Application.platform == RuntimePlatform.Android)
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        else
            return null;
    }

    public static IGCloudVoice GVoiceEngine
    {
        get
        {
            return GCloudVoice.GetEngine();
        }
    }
    const string GVOICE_FILE = "gvoice.json";
    string m_strAppId;
    string m_strAppkey;
    Dictionary<string, int> m_dicVoiceLength = new Dictionary<string, int>();
    
    public GCloudVoiceMode m_GCloudVoiceMode = GCloudVoiceMode.Translation;
    bool m_bPlayRecordFile = false;
    public bool IsPlaying
    {
        get { return m_bPlayRecordFile; }
    }
    string  m_strLastJoinRoomName = "";
    GCloudVoiceRole m_lastRole = GCloudVoiceRole.AUDIENCE;
    public string JoinRoomName;
    public int MemberId;
    public JoinRoomEnum PreJoinRoomType = JoinRoomEnum.None;
    public JoinRoomEnum JoinRoomType = JoinRoomEnum.None;
    public bool IsOpenMic { get; set; }
    public bool IsOpenMicInRoom { get; set; }
    public bool IsOpenSpeeker { get; set; }
    string m_strDownLoadingFileid = "";

    Action<bool> m_onQuitRoom = null;

    //
    [SerializeField]
    int initRet = -1;
    int m_nJoinRoomErrorTime = 0;
    public void InitGvoice()
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            //Debug.LogError("OSXEditor 不初始化:");
            return;
        }
        m_voiceengine = GCloudVoice.GetEngine();
        if (!System.IO.Directory.Exists(VoiceDataPath))
        {
            System.IO.Directory.CreateDirectory(VoiceDataPath);
        }

        Engine.JsonNode root = Engine.RareJson.ParseJsonFile(GVOICE_FILE);
        if (root == null)
        {
            Engine.Utility.Log.Error("GVoiceManger 解析{0}文件失败!", GVOICE_FILE);
            return;
        }
        m_strAppId = root["appid"];
        m_strAppkey = root["appkey"];


        if (Application.platform == RuntimePlatform.Android)
        {
           // Debug.LogError("InitGvoice:");
            AndroidJavaClass jc = new AndroidJavaClass("com.hxll.gvoice.Gvoice");
            if (jc != null)
            {
                initRet = jc.CallStatic<int>("InitGVoiceEngine");
            }
            //Debug.LogError("InitGvoice ret :" + initRet);
        }

        m_voiceengine.OnApplyMessageKeyComplete += OnApplyMessageKeyCompleteHandler;
        m_voiceengine.OnUploadReccordFileComplete += OnUploadReccordFileCompleteHandler;
        m_voiceengine.OnDownloadRecordFileComplete += OnDownloadRecordFileCompleteHandler;
        m_voiceengine.OnPlayRecordFilComplete += OnPlayRecordFilCompletehandler;
        m_voiceengine.OnSpeechToText += OnSpeechToTextHandler;

        m_voiceengine.OnJoinRoomComplete += OnJoinRoomCompleteHandler;
        m_voiceengine.OnQuitRoomComplete += OnQuitRoomComplete;
        m_voiceengine.OnMemberVoice += OnMemberVoiceHandler;
    }

    public void SetAppInfo(string strUid)
    {
        //Debug.LogError("GVoiceManger -- SetAppInfo strUid :" + strUid);
        if (m_voiceengine != null && !string.IsNullOrEmpty(strUid))
        {
            int ret = m_voiceengine.SetAppInfo(m_strAppId, m_strAppkey, strUid);

          //  Debug.LogError("GVoiceManger -- SetAppInfo " + ret);
            ret = m_voiceengine.Init();
            //Debug.LogError("GVoiceManger -- Init " + ret);

            SetModel(GCloudVoiceMode.Translation,true);
            ApplyMessageKey(6000);
        }
    }

    public void SetModel(GCloudVoiceMode model,bool bforce = false)
    {
        if (m_voiceengine != null && (m_GCloudVoiceMode != model || bforce))
        {
            int ret = m_voiceengine.SetMode(model);
            Debug.LogError("GVoiceManger -- SetMode " + model + " ret :" + ret);
            if (ret == 0)
            {
                m_GCloudVoiceMode = model;
            }
        }
    }

    #region 离线语音

    /// <summary>
    /// 获取语音消息安全密钥key信息
    /// 当申请成功后会通过OnApplyMessageKeyComplete进行回调
    /// </summary>
    public void ApplyMessageKey(int msTimeout = 10000)
    {
        if (m_voiceengine != null)
        {
            m_voiceengine.ApplyMessageKey(msTimeout);
        }
    }

    public void StartRecording()
    {
        if (m_voiceengine != null)
        {
            CloseMic();
            CloseSpeaker();

            if (m_GCloudVoiceMode != GCloudVoiceMode.Translation)
            {
                SetModel(GCloudVoiceMode.Translation);
            }

            m_filePath = string.Format("{0}/{1}.dat", VoiceDataPath, DateTime.Now.ToFileTime());
            int ret = m_voiceengine.StartRecording(m_filePath);
            if (ret != 0)
            {
                GVoiceManger.Instance.SetRealTimeModel();
            }
            Debug.Log("GVoiceManger StartRecording ret :" + ret + "path:" + m_filePath);
        }
    }

    /// <summary>
    /// 取消录制
    /// </summary>
    public void StopRecording()
    {
        if (m_voiceengine == null)
        {
            return;
        }
        GCloudVoiceErr error = (GCloudVoiceErr)m_voiceengine.StopRecording();
        Debug.Log("GVoiceManger StopRecording error:" + error);
        if (error != GCloudVoiceErr.GCLOUD_VOICE_SUCC)
        {
            m_filePath = "";
            SetRealTimeModel();
        }
    }

    /// <summary>
    /// 当录制完成后，调用UploadRecordedFile将文件上传到GcloudVoice的服务器上，
    /// 该过程会通过OnUploadReccordFileComplete回调在上传成功的时候返还一个ShareFileID.该ID是这个文件的唯一标识符，
    /// 用于其他用户收听时候的下载。服务器需要对其进行管理和转发
    /// </summary>
    public void UploadRecordedFile()
    {
        if (!string.IsNullOrEmpty(m_filePath))
        {
            int[] bytes = new int[1];
            bytes[0] = 0;
            float[] seconds = new float[1];
            seconds[0] = 0;
            
            if (m_GCloudVoiceMode != GCloudVoiceMode.Translation)
            {
                SetModel(GCloudVoiceMode.Translation);
            }

            m_voiceengine.GetFileParam(m_filePath, bytes, seconds);
            Debug.Log("GVoiceManger UploadRecordedFile m_filePath:" + m_filePath + " seconds :" + seconds[0]);
            if (seconds[0] <= 0)
            {
                TipsManager.Instance.ShowTips("输入语音过短");
                SetRealTimeModel();
                return;
            }
 
            GCloudVoiceErr error = (GCloudVoiceErr)m_voiceengine.UploadRecordedFile(m_filePath, 6000);
            if (error != GCloudVoiceErr.GCLOUD_VOICE_SUCC)
            {
                TipsManager.Instance.ShowTips("语音发送成功");
                SetRealTimeModel();
            }
            Debug.Log("GVoiceManger UploadRecordedFile error:" + error);
        }
        else
        {
            SetRealTimeModel();
        }
    }

    
    public void DownloadRecordedFile(string strShareFileID,string strFileName)
    {
        if (!string.IsNullOrEmpty(strShareFileID))
        {
            if (m_voiceengine != null)
            {
                if (m_strDownLoadingFileid.Equals(strShareFileID))
                {
                    return;
                }
                m_strDownLoadingFileid = strShareFileID;
                string filePath = string.Format("{0}/{1}.amr", VoiceDataPath, strFileName);
                if (m_GCloudVoiceMode == GCloudVoiceMode.RealTime)
                {
                    CloseMic();
                    CloseSpeaker();
                    SetModel(GCloudVoiceMode.Translation);
                }
                GCloudVoiceErr error = (GCloudVoiceErr)m_voiceengine.DownloadRecordedFile(strShareFileID, filePath, 6000);
                if (error != 0)
                {
                    SetRealTimeModel();
                }
                Debug.Log("DownloadRecordedFile error:" + error);
            }
        }
    }

    public void StopPlayRecordFile()
    {
        if (m_voiceengine != null && m_bPlayRecordFile)
        {
            int ret = m_voiceengine.StopPlayFile();
            if (ret == 0)
            {
                m_bPlayRecordFile = false;
            }
        }
    }
    //回调
    //-----------------------------------------------------------------------------------------------
    /// <summary>
    /// 语音消息安全密钥key 回调
    /// </summary>
    /// <param name="code"></param>
    public void OnApplyMessageKeyCompleteHandler(IGCloudVoice.GCloudVoiceCompleteCode code)
    {
        Debug.Log(string.Format(" GVoiceManger OnApplyMessageKeyCompleteHandler {0}", code));

    }

    /// <summary>
    /// 上传语音回调
    /// </summary>
    /// <param name="code"></param>
    /// <param name="filepath"></param>
    /// <param name="fileid"></param>
    public void OnUploadReccordFileCompleteHandler(IGCloudVoice.GCloudVoiceCompleteCode code, string filepath, string fileid)
    {
        Debug.Log(string.Format(" GVoiceManger 上传语音回调 {0} / {1} / {2}", code, filepath, fileid));

        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_UPLOAD_RECORD_DONE)
        {
            if (m_GCloudVoiceMode != GCloudVoiceMode.Translation)
            {
                SetModel(GCloudVoiceMode.Translation);
            }
            int[] bytes = new int[1];
            bytes[0] = 0;
            float[] seconds = new float[1];
            seconds[0] = 0;
            m_voiceengine.GetFileParam(filepath, bytes, seconds);
            Debug.Log("GVoiceManger UploadRecordedFile seconds:" + seconds[0]);

            m_dicVoiceLength.Add(fileid, Mathf.CeilToInt(seconds[0]));

            int ret = m_voiceengine.SpeechToText(fileid);
            if (ret != 0)
            {
                SetRealTimeModel();
            }
        }
        else if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_UPLOAD_RECORD_ERROR)
        {
            SetRealTimeModel();
        }
    }
    /// <summary>
    /// 当游戏客户端需要收听其他人的录音时，
    /// 首先从服务器获取转发的ShareFileID，然后调用DownloadRecordedFile下载该语言文件，
    /// 下载结果通过OnDownloadRecordFileComplete回调来通知
    /// </summary>
    /// <param name="code"></param>
    /// <param name="filepath"></param>
    /// <param name="fileid"></param>
    public void OnDownloadRecordFileCompleteHandler(IGCloudVoice.GCloudVoiceCompleteCode code, string filepath, string fileid)
    {
        Debug.Log(string.Format(" GVoiceManger 收听语音回调 {0} / {1} / {2}", code, filepath, fileid));
        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_DOWNLOAD_RECORD_DONE)
        {
            if (m_voiceengine != null)
            {
                if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_DOWNLOAD_RECORD_DONE)
                {
                    m_bPlayRecordFile = true;
                    m_strDownLoadingFileid = "";

                    if (m_GCloudVoiceMode == GCloudVoiceMode.RealTime)
                    {
                        SetModel(GCloudVoiceMode.Translation);
                    }
                    int ret = m_voiceengine.PlayRecordedFile(filepath);
                    if (ret != 0)
                    {
                        SetRealTimeModel();
                    }
                }
            }
        }
        else
        {
            SetRealTimeModel();
        }
    }

    public void OnPlayRecordFilCompletehandler(IGCloudVoice.GCloudVoiceCompleteCode code, string filepath)
    {
        Debug.Log(string.Format(" GVoiceManger 播放语音 {0} / {1} ", code, filepath));
        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_PLAYFILE_DONE)
        {
            //string filePath = string.Format("{0}/{1}.amr", VoiceDataPath, strFileName);
            string path = filepath.Replace(VoiceDataPath + "/", "");
            path = path.Replace(".amr", "");
            m_bPlayRecordFile = false;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_VOICE_PLAY, new Client.stVoicePlay() { fileId = path });
        }
        SetRealTimeModel();
    }

    public void OnSpeechToTextHandler(IGCloudVoice.GCloudVoiceCompleteCode code, string fileID, string result)
    {
        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_STT_SUCC)
        {
            if (m_dicVoiceLength.ContainsKey(fileID))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    //string utf8_result = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(result));
                    Debug.LogError("GVoiceManger OnSpeechToTextHandler :" + result);

                    DataManager.Manager<ChatDataManager>().SendVoice(fileID, result, m_dicVoiceLength[fileID]);
                }
                else
                {
                    TipsManager.Instance.ShowTips("不能识别语音");
                    Debug.LogError("GVoiceManger不能识别语音");
                }
            }
            else
            {
                Debug.LogError("Not found fileid :" + fileID);
            }
        }
        SetRealTimeModel();
    }
    #endregion

    #region 实时语音

    public void JoinTeamRoom()
    {
        Engine.Utility.Log.Error("enter JoinTeamRoom");
        if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
        {
            GVoiceManger.Instance.SetModel(gcloud_voice.GCloudVoiceMode.RealTime);
            string strRoomName = DataManager.Manager<LoginDataManager>().CurSelectZoneID.ToString() + DataManager.Manager<TeamDataManager>().TeamId.ToString();
            GVoiceManger.Instance.JoinTeamRoom(strRoomName);
        }
        else
        {
            TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_haiweijiaruduiwuwufajinruliaotianshi);
        }
    }

    public void JoinClanRoom()
    {
        if (DataManager.Manager<ClanManger>().IsJoinClan == false)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Talk_ActualTime_haiweijiarushizuwufajinruliaotianshi);
            return;
        }
        bool manager = false;
        if (DataManager.Manager<ClanManger>().ClanInfo != null)
        {
            GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo((uint)MainPlayerHelper.GetPlayerID());

            if (clanInfo != null)
            {
                manager = clanInfo.duty != GameCmd.enumClanDuty.CD_Member;
            }
        }
        SetModel(gcloud_voice.GCloudVoiceMode.RealTime);

        
        string strRoomName = DataManager.Manager<LoginDataManager>().CurSelectZoneID.ToString() + DataManager.Manager<ClanManger>().ClanInfo.Id.ToString();
        JoinNationalRoom(strRoomName, GCloudVoiceRole.ANCHOR);
    }

    private void JoinTeamRoom(string strRoomName)
    {
        Engine.Utility.Log.Error("enter JoinTeamRoom name is {0}",strRoomName);
        if (m_voiceengine != null)
        {
            if (m_GCloudVoiceMode != GCloudVoiceMode.RealTime)
            {
                SetModel(GCloudVoiceMode.RealTime);
            }
            IsOpenMicInRoom = false;
            m_strLastJoinRoomName = "team" + strRoomName;
            Debug.Log("GVoiceManger JoinTeamRoom:" + m_strLastJoinRoomName);
            m_voiceengine.JoinTeamRoom(m_strLastJoinRoomName,6000);
        }
    }

    private void JoinNationalRoom(string strRoomName, GCloudVoiceRole role)
    {
        if (m_voiceengine != null)
        {
            if (m_GCloudVoiceMode != GCloudVoiceMode.RealTime)
            {
                SetModel(GCloudVoiceMode.RealTime);
            }
            IsOpenMicInRoom = false;
            m_strLastJoinRoomName = "national" + strRoomName;
            m_lastRole = role;
            Debug.Log("GVoiceManger JoinNationalRoom:" + m_strLastJoinRoomName + role);

            m_voiceengine.JoinNationalRoom(m_strLastJoinRoomName, role,6000);
        }
    }
    public void OpenMicInRoom()
    {
        IsOpenMicInRoom = true;
        OpenMic();
        GVoiceManger.Instance.SoundMute(false);
    }
    public void CloseMicInRoom()
    {
        IsOpenMicInRoom = false;
        CloseMic();
        GVoiceManger.Instance.SoundMute(true);
    }


    public void OpenMicInRoom_Small()
    {
        IsOpenMicInRoom = true;
        OpenMic();
        //GVoiceManger.Instance.SoundMute(false);

        Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
        if (au != null)
        {
            if (m_fSrcEffectVolume > 0f)
                au.SetEffectVolume(m_fSrcEffectVolume);
        }
    }

    public void CloseMicInRoom_Small()
    {
        IsOpenMicInRoom = false;
        CloseMic();
        //GVoiceManger.Instance.SoundMute(true);
        Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
        if (au != null)
        {
            m_fSrcEffectVolume = au.GetEffectVolume();
            float f = m_fSrcEffectVolume * 0.7f;
            au.SetEffectVolume(f);
        }

    }


    /// <summary>
    /// 加入房间成功后，就可以调用OpenMic()打开麦克风进行采集并发送到网络
    /// </summary>
    public void OpenMic()
    {
        if (m_voiceengine != null && m_GCloudVoiceMode == GCloudVoiceMode.RealTime && !IsOpenMic)
        {
            
            int ret = m_voiceengine.OpenMic();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_MICKSTATE,ret == 0);
            if (ret == 0)
            {
                IsOpenMic = true;
                IsOpenMicInRoom = true;
                Debug.Log("GVoiceManger 打开麦克风成功");
            }
            else
            {
                Debug.Log("GVoiceManger打开麦克风失败");
            }
        }
    }
    //说话的时候暂时把背景音乐关掉
    public void SoundMute(bool flag)
    {
        Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
        if (flag == true)
        {
            Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
            bool value= option.GetInt("Sound", "CheckSound", 1) == 1;
            au.Mute(value);
        }
        else
        {
            au.Mute(!flag);
        }
       

    }
    public void CloseMic(bool foce = false)
    {
        if (m_voiceengine != null && ((m_GCloudVoiceMode == GCloudVoiceMode.RealTime && IsOpenMic) || foce))
        {
            int ret = m_voiceengine.CloseMic();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_MICKSTATE, ret == 0 ? false : true);
            if (ret == 0)
            {
                IsOpenMic = false;
                Debug.Log("GVoiceManger 关闭麦克风成功");
            }
            else
            {
                Debug.Log("GVoiceManger关闭麦克风失败");
            }

        }
    }

    /// <summary>
    /// 调用OpenSpeaker()打开扬声器，开始接受网络上的音频流并自动进行播放。
    /// </summary>
    public void OpenSpeaker()
    {
        if (m_voiceengine != null && m_GCloudVoiceMode == GCloudVoiceMode.RealTime && !IsOpenSpeeker)
        {
            int ret = m_voiceengine.OpenSpeaker();
            if (ret == 0)
            {
                IsOpenSpeeker = true;
                Debug.Log("GVoiceManger打开扬声器成功");
            }
            else
            {
                Debug.Log("GVoiceManger打开扬声器失败");
            }
        }
    }
    
    public void CloseSpeaker(bool foce = false)
    {
        if (m_voiceengine != null && ((m_GCloudVoiceMode == GCloudVoiceMode.RealTime && IsOpenSpeeker) || foce))
        {
            int ret = m_voiceengine.CloseSpeaker();
            if (ret == 0)
            {
                IsOpenSpeeker = false;
                Debug.Log("GVoiceManger 关闭扬声器成功");
            }
            else
            {
                Debug.Log("GVoiceManger关闭扬声器失败");
            }
        }
    }

    public void QuitRoom(string strRoomName,Action<bool> onQuitRoom = null )
    {
        if (m_voiceengine != null)
        {

            CloseMic();
            CloseSpeaker();
            if (string.IsNullOrEmpty(strRoomName))
            {
                Debug.LogError("退出聊天室失败 房间名空");
                onQuitRoom(true);
                return;
            }
            m_onQuitRoom = onQuitRoom;
            m_voiceengine.QuitRoom(strRoomName, 6000);
        }
    }


    private float m_fSrcMusicVolume = 0.5f;
    private float m_fSrcEffectVolume = 0f;
    private void JoinRoomAudioUpdate()
    {
        Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
        if (au != null)
        {
            m_fSrcMusicVolume = au.GetMusicVolume();
            m_fSrcEffectVolume = au.GetEffectVolume();

            au.SetMusicVolume(0);

            float f = m_fSrcEffectVolume * 0.7f;
            au.SetEffectVolume(f);
        }
    }
    private void QuitRoomAudioUpdate()
    {
        Engine.IAudio au = Engine.RareEngine.Instance().GetAudio();
        if (au != null)
        {
            if (m_fSrcEffectVolume > 0f)
            {
                au.SetEffectVolume(m_fSrcEffectVolume);
            }
            au.SetMusicVolume(m_fSrcMusicVolume);
        }
    }
    /// 进入房间回调
    /// </summary>
    /// <param name="code"></param>
    /// <param name="roomName"></param>
    /// <param name="memberID"></param>
    /// 
    public void OnJoinRoomCompleteHandler(IGCloudVoice.GCloudVoiceCompleteCode code, string roomName, int memberID)
    {
        Debug.Log(string.Format("GVoiceManger OnJoinRoomCompleteHandler {0}/{1}/{2}", code, roomName, memberID));

        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_JOINROOM_SUCC)
        {
            if (roomName.StartsWith("team"))
            {
                JoinRoomAudioUpdate();
            }
            else if (roomName.StartsWith("national"))
            {

            }
            else
            { 
                JoinRoomAudioUpdate();
            }

        }


        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_JOINROOM_SUCC)
        {
            m_nJoinRoomErrorTime = 0;
            this.JoinRoomName = roomName;
            this.MemberId = memberID;
            GameCmd.stVoiceChatOperChatUserCmd_CS cmd = new GameCmd.stVoiceChatOperChatUserCmd_CS();
            if (roomName.StartsWith("team"))
            {
                cmd.chtype = GameCmd.VoChatType.VoChatType_Team;
                JoinRoomType = JoinRoomEnum.Team;
            }else if (roomName.StartsWith("national"))
            {
                cmd.chtype = GameCmd.VoChatType.VoChatType_Clan;
                JoinRoomType = JoinRoomEnum.Nation;
            }
            
            cmd.posid = (uint)memberID;
            cmd.optype = GameCmd.VoChatOpType.VoChatOpType_Add;
            NetService.Instance.Send(cmd);

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_JOINROOM, new Client.stVoiceJoinRoom() {  succ = true, memberid = memberID, name = roomName});

        }
        else if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_JOINROOM_TIMEOUT || code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_JOINROOM_SVR_ERR)
        {
            if (!string.IsNullOrEmpty(m_strLastJoinRoomName))
            {
                m_nJoinRoomErrorTime++;
                if (m_strLastJoinRoomName.StartsWith("team"))
                {
                    if (m_nJoinRoomErrorTime >= 3)
                    {
                        TipsManager.Instance.ShowTips("加入队伍聊天室失败重新加入");
                    }
                    JoinTeamRoom();
                }else if (m_strLastJoinRoomName.StartsWith("national"))
                {
                    if (m_nJoinRoomErrorTime >= 3)
                    {
                        TipsManager.Instance.ShowTips("加入氏族聊天室失败重新加入");
                    }
                    JoinClanRoom();
                }
                else
                {
                    TipsManager.Instance.ShowTips("加入房间失败 房间名字未知");
                }
            }
            else
            {
                TipsManager.Instance.ShowTips("加入房间失败 房间名字为空");
            }
        }
    }

    public void OnQuitRoomComplete(IGCloudVoice.GCloudVoiceCompleteCode code, string roomName, int memberID)
    {
        Debug.Log(string.Format("GVoiceManger OnQuitRoomComplete {0}/{1}/{2}", code, roomName, memberID));
        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_QUITROOM_SUCC)
        {

            GameCmd.stVoiceChatOperChatUserCmd_CS cmd = new GameCmd.stVoiceChatOperChatUserCmd_CS();
            if (roomName.StartsWith("team"))
            {
                QuitRoomAudioUpdate();

                cmd.chtype = GameCmd.VoChatType.VoChatType_Team;
                //退出游戏的时候退出聊天室 回调没有回来 重新登入游戏 回调回再次回来
                if (JoinRoomType != JoinRoomEnum.None)
                {
                    PreJoinRoomType = JoinRoomEnum.Team;
                }
                if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
                {
                    
                    cmd.posid = (uint)memberID;
                    cmd.optype = GameCmd.VoChatOpType.VoChatOpType_Leave;
                    NetService.Instance.Send(cmd);
                }
            }
            else if (roomName.StartsWith("national"))
            {
                cmd.chtype = GameCmd.VoChatType.VoChatType_Clan;
                if (JoinRoomType != JoinRoomEnum.None)
                {
                    PreJoinRoomType = JoinRoomEnum.Nation;
                }
                
                if (DataManager.Manager<ClanManger>().IsJoinClan)
                {


                    cmd.posid = (uint)memberID;
                    cmd.optype = GameCmd.VoChatOpType.VoChatOpType_Leave;
                    NetService.Instance.Send(cmd);
                }
            }
            else 
            {
                QuitRoomAudioUpdate();
            }

            JoinRoomType = JoinRoomEnum.None;
            JoinRoomName = "";
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_LEVELROOM, null);

            if (m_onQuitRoom != null)
            {
                m_onQuitRoom(true);
            }
        }
        else
        {
            QuitRoom(roomName);
        }
        IsOpenMic = false;
        m_strLastJoinRoomName = "";
    }

    public void OnMemberVoiceHandler(int[] members, int count) 
    {
      //  string  s_logstr = "\r\ncount:" + count;
        for (int i = 0; i < count && (i + 1) < members.Length; ++i)
        {
//             s_logstr +="\r\nmemberid: " + members[i]+"  state:" + members[i+1];
//             ++i;
            if (members[i + 1] == 1)
            {
                string name = "";
                if (m_dicRoleName.TryGetValue(members[i], out name))
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_SPEEKERNOW, name);
                    break;
                }
            }
        }
    }

    public void SetRealTimeModel()
    {
        if (JoinRoomType != JoinRoomEnum.None)
        {
            SetModel(GCloudVoiceMode.RealTime);
            if (IsOpenMicInRoom)
            {
                OpenMic();
                GVoiceManger.Instance.SoundMute(false);
            }
            else
            {
                GVoiceManger.Instance.SoundMute(true);
            }
           
            OpenSpeaker();
        }
    }
    #endregion
    void Update()
    {
        if (m_voiceengine != null)
        {
            m_voiceengine.Poll();
        }
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if (m_voiceengine == null)
            {
                return;
            }
            m_voiceengine.Pause();
        }
        else
        {
            if (m_voiceengine == null)
            {
                return;
            }
            m_voiceengine.Resume();
        }
    }

    public void OnQuit()
    {
        if (m_voiceengine != null && !string.IsNullOrEmpty(JoinRoomName))
        {
            GameCmd.stVoiceChatOperChatUserCmd_CS cmd = new GameCmd.stVoiceChatOperChatUserCmd_CS();
            if (JoinRoomName.StartsWith("team"))
            {
                cmd.chtype = GameCmd.VoChatType.VoChatType_Team;
            }
            else if (JoinRoomName.StartsWith("national"))
            {
                cmd.chtype = GameCmd.VoChatType.VoChatType_Clan;
            }
            cmd.posid = (uint)this.MemberId;
            cmd.optype = GameCmd.VoChatOpType.VoChatOpType_Leave;
            NetService.Instance.Send(cmd);

            QuitRoom(JoinRoomName);
        }
    }

    //-----------------------------------------------------------------------------------------------

    Dictionary<int, string> m_dicRoleName = new Dictionary<int, string>();

    public void AddRole(int pos,string name)
    {
        if (!m_dicRoleName.ContainsKey(pos))
        {
            m_dicRoleName.Add(pos, name);
        }
    }

    public void RemoveRole(int pos)
    {
        if (m_dicRoleName.ContainsKey(pos))
        {
            m_dicRoleName.Remove(pos);
        }
    }

    public void ClearRole()
    {
        m_dicRoleName.Clear();
    }
}
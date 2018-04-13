using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Client;
using Common;


partial class Protocol
{
    [Execute]
    public void OnOpenFishing(stRequestOpenFishingPropertyUserCmd_CS msg)
    {
        DataManager.Manager<FishingManager>().OnOpenFishing(msg);
    }

    [Execute]
    public void OnCloseFishing(stClosetFishingPropertyUserCmd_S msg)
    {
        DataManager.Manager<FishingManager>().OnCloseFishing(msg);
    }

    [Execute]
    public void OnGetOneFish(stGetOneFihsPropertyUserCmd_S msg)
    {
        DataManager.Manager<FishingManager>().OnGetOneFish(msg);
    }

    [Execute]
    public void OnSucessFishing(stSucessFishingPropertyUserCmd_S msg)
    {
        DataManager.Manager<FishingManager>().OnSucessFishing(msg);
    }

    [Execute]
    public void OnFishRankingList(stRequestFishRankingListRelationUserCmd_CS msg)
    {
        DataManager.Manager<FishingManager>().OnFishRankingList(msg);
    }

    [Execute]
    public void OnFishingToNine(stFishToNinePropertyUserCmd_S msg)
    {
        DataManager.Manager<FishingManager>().OnFishingAniToNine(msg);
    }

    [Execute]
    public void OnFishingStateToNine(stFishStateToNinePropertyUserCmd_S msg)
    {
        DataManager.Manager<FishingManager>().OnFishingStateToNine(msg);
    }

}


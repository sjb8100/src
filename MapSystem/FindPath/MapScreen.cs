////using Cmd;
////using Cmd.MapScreenUserCmd;
//using UnityEngine;
//using System.Linq;
////using Cmd.MagicUserCmd;
////using Cmd.DataUserCmd;

//public enum TileType// : ushort // Unity not support
//{
//    #region 服务器使用
//    /// <summary>
//    /// 固体阻挡点
//    /// </summary>
//    TILE_SOLID_BLOCK = 0x01,
//    /// <summary>
//    /// 飞行阻挡点
//    /// </summary>
//    TILE_FLY_BLOCK = 0x02,
//    /// <summary>
//    /// 人物或者Npc阻挡
//    /// </summary>
//    TILE_ENTRY_BLOCK = 0x04,
//    /// <summary>
//    /// 道具阻挡
//    /// </summary>
//    TILE_OBJECT_BLOCK = 0x08,
//    #endregion

//    #region 非服务器使用
//    /// <summary>
//    /// 陆地交通运输
//    /// </summary>
//    TILE_ROAD_TRAFFIC_BLOCK = 0x100,
//    /// <summary>
//    /// 水上交通运输
//    /// </summary>
//    TILE_NAVY_TRAFFIC_BLOCK = 0x200,
//    /// <summary>
//    /// 跳跃
//    /// </summary>
//    TILE_JUMP_BLOCK = 0x400,
//    /// <summary>
//    /// 2D遮罩
//    /// </summary>
//    TILE_GROUND_BLOCK = 0x800,
//    #endregion


//    TileType_None = 0,
//    TileType_Walk_BLOCK = TILE_SOLID_BLOCK | TILE_FLY_BLOCK,
//}


//class MapScreen : MonoBehaviour
//{
//    /// <summary>
//    /// 地图数据打包发送
//    /// </summary>
//    /// <param name="cmd"></param>
//    [Execute]
//    public static void Execute(GameCmd.stMapDataMapScreenUserCmd cmd)
//    {
//        //foreach (var npcData in cmd.npclist)
//        //{
//        //    Npc.AddOneNpc(npcData.ToCmd());
//        //}

//        //foreach (var roleData in cmd.userlist)
//        //{
//        //    // 若是主角，则更新主角的坐标和朝向
//        //    if (UserData.IsMainRoleID(roleData.mapuserdata.userdata.dwUserID))
//        //    {
//        //        ProtoBuf.IExtensible send;
//        //        if (cmd.Type == SceneEntryType.SceneEntry_NPC)
//        //            send = new GameCmd.stReturnObjectPosMagicUserCmd()
//        //            {
//        //                userpos = new GameCmd.stEntryPosition() { x = roleData.x, y = roleData.y, byDir = roleData.byDir },
//        //            };
//        //        else
//        //            send = new GameCmd.stReturnObjectPosMagicUserCmd()
//        //            {
//        //                userpos = new GameCmd.stEntryPosition() { x = roleData.x, y = roleData.y, byDir = roleData.byDir },
//        //            };
//        //        Net.Instance.SendToMe(send);
//        //        continue;
//        //    }

//        //    Role.AddOneRole(roleData.ToCmd());
//        //}
//    }

//    /// <summary>
//    /// 设置场景物件生命
//    /// </summary>
//    /// <param name="cmd"></param>
//    [Execute]
//    public static void Execute(stSetHPDataUserCmd cmd)
//    {
//        //switch (cmd.Type)
//        //{
//        //    case SceneEntryType.SceneEntry_NPC:
//        //        {
//        //            Npc npc = Npc.All[cmd.id];

//        //            if (npc == null)
//        //                return;

//        //            npc.UpdateHP(cmd);

//        //        }
//        //        break;
//        //    case SceneEntryType.SceneEntry_Player:
//        //        {
//        //            //普通角色
//        //            var role = Role.All[cmd.id];
//        //            if (role == null)
//        //                return;
//        //            role.UpdateHP(cmd);

//        //        }
//        //        break;
//        //    default:
//        //        break;
//        //}
//    }

//    #region 状态消息处理
//    /// <summary>
//    ///  状态信息下行消息
//    /// </summary>
//    /// <param name="cmd"></param>
//    [Execute]
//    public static void Execute(GameCmd.stSendStateIcoListMagicUserCmd cmd)
//    {
//        //switch (cmd.byType)
//        //{
//        //    case GameCmd.SceneEntryType.SceneEntry_NPC:
//        //        {
//        //            var npc = Npc.All[cmd.dwTempID];
//        //            if (npc == null)
//        //                return;

//        //            foreach (var item in cmd.statelist)
//        //            {
//        //                npc.buffManager.Add(item);
//        //            }
//        //        }
//        //        break;
//        //    case GameCmd.SceneEntryType.SceneEntry_Player:
//        //        {
//        //            var role = Role.All[cmd.dwTempID];
//        //            if (role == null)
//        //            {
//        //                Debug.Log("当前角色不存在");
//        //                return;
//        //            }

//        //            foreach (var item in cmd.statelist)
//        //            {
//        //                role.buffManager.Add(item);
//        //            }
//        //        }
//        //        break;
//        //    default:
//        //        break;
//        //}
//    }

//    /// <summary>
//    /// 取消某个对象身上的状态图标
//    /// </summary>
//    /// <param name="cmd"></param>
//    [Execute]
//    public static void Execute(GameCmd.stCancelStateIcoListMagicUserCmd cmd)
//    {
//        //switch ((Cmd.SceneEntryType)cmd.byType)
//        //{
//        //    case SceneEntryType.SceneEntry_NPC:
//        //        {
//        //            var npc = Npc.All[cmd.dwTempID];
//        //            if (npc == null)
//        //                return;
//        //            npc.buffManager.Remove(cmd.wdState, cmd.dwStateThisID);
//        //        }
//        //        break;
//        //    case SceneEntryType.SceneEntry_Player:
//        //        {
//        //            var role = Role.All[cmd.dwTempID];
//        //            if (role == null)
//        //                return;
//        //            role.buffManager.Remove(cmd.wdState, cmd.dwStateThisID);
//        //        }
//        //        break;
//        //    default:
//        //        break;
//        //}
//    }

//    #endregion
//    /// <summary>
//    /// 地图掉落
//    /// </summary>
//    /// <param name="cmd"></param>
//    [Execute]
//    public static void Execute(GameCmd.stAddMapObjectMapScreenUserCmd cmd)
//    {
//        //SceneItem.AddOneItem(cmd.data.ToCmd());
//    }

//    /// <summary>
//    /// 删除地图道具
//    /// </summary>
//    /// <param name="cmd"></param>
//    [Execute]
//    public static void Execute(GameCmd.stRemoveMapObjectMapScreenUserCmd cmd)
//    {
//        //SceneItem.removeOneItem(cmd.qwThisID);
//    }

//    /// <summary>
//    /// 角色进地图的第一个消息
//    /// </summary>
//    /// <param name="cmd"></param>
//    /// <returns></returns>
//    [Execute]
//    public static void Execute(GameCmd.stFirstMainUserPosMapScreenUserCmd cmd)
//    {
//        //UserData.SetMsgTypeToEnterScene(); // 加载地图时，阻塞消息
//        //resetSceneData();

//        //UserData.MapID = cmd.mapid;
//        //UserData.CurrentCountryID = cmd.countryid;
//        //UserData.Pos = new MapVector2(cmd.x, cmd.y);

//        //yield return Application.LoadLevelAsync("BattleScene");

//        //var mapName = UserData.Map.filename;
//        //while (BattleScene.Instance == null)
//        //{
//        //    yield return new WaitForEndOfFrame();
//        //}
//        //LoadingShow.Instance.currentMapName = mapName;
//        //LoadingShow.Instance.loadingValue = 0.4f;
//        //// 此处等2帧，要不然场景会穿帮
//        //yield return new WaitForEndOfFrame();
//        //yield return new WaitForEndOfFrame();
//        //var map = UserData.Map;
//        //Debug.Log(string.Format("LoadMap: {0}", map));
//        //IsMapOk = BattleScene.Instance.LoadMap(map.filename_client);
//        ////BattleScene.Instance.LoadAttackEffect();

//        //LoadingShow.Instance.loadingValue = 0.8f;
//        //yield return 1;

//        //UserData.SetMsgTypeToNormal();
//    }
//}


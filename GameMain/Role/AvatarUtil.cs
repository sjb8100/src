using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cmd;

namespace Client
{

    public class SuitInfo
    {
        public SuitPos pos = SuitPos.None;  // 部位
        public string modelPath = "";       // 模型路径
        public string locatorName = "";     // 挂接点
    }

    public class AvatarUtil
    {
        #region 通用功能函数
        /// <summary>
        /// 获取模型信息
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="pos">装备部位</param>
        /// <param name="profession">职业</param>
        /// <param name="faceID"></param>
        /// <param name="ifJudgeSex">是否需要职业</param>
        /// <returns></returns>
        public static string GetModelPath(ushort itemID, Client.EquipPos pos, byte profession, byte faceID, bool ifJudgeSex = true)
        {
            var modelPath = "";
            //var tbl = Table.Query<table.ItemDatabase>().FirstOrDefault(i => i.dwID == itemID);
            //if (tbl == null) return modelPath;
            var table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(itemID);
            if (table_data == null)
            {
                int nSex = 1;
                if (ifJudgeSex)
                {
                    //modelPath = GameUtil.FaceToSex(faceID) == GameCmd.enmCharSex.MALE ? tbl.wdMaleModelSetID.ToString() : tbl.wdFemaleModelSetID.ToString();
                    nSex = (int)GameUtil.FaceToSex(faceID);
                }
                // 根据职业、性别和部位去查找
                var database = table.SelectRoleDataBase.Where((GameCmd.enumProfession)profession, (GameCmd.enmCharSex)nSex);
                if (database != null)
                {
                    //Engine.Utility.Log.Error("BuildPlayerPropList:job{0}或者sex{1}数据非法!", data.mapnpcdata.npcdata.job, data.mapnpcdata.npcdata.sex);
                    //return;
                    switch (pos)
                    {
                        case Client.EquipPos.EquipPos_Body:
                            {
                                var sult_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)database.bodyPathID);
                                if (sult_data != null)
                                {
                                    table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(sult_data.resid);
                                    if (db != null)
                                    {
                                        modelPath = db.strPath;
                                    }
                                }
                                break;
                            }
                        case Client.EquipPos.EquipPos_Weapon:
                            {
                                var sult_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)database.weaponPath);
                                if (sult_data != null)
                                {
                                    table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(sult_data.resid);
                                    if (db != null)
                                    {
                                        modelPath = db.strPath;
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            else
            {
                table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.resid);
                if (db != null)
                {
                    modelPath = db.strPath;
                }
            }

            return modelPath;
        }

        //-------------------------------------------------------------------------------------------------------
        public static string GetModelPath(uint uSuitID, Client.EquipPos pos, byte profession, byte sex)
        {
            var modelPath = "";
            var table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(uSuitID);
            if (table_data == null)
            {
                int nSex = sex;
                // 根据职业、性别和部位去查找
                var database = table.SelectRoleDataBase.Where((GameCmd.enumProfession)profession, (GameCmd.enmCharSex)nSex);
                if (database != null)
                {
                    switch (pos)
                    {
                        case Client.EquipPos.EquipPos_Body:
                            {
                                var sult_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)database.bodyPathID);
                                if (sult_data != null)
                                {
                                    table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(sult_data.resid);
                                    if (db != null)
                                    {
                                        modelPath = db.strPath;
                                    }
                                }
                                break;
                            }
                        case Client.EquipPos.EquipPos_Weapon:
                            {
                                var sult_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)database.weaponPath);
                                if (sult_data != null)
                                {
                                    table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(sult_data.resid);
                                    if (db != null)
                                    {
                                        modelPath = db.strPath;
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            else
            {
                table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.resid);
                if (db != null)
                {
                    modelPath = db.strPath;
                }
            }

            return modelPath;
        }

        public static table.SuitDataBase GetSuitID(uint uSuitID, Client.EquipPos pos, byte profession, byte sex)
        {
            var table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(uSuitID);
            if (table_data == null)
            {
                int nSex = sex;
                // 根据职业、性别和部位去查找
                var database = table.SelectRoleDataBase.Where((GameCmd.enumProfession)profession, (GameCmd.enmCharSex)nSex);
                if (database != null)
                {
                    switch (pos)
                    {
                        case Client.EquipPos.EquipPos_Body:
                            {
                                table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)database.bodyPathID);
                                break;
                            }
                        case Client.EquipPos.EquipPos_Weapon:
                            {
                                table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)database.weaponPath);
                                break;
                            }
                    }
                }
            }

            return table_data;
        }

        public static SuitInfo GetResPathBySuitID(uint uSuitID, Client.EquipPos pos, uint profession, uint sex,uint skillStatu)
        {
            SuitInfo info = new SuitInfo();
            info.pos = (SuitPos)pos;
            var table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(uSuitID);
            if (table_data == null)
            {
                GameCmd.enmCharSex nSex = (GameCmd.enmCharSex)GameUtil.FaceToSex(sex);
                // 根据职业、性别和部位去查找
                var database = table.SelectRoleDataBase.Where((GameCmd.enumProfession)profession, (GameCmd.enmCharSex)nSex);
                if (database != null)
                {
                    switch (pos)
                    {
                        case Client.EquipPos.EquipPos_Body:
                            {
                                table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)database.bodyPathID);
                                break;
                            }
                        case Client.EquipPos.EquipPos_Weapon:
                            {
                                table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)database.weaponPath);
                                break;
                            }
                    }

                }
            }
            if(table_data == null)
            {
                return info;
            }
            uint viewID = skillStatu == 0?table_data.viewresid:table_data.viewresid2;

            table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(viewID); // 使用观察路径
            if (db != null)
            {
                info.modelPath = db.strPath;
                info.locatorName = table_data.locator_name;

            }

            return info;
        }
        #endregion

        //public static GameObject Create(GameCmd.SelectUserInfo info)
        //{
        //    var bodyPath = GetModelPath((ushort)info.suite, (byte)info.face);
        //    var weaponPath = GetModelPath((ushort)info.weapon, (byte)info.face, false);
        //    switch ((enumProfession)info.type)
        //    {
        //        case enumProfession.Profession_Soldier:
        //            weaponPath = "Prefabs/Models/Weapon/ZS/Wepon_Zs10";
        //            break;
        //        case enumProfession.Profession_Gunman:
        //            weaponPath = "Prefabs/Models/Weapon/GS/Wepon_Gs10";
        //            break;
        //        case enumProfession.Profession_Freeman:
        //            weaponPath = "Prefabs/Models/Weapon/FS/Wepon_Fs10";
        //            break;
        //        case enumProfession.Profession_Doctor:
        //            weaponPath = "Prefabs/Models/Weapon/MS/Wepon_Ms10";
        //            break;
        //        default:
        //            break;
        //    }
        //    var wingPath = GetModelPath((ushort)info.wing, (byte)info.face, false);
        //    var tbl = table.SelectRoleDataBase.Where(info.type, (GameCmd.enmCharSex)GameUtil.FaceToSex((byte)info.face));
        //    var avatarRole = CreateAvatarByPath(info.type, bodyPath, weaponPath, wingPath, tbl.bodyPath);
        //    return avatarRole;
        //}

        public static bool CreateAvater(ref Client.Avater refAvater, GameCmd.SelectUserInfo info, Transform parent, int nLayer, Action<object> callback, object param = null)
        {
            List<SuitInfo> lstSuit = new List<SuitInfo>();
            lstSuit.Add(GetResPathBySuitID(info.sclothes, Client.EquipPos.EquipPos_Body, (uint)info.type, info.face,info.skillstatus));
            lstSuit.Add(GetResPathBySuitID(info.sqibing, Client.EquipPos.EquipPos_Weapon, (uint)info.type, info.face, info.skillstatus));
            lstSuit.Add(GetResPathBySuitID(info.sback, Client.EquipPos.EquipPos_Wing, (uint)info.type, info.face, info.skillstatus));
            lstSuit.Add(GetResPathBySuitID(info.sface, Client.EquipPos.EquipPos_Face, (uint)info.type, info.face, info.skillstatus));

            refAvater = new Client.Avater();
            return refAvater.CreateAvatar(parent.gameObject, lstSuit, nLayer, callback, param);

        }

        public static bool CreateAvater(ref Client.Avater refAvater, string strBodyPath, Transform parent, int nLayer, Action<object> callback, object param = null)
        {
            var bodyPath = strBodyPath;


            List<SuitInfo> lstSuit = new List<SuitInfo>();
            SuitInfo info = new SuitInfo();
            info.modelPath = bodyPath;
            info.pos = SuitPos.Cloth;
            lstSuit.Add(info);
       
            refAvater = new Client.Avater();
            return refAvater.CreateAvatar(parent.gameObject, lstSuit, nLayer, callback, param);

        }

        /// <summary>
        /// 从选人界面表创建模型。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //public static GameObject Create(table.SelectRoleDataBase item)
        //{
        //    if (string.IsNullOrEmpty(item.bodyPath))
        //    {
        //        Debug.LogWarning("选人界面表暂未配置！");
        //        return null;
        //    }
        //    // 选人界面表提供的是裸模，应仅显示body
        //    return AvatarHandle.CreateAvatarByPath(item.Profession, item.bodyPath, null, null, item.bodyPathRoleShowcase);
        //}
    }
}

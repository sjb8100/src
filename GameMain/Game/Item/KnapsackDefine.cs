using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KnapsackDefine
{
    #region define
    public class LocalUnlockInfo
    {
        #region property
        //背包类型
        private GameCmd.PACKAGETYPE m_pType = GameCmd.PACKAGETYPE.PACKAGETYPE_NONE;
        private GameCmd.PACKAGETYPE PType
        {
            get
            {
                return m_pType;
            }
        }

        //是否解锁
        public bool IsUnlock
        {
            get
            {
                //随身包裹，装备包裹默认开启
                if (PType == GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN 
                    || PType == GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP)
                {
                    return true;
                }
                if (null != m_ServerUnlockInfo)
                {
                    return (m_ServerUnlockInfo.active != 0); 
                }
                return false;
            }set
            {
                if (null != m_ServerUnlockInfo)
                {
                    m_ServerUnlockInfo.active = (uint)((value) ? 1 : 0);
                }
            }
        }

        //服务端信息
        private GameCmd.UnlockInfo m_ServerUnlockInfo;
        //本地表格
        private table.UnlockStoreDataBase m_unlockDatabase = null;
        public table.UnlockStoreDataBase UnlockDB
        {
            get
            {
                return m_unlockDatabase;
            }
        }

        //背包总格子数量
        public int TotalNum
        {
            get
            {
                return (null != UnlockDB) ? (int)UnlockDB.maxNum:0;
            }
        }

        //解锁数量
        public int UnlockNum
        {
            get
            {
                return (IsUnlock ? (int)m_ServerUnlockInfo.num : 0);
            }
            set
            {
                if (IsUnlock && value >=0)
                {
                    m_ServerUnlockInfo.num = (uint)value;
                }
            }
        }

        //金钱解锁
        public int MoneyUnlockNum
        {
            get
            {
                return (IsUnlock) ? (int)m_ServerUnlockInfo.moneyUnlockedNum : 0;
            }
        }

        //等级解锁
        public int LvUnlockNum
        {
            get
            {
                return (IsUnlock) ? (int)m_ServerUnlockInfo.levelUnlockedNum : 0;
            }
        }
        #endregion

        #region structmethod
        private void UpdateLockInfo(GameCmd.PACKAGETYPE ptype
            ,table.UnlockStoreDataBase unlockdb
            ,GameCmd.UnlockInfo serverUnlockInfo)
        {
            this.m_unlockDatabase = unlockdb;
            m_pType = ptype;
            m_ServerUnlockInfo = serverUnlockInfo;
        }
        private LocalUnlockInfo()
        {

        }
        #endregion

        #region Create
        /// <summary>
        /// 创建本地仓库解锁信息
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="serverUnlockInfo"></param>
        /// <returns></returns>
        public static LocalUnlockInfo Create(GameCmd.PACKAGETYPE pType,GameCmd.UnlockInfo serverUnlockInfo)
        {
            table.UnlockStoreDataBase unlockDatabase 
                = GameTableManager.Instance.GetTableItem<table.UnlockStoreDataBase>((uint)pType, 0);
            if (null == unlockDatabase)
            {
                Engine.Utility.Log.Error("Get Package unlock info failed ,pt={0}", pType);
                return null ;
            }
            LocalUnlockInfo info = new LocalUnlockInfo();
            info.UpdateLockInfo(pType, unlockDatabase, serverUnlockInfo);
            return info;
        }
        #endregion
    }
    #endregion
    /// <summary>
    /// 获取背包标识
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public static byte[] GetPackageMask(GameCmd.PACKAGETYPE pType)
    {
        int intValue = (int)pType;
        int size = intValue / 8 + (((intValue % 8) > 0) ? 1 : 0);
        if (size == 0)
            size = 1;
        byte[] maskByteArray = new byte[size];
        maskByteArray[intValue / 8] |= (byte)(0xff & (1 << (intValue % 8)));
        return maskByteArray; 
    }

    /// <summary>
    /// 是否背包标识包含当前背包
    /// </summary>
    /// <param name="pMask"></param>
    /// <param name="pType"></param>
    /// <returns></returns>
    public static bool IsPackageMaskContainType(byte[] pMask,GameCmd.PACKAGETYPE pType)
    {
        if (null == pMask && pMask.Length > 0)
            return false;
        int intValue = (int)pType;
        int size = intValue / 8 + (((intValue % 8) > 0) ? 1 : 0);
        if (size == 0)
            size = 1;
        if (pMask.Length >= size)
        {
            return 0 != (pMask[intValue / 8] & (0xff & (1 << intValue % 8)));
        }
        return false;
    }
}
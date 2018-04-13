using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    class Tree : HomeEntityBase,ITree
    {
        private Engine.Utility.SecurityInt[] m_TreeProp = new Engine.Utility.SecurityInt[TreeProp.End - TreeProp.Begin];

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)TreeProp.Begin || nPropID >= (int)TreeProp.End)
            {
                return;
            }

            m_TreeProp[nPropID - (int)TreeProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)TreeProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)TreeProp.End)
            {
                return m_TreeProp[nPropID - (int)TreeProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < TreeProp.End - TreeProp.Begin; ++i)
            {
                m_TreeProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Tree;
        }
        private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        {
            if ( m_EntityView == null )
            {
                return;
            }

            m_EntityView.SetRenderObj( obj );

            int nID = GetProp( (int)EntityProp.BaseID );
            var table_data = GameTableManager.Instance.GetTableItem<table.HomeLandViewDatabase>( (uint)nID , GetProp( (int)HomeProp.State ) );
            if ( table_data == null )
            {
                Engine.Utility.Log.Error( "不存在这个对象: " + nID );
                return;
            }

            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>( (uint)table_data.dwModelID );
            if ( resDB == null )
            {
                Engine.Utility.Log.Error( "Homeland:找不到Homeland模型资源路径配置{0}" , table_data.dwModelID );
                return;
            }

            //string strPartName = "main";
            //string strResName = resDB.strPath;
           // m_EntityView.ChangePart( ref strPartName , ref strResName );

            // 根据全局配置设置阴影
            int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt( "Render" , "shadow" , 1 );
            m_EntityView.SetShadowLevel( nShadowLevel );
        }
        public override bool CreateEntityView(EntityCreateData data)
        {
            base.CreateEntityView( data );
            //if (GetEntityType() != EntityType.EntityType_NPC)
            //{
            //    Engine.Utility.Log.Error("NPC:CreateEntityView failed:{0}", GetName());
            //    return false;
            //}

            int nID = GetProp( (int)EntityProp.BaseID );
            string strResName = "";
            if ( nID == 0 )
            {
                // 则使用默认数据
                // strResName = data.strName;
            }
            else
            {
                var table_data = GameTableManager.Instance.GetTableItem<table.HomeLandViewDatabase>( (uint)nID , GetProp( (int)HomeProp.State ) );
                if ( table_data == null )
                {
                    Engine.Utility.Log.Error( "不存在这个对象: " + nID );
                    return false;
                }

                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>( (uint)table_data.dwModelID );
                if ( resDB == null )
                {
                    Engine.Utility.Log.Error( "Homeland:找不到Homeland模型资源路径配置{0}" , table_data.dwModelID );
                    return false;
                }

                strResName = resDB.strPath;
            }

            SetName( data.strName );

            // TODO: 根据配置表读取数据               
            if ( !m_EntityView.Create( ref strResName , OnCreateRenderObj ) )
            {
                Engine.Utility.Log.Error( "CreateEntityView failed:{0}" , strResName );
            }

            return false;
        }
    }
}

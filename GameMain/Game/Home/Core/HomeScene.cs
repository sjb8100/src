using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;

// 家园场景
class HomeScene : Singleton<HomeScene> , IClickSink
{
    private static uint HomeSceneID = 152;  // 家园场景

    // 实体ID
    private uint m_uEntityID = 0;
    // 小动物移动管理
    private AnimalManager m_AnimalManager = new AnimalManager();

    private Dictionary<int , IEntity> m_dicEntity = new Dictionary<int , IEntity>();
    private Dictionary<int , IPuppet> m_dicPuppet = new Dictionary<int , IPuppet>();

    public void Init()
    {
    }

    public void Close()
    {
    }
    // 进入场景
    public void Enter()
    {
        HomeSceneID = GameTableManager.Instance.GetGlobalConfig<uint>( "HomeSceneID" );
        IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
        if ( mapSys == null )
        {
            return;
        }

        mapSys.EnterMap(HomeSceneID, Vector3.zero);

        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if ( cs == null )
        {
            return;
        }

        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer!=null)
        {
            mainPlayer.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
        }
        cs.GetActiveCtrl().SetClickSink( this );

        m_AnimalManager.Init();
    }

    // 离开场景
    public void Leave()
    {
        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if ( cs == null )
        {
            return;
        }

        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            mainPlayer.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
            cs.GetActiveCtrl().SetHost(mainPlayer);
            mainPlayer.SendMessage( EntityMessage.EntityCommand_SetVisible , true );
        }
        
        cs.GetActiveCtrl().SetClickSink( null );

        m_AnimalManager.Clear();
        HomeSceneUIRoot.Instance.ReleaseUI();
        Clear();
    }

    //-------------------------------------------------------------------------------------------------------
    public void Clear()
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if ( es == null )
        {
            return;
        }

        Dictionary<int , IEntity>.Enumerator iter = m_dicEntity.GetEnumerator();
        while ( iter.MoveNext() )
        {
            if ( iter.Current.Value != null )
            {
                Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                removeEntiy.uid = iter.Current.Value.GetUID();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);
                es.RemoveEntity( iter.Current.Value );
            }
        }

        Dictionary<int , IPuppet>.Enumerator itPuppet = m_dicPuppet.GetEnumerator();
        while ( itPuppet.MoveNext() )
        {
            if ( itPuppet.Current.Value != null )
            {
                Client.stRemoveEntity removeEntiy = new Client.stRemoveEntity();
                removeEntiy.uid = itPuppet.Current.Value.GetUID();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, removeEntiy);
                es.RemoveEntity( itPuppet.Current.Value );
            }
        }
        Client.IPlayer player = ClientGlobal.Instance().MainPlayer;
        if(player != null)
        {
            CameraFollow.Instance.target = player;
        }
  
    }

    public void OnClickEntity(IEntity entity)
    {
        if ( entity == null )
        {
            return;
        }

        stClickEntity clickEntity = new stClickEntity();
        clickEntity.uid = entity.GetUID();
        Engine.Utility.EventEngine.Instance().DispatchEvent((int) (int)GameEventID.HOMELAND_CLICKENTITY , clickEntity );
    }

    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 添加玩家（木偶）
    @param strName 名称
    @param nJob 职业
    @param nSex 性别
    @param propList 外观属性
    @param bMainHost 是否是主角对象
    */
    public IPuppet AddPuppet(string strName , int nJob , int nSex , EntityViewProp[] propList , bool bMainHost = false)
    {
        EntityCreateData data = new EntityCreateData();
        data.ID = ++m_uEntityID;
        data.strName = strName;

        int speed = 0;
        Client.IPlayer player = ClientGlobal.Instance().MainPlayer;
        if(player!=null)
        {
            speed = player.GetProp((int)WorldObjProp.MoveSpeed);
        }


        data.PropList = new EntityAttr[(int)PuppetProp.End - (int)EntityProp.Begin];
        int index = 0;
        data.PropList[index++] = new EntityAttr( (int)PuppetProp.Job , nJob );
        data.PropList[index++] = new EntityAttr( (int)PuppetProp.Sex , nSex );
        data.PropList[index++] = new EntityAttr( (int)EntityProp.BaseID , 0 );
        data.PropList[index++] = new EntityAttr((int)WorldObjProp.MoveSpeed, speed);
        data.ViewList = propList;

        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if ( es == null )
        {
            Engine.Utility.Log.Error( "严重错误：EntitySystem is null!" );
            return null;
        }

        IPuppet entity = es.CreateEntity( EntityType.EntityType_Puppet , data ) as IPuppet;
        if ( entity == null )
        {
            Engine.Utility.Log.Error( "AddEntity:创建家园对象失败!" );
            return null;
        }

        if ( bMainHost )
        {
            IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
            if ( cs == null )
            {
                return null;
            }

            cs.GetActiveCtrl().SetHost( entity );
            CameraFollow.Instance.target = entity;
       
        }

        m_dicPuppet.Add( (int)entity.GetID() , entity );

        return entity;
    }

    public void RemoveEntity(IPuppet puppet)
    {
        if ( puppet == null )
        {
            return;
        }
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if ( es == null )
        {
            return;
        }

        m_dicPuppet.Remove( (int)puppet.GetID() );
        es.RemoveEntity( puppet );
    }

    //添加实体
    public IEntity AddEntity(string strName , EntityType type , uint nBaseID , int nState)
    {
        EntityCreateData data = BuildCreateEntityData( type , nBaseID , nState );
        if ( data == null )
        {
            return null;
        }

        data.strName = strName;

        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if ( es == null )
        {
            Engine.Utility.Log.Error( "严重错误：EntitySystem is null!" );
            return null;
        }

        IEntity entity = es.CreateEntity( type , data );
        if ( entity == null )
        {
            Engine.Utility.Log.Error( "AddEntity:创建家园对象失败!" );
            return null;
        }

        if ( entity.GetEntityType() == EntityType.EntityType_Animal )
        {
            m_AnimalManager.AddAnimal( entity as IAnimal );
        }

        m_dicEntity.Add( (int)entity.GetID() , entity );

        return entity;
    }
    public void RemoveEntity(long uid)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if ( es == null )
        {
            return;
        }
        IEntity en = es.FindEntity( uid );
        RemoveEntity( en );
    }
    public void RemoveEntity(IEntity en)
    {
        if ( en == null )
        {
            return;
        }
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if ( es == null )
        {
            return;
        }

        if ( en.GetEntityType() == EntityType.EntityType_Animal )
        {
            m_AnimalManager.RemoveAnimal( en as IAnimal );
        }

        m_dicEntity.Remove( (int)en.GetID() );
        es.RemoveEntity( en );
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 构建实体创建数据
    @param type 实体类型
    @param data 服务器数据
    */
    public EntityCreateData BuildCreateEntityData(EntityType type , uint nBaseID , int nState)
    {
        EntityCreateData entityData = new EntityCreateData();
        // 构建EntityCreateData
        entityData.ID = ++m_uEntityID;
        entityData.strName = "";

        switch ( type )
        {
            case EntityType.EntityType_Plant:
                {
                    entityData.PropList = new EntityAttr[(int)PlantProp.End - (int)EntityProp.Begin];
                    int index = 0;
                    entityData.PropList[index++] = new EntityAttr( (int)HomeProp.State , nState );
                    entityData.PropList[index++] = new EntityAttr( (int)EntityProp.BaseID , (int)nBaseID );
                    break;
                }
            case EntityType.EntityType_Animal:
                {
                    entityData.PropList = new EntityAttr[(int)AnimalProp.End - (int)EntityProp.Begin];
                    int index = 0;
                    entityData.PropList[index++] = new EntityAttr( (int)HomeProp.State , nState );
                    entityData.PropList[index++] = new EntityAttr( (int)EntityProp.BaseID , (int)nBaseID );
                    break;
                }
            case EntityType.EntityType_Tree:
                {
                    entityData.PropList = new EntityAttr[(int)TreeProp.End - (int)EntityProp.Begin];
                    int index = 0;
                    entityData.PropList[index++] = new EntityAttr( (int)HomeProp.State , nState );
                    entityData.PropList[index++] = new EntityAttr( (int)EntityProp.BaseID , (int)nBaseID );
                    break;
                }
            case EntityType.EntityType_Minerals:
                {
                    entityData.PropList = new EntityAttr[(int)MineralsProp.End - (int)EntityProp.Begin];
                    int index = 0;
                    entityData.PropList[index++] = new EntityAttr( (int)HomeProp.State , nState );
                    entityData.PropList[index++] = new EntityAttr( (int)EntityProp.BaseID , (int)nBaseID );
                    break;
                }
            case EntityType.EntityType_Soil:
                {
                    entityData.PropList = new EntityAttr[(int)SoilProp.End - (int)EntityProp.Begin];
                    int index = 0;
                    entityData.PropList[index++] = new EntityAttr( (int)HomeProp.State , nState );
                    entityData.PropList[index++] = new EntityAttr( (int)EntityProp.BaseID , (int)nBaseID );
                    break;
                }
        }

        return entityData;
    }
}


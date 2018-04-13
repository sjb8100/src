//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using table;


//class LearnSkillItem:MonoBehaviour
//{
//    SkillDatabase dataBase;
//    public SkillDatabase ItemDataBase
//    {
//        get
//        {
//            if(dataBase == null)
//            {
//                Engine.Utility.Log.Error( "LearnSkillItem  database is null" );
//                return null;
//            }
//            return dataBase;
//        }
//    }

//    LearnSkillDataManager dataManager
//    {
//        get
//        {
//            return  DataManager.Manager<LearnSkillDataManager>();
//        }
//    }

//    void Start()
//    {
     
//    }
//    public void InitItem(SkillDatabase db)
//    {
//        dataBase = db;
//        Transform parent = transform.parent;
//        if ( parent == null )
//            return;
//        Transform levelTrans = parent.Find( "level" );
//        if(levelTrans != null)
//        {
//            levelTrans.GetComponent<UILabel>().text = db.wdLevel + "级";
//        }

//        Transform nameTrans = parent.Find( "name" );
//        if(nameTrans != null)
//        {
//            nameTrans.GetComponent<UILabel>().text = db.strName;
//        }

//        GameObject go = parent.Find( "select" ).gameObject;
//        if(go != null)
//        {
//            UISprite spr = parent.Find( "select" ).GetComponent<UISprite>();
//            if ( spr != null )
//            {
//                spr.spriteName = db.iconPath;
//            }
//        }
       
//        LearnSkillDataManager skilldataManager = DataManager.Manager<LearnSkillDataManager>();
//        int level = skilldataManager.GetPlayerLevel();

//        Transform maskTrans = parent.Find( "mask" );

//        if(maskTrans != null)
//        {
//            if(!dataManager.IsSettingPanel)
//            {//升级界面高亮
//                maskTrans.gameObject.SetActive( false );
//                return;
//            }
//            else
//            {
//                maskTrans.gameObject.SetActive( true );
//            }
//            SkillDatabase sdb = GameTableManager.Instance.GetTableItem<SkillDatabase>( db.wdID , 1 );
//            if ( level < sdb.dwNeedLevel )
//            {
//                maskTrans.gameObject.SetActive( true );
//            }
//            else
//            {
//                maskTrans.gameObject.SetActive( false );
//                SkillSettingState state = dataManager.ShowState;
//                int job = dataManager.GetPlayerJob();
//                if ( db.dwSkillSatus == 0 )
//                    return;
//                if ( state == SkillSettingState.StateOne )
//                {
//                    if ( db.dwSkillSatus != job )
//                    {
//                        maskTrans.gameObject.SetActive( true );
//                    }
//                    else
//                    {
//                        maskTrans.gameObject.SetActive( false );
//                    }
//                }
//                if ( state == SkillSettingState.StateTwo )
//                {
//                    if ( db.dwSkillSatus != job+10 )
//                    {
//                        maskTrans.gameObject.SetActive( true );
//                    }
//                    else
//                    {
//                        maskTrans.gameObject.SetActive( false );
//                    }
//                }
//            }
          
//        }
       
//    }
//}


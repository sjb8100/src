using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Mono.Xml;
using System.Security;
using Client;

namespace WellFired
{
    public class USSequencerLoad
    {
        private static uint s_uPuppetID = 100000000; // 从1亿开始
        private static PropertyTypeInfo PropertyTypeInfoFromString(string str)
        {
            PropertyTypeInfo type = PropertyTypeInfo.None;
            switch (str)
            {
                case "Int":
                    type = PropertyTypeInfo.Int;
                    break;
                case "Long":
                    type = PropertyTypeInfo.Long;
                    break;
                case "Float":
                    type = PropertyTypeInfo.Float;
                    break;
                case "Double":
                    type = PropertyTypeInfo.Double;
                    break;
                case "Bool":
                    type = PropertyTypeInfo.Bool;
                    break;
                case "Vec2":
                    type = PropertyTypeInfo.Vec2;
                    break;
                case "Vec3":
                    type = PropertyTypeInfo.Vec3;
                    break;
                case "Vec4":
                    type = PropertyTypeInfo.Vec4;
                    break;
                case "Quat":
                    type = PropertyTypeInfo.Quat;
                    break;
                case "Colour":
                    type = PropertyTypeInfo.Colour;
                    break;
            }
            return type;
        }

        private static Vector2 Vector2FromString(string str)
        {
            string[] temp = str.Substring(1, str.Length - 2).Split(',');
            float x = float.Parse(temp[0]);
            float y = float.Parse(temp[1]);
            Vector2 rValue = new Vector2(x, y);
            return rValue;
        }
        private static Vector3 Vector3FromString(string str)
        {
            string[] temp = str.Substring(1, str.Length - 2).Split(',');
            float x = float.Parse(temp[0]);
            float y = float.Parse(temp[1]);
            float z = float.Parse(temp[2]);
            Vector3 rValue = new Vector3(x, y, z);
            return rValue;
        }
        private static Vector4 Vector4FromString(string str)
        {
            string[] temp = str.Substring(1, str.Length - 2).Split(',');
            float x = float.Parse(temp[0]);
            float y = float.Parse(temp[1]);
            float z = float.Parse(temp[2]);
            float w = float.Parse(temp[3]);
            Vector4 rValue = new Vector4(x, y, z, w);
            return rValue;
        }

        private static Quaternion QuaternionFromString(string str)
        {
            string[] temp = str.Substring(1, str.Length - 2).Split(',');
            float x = float.Parse(temp[0]);
            float y = float.Parse(temp[1]);
            float z = float.Parse(temp[2]);
            float w = float.Parse(temp[3]);
            Quaternion rValue = new Quaternion(x, y, z, w);
            return rValue;
        }


        //相机
        private static CameraClearFlags CameraClearFlagsFromString(string str)
        {
            CameraClearFlags flag = CameraClearFlags.Nothing;
            switch (str)
            {
                case "Skybox":
                    {
                        flag = CameraClearFlags.Skybox;
                    }
                    break;
                case "SolidColor":
                    {
                        flag = CameraClearFlags.SolidColor;
                    }
                    break;
                case "Color":
                    {
                        flag = CameraClearFlags.Color;
                    }
                    break;
                case "Depth":
                    {
                        flag = CameraClearFlags.Depth;
                    }
                    break;
            }
            return flag;
        }


        private static WellFired.Shared.TypeOfTransition TypeOfTransitionFromString(string str)
        {
            WellFired.Shared.TypeOfTransition type = WellFired.Shared.TypeOfTransition.Cut;
            switch (str)
            {
                case "Cut":
                    {
                        type = WellFired.Shared.TypeOfTransition.Cut;
                    }
                    break;
                case "Dissolve":
                    {
                        type = WellFired.Shared.TypeOfTransition.Dissolve;
                    }
                    break;
                case "WipeLeft":
                    {
                        type = WellFired.Shared.TypeOfTransition.WipeLeft;
                    }
                    break;
                case "WipeRight":
                    {
                        type = WellFired.Shared.TypeOfTransition.WipeRight;
                    }
                    break;
                case "WipeUp":
                    {
                        type = WellFired.Shared.TypeOfTransition.WipeUp;
                    }
                    break;
                case "WipeDown":
                    {
                        type = WellFired.Shared.TypeOfTransition.WipeDown;
                    }
                    break;
            }
            return type;
        }


        private static WellFired.UILayer UILayerFromString(string str)
        {
            WellFired.UILayer layer = UILayer.Back;
            switch (str)
            {
                case "Front":
                    {
                        layer = WellFired.UILayer.Front;
                    }
                    break;
                case "Middle":
                    {
                        layer = WellFired.UILayer.Middle;
                    }
                    break;
                case "Back":
                    {
                        layer = WellFired.UILayer.Back;
                    }
                    break;
            }
            return layer;
        }

        private static WellFired.UIPosition UIPositionFromString(string str)
        {
            WellFired.UIPosition position = UIPosition.BottomLeft;
            switch (str)
            {
                case "Center":
                    {
                        position = WellFired.UIPosition.Center;
                    }
                    break;
                case "TopLeft":
                    {
                        position = WellFired.UIPosition.TopLeft;
                    }
                    break;
                case "TopRight":
                    {
                        position = WellFired.UIPosition.TopRight;
                    }
                    break;
                case "BottomLeft":
                    {
                        position = WellFired.UIPosition.BottomLeft;
                    }
                    break;
                case "BottomRight":
                    {
                        position = WellFired.UIPosition.BottomRight;
                    }
                    break;
            }
            return position;
        }


        private static Texture LoadTexture(string strPath)
        {
            Texture tex = null;
            Engine.ITexture mapTex = null;
            Engine.RareEngine.Instance().GetRenderSystem().CreateTexture(ref strPath, ref mapTex, null, null, Engine.TaskPriority.TaskPriority_Immediate);

            tex = mapTex.GetTexture();

            return tex;
        }



        private static int GetPropPos(Client.EquipPos pos, EntityViewProp[] propList)
        {
            if (propList == null)
            {
                return -1;
            }

            for (int i = 0; i < propList.Length; ++i)
            {
                if (propList[i] == null)
                {
                    continue;
                }

                if (propList[i].nPos == (int)pos)
                {
                    return i;
                }
            }

            return -1;
        }

        private static GameObject ParseAffectedObjectPath(SecurityElement child_TimelineContainers, USTimelineContainer tc)
        {
            if (child_TimelineContainers == null)
                return null;

            GameObject gameObject = null;
            string strType = child_TimelineContainers.Attribute("ObjectType");
            switch (strType)
            {
                case "1"://mainCamera
                    gameObject = GameObject.Find("MainCamera");

                    tc.AffectedObject = gameObject.transform;

                    break;
                case "2"://npc
                    {
                        string strID = child_TimelineContainers.Attribute("NpcID");
                        string strNpcName = child_TimelineContainers.Attribute("NpcName");
                        //string strNpc = string.Format("NPC_{0}", strID);

#if AAAA
                    gameObject = GameObject.Find(strNpc);
                    tc.AffectedObject = gameObject.transform;

#else
                        var table_data = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(uint.Parse(strID));
                        if (table_data == null)
                        {
                            Engine.Utility.Log.Error("不存在这个NPC:" + uint.Parse(strID));
                            return null;

                        }

                        table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.dwModelSet);
                        if (resDB == null)
                        {
                            Engine.Utility.Log.Error("NPC:找不到NPC模型资源路径配置{0}", table_data.dwModelSet);
                            return null;
                        }
                        string strResName = resDB.strPath;

                        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                        Engine.IRenderObj obj = null;
                        if (!rs.CreateRenderObj(ref strResName, ref obj))
                        {
                            return null;
                        }

                        tc.AffectedObject = obj.GetNode().GetTransForm();
                        tc.RenderObj = obj;//以后需要用的这个东西播放动画
                        tc.AffectedObject.position = Vector3FromString(child_TimelineContainers.Attribute("Position"));
                        tc.AffectedObject.rotation = QuaternionFromString(child_TimelineContainers.Attribute("Rotation"));
                        tc.AffectedObject.localScale = Vector3FromString(child_TimelineContainers.Attribute("LocalScale"));

                        gameObject = obj.GetNode().GetTransForm().gameObject;
                        gameObject.name = strNpcName;
                        SequencerManager.Instance().m_SequencerTempRender.Add(obj);
#endif

                    }
                    break;
                case "3"://effect
                    {
                        string strResName = child_TimelineContainers.Attribute("Path");

                        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                        Engine.IEffect eff = null;
                        if (!rs.CreateEffect(ref strResName, ref eff))
                        {
                            return null;
                        }

                        tc.AffectedObject = eff.GetNode().GetTransForm();
                        tc.AffectedObject.position = Vector3FromString(child_TimelineContainers.Attribute("Position"));
                        tc.AffectedObject.rotation = QuaternionFromString(child_TimelineContainers.Attribute("Quaternion"));

                        gameObject = eff.GetNode().GetTransForm().gameObject;

                        SequencerManager.Instance().m_SequencerTempEffect.Add(eff);

                    }

                    break;
                case "4":   //相机
                    {
                        string strCameraName = child_TimelineContainers.Attribute("CameraName");
                        Vector3 pos = Vector3FromString(child_TimelineContainers.Attribute("Position"));
                        Quaternion quaternion = QuaternionFromString(child_TimelineContainers.Attribute("Quaternion"));

                        GameObject objCamera = new GameObject(strCameraName);
                        objCamera.transform.position = pos;
                        objCamera.transform.rotation = quaternion;

                        Camera camera = objCamera.AddComponent<Camera>();
                        camera.clearFlags = CameraClearFlagsFromString(child_TimelineContainers.Attribute("ClearFlags"));
                        camera.cullingMask = int.Parse(child_TimelineContainers.Attribute("CullingMask"));
                        camera.orthographic = bool.Parse(child_TimelineContainers.Attribute("Orthographic"));

                        if (child_TimelineContainers.Attribute("orthographicSize") != null)
                            camera.orthographicSize = float.Parse(child_TimelineContainers.Attribute("orthographicSize"));

                        camera.fieldOfView = float.Parse(child_TimelineContainers.Attribute("FieldOfView"));
                        camera.farClipPlane = float.Parse(child_TimelineContainers.Attribute("FarClipPlane"));
                        camera.nearClipPlane = float.Parse(child_TimelineContainers.Attribute("NearClipPlane"));

                        tc.AffectedObject = objCamera.transform;

                        gameObject = objCamera;

                        SequencerManager.Instance().m_SequencerTempObject.Add(gameObject);
                    }
                    break;
                case "5"://Observer
                    {
                        GameObject _observer = new GameObject("Observer");
                        _observer.transform.parent = tc.transform;
                        USTimelineObserver timelineObserver = _observer.AddComponent<USTimelineObserver>();
                        timelineObserver.layersToIgnore = LayerMask.GetMask("UI");

                        gameObject = _observer;
                    }
                    break;
                case "6"://MainPlayer
                    {

                        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
                        if (es == null) { return null; };

                        Client.EntityCreateData data = new Client.EntityCreateData();
                        data.ID = ++s_uPuppetID;
                        data.strName = "";

                        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
                        if (mainPlayer == null) { return null; }

                        //int speed = player.GetProp((int)WorldObjProp.MoveSpeed);
                        data.PropList = new EntityAttr[(int)PuppetProp.End - (int)EntityProp.Begin];
                        int index = 0;

                        data.PropList[index++] = new EntityAttr((int)PuppetProp.Job, mainPlayer.GetProp((int)PlayerProp.Job));
                        data.PropList[index++] = new EntityAttr((int)PuppetProp.Sex, mainPlayer.GetProp((int)PlayerProp.Sex));
                        //data.PropList[index++] = new EntityAttr((int)EntityProp.BaseID, 0);
                        //data.PropList[index++] = new EntityAttr((int)WorldObjProp.MoveSpeed, speed);

                        // 处理时装外观数据
                        EntityViewProp[] propList = new EntityViewProp[(int)Client.EquipPos.EquipPos_Max];
                        index = 0;
                        propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Body, 0);
                        propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Weapon, 0);


                        List<GameCmd.SuitData> lstSuit = null;
                        mainPlayer.GetSuit(out lstSuit);


                        if (lstSuit.Count > 0)
                        {
                            for (int i = 0; i < lstSuit.Count; ++i)
                            {
                                if (lstSuit[i] == null)
                                {
                                    continue;
                                }

                                int pos = GetPropPos((Client.EquipPos)lstSuit[i].suit_type, propList);
                                if (pos >= 0)
                                {
                                    propList[pos] = new EntityViewProp((int)lstSuit[i].suit_type, (int)lstSuit[i].baseid);
                                }
                                else
                                {
                                    Client.EquipPos equipPos = (Client.EquipPos)lstSuit[i].suit_type;
                                    propList[index++] = new EntityViewProp((int)equipPos, (int)lstSuit[i].baseid);
                                }
                            }
                        }

                        data.ViewList = propList;
                        //data.nLayer = LayerMask.NameToLayer("ShowModel");

                        IEntity pPuppetObj = es.CreateEntity(Client.EntityType.EntityType_Puppet, data, true) as Client.IPuppet;
                        if (pPuppetObj == null)
                        {
                            Engine.Utility.Log.Error("创建Renderobj失败{0}！", "");
                            return null;
                        }

                        Engine.IRenderObj renderObj = pPuppetObj.renderObj;
                        if (renderObj == null) { return null; }
                        pPuppetObj.SendMessage(EntityMessage.EntityCommand_SetVisible, true);

                        tc.AffectedObject = renderObj.GetNode().GetTransForm();
                        tc.RenderObj = renderObj;//以后需要用的这个东西播放动画
                        tc.AffectedObject.position = Vector3FromString(child_TimelineContainers.Attribute("Position"));
                        tc.AffectedObject.rotation = QuaternionFromString(child_TimelineContainers.Attribute("Quaternion"));

                        pPuppetObj.SendMessage(EntityMessage.EntityCommand_SetPos,Vector3FromString(child_TimelineContainers.Attribute("Position")));
                        Quaternion rotation = QuaternionFromString(child_TimelineContainers.Attribute("Quaternion"));
                        pPuppetObj.SendMessage(EntityMessage.EntityCommand_SetRotate, rotation.eulerAngles);

                        gameObject = renderObj.GetNode().GetTransForm().gameObject;
                        gameObject.name = "MainPlayer";

                        string str = string.Format("x{0},y{1},z{2}", tc.AffectedObject.position.x, tc.AffectedObject.position.y, tc.AffectedObject.position.z);
                        Debug.Log(str);

                        //SequencerManager.Instance().m_SequencerTempRender.Add(renderObj);
                        SequencerManager.Instance().m_SequencerTempEntity.Add(pPuppetObj);

                    }
                    break;
                case "7"://Obj
                    {
                        string strResName = child_TimelineContainers.Attribute("Path");
                        string strObjName = child_TimelineContainers.Attribute("ObjName");

                        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                        Engine.IRenderObj obj = null;
                        if (!rs.CreateRenderObj(ref strResName, ref obj))
                        {
                            return null;
                        }

                        tc.AffectedObject = obj.GetNode().GetTransForm();
                        tc.RenderObj = obj;//以后需要用的这个东西播放动画
                        tc.AffectedObject.position = Vector3FromString(child_TimelineContainers.Attribute("Position"));
                        tc.AffectedObject.rotation = QuaternionFromString(child_TimelineContainers.Attribute("Quaternion"));

                        gameObject = obj.GetNode().GetTransForm().gameObject;
                        gameObject.name = strObjName;
                        SequencerManager.Instance().m_SequencerTempRender.Add(obj);

                    }

                    break;
                default:
                    break;
            }


            return gameObject;
        }

        public static UnityEngine.GameObject LoadSequencerFromXml(string strPath)
        {
            UnityEngine.GameObject result = null;
            byte[] bytes = Engine.Utility.FileUtils.Instance().ReadFile(strPath);
            if (bytes == null)
            {
                return null;
            }
            string xml = System.Text.Encoding.UTF8.GetString(bytes);
            bytes = null;
            SecurityElement xmlRoot = XmlParser.Parser(xml);
            if (xmlRoot == null)
                return null;

            // 创建剧情根节点
            GameObject _sequence = new GameObject("Sequence");
            USSequencer sequencerComponent = _sequence.AddComponent<USSequencer>();
            sequencerComponent.Duration = float.Parse(xmlRoot.Attribute("duration"));
            //sequencerComponent.PlaybackRate = 0.1f;

            result = _sequence;

            foreach (SecurityElement child_TimelineContainers in xmlRoot.Children)
            {
                if (child_TimelineContainers.Tag == "TimelineContainers")
                {
                    string strName = child_TimelineContainers.Attribute("Name");
                    GameObject gameObject_TimelineContainers = new GameObject(strName);
                    gameObject_TimelineContainers.transform.parent = _sequence.transform;
                    USTimelineContainer tc = gameObject_TimelineContainers.AddComponent<USTimelineContainer>();

                    GameObject affectedObject = ParseAffectedObjectPath(child_TimelineContainers, tc);
                    if (affectedObject == null)
                        continue;


                    if (child_TimelineContainers.Children != null)
                    {
                        foreach (SecurityElement child_Info in child_TimelineContainers.Children)
                        {
                            if (child_Info.Tag == "TimelineObjectPath")
                            {
                                GameObject gameObject_TimelineObjectPath = new GameObject("Object Path Timeline");
                                gameObject_TimelineObjectPath.transform.parent = gameObject_TimelineContainers.transform;

                                USTimelineObjectPath top = gameObject_TimelineObjectPath.AddComponent<USTimelineObjectPath>();

                                List<SplineKeyframe> keyframes = new List<SplineKeyframe>();

                                top.Build();
                                //SplineKeyframe
                                foreach (SecurityElement child_SplineKeyframe in child_Info.Children)
                                {
                                    string strAsset = child_SplineKeyframe.Attribute("AssetName");
                                    string strSplineKeyframe = child_SplineKeyframe.Attribute("SplineKeyframe");

                                    SplineKeyframe splineKeyframe = ScriptableObject.CreateInstance<WellFired.SplineKeyframe>();
                                    splineKeyframe.Position = Vector3FromString(strSplineKeyframe);

                                    keyframes.Add(splineKeyframe);

                                }

                                top.SetKeyframes(keyframes);

                            }
                            else if (child_Info.Tag == "TimelineProperty")
                            {
                                GameObject gameObject_TimelineProperty = new GameObject("Property Timeline");
                                gameObject_TimelineProperty.transform.parent = gameObject_TimelineContainers.transform;
                                USTimelineProperty tp = gameObject_TimelineProperty.AddComponent<USTimelineProperty>();

                                //PropertyList
                                foreach (SecurityElement child_PropertyList in child_Info.Children)
                                {
                                    WellFired.USPropertyInfo propertyInfo = ScriptableObject.CreateInstance<WellFired.USPropertyInfo>();
                                    tp.AddProperty(propertyInfo);

                                    string strAsset = child_PropertyList.Attribute("Asset");
                                    propertyInfo.Component = affectedObject.transform;
                                    propertyInfo.PropertyType = PropertyTypeInfoFromString(child_PropertyList.Attribute("PropertyType"));
                                    propertyInfo.propertyName = child_PropertyList.Attribute("PropertyName");
                                    propertyInfo.baseInt = int.Parse(child_PropertyList.Attribute("BaseInt"));
                                    propertyInfo.baseLong = long.Parse(child_PropertyList.Attribute("BaseLong"));
                                    propertyInfo.baseFloat = float.Parse(child_PropertyList.Attribute("BaseFloat"));
                                    propertyInfo.baseDouble = double.Parse(child_PropertyList.Attribute("BaseDouble"));
                                    propertyInfo.baseBool = bool.Parse(child_PropertyList.Attribute("BaseBool"));
                                    propertyInfo.baseVector2 = Vector2FromString(child_PropertyList.Attribute("BaseVector2"));
                                    propertyInfo.baseVector3 = Vector3FromString(child_PropertyList.Attribute("BaseVector3"));
                                    propertyInfo.baseVector4 = Vector4FromString(child_PropertyList.Attribute("BaseVector4"));
                                    propertyInfo.InternalName = child_PropertyList.Attribute("InternalName");

                                    foreach (SecurityElement child_Curves in child_PropertyList.Children)
                                    {
                                        string str = child_Curves.Attribute("Curves");
                                        string strCurvesID = child_Curves.Attribute("CurvesID");

                                        USInternalCurve internalCurve = ScriptableObject.CreateInstance<USInternalCurve>();
                                        propertyInfo.curves.Add(internalCurve);

                                        internalCurve.UseCurrentValue = bool.Parse(child_Curves.Attribute("UseCurrentValue"));
                                        internalCurve.Duration = float.Parse(child_Curves.Attribute("Duration"));

                                        internalCurve.UnityAnimationCurve = new AnimationCurve();
                                        foreach (SecurityElement child_AnimationCurve in child_Curves.Children)
                                        {
                                            if (child_AnimationCurve.Tag == "AnimationCurve")
                                            {
                                                Keyframe keyframe = new Keyframe();
                                                keyframe.inTangent = float.Parse(child_AnimationCurve.Attribute("InTangent"));
                                                keyframe.outTangent = float.Parse(child_AnimationCurve.Attribute("OutTangent"));
                                                keyframe.tangentMode = int.Parse(child_AnimationCurve.Attribute("TangentMode"));
                                                keyframe.time = float.Parse(child_AnimationCurve.Attribute("Time"));
                                                keyframe.value = float.Parse(child_AnimationCurve.Attribute("Value"));

                                                internalCurve.UnityAnimationCurve.AddKey(keyframe);
                                            }
                                        }

                                        internalCurve.internalKeyframes = new List<USInternalKeyframe>();
                                        foreach (SecurityElement child_Keyframe in child_Curves.Children)
                                        {
                                            if (child_Keyframe.Tag == "Keyframe")
                                            {
                                                USInternalKeyframe internalKeyframe = ScriptableObject.CreateInstance<USInternalKeyframe>();
                                                internalKeyframe.curve = internalCurve;
                                                internalKeyframe.value = float.Parse(child_Keyframe.Attribute("Value"));
                                                internalKeyframe.time = float.Parse(child_Keyframe.Attribute("Time"));
                                                internalKeyframe.inTangent = float.Parse(child_Keyframe.Attribute("InTangent"));
                                                internalKeyframe.outTangent = float.Parse(child_Keyframe.Attribute("OutTangent"));
                                                internalKeyframe.brokenTangents = bool.Parse(child_Keyframe.Attribute("BrokenTangents"));

                                                internalCurve.internalKeyframes.Add(internalKeyframe);

                                            }

                                        }

                                    }

                                }

                            }
                            else if (child_Info.Tag == "ObserverKeyframes")
                            {
                                USObserverKeyframe keyframe = ScriptableObject.CreateInstance<USObserverKeyframe>();

                                keyframe.FireTime = float.Parse(child_Info.Attribute("FireTime"));
                                GameObject ObjCamera = GameObject.Find(child_Info.Attribute("KeyframeCamera"));
                                keyframe.KeyframeCamera = ObjCamera.GetComponent<Camera>();
                                keyframe.TransitionType = TypeOfTransitionFromString(child_Info.Attribute("TransitionType"));
                                keyframe.TransitionDuration = float.Parse(child_Info.Attribute("TransitionDuration"));

                                USTimelineObserver timelineObserver = affectedObject.GetComponent<USTimelineObserver>();
                                timelineObserver.observerKeyframes.Add(keyframe);


                            }
                            else if (child_Info.Tag == "TimelineEvent")
                            {
                                //int i = 2;
                                GameObject gameObject_TimelineEvent = new GameObject("Event Timeline");
                                gameObject_TimelineEvent.transform.parent = gameObject_TimelineContainers.transform;

                                USTimelineEvent te = gameObject_TimelineEvent.AddComponent<USTimelineEvent>();

                                if (child_Info.Children != null)
                                {
                                    //Event
                                    foreach (SecurityElement child_TimelineEvent in child_Info.Children)
                                    {
                                        string strEventType = child_TimelineEvent.Attribute("type");
                                        if (strEventType == "SetActiveEvent")
                                        {
                                            GameObject gameObject_Event = new GameObject("USSetActive");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USSetActive activeEvent = gameObject_Event.AddComponent<USSetActive>();
                                            activeEvent.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            activeEvent.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            activeEvent.active = bool.Parse(child_TimelineEvent.Attribute("active"));

                                        }
                                        else if (strEventType == "ShakeCamera")
                                        {
                                            GameObject gameObject_Event = new GameObject("USShakeCamera");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USShakeCamera shakeCamera = gameObject_Event.AddComponent<USShakeCamera>();
                                            shakeCamera.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            shakeCamera.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            shakeCamera.strength = float.Parse(child_TimelineEvent.Attribute("strength"));
                                            shakeCamera.vibrato = int.Parse(child_TimelineEvent.Attribute("vibrato"));
                                            shakeCamera.randomness = float.Parse(child_TimelineEvent.Attribute("randomness"));


                                        }
                                        else if (strEventType == "ChangeColorEvent")
                                        {
                                            GameObject gameObject_Event = new GameObject("USChangeColor");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USChangeColor changeColor = gameObject_Event.AddComponent<USChangeColor>();
                                            changeColor.FireOnSkip = bool.Parse(child_TimelineEvent.Attribute("fireOnSkip"));
                                            changeColor.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            changeColor.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            changeColor.newColor = Engine.Utility.StringUtility.ParseColor(child_TimelineEvent.Attribute("newColor"));

                                        }
                                        else if (strEventType == "PlayAnimationEvent")
                                        {
                                            GameObject gameObject_Event = new GameObject("USPlayAnimEvent");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USPlayAnimEvent playAnimEvent = gameObject_Event.AddComponent<USPlayAnimEvent>();

                                            playAnimEvent.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            playAnimEvent.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            playAnimEvent.wrapMode = (WrapMode)int.Parse(child_TimelineEvent.Attribute("wrapMode"));
                                            playAnimEvent.animationName = child_TimelineEvent.Attribute("name");


                                        }
                                        else if (strEventType == "BlendAnimEvent")
                                        {
                                            GameObject gameObject_Event = new GameObject("USBlendAnimEvent");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USBlendAnimEvent blendAnimEvent = gameObject_Event.AddComponent<USBlendAnimEvent>();

                                            blendAnimEvent.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            blendAnimEvent.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));
                                            blendAnimEvent.blendPoint = float.Parse(child_TimelineEvent.Attribute("blendPoint"));
                                            blendAnimEvent.fadeLength = float.Parse(child_TimelineEvent.Attribute("fadeLength"));

                                            blendAnimEvent.animationNameSource = child_TimelineEvent.Attribute("animationNameSource");
                                            blendAnimEvent.animationNameDest = child_TimelineEvent.Attribute("animationNameDest");

                                        }
                                        else if (strEventType == "LookAtObjectEvent")
                                        {
                                            GameObject gameObject_Event = new GameObject("USLookAtObjectEvent");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USLookAtObjectEvent lookAtObjectEvent = gameObject_Event.AddComponent<USLookAtObjectEvent>();

                                            lookAtObjectEvent.FireTime = float.Parse(child_TimelineEvent.Attribute("FireTime"));
                                            lookAtObjectEvent.FireOnSkip = bool.Parse(child_TimelineEvent.Attribute("FireOnSkip"));
                                            lookAtObjectEvent.lookAtTime = float.Parse(child_TimelineEvent.Attribute("lookAtTime"));
                                            lookAtObjectEvent.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            lookAtObjectEvent.objectToLookAt = SequencerManager.Instance().GetGameObject(child_TimelineEvent.Attribute("ObjectToLookAt"));



                                        }
                                        else if (strEventType == "OpenDialog")
                                        {
                                            GameObject gameObject_Event = new GameObject("USOpenDialog");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USOpenDialog openDialog = gameObject_Event.AddComponent<USOpenDialog>();

                                            openDialog.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            openDialog.FireOnSkip = bool.Parse(child_TimelineEvent.Attribute("FireOnSkip"));
                                            openDialog.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));
                                            openDialog.dialogID = int.Parse(child_TimelineEvent.Attribute("dialogID"));
                                            openDialog.backgroundImage = child_TimelineEvent.Attribute("backgroundImage");

                                        }
                                        else if (strEventType == "RotateObjectEvent")
                                        {
                                            GameObject gameObject_Event = new GameObject("USRotateObjectEvent");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USRotateObjectEvent rotateObjectEvent = gameObject_Event.AddComponent<USRotateObjectEvent>();
                                            rotateObjectEvent.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            rotateObjectEvent.FireOnSkip = bool.Parse(child_TimelineEvent.Attribute("FireOnSkip"));
                                            rotateObjectEvent.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            rotateObjectEvent.rotateSpeedPerSecond = float.Parse(child_TimelineEvent.Attribute("rotateSpeedPerSecond"));
                                            rotateObjectEvent.rotationAxis = Vector3FromString(child_TimelineEvent.Attribute("rotationAxis"));

                                        }
                                        else if (strEventType == "LoadScene")
                                        {
                                            GameObject gameObject_Event = new GameObject("USLoadScene");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USLoadScene loadScene = gameObject_Event.AddComponent<USLoadScene>();
                                            loadScene.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            loadScene.FireOnSkip = bool.Parse(child_TimelineEvent.Attribute("FireOnSkip"));

                                            loadScene.sceneID = uint.Parse(child_TimelineEvent.Attribute("scene"));


                                        }
                                        else if(strEventType == "OpenSeqLoading")
                                        {
                                            GameObject gameObject_Event = new GameObject("USOpenSeqLoading");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USOpenSeqLoading openSeqLoading = gameObject_Event.AddComponent<USOpenSeqLoading>();
                                            openSeqLoading.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));

                                        }
                                        else if (strEventType == "TimeScale")
                                        {
                                            GameObject gameObject_Event = new GameObject("USTimeScale");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USTimeScaleEvent timeScale = gameObject_Event.AddComponent<USTimeScaleEvent>();
                                            timeScale.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            timeScale.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            Keyframe[] keyframes = new Keyframe[child_TimelineEvent.Children.Count];
                                            int nIndex = 0;
                                            foreach (SecurityElement child_Keyframe in child_TimelineEvent.Children)
                                            {
                                                Keyframe keyframe = new Keyframe();
                                                keyframe.inTangent = float.Parse(child_Keyframe.Attribute("inTangent"));
                                                keyframe.outTangent = float.Parse(child_Keyframe.Attribute("outTangent"));
                                                keyframe.tangentMode = int.Parse(child_Keyframe.Attribute("tangentMode"));
                                                keyframe.time = float.Parse(child_Keyframe.Attribute("time"));
                                                keyframe.value = float.Parse(child_Keyframe.Attribute("value"));

                                                keyframes[nIndex++] = keyframe;
                                  
                                            }
                                            timeScale.scaleCurve.keys = keyframes;
                                            

                                        }
                                        else if (strEventType == "DisplayImageEvent")
                                        {
                                            GameObject gameObject_Event = new GameObject("USDisplayImageEvent");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USDisplayImageEvent displayImage = gameObject_Event.AddComponent<USDisplayImageEvent>();
                                            displayImage.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            displayImage.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            displayImage.uiLayer = UILayerFromString(child_TimelineEvent.Attribute("uiLayer"));

                                            displayImage.displayPosition = UIPositionFromString(child_TimelineEvent.Attribute("displayPosition"));
                                            displayImage.anchorPosition = UIPositionFromString(child_TimelineEvent.Attribute("anchorPosition"));
                                            displayImage.offsetX = float.Parse(child_TimelineEvent.Attribute("offsetX"));
                                            displayImage.offsetY = float.Parse(child_TimelineEvent.Attribute("offsetY"));
         
                                            string strImagePath = child_TimelineEvent.Attribute("displayImage");
                                            strImagePath = "story/" + strImagePath + ".unity3d";
                                            displayImage.displayImage = (Texture2D)LoadTexture(strImagePath);


                                            Keyframe[] keyframes = new Keyframe[child_TimelineEvent.Children.Count];
                                            int nIndex = 0;
                                            foreach (SecurityElement child_Keyframe in child_TimelineEvent.Children)
                                            {
                                                Keyframe keyframe = new Keyframe();
                                                keyframe.inTangent = float.Parse(child_Keyframe.Attribute("inTangent"));
                                                keyframe.outTangent = float.Parse(child_Keyframe.Attribute("outTangent"));
                                                keyframe.tangentMode = int.Parse(child_Keyframe.Attribute("tangentMode"));
                                                keyframe.time = float.Parse(child_Keyframe.Attribute("time"));
                                                keyframe.value = float.Parse(child_Keyframe.Attribute("value"));

                                                keyframes[nIndex++] = keyframe;

                                            }
                                            displayImage.fadeCurve.keys = keyframes;

                                        }
                                        else if (strEventType == "CameraFade")
                                        {
                                            GameObject gameObject_Event = new GameObject("USCameraFade");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USCameraFade cameraFade = gameObject_Event.AddComponent<USCameraFade>();
                                            cameraFade.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            cameraFade.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            if (child_TimelineEvent.Attribute("bOutFade") != null)
                                                cameraFade.bOutFade = bool.Parse(child_TimelineEvent.Attribute("bOutFade"));

                                        }
                                        else if (strEventType == "PrintTextEvent")
                                        {
                                            GameObject gameObject_Event = new GameObject("USPrintTextEvent");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USPrintTextEvent printText = gameObject_Event.AddComponent<USPrintTextEvent>();
                                            printText.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            printText.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            printText.uiLayer = UILayerFromString(child_TimelineEvent.Attribute("uiLayer"));

                                            printText.offsetX = int.Parse(child_TimelineEvent.Attribute("offsetX"));
                                            printText.offsetY = int.Parse(child_TimelineEvent.Attribute("offsetY"));
                                            printText.position.width = float.Parse(child_TimelineEvent.Attribute("rectWidth"));
                                            printText.position.height = float.Parse(child_TimelineEvent.Attribute("rectHeight"));
                                            printText.fontSize = int.Parse(child_TimelineEvent.Attribute("fontSize"));

                                            printText.textToPrint = child_TimelineEvent.Attribute("textToPrint");

                                        }
                                        else if (strEventType == "ShowBook")
                                        {
                                            GameObject gameObject_Event = new GameObject("USShowBook");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USShowBook showBook = gameObject_Event.AddComponent<USShowBook>();
                                            showBook.FireTime = float.Parse(child_TimelineEvent.Attribute("firetime"));
                                            showBook.Duration = float.Parse(child_TimelineEvent.Attribute("duration"));

                                            showBook.nPageNumber = int.Parse(child_TimelineEvent.Attribute("pageNumber"));
                                            showBook.fTurnSpeed = float.Parse(child_TimelineEvent.Attribute("turnSpeed"));
                                            showBook.fNextPage = float.Parse(child_TimelineEvent.Attribute("nextPage"));

                                            showBook.pTexture2D = new Texture2D[showBook.nPageNumber * 2];
                                            int nIndex = 0;
                                            foreach (SecurityElement child_Texture in child_TimelineEvent.Children)
                                            {
                                                string tex = child_Texture.Attribute("texture");
                                                showBook.pTexture2D[nIndex++] = (Texture2D)LoadTexture(tex);

                                            }

                                        }
                                        else if (strEventType == "")
                                        {
                                            GameObject gameObject_Event = new GameObject("");
                                            gameObject_Event.transform.parent = gameObject_TimelineEvent.transform;

                                            USBlendAnimEvent blendAnimEvent = gameObject_Event.AddComponent<USBlendAnimEvent>();






                                        }

                                    }
                                }

                            }

                        }
                    }


                    //其他



                }
            }


            return result;
        }
    }

    /// <summary>
    /// The uSequencer behaviour, this deals with updating and processing all our timelines.
    /// The structure for a sequencer is as such
    /// USequencer
    /// 	USTimelineContainer
    /// 		USTimelineBase
    /// </summary>
    [ExecuteInEditMode]
    [Serializable]
    public class USSequencer : MonoBehaviour
    {
        #region Member Variables
        /// <summary>
        /// The observed objects is basically every game object referenced in the usequencer, we need this list, so 
        /// we don't have to do anything crazy like adding un-needed components to our users game objects.
        /// </summary>
        [SerializeField]
        private List<Transform> observedObjects = new List<Transform>();

        [SerializeField]
        private float runningTime = 0.0f;

        [SerializeField]
        private float playbackRate = 1.0f;

        [SerializeField]
        private int version = 2;

        [SerializeField]
        private float duration = 10.0f;

        [SerializeField]
        private bool isLoopingSequence = false;

        [SerializeField]
        private bool isPingPongingSequence = false;

        [SerializeField]
        private bool updateOnFixedUpdate = false;

        [SerializeField]
        private bool autoplay = false;

        private bool playing = false;

        private bool isFreshPlayback = true;

        private float previousTime = 0.0f;

        private float minPlaybackRate = -100.0f;

        private float maxPlaybackRate = 100.0f;

        private float setSkipTime = -1.0f;
        #endregion

        #region Properties
        /// <summary>
        /// Our current Version number, you can manipulate this if you would like to force the uSequencer to upgrade
        /// your sequence, for some reason.
        /// </summary>
        public int Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        /// <summary>
        /// This is a list of every object that the uSequencer sequences.
        /// </summary>
        public List<Transform> ObservedObjects
        {
            get
            {
                return observedObjects;
            }
        }

        /// <summary>
        /// The duration of this sequence, you CAN manipulate this in script if you like.
        /// </summary>
        public float Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                if (duration <= 0.0f)
                    duration = 0.1f;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this sequence is playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return playing; }
        }

        /// <summary>
        /// Gets a value indicating whether this sequence is looping, you CAN manipulate this through script.
        /// </summary>
        public bool IsLopping
        {
            get { return isLoopingSequence; }
            set { isLoopingSequence = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this sequence is ping ponging, you CAN manipulate this through script.
        /// </summary>
        public bool IsPingPonging
        {
            get { return isPingPongingSequence; }
            set { isPingPongingSequence = value; }
        }

        /// <summary>
        /// Method that allows you to determine if a sequence is still playing
        /// </summary>
        /// <value><c>true</c> if this instance is complete; otherwise, <c>false</c>.</value>
        public bool IsComplete
        {
            get { return (!IsPlaying && RunningTime >= Duration); }
            set { ; }
        }

        /// <summary>
        /// If you need to manually set the running time of this sequence, you can do it with this attribute.
        /// You can do this in the editor, or in game. Skipping forward or backwards is supported.
        /// </summary>
        public float RunningTime
        {
            get { return runningTime; }
            set
            {
                runningTime = value;
                if (runningTime <= 0.0f)
                    runningTime = 0.0f;

                if (runningTime > duration)
                    runningTime = duration;

                if (isFreshPlayback)
                {
                    foreach (USTimelineContainer timelineContainer in TimelineContainers)
                    {
                        foreach (USTimelineBase timeline in timelineContainer.Timelines)
                            timeline.StartTimeline();
                    }
                    isFreshPlayback = false;
                }

                foreach (USTimelineContainer timelineContainer in TimelineContainers)
                {
                    timelineContainer.ManuallySetTime(RunningTime);
                    timelineContainer.ProcessTimelines(RunningTime, PlaybackRate);
                }

                OnRunningTimeSet(this);
            }
        }

        /// <summary>
        /// Our current playback rate, you CAN manipulate this through script a negative value will play this sequence in reverse.
        /// </summary>
        public float PlaybackRate
        {
            get { return playbackRate; }
            set { playbackRate = Mathf.Clamp(value, MinPlaybackRate, MaxPlaybackRate); }
        }

        public float MinPlaybackRate
        {
            get { return minPlaybackRate; }
        }

        public float MaxPlaybackRate
        {
            get { return maxPlaybackRate; }
        }

        /// <summary>
        /// This will tell you if the sequence has been played, but isn't currently playing. For instance, if someone pressed Play and the Pause.
        /// </summary>
        /// <value><c>true</c> if this instance has sequence been started; otherwise, <c>false</c>.</value>
        public bool HasSequenceBeenStarted
        {
            get { return !isFreshPlayback; }
        }

        private USTimelineContainer[] timelineContainers;
        public USTimelineContainer[] TimelineContainers
        {
            get
            {
                if (timelineContainers == null)
                    timelineContainers = GetComponentsInChildren<USTimelineContainer>();
                return timelineContainers;
            }
        }

        public USTimelineContainer[] SortedTimelineContainers
        {
            get
            {
                var timelineContainers = TimelineContainers;
                Array.Sort(timelineContainers, USTimelineContainer.Comparer);

                return timelineContainers;
            }
        }

        public int TimelineContainerCount
        {
            get
            {
                return TimelineContainers.Length;
            }
        }

        public int ObservedObjectCount
        {
            get
            {
                return ObservedObjects.Count;
            }
        }

        public bool UpdateOnFixedUpdate
        {
            get { return updateOnFixedUpdate; }
            set { updateOnFixedUpdate = value; }
        }

        /// <summary>
        /// All sequences are updated on a coroutine, we yield return new WaitForSeconds(SequenceUpdateRate); on the coroutine
        /// </summary>
        /// <value>The sequence update rate.</value>
        public static float SequenceUpdateRate
        {
            get { return 0.01f * Time.timeScale; }
        }
        #endregion

        #region Delegates
        public delegate void PlaybackDelegate(USSequencer sequencer);
        public delegate void UpdateDelegate(USSequencer sequencer, float newRunningTime);
        /// <summary>
        /// This Delegate will be called when Playback has Started, add delegates with +=
        /// </summary>
        public PlaybackDelegate PlaybackStarted = delegate { };
        /// <summary>
        /// This Delegate will be called when Playback has Stopped, add delegates with +=
        /// </summary>
        public PlaybackDelegate PlaybackStopped = delegate { };
        /// <summary>
        /// This Delegate will be called when Playback has Paused, add delegates with +=
        /// </summary>
        public PlaybackDelegate PlaybackPaused = delegate { };
        /// <summary>
        /// This Delegate will be called when Playback has Finished, add delegates with +=
        /// </summary>
        public PlaybackDelegate PlaybackFinished = delegate { };
        /// <summary>
        /// This Delegate will be called before an update with the new runningTime, and before timelines have been processed add delegates with +=
        /// </summary>
        public UpdateDelegate BeforeUpdate = delegate { };
        /// <summary>
        /// This Delegate will be called after an update with the new runningTime, and after timelines have been processed add delegates with +=
        /// </summary>
        public UpdateDelegate AfterUpdate = delegate { };
        /// <summary>
        /// This Delegate will be called whenever the RunningTime is set add delegates with +=
        /// </summary>
        public PlaybackDelegate OnRunningTimeSet = delegate { };
        #endregion


        public static string GetFullHierarchyPath(Transform transform)
        {
            string path = "/" + transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = "/" + transform.name + path;
            }
            return path;
        }


        private void OnDestroy()
        {
            StopCoroutine("UpdateSequencerCoroutine");
        }

        private void Start()
        {
            // Attempt to auto fix our Event Objects
            foreach (USTimelineContainer timelineContainer in TimelineContainers)
            {
                if (!timelineContainer)
                    continue;

                foreach (USTimelineBase timelineBase in timelineContainer.Timelines)
                {
                    USTimelineEvent timelineEvent = timelineBase as USTimelineEvent;
                    if (timelineEvent)
                    {
                        foreach (USEventBase eventBase in timelineEvent.Events)
                            eventBase.FixupAdditionalObjects();
                    }

                    USTimelineObjectPath timelineObjectPath = timelineBase as USTimelineObjectPath;
                    if (timelineObjectPath)
                    {
                        timelineObjectPath.FixupAdditionalObjects();
                    }
                }
            }

            if (autoplay && Application.isPlaying)
                Play();
        }

        public void TogglePlayback()
        {
            if (playing)
                Pause();
            else
                Play();
        }

        public void Play()
        {
            if (PlaybackStarted != null)
                PlaybackStarted(this);

            // Playback runs on a coroutine.
            StartCoroutine("UpdateSequencerCoroutine");

            // Start or resume our playback.
            if (isFreshPlayback)
            {
                foreach (USTimelineContainer timelineContainer in TimelineContainers)
                {
                    foreach (USTimelineBase timeline in timelineContainer.Timelines)
                    {
                        timeline.StartTimeline();
                    }
                }
                isFreshPlayback = false;
            }
            else
            {
                foreach (USTimelineContainer timelineContainer in TimelineContainers)
                {
                    foreach (USTimelineBase timeline in timelineContainer.Timelines)
                    {
                        timeline.ResumeTimeline();
                    }
                }
            }
          

            playing = true;
            previousTime = Time.time;

            UpdateSequencer(0.0f);
        }

        public void Pause()
        {
            if (PlaybackPaused != null)
                PlaybackPaused(this);

            playing = false;

            foreach (USTimelineContainer timelineContainer in TimelineContainers)
            {
                foreach (USTimelineBase timeline in timelineContainer.Timelines)
                {
                    timeline.PauseTimeline();
                }
            }
        }

        public void Stop()
        {
            if (PlaybackStopped != null)
                PlaybackStopped(this);

            // Playback runs on a coroutine.
            StopCoroutine("UpdateSequencerCoroutine");

            foreach (USTimelineContainer timelineContainer in TimelineContainers)
            {
                foreach (USTimelineBase timeline in timelineContainer.Timelines)
                {
                    if (timeline.GetType() == typeof(USTimelineObserver) || timeline.AffectedObject != null)
                        timeline.StopTimeline();
                }
            }

            isFreshPlayback = true;
            playing = false;
            runningTime = 0.0f;
        }

        /// <summary>
        ///  This method will be called when the scrub head has reached the end of playback, for a 10s long sequence, this will be 10 seconds in.
        /// </summary>
        private void End()
        {
            if (PlaybackFinished != null)
                PlaybackFinished(this);

            if (isLoopingSequence || isPingPongingSequence)
                return;

            foreach (USTimelineContainer timelineContainer in TimelineContainers)
            {
                foreach (USTimelineBase timeline in timelineContainer.Timelines)
                {
                    if (timeline.AffectedObject != null)
                        timeline.EndTimeline();
                }
            }
        }

        /// <summary>
        /// Untility Function to creates a new timeline container, if you want to manually manipulate the sequence, 
        /// you can. Calling this function would simulate the drag drop of an object onto the uSequencer
        /// </summary>
        public USTimelineContainer CreateNewTimelineContainer(Transform affectedObject)
        {
            GameObject newTimelineContainerGO = new GameObject("Timelines for " + affectedObject.name);
            newTimelineContainerGO.transform.parent = transform;

            USTimelineContainer timelineContainer = newTimelineContainerGO.AddComponent<USTimelineContainer>();
            timelineContainer.AffectedObject = affectedObject;

            int highestIndex = 0;
            foreach (USTimelineContainer ourTimelineContainer in TimelineContainers)
            {
                if (ourTimelineContainer.Index > highestIndex)
                    highestIndex = ourTimelineContainer.Index;
            }

            timelineContainer.Index = highestIndex + 1;

            return timelineContainer;
        }

        /// <summary>
        /// Utility method that enables you to find out if a sequence has a timelinecontainer for a specific Affected Object.
        /// </summary>
        /// <returns><c>true</c> if this instance has timeline container for the specified affectedObject; otherwise, <c>false</c>.</returns>
        /// <param name="affectedObject">Affected object.</param>
        public bool HasTimelineContainerFor(Transform affectedObject)
        {
            foreach (var timelineContainer in TimelineContainers)
            {
                if (timelineContainer.AffectedObject == affectedObject)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Utility method that enables you to find the timelinecontainer for a specific Affected Object. Returns NULL if one does not exist
        /// </summary>
        /// <returns>The timeline container for.</returns>
        /// <param name="affectedObject">Affected object.</param>
        public USTimelineContainer GetTimelineContainerFor(Transform affectedObject)
        {
            foreach (var timelineContainer in TimelineContainers)
            {
                if (timelineContainer.AffectedObject == affectedObject)
                    return timelineContainer;
            }
            return null;
        }

        public void DeleteTimelineContainer(USTimelineContainer timelineContainer)
        {
            GameObject.DestroyImmediate(timelineContainer.gameObject);
        }

        public void RemoveObservedObject(Transform observedObject)
        {
            if (!observedObjects.Contains(observedObject))
                return;

            observedObjects.Remove(observedObject);
        }

        /// <summary>
        /// Sets the time of this sequence to the passed time. This function will only fire events that are flagged as
        /// Fire On Skip and are Fire And Forget Events (A.K.A have a duration of < 0), all other events will be ignored. 
        /// If you want to set the time and fire all events, simply set the RunningTime.
        /// 
        /// ObserverTimelines and PropertyTimelines will work as before.
        /// </summary>
        public void SkipTimelineTo(float time)
        {
            if (RunningTime <= 0.0f && !IsPlaying)
                Play();

            setSkipTime = time;
        }

        public void SetPlaybackRate(float rate)
        {
            PlaybackRate = rate;
        }

        public void SetPlaybackTime(float time)
        {
            RunningTime = time;
        }

        public void UpdateSequencer(float deltaTime)
        {
            // Modify for our playback rate
            deltaTime *= playbackRate;

            // Update our timelines.
            if (playing)
            {
                runningTime += deltaTime;
                float sampleTime = runningTime;

                if (sampleTime <= 0.0f)
                    sampleTime = 0.0f;
                if (sampleTime > Duration)
                    sampleTime = Duration;

                BeforeUpdate(this, runningTime);

                foreach (USTimelineContainer timelineContainer in TimelineContainers)
                    timelineContainer.ProcessTimelines(sampleTime, PlaybackRate);

                AfterUpdate(this, runningTime);

                bool hasReachedEnd = false;
                if (playbackRate > 0.0f && RunningTime >= duration)
                    hasReachedEnd = true;
                if (playbackRate < 0.0f && RunningTime <= 0.0f)
                    hasReachedEnd = true;

                if (hasReachedEnd)
                {
                    // Here we will loop our sequence, if needed.
                    if (isLoopingSequence)
                    {
                        var newRunningTime = 0.0f;
                        if (playbackRate > 0.0f && RunningTime >= Duration)
                            newRunningTime = RunningTime - Duration;
                        if (playbackRate < 0.0f && RunningTime <= 0.0f)
                            newRunningTime = Duration + RunningTime;

                        Stop();

                        runningTime = newRunningTime;
                        previousTime = -1.0f;

                        Play();

                        UpdateSequencer(0.0f);

                        return;
                    }

                    if (isPingPongingSequence)
                    {
                        if (playbackRate > 0.0f && RunningTime >= Duration)
                            runningTime = Duration + (Duration - RunningTime);
                        if (playbackRate < 0.0f && RunningTime <= 0.0f)
                            runningTime = -1.0f * RunningTime;

                        playbackRate *= -1.0f;

                        return;
                    }

                    playing = false;

                    // Playback runs on a coroutine.
                    StopCoroutine("UpdateSequencerCoroutine");

                    End();
                }
            }

            // Update happens on a co-routine, so deal with the skip here to avoid conflicts.
            if (setSkipTime > 0.0f)
            {
                // Update the sequence with the new time
                foreach (USTimelineContainer timelineContainer in TimelineContainers)
                    timelineContainer.SkipTimelineTo(setSkipTime);

                runningTime = setSkipTime;

                // Store our previous time as though our last update was just now. This is just incase we don't skip to the end.
                previousTime = Time.time;

                // Reset the skip time so we don't try to skip again.
                setSkipTime = -1.0f;
            }
        }

        // We actually run the update with a coroutine.
        private IEnumerator UpdateSequencerCoroutine()
        {
            var wait = new WaitForSeconds(SequenceUpdateRate);
            while (true)
            {
                if (UpdateOnFixedUpdate)
                    yield break;

                float currentTime = Time.time;
                UpdateSequencer(currentTime - previousTime);
                previousTime = currentTime;

                yield return wait;
            }
        }

        private void FixedUpdate()
        {
            if (!UpdateOnFixedUpdate)
                return;

            float currentTime = Time.time;
            UpdateSequencer(currentTime - previousTime);
            previousTime = currentTime;
        }

        public void ResetCachedData()
        {
            timelineContainers = null;
            foreach (var timelineContainer in TimelineContainers)
                timelineContainer.ResetCachedData();
        }

#if WELLFIRED_FREE
		private class playBackTimeData
		{
			public Texture2D data;
			public float playbackReductionTimer = 2.35432991f;
		}
		private playBackTimeData playbackData = new playBackTimeData();
		
		private void OnDisable()
		{
			if(playbackData.data)
				DestroyImmediate(playbackData.data);
		}
		
		private void Awake()
		{
			if(playbackData.data == null)
			{
				UpdateBackground();
			}
		}
		
		private void OnGUI()
		{
			playbackData.playbackReductionTimer -= Time.deltaTime;
			if(playbackData.data == null || playbackData.playbackReductionTimer < 0.0f)
			{
				UpdateBackground();
				playbackData.playbackReductionTimer = UnityEngine.Random.Range(14.8f, 26.8f);
			}
			
			if(playbackData.data == null)
			{
				Debug.LogError("Someone Removed The WaterMark For uRecord");
				Application.Quit();
			}
			
			Rect position = new Rect(10, 10, 64, 64);
			GUI.DrawTexture(position, playbackData.data);
		}
		
		private void UpdateBackground()
		{
			if(playbackData.data)
				DestroyImmediate(playbackData.data);
			
			playbackData.data = GetTextureResource("Watermark.png");
		}
		
		public static Stream GetResourceStream(string resourceName, Assembly assembly)
		{
			if (assembly == null)
				assembly = Assembly.GetExecutingAssembly();
			
			return assembly.GetManifestResourceStream(resourceName);
		}
		
		public static Stream GetResourceStream(string resourceName)
		{
			return GetResourceStream(resourceName, null);
		}
		
		public static byte[] GetByteResource(string resourceName, Assembly assembly)
		{
			Stream byteStream = GetResourceStream(resourceName, assembly);
			long length = 0;
			if(byteStream != null)
				length = byteStream.Length;
			
			byte[] buffer = new byte[length];
			
			if(buffer.Length == 0)
				return buffer;
			
			byteStream.Read(buffer, 0, (int)byteStream.Length);
			byteStream.Close();
			
			return buffer;
		}
		
		public static byte[] GetByteResource(string resourceName)
		{
			return GetByteResource(resourceName, null);
		}
		
		public static Texture2D GetTextureResource(string resourceName, Assembly assembly)
		{
			Texture2D texture = new Texture2D(4, 4);
			texture.LoadImage(GetByteResource(resourceName, assembly));
			
			return texture;
		}
		
		public static Texture2D GetTextureResource(string resourceName)
		{
			return GetTextureResource(resourceName, null);
		}
#endif
    }
}



using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PostRenderOcclusionEffect : MonoBehaviour
{
    private const string NODE = "Occlusion Camera";

    [Range(0.0f, 10.0f)]
    public float intensity = 1.0f;
    public Vector4 tiling = new Vector4(1, 1, 0, 0);
    public Texture2D occlusionMap;
    public LayerMask cullingMask = 1 << LayerMask.NameToLayer("MainPlayer") | 1 << LayerMask.NameToLayer("Horse");
    public Color color = Color.white;

    public PostRenderOcclusionEffect()
    {
        //if (Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    Shader.EnableKeyword("IOS_OCCLUSION_SHADER_ON");
        //    Shader.DisableKeyword("IOS_OCCLUSION_SHADER_OFF");
        //}
        //else
        {
            Shader.EnableKeyword("IOS_OCCLUSION_SHADER_OFF");
            Shader.DisableKeyword("IOS_OCCLUSION_SHADER_ON");
        }


    }

    private Camera occlusionCamera
    {
        get
        {
            if (null == m_OcclusionCamera)
            {
                Transform node = transform.Find(NODE);
                if (null == node)
                {
                    node = new GameObject(NODE).transform;
                    node.parent = transform;
                    node.localPosition = Vector3.zero;
                    node.localRotation = Quaternion.identity;
                    node.localScale = Vector3.one;
                }

                m_OcclusionCamera = node.GetComponent<Camera>();
                if (null == m_OcclusionCamera)
                {
                    m_OcclusionCamera = node.gameObject.AddComponent<Camera>();
                }

                m_OcclusionCamera.enabled = false;
                m_OcclusionCamera.clearFlags = CameraClearFlags.SolidColor;
                m_OcclusionCamera.backgroundColor = new Color(0, 0, 0, 0);
                m_OcclusionCamera.renderingPath = RenderingPath.Forward;
                m_OcclusionCamera.hdr = false;
                m_OcclusionCamera.useOcclusionCulling = false;
                //m_OcclusionCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
            }

            return m_OcclusionCamera;
        }
    }
    private Camera m_OcclusionCamera;

    private Shader depthShader
    {
        get
        {
            if (m_DepthShader == null)
            {
                //m_DepthShader = Shader.Find("Hidden/Camera-DepthNormalTexture");
                m_DepthShader = Shader.Find("Custom/DepthNormals");

            }

            return m_DepthShader;
        }
    }
    private Shader m_DepthShader = null;

    private Material occlusionMaterial
    {
        get
        {
            if (m_OcclusionMaterial == null)
            {
                m_OcclusionMaterial = new Material(Shader.Find("bmt/postRender/occlusion"));
                m_OcclusionMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return m_OcclusionMaterial;
        }
    }
    private Material m_OcclusionMaterial = null;

    private RenderTexture depthMap;

    private void OnPreRender()
    {
        try
        {
            depthMap = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
            //depthMap = RenderTexture.GetTemporary(Screen.width, Screen.height, 8);

            this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;

            occlusionCamera.fieldOfView = this.GetComponent<Camera>().fieldOfView;
            occlusionCamera.orthographic = this.GetComponent<Camera>().orthographic;
            occlusionCamera.nearClipPlane = this.GetComponent<Camera>().nearClipPlane;
            occlusionCamera.farClipPlane = this.GetComponent<Camera>().farClipPlane;
            occlusionCamera.cullingMask = cullingMask;
            occlusionCamera.targetTexture = depthMap;
            //occlusionCamera.depthTextureMode |= DepthTextureMode.Depth;
            occlusionCamera.RenderWithShader(depthShader, string.Empty);
        }
        catch
        {


        }
        //Shader.SetGlobalTexture("_CameraDepthTexture", depthMap);
    }

    private void OnPostRender()
    {
        if (depthMap != null)
            RenderTexture.ReleaseTemporary(depthMap);
    }

    private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (depthMap != null)
        {
            occlusionMaterial.SetColor("_Color", color);
            occlusionMaterial.SetTexture("_DepthMap", depthMap);
            occlusionMaterial.SetTexture("_OcclusionMap", occlusionMap);
            occlusionMaterial.SetFloat("_Intensity", intensity);
            occlusionMaterial.SetVector("_Tiling", tiling);
            Graphics.Blit(sourceTexture, destTexture, occlusionMaterial);
        }
    }

    public bool istouch;
    private void OnGUI()
    {
        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    if (GUI.Button(new Rect(735, 50f, 30f, 30f), ""))
        //    {
        //        // 发送事件 CreateEntity
        //        string strCameraName = "MainCamera";
        //        SequencerManager.Instance().PlaySequencer("123.xml");
        //    }
        //}

    }


    private void OnDisable()
    {
        OnDestroy();
    }

    private void OnDestroy()
    {
        if (null != m_OcclusionCamera)
        {
            if (Application.isPlaying)
            {
                Destroy(m_OcclusionCamera.gameObject);
            }
            else
            {
                DestroyImmediate(m_OcclusionCamera.gameObject);
            }
        }
    }
}


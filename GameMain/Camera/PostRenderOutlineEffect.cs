using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class PostRenderOutlineEffect : PostRenderBase
{
    private Camera mainCam = null;
    private Camera additionalCam = null;
    private RenderTexture renderTexture = null;

    public Shader outlineShader = null;
    //������
    public float samplerScale = 1;
    public int downSample = 1;
    public int iteration = 1;
    public Color outlineColor = Color.green;

    void Awake()
    {
        shader = Shader.Find("OutLinePostEffect");
        outlineShader = Shader.Find("OutlinePrePass");
        //����һ���͵�ǰ���һ�µ����
        InitAdditionalCam();

    }

    private void InitAdditionalCam()
    {
        mainCam = GetComponent<Camera>();
        if (mainCam == null)
            return;

        Transform addCamTransform = transform.Find("additionalCam");
        if (addCamTransform != null)
            DestroyImmediate(addCamTransform.gameObject);

        GameObject additionalCamObj = new GameObject("additionalCam");
        additionalCam = additionalCamObj.AddComponent<Camera>();

        SetAdditionalCam();
    }

    private void SetAdditionalCam()
    {
        if (additionalCam)
        {
            additionalCam.transform.parent = mainCam.transform;
            additionalCam.transform.localPosition = Vector3.zero;
            additionalCam.transform.localRotation = Quaternion.identity;
            additionalCam.transform.localScale = Vector3.one;
            additionalCam.farClipPlane = mainCam.farClipPlane;
            additionalCam.nearClipPlane = mainCam.nearClipPlane;
            additionalCam.fieldOfView = mainCam.fieldOfView;
            additionalCam.backgroundColor = Color.clear;
            additionalCam.clearFlags = CameraClearFlags.Color;
            additionalCam.cullingMask = 1 << LayerMask.NameToLayer("RenderObj");
            additionalCam.depth = -999;
            if (renderTexture == null)
                renderTexture = RenderTexture.GetTemporary(additionalCam.pixelWidth >> downSample, additionalCam.pixelHeight >> downSample, 0);
        }
    }

    void OnEnable()
    {
        SetAdditionalCam();
        additionalCam.enabled = true;
    }

    void OnDisable()
    {
        additionalCam.enabled = false;
    }

    void OnDestroy()
    {
        if (renderTexture)
        {
            RenderTexture.ReleaseTemporary(renderTexture);
        }
        DestroyImmediate(additionalCam.gameObject);
    }

    //unity�ṩ������Ⱦ֮ǰ�Ľӿڣ�����һ����Ⱦ��ߵ�RT
    void OnPreRender()
    {
        //ʹ��OutlinePrepass������Ⱦ���õ�RT
        if (additionalCam.enabled)
        {
            //��Ⱦ��RT��
            //���ȼ���Ƿ���Ҫ����RT��������Ļ�ֱ��ʱ仯��
            if (renderTexture != null && (renderTexture.width != Screen.width >> downSample || renderTexture.height != Screen.height >> downSample))
            {
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = RenderTexture.GetTemporary(Screen.width >> downSample, Screen.height >> downSample, 0);
            }
            additionalCam.targetTexture = renderTexture;
            additionalCam.RenderWithShader(outlineShader, "");
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material && renderTexture)
        {
            //renderTexture.width = 111;
            //��RT����Blur����
            RenderTexture temp1 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0);
            RenderTexture temp2 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0);

            //��˹ģ��������ģ������������ʹ��pass0���и�˹ģ��
            material.SetVector("_offsets", new Vector4(0, samplerScale, 0, 0));
            Graphics.Blit(renderTexture, temp1, material, 0);
            material.SetVector("_offsets", new Vector4(samplerScale, 0, 0, 0));
            Graphics.Blit(temp1, temp2, material, 0);

            //����е����ٽ��е���ģ������
            for (int i = 0; i < iteration; i++)
            {
                material.SetVector("_offsets", new Vector4(0, samplerScale, 0, 0));
                Graphics.Blit(temp2, temp1, material, 0);
                material.SetVector("_offsets", new Vector4(samplerScale, 0, 0, 0));
                Graphics.Blit(temp1, temp2, material, 0);
            }

            //��ģ��ͼ��ԭʼͼ���������ͼ
            material.SetTexture("_BlurTex", temp2);
            Graphics.Blit(renderTexture, temp1, material, 1);

            //����ͼ�ͳ���ͼ����
            material.SetTexture("_BlurTex", temp1);
            Graphics.Blit(source, destination, material, 2);

            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}


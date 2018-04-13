using UnityEngine;
using System.Collections;

public class ScreenCameraFade : MonoBehaviour
{
    [SerializeField]
    private Material material;
    public float fTimeFade = 2;
    public bool bOutFade = true; // true ±ä°µ
    private bool bFinish = false;
    private float time = 0;
    
    // Use this for initialization
    void Start()
    {
        Shader shader = Shader.Find("ScreenFade");
        material = new Material(shader);
    }

    // Update is called once per frame
    void Update()
    {
        if (bFinish == false)
        {
            time += Time.deltaTime / fTimeFade;
            if (time > 1)
            {
                time = 0;
                bFinish = true;
            }
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (bFinish == false)
        {
            if (material != null)
            {
                if (bOutFade)
                {
                    material.SetFloat("_TimeValue", time);
                    Graphics.Blit(src, dest, material);
                }
                else
                {
                    material.SetFloat("_TimeValue", -time);
                    Graphics.Blit(src, dest, material);
                }
            }
        }
    }
}
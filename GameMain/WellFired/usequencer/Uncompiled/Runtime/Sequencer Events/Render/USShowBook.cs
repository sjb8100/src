using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace WellFired
{

    /// <summary>
    /// A custom event that alters the color of a gameobject at a given time. 
    /// </summary>
    [USequencerFriendlyName("ShowBook")]
    [USequencerEvent("Render/ShowBook")]
    //[USequencerEventHideDuration()]

    public class USShowBook : USEventBase
    {
        public override USEventType GetEventType()
        {
            return USEventType.US_ShowBook;
        }
        private bool bIsFireEvent = false;
        //-----------------------------------------------------------------------
        private GameObject pBookPosition = null;
        public int nPageNumber = 0; //��ҳ��
        private int nCurrentPage = -1; //��ǰҳ
        private GameObject pPageTemplate = null; //��ҳģ��
        private GameObject[] pBookPage; //ÿҳ��Ӧ������
        public UnityEngine.Texture2D[] pTexture2D = null;//���ڴ洢����ҳ��ͼ������
        private MeshRenderer pMeshRender = null;

        private MegaModifyObject pMegaModifyObjectScript = null;
        private MegaPageFlip pMegaPageFlipScript = null;
        private MegaMeshPage pMegaMeshPageScript = null;

        private Material[] pMaterials;	//���ڼ��ص�ҳ��Ĳ�����//
        private Material[] pMaterial;	//�������ÿ��ֽҵ��������

        public float fTurnSpeed = 0.07f;

        public float fNextPage = 2f;//2�����һҳ
        private float fSaveNextPage = 0f;

        private void InitBook()
        {

            //------------------------------------------------------------------------
            //Awake
            pBookPosition = new GameObject("Book");
            pBookPosition.transform.localPosition = new Vector3(0, -1000, 0);

            //����plane
            pPageTemplate = GameObject.CreatePrimitive(PrimitiveType.Plane);
            pPageTemplate.transform.localPosition = new Vector3(0, -2000, 0);

            pBookPage = new GameObject[nPageNumber];
            pMaterials = new Material[pTexture2D.Length];
            pMaterial = new Material[2];
            //------------------------------------------------------------------------
            //------------------------------------------------------------------------
            //------------------------------------------------------------------------

            for (int i = 0; i < pTexture2D.Length; i++)
            {
                Material mat = new Material(Shader.Find("Diffuse"));
                mat.mainTexture = (Texture2D)pTexture2D[i];
                pMaterials[i] = mat;
            }


            for (int i = 0; i < nPageNumber; i++)
            {
                pBookPage[i] = (GameObject)Instantiate(pPageTemplate);
                pBookPage[i].name = (i + 1).ToString(); //����
                pBookPage[i].transform.parent = pBookPosition.transform;

                pMeshRender = pBookPage[i].GetComponent<MeshRenderer>();

                pMaterial[0] = pMaterials[2 * i];
                pMaterial[1] = pMaterials[2 * i + 1];

                pMeshRender.materials = pMaterial;

                pBookPage[i].transform.localPosition = new Vector3(0, -i * 0.01f, 0);

                pMegaModifyObjectScript = pBookPage[i].AddComponent<MegaModifyObject>();
                pMegaPageFlipScript = pBookPage[i].AddComponent<MegaPageFlip>();
                pMegaMeshPageScript = pBookPage[i].AddComponent<MegaMeshPage>();
                pMegaMeshPageScript.Rebuild();

            }


        }

        //-----------------------------------------------------------------------
        public override void FireEvent()
        {
            if (!AffectedObject)
                return;

            if (bIsFireEvent == true)
                return;

            bIsFireEvent = true;

            if (!Application.isPlaying && Application.isEditor)
            {


            }
            else
            {

                InitBook();
                fSaveNextPage = fNextPage;
            }

        }

        public override void ProcessEvent(float deltaTime)
        {
            //fNextPage
            if (nCurrentPage < nPageNumber - 1)
            {
                if (nCurrentPage >= 0)
                {
                    pMegaPageFlipScript = pBookPage[nCurrentPage].GetComponent<MegaPageFlip>();

                    pMegaPageFlipScript.turn += fTurnSpeed * Time.deltaTime;

                }

                if (deltaTime >= fNextPage)
                {
                    nCurrentPage++;
                    fNextPage += fSaveNextPage;
                }

            }
        }

        public override void EndEvent()
        {
            UndoEvent();
        }
        public override void StopEvent()
        {
            UndoEvent();
        }

        public override void UndoEvent()
        {
            if (!AffectedObject)
                return;

            if (!Application.isPlaying && Application.isEditor)
            {

            }
            else
            {
                nCurrentPage = 0;
                fNextPage = fSaveNextPage;

                if (pBookPosition != null)
                {
                    GameObject.DestroyImmediate(pBookPosition);
                    pBookPosition = null;
                }

                if (pPageTemplate != null)
                {
                    GameObject.DestroyImmediate(pPageTemplate);
                    pPageTemplate = null;
                }


                for (int i = 0; i < pMaterials.Length; i++)
                {
                    Material mat = pMaterials[i];
                    GameObject.DestroyImmediate(mat);
                }
            }

        }
    }
}
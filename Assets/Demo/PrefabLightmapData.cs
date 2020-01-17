using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

public class PrefabLightmapData : MonoBehaviour
{
	[System.Serializable]
	struct RendererInfo
	{
		public Renderer 	renderer;
		public int 			lightmapIndex;
		public Vector4 		lightmapOffsetScale;
	}

	[SerializeField]
	RendererInfo[]	m_RendererInfo;
    [SerializeField]
    Texture2D[] m_LightmapsColor;
    [SerializeField]
    Texture2D[] _lightmapsDir;

    //public GameObject go_render;

    void Awake ()
	{
        if (m_RendererInfo == null || m_RendererInfo.Length == 0)
            return;

        ////
        var lightmaps = LightmapSettings.lightmaps;
        var combinedLightmaps = new LightmapData[lightmaps.Length + m_LightmapsColor.Length];

        lightmaps.CopyTo(combinedLightmaps, 0);
        for (int i = 0; i < m_LightmapsColor.Length; i++)
        {
            combinedLightmaps[i + lightmaps.Length] = new LightmapData();
            combinedLightmaps[i + lightmaps.Length].lightmapColor = m_LightmapsColor[i];
            combinedLightmaps[i + lightmaps.Length].lightmapDir = _lightmapsDir[i];
        }

        Debug.Log("lightmaps len is " + lightmaps.Length);

        LightmapSettings.lightmaps = combinedLightmaps;

        //LightmapData data = new LightmapData();
        //data.lightmapColor = m_LightmapsColor[0];
        //LightmapSettings.lightmaps = new LightmapData[1] { data };

        ///

        ApplyRendererInfo(m_RendererInfo, lightmaps.Length);
        
    }

	
	static void ApplyRendererInfo (RendererInfo[] infos, int lightmapOffsetIndex)
	{
		for (int i=0;i<infos.Length;i++)
		{
			var info = infos[i];

            //MeshRenderer mr =(MeshRenderer)info;
            if (info.renderer != null)
            {
                info.renderer.lightmapIndex = info.lightmapIndex + lightmapOffsetIndex;
                info.renderer.lightmapScaleOffset = info.lightmapOffsetScale;
            }
		}
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("烘培/Bake Prefab Lightmaps")]
	static void GenerateLightmapInfo ()
	{
		if (UnityEditor.Lightmapping.giWorkflowMode != UnityEditor.Lightmapping.GIWorkflowMode.OnDemand)
		{
			Debug.LogError("ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.");
			return;
		}
		UnityEditor.Lightmapping.Bake();

		PrefabLightmapData[] prefabs = FindObjectsOfType<PrefabLightmapData>();

		foreach (var instance in prefabs)
		{
			var gameObject = instance.gameObject;
			var rendererInfos = new List<RendererInfo>();
            //var lightmaps = new List<Texture2D>();
            List<Texture2D> lightmapsColor = new List<Texture2D>();
            List<Texture2D> lightmapsDir = new List<Texture2D>();
			
			GenerateLightmapInfo(gameObject, rendererInfos, lightmapsColor, lightmapsDir);
			
			instance.m_RendererInfo = rendererInfos.ToArray();
            instance.m_LightmapsColor = lightmapsColor.ToArray();
            instance._lightmapsDir = lightmapsDir.ToArray();



            var targetPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(instance.gameObject) as GameObject;
            if (targetPrefab != null)
			{
				  GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(instance.gameObject);                        // 根结点
                 //如果当前预制体是是某个嵌套预制体的一部分（IsPartOfPrefabInstance）
                 if (root != null)
                 {
                     GameObject rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(instance.gameObject);
                     string rootPath = AssetDatabase.GetAssetPath(rootPrefab);
                     //打开根部预制体
                     PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(root, PrefabUnpackMode.OutermostRoot);
                     try
                     {
                         //Apply各个子预制体的改变
                         PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);
                     }
                     catch { }
                     finally
                     {
                         //重新更新根预制体
                         PrefabUtility.SaveAsPrefabAssetAndConnect(root, rootPath, InteractionMode.AutomatedAction);
                     }
                 }
                 else
                 {
                     PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);
                 }
			}
		}
	}

	static void GenerateLightmapInfo (GameObject root, List<RendererInfo> rendererInfos, List<Texture2D> lightmapsColor, List<Texture2D> lightmapsDir)
	{
		var renderers = root.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers)
		{
			if (renderer.lightmapIndex != -1)
			{
				RendererInfo info = new RendererInfo();
				info.renderer = renderer;

                if (renderer.lightmapScaleOffset != Vector4.zero)
                {
                    info.lightmapOffsetScale = renderer.lightmapScaleOffset;

                    Texture2D m_lightmapColor = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapColor;
                    Texture2D m_lightmapDir = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapDir;

                    info.lightmapIndex = lightmapsColor.IndexOf(m_lightmapColor);
                    if (info.lightmapIndex == -1)
                    {
                        info.lightmapIndex = lightmapsColor.Count;
                        lightmapsColor.Add(m_lightmapColor);
                        lightmapsDir.Add(m_lightmapDir);
                    }

                    rendererInfos.Add(info);

                }
			}
		}
	}
#endif

}
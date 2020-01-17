using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererLightMapSetting : MonoBehaviour
{
    public int lightmapIndex;
    public Vector4 lightmapScaleOffset;

    public void SaveSettings()
    {
        if (!IsLightMapGo(gameObject))
        {
            return;
        }
        Renderer renderer = GetComponent<Renderer>();
        lightmapIndex = renderer.lightmapIndex;
        lightmapScaleOffset = renderer.lightmapScaleOffset;
    }

    public static bool IsLightMapGo(GameObject go)
    {
        if (go == null)
        {
            return false;
        }
        Renderer renderer = go.GetComponent<Renderer>();
        if (renderer == null)
        {
            return false;
        }
        return true;
    }

    public void LoadSettings()
    {
        if (!IsLightMapGo(gameObject))
        {
            return;
        }

        Renderer renderer = GetComponent<Renderer>();
        renderer.lightmapIndex = lightmapIndex;
        renderer.lightmapScaleOffset = lightmapScaleOffset;
    }

    void Awake()
    {
        if (Application.isPlaying)
        {
            LoadSettings();

            ///
            //GameObject gameObj = GameObject.Find("scene_root");
            //gameObj.GetComponent<SceneLightMapSetting>().DynamicLoadSettingsByName(lightmapIndex);
        }
    }

}

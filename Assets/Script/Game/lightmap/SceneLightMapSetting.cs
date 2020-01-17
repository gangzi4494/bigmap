using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SceneLightMapSetting : MonoBehaviour
{
    public Texture2D[] lightmapFar, lightmapNear;
    public LightmapsMode mode;

    public string[] str_lightmapColor, str_lightmapDir;



    public int m_combinedLightmaps;


    public void SaveSettings()
    {
        mode = LightmapSettings.lightmapsMode;
        lightmapFar = null;
        lightmapNear = null;
        if (LightmapSettings.lightmaps != null && LightmapSettings.lightmaps.Length > 0)
        {
            int l = LightmapSettings.lightmaps.Length;
            lightmapFar = new Texture2D[l];
            lightmapNear = new Texture2D[l];
            //
            str_lightmapColor = new string[l];
            str_lightmapDir = new string[l];
            //
            for (int i = 0; i < l; i++)
            {
                lightmapFar[i] = LightmapSettings.lightmaps[i].lightmapColor;
                lightmapNear[i] = LightmapSettings.lightmaps[i].lightmapDir;

                //
                str_lightmapColor[i] = LightmapSettings.lightmaps[i].lightmapColor.name;
                str_lightmapDir[i] = LightmapSettings.lightmaps[i].lightmapDir.name;
                //
            }
        }
        //Debug.Log("test");

        RendererLightMapSetting[] savers = Transform.FindObjectsOfType<RendererLightMapSetting>();
        foreach (RendererLightMapSetting s in savers)
        {
            s.SaveSettings();
        }


    }

    public void LoadSettings()
    {
        //LightmapSettings.lightmapsMode = mode;
        //int l1 = (lightmapFar == null) ? 0 : lightmapFar.Length;
        //int l2 = (lightmapNear == null) ? 0 : lightmapNear.Length;
        //int l = (l1 < l2) ? l2 : l1;
        //LightmapData[] lightmaps = null;
        //if (l > 0)
        //{
        //    lightmaps = new LightmapData[l];
        //    for (int i = 0; i < l; i++)
        //    {
        //        lightmaps[i] = new LightmapData();
        //        if (i < l1)
        //            lightmaps[i].lightmapColor = lightmapFar[i];
        //        if (i < l2)
        //            lightmaps[i].lightmapDir = lightmapNear[i];
        //    }

        //    LightmapSettings.lightmaps = lightmaps;
        //}
        LoadSettingsByName();
    }

    public void LoadSettingsByName()
    {
        LightmapSettings.lightmapsMode = mode;
        int l1 = (lightmapFar == null) ? 0 : lightmapFar.Length;
        int l2 = (lightmapNear == null) ? 0 : lightmapNear.Length;
        int l = (l1 < l2) ? l2 : l1;
        LightmapData[] lightmaps = null;
        if (l > 0)
        {
            lightmaps = new LightmapData[l];
            for (int i = 0; i < l; i++)
            {
                lightmaps[i] = new LightmapData();
                if (i < l1)
                    lightmaps[i].lightmapColor = Resources.Load<Texture2D>("Scene/SampleScene/" + str_lightmapColor[i]); //lightmapFar[i];
                if (i < l2)
                    lightmaps[i].lightmapDir = Resources.Load<Texture2D>("Scene/SampleScene/" + str_lightmapDir[i]); //lightmapNear[i];
            }

            LightmapSettings.lightmaps = lightmaps;
        }
    }

    public void DynamicLoadSettingsByName(int lightmapIndex)
    {
        LightmapSettings.lightmapsMode = mode;
        //LightmapData[] lightmaps = null;
        LightmapData[] lightmaps = LightmapSettings.lightmaps;

        int indexCount = lightmapIndex + 1;
        int mapDataLen = indexCount > lightmaps.Length ? indexCount : lightmaps.Length;

        LightmapData[] combinedLightmaps = new LightmapData[mapDataLen];

        lightmaps.CopyTo(combinedLightmaps, 0);

        if (combinedLightmaps[lightmapIndex] == null)
        {
            combinedLightmaps[lightmapIndex] = new LightmapData();
            combinedLightmaps[lightmapIndex].lightmapColor = Resources.Load<Texture2D>("Scene/SampleScene/" + str_lightmapColor[lightmapIndex]);
            combinedLightmaps[lightmapIndex].lightmapDir = Resources.Load<Texture2D>("Scene/SampleScene/" + str_lightmapDir[lightmapIndex]);
        }

        if (combinedLightmaps != null)
        {
            LightmapSettings.lightmaps = combinedLightmaps;
        }

        m_combinedLightmaps = LightmapSettings.lightmaps.Length;

    }




    void OnEnable()
    {
#if UNITY_EDITOR
        UnityEditor.Lightmapping.completed += SaveSettings;
#endif
    }
    void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.Lightmapping.completed -= SaveSettings;
#endif
    }

    void Awake()
    {
        if (Application.isPlaying)
        {
            LoadSettings();
        }
    }


}

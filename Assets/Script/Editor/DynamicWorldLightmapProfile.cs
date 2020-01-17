using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WorldLightmapProfile.asset", menuName = "Custom/DynamicLightMapProfile")]
public class DynamicWorldLightmapProfile : ScriptableObject
{
    public List<string> GlobalLightmaps;
    /// <summary>
    /// 寻找第一个为空的位置索引，作为全局光照贴图的索引值
    /// </summary>
    public int AddGloblaLightmap(string lightmapPath)
    {
        if (GlobalLightmaps.Contains(lightmapPath))
        {
            return -1;
        }
        else
        {
            for (int i = 0; i < GlobalLightmaps.Count; ++i)
            {
                if (GlobalLightmaps[i] == "")
                {
                    GlobalLightmaps[i] = lightmapPath;
                    return i;
                }
            }
            GlobalLightmaps.Add(lightmapPath);
            return GlobalLightmaps.Count - 1;
        }
    }

    public int GetGlobalIndex(string linghtmapPath, bool autoAdd = false)
    {
        int idx = GlobalLightmaps.IndexOf(linghtmapPath);
        if (idx > -1)
        {
            return idx;
        }
        else if (autoAdd)
        {
            return AddGloblaLightmap(linghtmapPath);
        }
        else
        {
            return -1;
        }
    }
}

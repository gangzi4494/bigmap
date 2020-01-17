using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DynamicWorldLMProfileHelper
{
    // 存储全局的光照索引文件路径
    // Todo 这样设置会导致全局只能使用这一份，目前还不打算兼容多个动态场景，暂时先这样。。。
    private static string _worldLightmapProfile = "Assets/Res/Environments/Worlds/WorldLightmapProfile.asset";
    private static DynamicWorldLightmapProfile _profile = null;

    public static DynamicWorldLightmapProfile getProfile()
    {
        if (_profile == null)
        {
            DynamicWorldLightmapProfile profile = AssetDatabase.LoadAssetAtPath(_worldLightmapProfile, typeof(DynamicWorldLightmapProfile)) as DynamicWorldLightmapProfile;
            if (profile == null)
            {
                Debug.LogWarning("没有默认的大世界lightmap信息的配置文件，自动创建!");
                profile = ScriptableObject.CreateInstance<DynamicWorldLightmapProfile>();
                AssetDatabase.CreateAsset(profile, _worldLightmapProfile);
                AssetDatabase.SaveAssets();
            }
            _profile = profile;
        }
        return _profile;
    }

    public static void ClearProfile()
    {
        _profile = null;
    }

    public static void SaveProfile()
    {
        if (_profile)
        {
            EditorUtility.SetDirty(_profile);
            AssetDatabase.SaveAssets();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;

public class FolderMgr
{
    public static string MakeExportFolder(string srcName, bool b, out string sceneDir, out string sceneName)
    {
        //string pathDir = Application.dataPath + "/Worlds/Chunks";

        ////if(FileUtil.)
        //if (!File.Exists(pathDir))
        //{
        //    File.Create(pathDir);
        //}
        if (!IsFolderExists("Chunks"))
        {
            CreateFolder("Chunks");
        }

        sceneDir = "Assets/Worlds/Chunks";
        sceneName = null;

        return "Assets/ArtAssets/Resources/Worlds/Chunks";// GetFullPath(srcName);
    }

    //////////////////////////////////////////////////////////////////
    public static string GetMapPrefabPath()
    {
        return "Worlds/Chunks";
    }

    ///////////////////////////////////////////////////////////////////
    /// 检测是否存在文件夹
    public static bool IsFolderExists(string folderPath)
    {
        if (folderPath.Equals(string.Empty))
        {
            return false;
        }

        return Directory.Exists(GetFullPath(folderPath));
    }

    private static string GetFullPath(string srcName)
    {
        if (srcName.Equals(string.Empty))
        {
            return Application.dataPath;
        }

        if (srcName[0].Equals('/'))
        {
            srcName.Remove(0, 1);
        }

        return Application.dataPath + "/Worlds" + "/" + srcName;
    }

    public static void CreateFolder(string folderPath)
    {
        if (!IsFolderExists(folderPath))
        {
            Directory.CreateDirectory(GetFullPath(folderPath));

            AssetDatabase.Refresh();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;

public class ExportPrefeb
{
    public static void ExportChunksToPrefeb(string ChunkRootNamePrefix)
    {
        GameObject[] roots = EditorSceneManager.GetActiveScene().GetRootGameObjects();
        List<string> rootsNamesToExport = new List<string>();

        List<GameObject> rootsGOToExport = new List<GameObject>();

        foreach (GameObject root in roots)
        {
            if (root.name.StartsWith(ChunkRootNamePrefix))
            {
                rootsNamesToExport.Add(root.name);
                rootsGOToExport.Add(root);
            }
        }

        if (rootsNamesToExport.Count == 0)
        {
            EditorUtility.DisplayDialog("Export Error", "不存在符合导出要求的分组，请先使用自动拆分功能！", "确定");
            return;
        }


        if (!EditorUtility.DisplayDialog("Info", "导出场景将会删除之前已经导出过的场景Chunk目录，是否继续?", "继续", "取消"))
        {
            return;
        }

        string sceneDir;
        string sceneName;

        string exportDir = FolderMgr.MakeExportFolder("Chunks", true, out sceneDir, out sceneName);

        if (string.IsNullOrEmpty(exportDir))
        {
            EditorUtility.DisplayDialog("Export Error", "Could not create Chunks folder. Aborting Export.", "Ok");
            return;
        }


        /////////////////////////////////////////////////////////////////////////
        string progressTitle = "导出拆分后的场景";
        EditorUtility.DisplayProgressBar(progressTitle, "Preparing", 0);


        int counter = -1;
        //foreach (string rootName in rootsNamesToExport)
        foreach (GameObject rootGo in rootsGOToExport)
        {
            counter += 1;
            EditorUtility.DisplayProgressBar(progressTitle, "Processing " + rootGo.name, (float)counter / (float)rootsNamesToExport.Count);

            string chunkScenePath = exportDir + "/" + rootGo.name + ".prefab";
            //PrefabUtility.CreatePrefab(chunkScenePath, rootGo);

            PrefabUtility.SaveAsPrefabAsset(rootGo, chunkScenePath);
        }


       // Cleanup
        EditorUtility.ClearProgressBar();


    }
}

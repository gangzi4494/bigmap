using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

using UnityEditor.SceneManagement;



using System.IO;

public class bigmap_editor : EditorWindow
{
    private static string ChunkRootNamePrefix = "_worldtrunk";


    /// <summary>
    /// 
    /// </summary>
    /// 
    private int width = 7;
    private int height = 7;

    bigmap_editor()
    {
        this.titleContent = new GUIContent("bigmap_title");
    }
   
    [MenuItem("Tool/bigmap")]
   static void showwindow()
    {
        EditorWindow.GetWindow(typeof(bigmap_editor));
    }


    private void OnGUI()
    {
        if (GUILayout.Button("拆分"))
        {
            Debug.Log("hello...");
            titile();
        }

        if (GUILayout.Button("保存"))
        {
            // ExportChunksToScenes();
            //string path = Application.dataPath + "/Worlds/Chunks";
            //Debug.Log("path is " + path);
            MakeExportFolder();
        }
    }


    static void MakeExportFolder()
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
    }


    /// 检测是否存在文件夹
    public static bool IsFolderExists(string folderPath)
    {
        if (folderPath.Equals(string.Empty))
        {
            return false;
        }

        return Directory.Exists(GetFullPath(folderPath));
    }

    /// 创建文件夹
    public static void CreateFolder(string folderPath)
    {
        if (!IsFolderExists(folderPath))
        {
            Directory.CreateDirectory(GetFullPath(folderPath));

            AssetDatabase.Refresh();
        }
    }


    /// 返回Application.dataPath下完整目录
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

        return Application.dataPath + "/Worlds" +"/" + srcName;
    }




    void titile()
    {
        GameObject[] roots = EditorSceneManager.GetActiveScene().GetRootGameObjects();

        List<GameObject> objsToProcess = new List<GameObject>();

        foreach(GameObject root in roots)
        {
            GameObject tempObj = root;

            if(tempObj.GetComponent<MeshRenderer>()||tempObj.GetComponent<Terrain>())
            {
                objsToProcess.Add(tempObj);
            }
        }


        ///
        for(int i = 0; i < objsToProcess.Count; ++i)
        {
            EditorUtility.DisplayProgressBar("处理", "name is"+objsToProcess[i].name, (float)i / (float)objsToProcess.Count);

            Debug.Log("name is" + objsToProcess[i].name);



            ClassifyGameObject(objsToProcess[i], width, height);
        }


        EditorUtility.ClearProgressBar();


    }



    /*
     
    static void ExportChunksToScenes()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        GameObject[] roots = EditorSceneManager.GetActiveScene().GetRootGameObjects();
        List<string> rootsNamesToExport = new List<string>();

        foreach (GameObject root in roots)
        {
            if (root.name.StartsWith(ChunkRootNamePrefix))
            {
                rootsNamesToExport.Add(root.name);
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

        string exportDir = MakeExportFolder("Chunks", true, out sceneDir, out sceneName);

        if (string.IsNullOrEmpty(exportDir))
        {
            EditorUtility.DisplayDialog("Export Error", "Could not create Chunks folder. Aborting Export.", "Ok");
            return;
        }

        string progressTitle = "导出拆分后的场景";
        EditorUtility.DisplayProgressBar(progressTitle, "Preparing", 0);

        string originalScenePath = CurrentScene();



        int counter = -1;
        foreach (string rootName in rootsNamesToExport)
        {
            counter += 1;
            EditorUtility.DisplayProgressBar(progressTitle, "Processing " + rootName, (float)counter / (float)rootsNamesToExport.Count);
            string chunkScenePath = exportDir + "/" + rootName + ".unity";
            AssetDatabase.CopyAsset(originalScenePath, chunkScenePath);
            EditorSceneManager.OpenScene(chunkScenePath, OpenSceneMode.Single);

            GameObject[] tempRoots = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject r in tempRoots)
            {
                if (r.name != rootName)
                {
                    EngineUtils.Destroy(r);
                }
            }
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            AssetDatabase.Refresh();
        }


        //// 拷贝出一个删除了Chunk物体的Base场景
        //string baseScenePath = sceneDir + "/" + "baseworld.unity";
        //AssetDatabase.DeleteAsset(baseScenePath);
        //AssetDatabase.CopyAsset(originalScenePath, baseScenePath);
        //EditorSceneManager.OpenScene(baseScenePath, OpenSceneMode.Single);

        //GameObject[] chunkRoots = EditorSceneManager.GetActiveScene().GetRootGameObjects();
        //foreach (GameObject r in chunkRoots)
        //{
        //    if (rootsNamesToExport.Contains(r.name))
        //    {
        //        EngineUtils.Destroy(r);
        //    }
        //}
        //EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        //AssetDatabase.Refresh();

        // Cleanup
        EditorUtility.ClearProgressBar();



    }

    */

    static void ClassifyGameObject(GameObject obj,float width,float height)
    {
        Vector3 pos = obj.transform.position;

        int targetChunkX = (int)(pos.x / width) + 1;
        int targetChunkZ = (int)(pos.z / width) + 1;

        string chunkName = ChunkRootNamePrefix + string.Format("{0}_{1}", targetChunkX, targetChunkZ);


        GameObject chunkRoot = GameObject.Find(chunkName);

        if(chunkRoot == null)
        {
            chunkRoot = new GameObject(chunkName);
        }


        //复制层次关系到Chunk的节点下面
        GameObject tempObj = obj;
        List<GameObject> objs2Copy = new List<GameObject>();
        while(tempObj.transform.parent)
        {
            objs2Copy.Add(tempObj.transform.parent.gameObject);
            tempObj = tempObj.transform.parent.gameObject;
        }

        tempObj = chunkRoot;

        for(int i = objs2Copy.Count -1;i> -1; --i)
        {
            GameObject targetObj = objs2Copy[i];

            // 对于符合Chunk命名规则的父节点不进行拷贝过程。
            if (targetObj.name.StartsWith(ChunkRootNamePrefix))
            {
                continue;
            }

            Transform parent = tempObj.transform.Find(targetObj.name);
            if (parent == null)
            {
                parent = new GameObject(targetObj.name).transform;
                //CopyComponents(targetObj, parent.gameObject);
                parent.parent = tempObj.transform;
                targetObj = parent.gameObject;
            }
            tempObj = parent.gameObject;
        }

        Transform tempParent = obj.transform.parent;
        obj.transform.parent = tempObj.transform;

        // 移动完毕之后发现父节点没有孩子节点的情况下，向上遍历将无用节点删除。
        while (tempParent != null && tempParent.childCount == 0)
        {
            Transform temp = tempParent.parent;
            //EngineUtils.Destroy(tempParent.gameObject);
            tempParent = temp;
        }

    }
}

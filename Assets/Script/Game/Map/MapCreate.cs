using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate
{

    private List<MapIndex> list_had_mapindexs = new List<MapIndex>();

    private Vector3 lastPos;

    public MapCreate()
    {
        lastPos.x = -10000;
        lastPos.y = 0;
        lastPos.z = -10000;
    }


    public void UpdateCreateMap(Vector3 pos)
    {
        //int targetChunkX = (int)(pos.x / width) + 1;
        //int targetChunkZ = (int)(pos.z / width) + 1;
        //是否需要   根据位置
        if (MapIndexConfig.IsneedCreateMapByPos(lastPos, pos) == false)
        {
            return;
        }
        //是否需要  根据

        // 1 需要的地图块  
        MapIndex[] mapIndexs = MapIndexConfig.GetMap9(pos);

        /// 对比是否已经加载过了
        List<MapIndex> needMapIndexs = new List<MapIndex>();
        for(int i = 0; i < mapIndexs.Length; i++)
        {
            bool is_had = false;
            foreach(MapIndex mapIndex in list_had_mapindexs)
            {
                if (MapIndexConfig.CompareMapIndex(mapIndex, mapIndexs[i]) == true)
                {
                    is_had = true;
                }
            }

            if(is_had == false)
            {
                needMapIndexs.Add(mapIndexs[i]);
            }
        }

        ///

        foreach(MapIndex mapIndex in needMapIndexs)
        {
            CreateMap(mapIndex);
        }
        ///

        lastPos = pos;
    }

    public void CreateMap(MapIndex mapIndex)
    {
        string mapName = MapIndexConfig.GetMapNameByMapIndex(mapIndex);

        string mapPath = FolderMgr.GetMapPrefabPath();

        string mapFrefab = mapPath + "/" + mapName;

        Object obj = Resources.Load(mapFrefab);
        if (obj == null)
        {
            Debug.Log("path is " + mapFrefab);
            return;
        }

        GameObject instance = GameObject.Instantiate(obj) as GameObject;


    }
}

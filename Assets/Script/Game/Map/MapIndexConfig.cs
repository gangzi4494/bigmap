using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct MapIndex
{
    public int x;
    public int z;
}

/// <summary>
/// 地图位置  改名为MapIndexConfig
/// </summary>
public class MapIndexConfig
{
    public static int width = 10;
    public static int height = 10;


    private static string ChunkRootNamePrefix = "_worldtrunk";
    public static int GetMapX(float x)
    {
        return (int)(x / width) + 1;
    }

    public static int GetMapZ(float z)
    {
        return (int)(z / width) + 1;
    }

    public static string GetMapName(Vector3 pos)
    {
        MapIndex mapIndex = Pos2MapIndex(pos);
        return GetMapNameByMapIndex(mapIndex);
        //return ChunkRootNamePrefix + string.Format("{0}_{1}", GetMapX(pos.x), GetMapZ(pos.z));
    }

    private static MapIndex Pos2MapIndex(Vector3 pos)
    {
        MapIndex mapIndex;

        mapIndex.x = GetMapX(pos.x);
        mapIndex.z = GetMapZ(pos.z);

        return mapIndex;

    }

    public static string GetMapNameByMapIndex(MapIndex mapIndex)
    {
        return ChunkRootNamePrefix + string.Format("{0}_{1}", mapIndex.x, mapIndex.z);
    }

    public static MapIndex[] GetMap9(Vector3 pos)
    {
        //ArrayList arrayList = 
        MapIndex deafultMi;
        deafultMi.x = 0;
        deafultMi.z = 0;

        MapIndex[] mapIndices =
        {
            deafultMi,deafultMi,deafultMi,
            deafultMi,deafultMi,deafultMi,
            deafultMi,deafultMi,deafultMi,
        };

        //int[] maps = {0,0,0,
        //              0,0,0,
        //              0,0,0};

        int centMapX = GetMapX(pos.x);
        int centMapZ = GetMapZ(pos.z);

        int index = 0;
        

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                index = i * 3 + j;
                mapIndices[index].x = centMapX - 1 + i;
                mapIndices[index].z = centMapZ - 1 + j;
                //Debug.Log("")
            }
        }


        return mapIndices;
    }


    public static bool CompareMapIndex(MapIndex mi1, MapIndex mi2)
    {
        if (mi1.x == mi2.x && mi1.z == mi2.z)
        {
            return true;
        }
        return false;
    }

    public static bool IsneedCreateMapByPos(Vector3 lastPos, Vector3 curPos)
    {
        if (GetMapX(lastPos.x) != GetMapX(curPos.x))
        {
            return true;
        }

        if (GetMapZ(lastPos.z) != GetMapZ(curPos.z))
        {
            return true;
        }

        return false;
    }

}

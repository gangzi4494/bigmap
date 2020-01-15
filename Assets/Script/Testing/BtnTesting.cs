using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        mapCreate = new MapCreate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    MapCreate mapCreate;

    private void OnGUI()
    {
        if (GUILayout.Button("test"))
        {
            MapIndex mapIndex;
            mapIndex.x = 1;
            mapIndex.z = 1;
            mapCreate.CreateMap(mapIndex);

            mapIndex.x = 1;
            mapIndex.z = 0;
            mapCreate.CreateMap(mapIndex);
        }
    }
}

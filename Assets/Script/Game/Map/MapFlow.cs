using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFlow : MonoBehaviour
{
    private MapCreate mapCreate;

    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
        target = GameObject.FindGameObjectWithTag("Player").transform;

        mapCreate = new MapCreate();

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            mapCreate.UpdateCreateMap(target.position);
        }
    }
}

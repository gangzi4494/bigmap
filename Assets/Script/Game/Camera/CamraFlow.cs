using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamraFlow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 5, 4);//相机相对于玩家的位置
    private Transform target;
    private Vector3 pos;

    public float speed = 2;



    public float distanceUp = 8f;
    public float distanceAway = 5f;

    public float smooth = 2f;//位置平滑移动值
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

       // offset = target.position - this.transform.position;
       
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = target.position - offset;

        ///位置
        //pos = target.position - offset;
        //this.transform.position = Vector3.Lerp(this.transform.position, pos, speed * Time.deltaTime);//调整相机与玩家之间的距离

        Vector3 disPos = target.position + Vector3.up * distanceUp - target.forward * distanceAway;
        transform.position = Vector3.Lerp(transform.position, disPos, Time.deltaTime * smooth);

        //角度
        //Quaternion angel = Quaternion.LookRotation(target.position - this.transform.position);//获取旋转角度
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, angel, speed * Time.deltaTime);


        transform.LookAt(target.position);

    }
}

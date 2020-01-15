using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveContrl : MonoBehaviour
{

    public float m_movSpeed = 10;//移动系数

    private Transform m_transform;

    private Transform m_camTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }

    void Control()
    {
        // 定义3个值控制移动
        float xm = 0, ym = 0, zm = 0;

        //按键盘W向上移动
        if (Input.GetKey(KeyCode.W))
        {
            zm += m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))//按键盘S向下移动
        {
            zm -= m_movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))//按键盘A向左移动
        {
            xm -= m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))//按键盘D向右移动
        {
            xm += m_movSpeed * Time.deltaTime;
        }



        m_transform.Translate(new Vector3(xm, ym, zm), Space.Self);
    }
}

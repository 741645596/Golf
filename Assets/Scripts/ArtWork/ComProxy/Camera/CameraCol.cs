using UnityEngine;
using System.Collections;

public class CameraCol : MonoBehaviour
{
    //控制视野缩放的速率
    public float view_value;
    //控制摄像机移动的速率
    public float move_speed;

    private void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }


    void Update()
    {
        if (RingDrag.s_check == true)
            return;
        //放大、缩小
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            this.gameObject.transform.Translate(new Vector3(0, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * view_value), 0);
        }
        //移动视角
        if (Input.GetMouseButton(0))
        {
            transform.Translate(Vector3.left * Input.GetAxis("Mouse X") * move_speed);
            transform.Translate(Vector3.up * Input.GetAxis("Mouse Y") * -move_speed);
        }
    }
}

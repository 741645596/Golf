using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 高尔夫运动相机
/// </summary>
public class GolfCamera
{
    /// <summary>
    /// 相机优先级枚举
    /// </summary>
    public enum GolfCameraPriority
    {
        Low = 0,
        Middle = 10,
        High = 20,
    }

    /// <summary>
    /// 相机类型
    /// </summary>
    public GolfCameraType Type { get; set; }

    /// <summary>
    /// 相机名字
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 虚拟相机
    /// </summary>
    public CinemachineVirtualCamera VCamera { get; set; } = null;

    /// <summary>
    /// 初始位置
    /// </summary>
    public Vector3 InitialPosition { get; set; } = Vector3.zero;

    /// <summary>
    /// 初始旋转
    /// </summary>
    public Quaternion InitialRotation { get; set; } = Quaternion.identity;

    public GolfCamera(GolfCameraType type,Transform parent, GolfCameraPriority priority, int fieldOfView, Vector3 initPos)
    {
        Type = type;
        InitialPosition = initPos;
        Name = "VCam_" + Type.ToString();
        Create(parent, priority, fieldOfView);
    }

    public void Clear()
    {
        Type = GolfCameraType.Sky;
        Name = "";
        VCamera = null;
        InitialPosition = Vector3.zero;
        InitialRotation = Quaternion.identity;
    }

    /// <summary>
    /// 创建相机
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="fieldOfView"></param>
    public void Create(Transform parent,GolfCameraPriority priority, int fieldOfView)
    {
        GameObject go = new GameObject(Name);
        go.transform.parent = parent;
        VCamera = go.GetComponent<CinemachineVirtualCamera>();
        if (VCamera == null)
        {
            VCamera = go.AddComponent<CinemachineVirtualCamera>();
            VCamera.Priority = (int)priority;
            VCamera.Follow = null;
            VCamera.LookAt = null;
            VCamera.m_StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.RoundRobin;

            VCamera.m_Lens.FieldOfView = fieldOfView;
            VCamera.m_Lens.NearClipPlane = 0.01f;
            VCamera.m_Lens.FarClipPlane = 5000;
            VCamera.m_Lens.Dutch = 0;

            VCamera.m_Transitions.m_BlendHint = CinemachineVirtualCameraBase.BlendHint.None;
            VCamera.m_Transitions.m_InheritPosition = false;

            VCamera.transform.position = InitialPosition;
        }
    }


    /// <summary>
    /// 激活相机
    /// </summary>
    public void SetActive()
    {
        //虚拟相机enabled改变即激活相机
        VCamera.enabled = false;
        VCamera.enabled = true;
    }

    public void SetPosition(Vector3 pos)
    {
        VCamera.transform.position = pos;
    }

    public void SetRotation(Quaternion rot)
    {
        VCamera.transform.rotation = rot;
    }

    public void LookAt(Transform trans)
    {
        VCamera.LookAt = trans;
    }

    public void Follow(Transform trans)
    {
        VCamera.Follow = trans;
    }
}

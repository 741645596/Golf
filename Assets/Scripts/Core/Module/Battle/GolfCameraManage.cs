using Cinemachine;
using IGG.Core;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 虚拟相机管理类
/// </summary>
public class GolfCameraManage
{
    /// <summary>
    /// 当前相机类型
    /// </summary>
    public GolfCameraType CurType { get; set; }

    /// <summary>
    /// 所有相机
    /// </summary>
    private Dictionary<uint, GolfCamera> m_vCameraDic;

    private CinemachineBlenderSettings blenderSetting;
    List<CinemachineBlenderSettings.CustomBlend> blends;

    public GolfCameraManage(GolfCameraType initType)
    {
        m_vCameraDic = new Dictionary<uint, GolfCamera>();
        blenderSetting = (CinemachineBlenderSettings)ScriptableObject.CreateInstance(typeof(CinemachineBlenderSettings));
        blends = new List<CinemachineBlenderSettings.CustomBlend>();

    }

    public void Clear()
    {
        CurType = GolfCameraType.Sky;
        if (m_vCameraDic != null)
        {
            foreach (GolfCamera camera in m_vCameraDic.Values)
            {
                camera.Clear();
            }
        }
        blenderSetting = null;
        blends = null;
    }

    /// <summary>
    /// 添加相机
    /// </summary>
    /// <param name="camera"></param>
    public void AddCamera(GolfCamera camera)
    {
        if (!m_vCameraDic.ContainsKey((uint)camera.Type))
        {
            m_vCameraDic.Add((uint)camera.Type, camera);
        }
    }

    public void RemoveCamera(GolfCameraType type)
    {
        if (!m_vCameraDic.ContainsKey((uint)type))
        {
            m_vCameraDic.Remove((uint)type);
        }
    }

    /// <summary>
    /// 切换相机
    /// </summary>
    /// <param name="cameraType"></param>
    /// <param name="canChange2Self"></param>
    public void SwitchCamera(GolfCameraType cameraType, bool canChange2Self = false)
    {
        if (!canChange2Self && CurType == cameraType)
        {
            return;
        }

        uint cameraIdx = (uint)cameraType;
        GolfCamera camera = null;
        if (m_vCameraDic.TryGetValue(cameraIdx, out camera))
        {
            camera.SetActive();
            CurType = cameraType;
        }
    }

    public void AddBlendSetting(string fromBlend, string toBlend, CinemachineBlendDefinition blendDef)
    {
        CinemachineBlenderSettings.CustomBlend blend;
        blend.m_From = fromBlend;
        blend.m_To = toBlend;
        blend.m_Blend = blendDef;
        blends.Add(blend);
        blenderSetting.m_CustomBlends = blends.ToArray();
        Camera.main.GetComponent<CinemachineBrain>().m_CustomBlends = blenderSetting;
    }

    public void SetCameraPosition(GolfCameraType cameraType, Vector3 pos)
    {
        uint cameraIdx = (uint)cameraType;
        GolfCamera camera = null;
        if (m_vCameraDic.TryGetValue(cameraIdx, out camera))
        {
            camera.SetPosition(pos);
        }
    }

    public void SetCameraRotation(GolfCameraType cameraType, Quaternion rot)
    {
        uint cameraIdx = (uint)cameraType;
        GolfCamera camera = null;
        if (m_vCameraDic.TryGetValue(cameraIdx, out camera))
        {
            camera.SetRotation(rot);
        }
    }

    public void LookAt(GolfCameraType cameraType, Transform trans)
    {
        uint cameraIdx = (uint)cameraType;
        GolfCamera camera = null;
        if (m_vCameraDic.TryGetValue(cameraIdx, out camera))
        {
            camera.LookAt(trans);
        }
    }

    public void LookAt(GolfCameraType cameraType, Vector3 pos)
    {
        uint cameraIdx = (uint)cameraType;
        GolfCamera camera = null;
        if (m_vCameraDic.TryGetValue(cameraIdx, out camera))
        {
            camera.VCamera.transform.LookAt(pos);
        }
    }

    public void Follow(GolfCameraType cameraType, Transform trans)
    {
        uint cameraIdx = (uint)cameraType;
        GolfCamera camera = null;
        if (m_vCameraDic.TryGetValue(cameraIdx, out camera))
        {
            camera.Follow(trans);
        }
    }

    public GolfCamera GetCamera(GolfCameraType cameraType)
    {
        uint cameraIdx = (uint)cameraType;
        GolfCamera camera = null;
        if (m_vCameraDic.TryGetValue(cameraIdx, out camera))
        {
            return camera;
        }
        return null;
    }
}

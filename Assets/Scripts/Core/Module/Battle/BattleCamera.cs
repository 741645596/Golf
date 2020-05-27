using Cinemachine;
using IGG.Core;
using IGG.Core.Data.DataCenter.Battle;
using IGG.Core.Data.DataCenter.GolfAI;
using System;
using UnityEngine;

/// <summary>
/// 虚拟相机类型
/// </summary>
public enum GolfCameraType
{
    Sky,            //天空视角
    Drive2Fly,      //正常发球
    Drive2Roll,     //推杆发球
    Fly2Land,       //空中到落地
    Land2Roll,      //落地到滚动
    NearHole,       //靠近球洞
}

public class BattleCamera : MonoBehaviour
{
    private GolfCameraManage CameraM { get; set; }
    public Transform ballTransform;

    // Start is called before the first frame update
    void Start()
    {
        RegisterHooks();
        InitBattleCameras(GolfCameraType.Sky);
    }


    private void RegisterHooks()
    {
        EventCenter.RegisterHooks(EventCenterType.Battle_LoadBallFinish, SetBallTransform);
        EventCenter.RegisterHooks(EventCenterType.Battle_SwitchCamera2Fly, SwitchCamera2Fly);
        EventCenter.RegisterHooks(EventCenterType.Battle_SwitchCamera2Roll, SwitchCamera2Roll);
        EventCenter.RegisterHooks(EventCenterType.Battle_SwitchCamera2RollDrive, SwitchCamera2RollDrive);
        EventCenter.RegisterHooks(EventCenterType.Battle_SwitchCamera2NearHole, SwitchCamera2NearHole);
        EventCenter.RegisterHooks(EventCenterType.Battle_SkyView, SwitchCamera2Sky);
        EventCenter.RegisterHooks(EventCenterType.Battle_SightView, SwitchCamera2FlyDrive);
        EventCenter.RegisterHooks(EventCenterType.Battle_DragMapMoveCamera, DragMapMoveCamera);
        EventCenter.RegisterHooks(EventCenterType.Battle_ScaleMapMoveCamera, ScaleMapMoveCamera);

        EventCenter.RegisterHooks(EventCenterType.Battle_RoundStart, RoundStart);
    }

    private void SwitchCamera2Roll(Vector3 velocity)
    {
        Vector3 ballPos = BattleM.BallPos;
        Vector3 dir = velocity.normalized;
        Vector3 pos = ballPos + new Vector3(-dir.x * 3, 2, -dir.z * 3);
        CameraM.SetCameraPosition(GolfCameraType.Land2Roll, pos);
        CameraM.LookAt(GolfCameraType.Land2Roll, ballPos);
        CameraM.SwitchCamera(GolfCameraType.Land2Roll);
    }

    private void SwitchCamera2Sky()
    {
        Vector3 ballPos = BattleM.BallPos;
        Vector3 ringPos = BattleM.RingPos;
        Vector3 tmpBallPos = new Vector3(ballPos.x, 0, ballPos.z);
        Vector3 tmpRingPos = new Vector3(ringPos.x, 0, ringPos.z);
        Vector3 dir = (tmpBallPos - tmpRingPos).normalized;
        float len = (tmpBallPos - tmpRingPos).magnitude;
        float angle = Vector3.Angle(Vector3.forward, dir);
        float deltaAngle = 25;
        float angleTarget = 0;
        if (dir.x >= 0)
        {
            angleTarget = angle - deltaAngle;
        }
        else
        {
            angleTarget = -(angle + deltaAngle);
        }

        if (angleTarget > 180)
        {
            angleTarget = 360 - angleTarget;
        }
        else if (angleTarget < -180)
        {
            angleTarget = 360 + angleTarget;
        }

        Quaternion rot = Quaternion.AngleAxis(angleTarget, Vector3.up);

        Vector3 dirTarget = rot * Vector3.forward;

        Vector3 tmpCameraPos = tmpRingPos + dirTarget.normalized * len * 1f;

        Vector3 dirCam2Ring = (tmpRingPos - tmpCameraPos).normalized;
        float angleY = Vector3.Angle(Vector3.forward, dirCam2Ring);
        if (dirCam2Ring.x < 0)
        {
            angleY *= -1;
        }

        Vector3 cameraPos = new Vector3(tmpCameraPos.x, len, tmpCameraPos.z);
        CameraM.SetCameraPosition(GolfCameraType.Sky, cameraPos);



        Vector3 tmpCameraPos2 = new Vector3(0, cameraPos.y, cameraPos.z);
        tmpRingPos = new Vector3(0, ringPos.y, ringPos.z);
        Vector3 dirRing2Cam = (tmpCameraPos2 - tmpRingPos).normalized;
        float angleX = Vector3.Angle(Vector3.back, dirRing2Cam);
        if (dirRing2Cam.z > 0)
        {
            angleX = Vector3.Angle(Vector3.forward, dirRing2Cam);
        }

        //Vector3 tmpCameraPos3 = new Vector3(cameraPos.x, cameraPos.y, 0);
        //tmpRingPos = new Vector3(ringPos.x, ringPos.y, 0);
        //dirRing2Cam = (tmpCameraPos3 - tmpRingPos).normalized;
        //float angleZ = Vector3.Angle(Vector3.up, dirRing2Cam);
        //if (dirRing2Cam.x < 0)
        //{
        //    angleZ *= -1;
        //}

        Quaternion rot2 = Quaternion.Euler(angleX, angleY, 0);

        CameraM.SetCameraPosition(GolfCameraType.Sky, cameraPos);
        CameraM.SetCameraRotation(GolfCameraType.Sky, rot2);
        CameraM.SwitchCamera(GolfCameraType.Sky, true);
    }
    private void SwitchCamera2FlyDrive()
    {
        Vector3 ballPos = BattleM.BallPos;
        Vector3 ringPos = BattleM.RingPos;
        Vector3 dir = (ringPos - ballPos).normalized;
        float len = (ringPos - ballPos).magnitude;
        Vector3 cameraPos = ballPos - new Vector3(dir.x * 1.5f, -1f, dir.z * 1.5f);
        Vector3 lookPos = ballPos + dir * len / 2;

        CameraM.SetCameraPosition(GolfCameraType.Drive2Fly, cameraPos);
        CameraM.LookAt(GolfCameraType.Drive2Fly, ballTransform);
        GolfCamera camera = CameraM.GetCamera(GolfCameraType.Drive2Fly);
        CinemachineComposer composer = camera.VCamera.GetCinemachineComponent<CinemachineComposer>();
        if (composer == null)
        {
            composer = camera.VCamera.AddCinemachineComponent<CinemachineComposer>();
        }
        composer.m_TrackedObjectOffset = new Vector3(0, 0.3f, 0);
        composer.m_HorizontalDamping = 5;
        composer.m_VerticalDamping = 5;
        composer.m_SoftZoneHeight = 0.5f;
        composer.m_SoftZoneWidth = 0.5f;
        CameraM.SwitchCamera(GolfCameraType.Drive2Fly);
    }

    private void SwitchCamera2RollDrive()
    {
        Vector3 ballPos = BattleM.BallPos;
        Vector3 holePos = BattleDC.Map.BallHolePos;
        Vector3 dir = (ballPos - holePos).normalized;
        Vector3 cameraPos = ballPos + new Vector3(dir.x * 3, 2, dir.z * 3);

        CameraM.SetCameraPosition(GolfCameraType.Drive2Roll, cameraPos);
        CameraM.LookAt(GolfCameraType.Drive2Roll, ballPos);

        //CameraM.Follow(GolfCameraType.Drive2Roll, ballTransform);
        //CameraM.LookAt(GolfCameraType.Drive2Roll, ballTransform);
        //GolfCamera camera = CameraM.GetCamera(GolfCameraType.Drive2Roll);

        //CinemachineTransposer transposer = camera.VCamera.GetCinemachineComponent<CinemachineTransposer>();
        //if (transposer == null)
        //{
        //    transposer = camera.VCamera.AddCinemachineComponent<CinemachineTransposer>();
        //}
        //transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;

        //CinemachineComposer composer = camera.VCamera.GetCinemachineComponent<CinemachineComposer>();
        //if (composer == null)
        //{
        //    composer = camera.VCamera.AddCinemachineComponent<CinemachineComposer>();
        //}
        //composer.m_TrackedObjectOffset = new Vector3(0, 0.3f, 0);
        //composer.m_HorizontalDamping = 5;
        //composer.m_VerticalDamping = 5;
        //composer.m_SoftZoneHeight = 0.5f;
        //composer.m_SoftZoneWidth = 0.5f;

        CameraM.SwitchCamera(GolfCameraType.Drive2Roll);
    }

    private void SwitchCamera2Fly(Vector3 pos)
    {
        Vector3 ballPos = BattleM.BallPos;
        CameraM.SetCameraPosition(GolfCameraType.Fly2Land, pos);
        CameraM.LookAt(GolfCameraType.Fly2Land, ballPos);
        CameraM.SwitchCamera(GolfCameraType.Fly2Land);
    }
    public void UpdateCamera2Fly()
    {
        if (CameraM.CurType == GolfCameraType.Fly2Land)
        {
            Vector3 pos = BattleM.BallPos;
            GolfCamera camera = CameraM.GetCamera(CameraM.CurType);
            pos = new Vector3(pos.x, camera.VCamera.transform.position.y, pos.z);
            CameraM.LookAt(GolfCameraType.Fly2Land, pos);
        }
    }

    private void SwitchCamera2NearHole()
    {
        Vector3 holePos = BattleDC.Map.BallHolePos;
        Vector3 ballPos = BattleM.BallPos;
        Vector3 dir = (holePos - ballPos).normalized;
        Vector3 cameraPos = holePos - dir * 3;
        cameraPos.y += 3;
        CameraM.SetCameraPosition(GolfCameraType.NearHole, cameraPos);
        CameraM.LookAt(GolfCameraType.NearHole, holePos);
        CameraM.SwitchCamera(GolfCameraType.NearHole);
    }

    private void ScaleMapMoveCamera(float axis, float scaleSpeed)
    {
        if (CameraM.CurType == GolfCameraType.Sky)
        {
            GolfCamera skyCamera = CameraM.GetCamera(GolfCameraType.Sky);
            Vector3 pos = skyCamera.VCamera.transform.position;
            pos.y -= axis * scaleSpeed;
            skyCamera.VCamera.transform.position = pos;
        }
    }

    private void DragMapMoveCamera(Vector3 dir, float moveSpeed)
    {
        if (CameraM.CurType == GolfCameraType.Sky)
        {
            dir = new Vector3(-dir.x, 0, -dir.y);
            dir.Normalize();
            GolfCamera skyCamera = CameraM.GetCamera(GolfCameraType.Sky);
            skyCamera.VCamera.transform.position += dir * moveSpeed;
        }
    }

    private void InitBattleCameras(GolfCameraType initType)
    {
        CameraM = new GolfCameraManage(GolfCameraType.Sky);
        CameraM.AddCamera(new GolfCamera(GolfCameraType.Sky, transform, GolfCamera.GolfCameraPriority.Middle, 60, new Vector3(0, 0, 0)));
        CameraM.AddCamera(new GolfCamera(GolfCameraType.Drive2Fly, transform, GolfCamera.GolfCameraPriority.Middle, 60, new Vector3(0, 0, 0)));
        CameraM.AddCamera(new GolfCamera(GolfCameraType.Drive2Roll, transform, GolfCamera.GolfCameraPriority.Middle, 60, new Vector3(0, 0, 0)));
        CameraM.AddCamera(new GolfCamera(GolfCameraType.Fly2Land, transform, GolfCamera.GolfCameraPriority.Middle, 60, new Vector3(0, 0, 0)));
        CameraM.AddCamera(new GolfCamera(GolfCameraType.Land2Roll, transform, GolfCamera.GolfCameraPriority.Middle, 60, new Vector3(0, 0, 0)));
        CameraM.AddCamera(new GolfCamera(GolfCameraType.NearHole, transform, GolfCamera.GolfCameraPriority.Middle, 60, new Vector3(0, 0, 0)));

        CinemachineBlendDefinition blendDef = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 1);
        CameraM.AddBlendSetting(CinemachineBlenderSettings.kBlendFromAnyCameraLabel, CameraM.GetCamera(GolfCameraType.Fly2Land).Name, blendDef);

        CameraM.SwitchCamera(initType, true);
    }

    private void ScaleMapMoveCamera(int Event_Send, object Param)
    {
        float axis = (float)Convert.ToDouble(Param);
        ScaleMapMoveCamera(axis, 5);
    }

    private void DragMapMoveCamera(int Event_Send, object Param)
    {
        Vector3 dir = (Vector3)Param;
        if (dir != null)
        {
            DragMapMoveCamera(dir, 0.5f);
        }
    }

    private void SwitchCamera2FlyDrive(int Event_Send, object Param)
    {
        if (BattleM.BallInAra == AreaType.PuttingGreen)
        {
            return;
        }
        SwitchCamera2FlyDrive();
    }

    private void SwitchCamera2RollDrive(int Event_Send, object Param)
    {
        SwitchCamera2RollDrive();
    }

    private void SwitchCamera2Sky(int Event_Send, object Param)
    {
        SwitchCamera2Sky();
    }

    private void SwitchCamera2Roll(int Event_Send, object Param)
    {
        Vector3 velocity = (Vector3)Param;
        SwitchCamera2Roll(velocity);
    }

    private void SwitchCamera2Fly(int Event_Send, object Param)
    {
        Vector3 pos = (Vector3)Param;
        SwitchCamera2Fly(pos);
    }

    private void SwitchCamera2NearHole(int Event_Send, object Param)
    {
        SwitchCamera2NearHole();
    }

    private void AntiRegisterHooks()
    {
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_LoadBallFinish, SetBallTransform);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_SwitchCamera2Fly, SwitchCamera2Fly);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_SwitchCamera2Roll, SwitchCamera2Roll);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_SwitchCamera2RollDrive, SwitchCamera2RollDrive);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_SwitchCamera2NearHole, SwitchCamera2NearHole);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_SkyView, SwitchCamera2Sky);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_SightView, SwitchCamera2FlyDrive);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_DragMapMoveCamera, DragMapMoveCamera);
        EventCenter.AntiRegisterHooks(EventCenterType.Battle_ScaleMapMoveCamera, ScaleMapMoveCamera);

        EventCenter.RegisterHooks(EventCenterType.Battle_RoundStart, RoundStart);
    }


    private void SetBallTransform(int Event_Send, object Param)
    {
        ballTransform = Param as Transform;
    }

    private void OnDestroy()
    {
        CameraM.Clear();
        AntiRegisterHooks();
    }

    private void RoundStart(int sender, object Param)
    {
        if (BattleM.BallInAra == AreaType.PuttingGreen)
        {
            SwitchCamera2RollDrive();
        }
        else
        {
            SwitchCamera2Sky();
        }
    }

    void LateUpdate()
    {
        UpdateCamera2Fly();
    }
}

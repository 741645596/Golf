
namespace IGG.Core.Utils
{


    // 关卡状态枚举
    public enum EStageStatus
    {
        KStageStatusNone,       // 不存在
        KStageStatusComplete,   // 已通过
        KStageStatusFight,      // 正在打
        KStageStatusNotFight,   // 不能打          
    }

    // 场景切入login 的 类型
    public enum EEnterLoginSceneType
    {
        eEnterLoginSceneType_None,
        eEnterLoginSceneType_Battle,    // 从战斗中切入
    }
}



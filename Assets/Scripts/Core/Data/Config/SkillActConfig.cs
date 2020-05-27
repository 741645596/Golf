using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.6
    /// Desc    skill_act配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SkillActConfig : IConfig<uint>
    {
        /// <summary>
        /// 表现ID
        /// </summary>
        public uint Id;

        /// <summary>
        /// 备注用
        /// </summary>
        public string Desc;

        /// <summary>
        /// 调用动作
        /// 调用触发参数名
        /// 敌方时生效
        /// </summary>
        public string Action;

        /// <summary>
        /// 调用地面特效
        /// </summary>
        public string EffectGround;

        /// <summary>
        /// 特效类型；1怪位置2指定点
        /// </summary>
        public uint GroundType;

        /// <summary>
        /// 攻击判定延迟时间
        /// 数组表示多段攻击
        /// 单位毫秒
        /// 动作与地面特效适用
        /// </summary>
        public uint[] AttackjudgeDelay;

        /// <summary>
        /// 弹道特效；配置资源名
        /// </summary>
        public string EffectMissile;

        /// <summary>
        /// 三消棋盘用
        /// 表示
        /// 每个弹道的发射间隔
        /// </summary>
        public uint EffectMissileMagicdelay;

        /// <summary>
        /// 弹道类型；1抛物线2折线
        /// </summary>
        public uint MissileType;

        /// <summary>
        /// 弹道加速曲线
        /// http://www.xuanfengge.com/easeing/easeing/
        /// Unset = 0,
        /// Linear = 1,
        /// InSine = 2,
        /// OutSine = 3,
        /// InOutSine = 4,
        /// InQuad = 5,
        /// OutQuad = 6,
        /// InOutQuad = 7,
        /// InCubic = 8,
        /// OutCubic = 9,
        /// InOutCubic = 10,
        /// InQuart = 11,
        /// OutQuart = 12,
        /// InOutQuart = 13,
        /// InQuint = 14,
        /// OutQuint = 15,
        /// InOutQuint = 16,
        /// InExpo = 17,
        /// OutExpo = 18,
        /// InOutExpo = 19,
        /// InCirc = 20,
        /// OutCirc = 21,
        /// InOutCirc = 22,
        /// InElastic = 23,
        /// OutElastic = 24,
        /// InOutElastic = 25,
        /// InBack = 26,
        /// OutBack = 27,
        /// InOutBack = 28,
        /// InBounce = 29,
        /// OutBounce = 30,
        /// InOutBounce = 31,
        /// Flash = 32,
        /// InFlash = 33,
        /// OutFlash = 34,
        /// InOutFlash = 35,
        /// </summary>
        public uint MissileAccelerate;

        /// <summary>
        /// 弹道飞行速度
        /// </summary>
        public uint MissileSpeed;

        /// <summary>
        /// 弹道参数1
        /// 数组表示消除珠子每行参数
        /// 
        /// 抛物线-百分比
        /// </summary>
        public int[] MissileParm1;

        /// <summary>
        /// 弹道参数2
        /// 数组表示消除珠子每行参数
        /// 
        /// 抛物线-高度(百分制）
        /// </summary>
        public int[] MissileParm2;

        /// <summary>
        /// 弹道参数3
        /// 抛物线-偏移角
        /// </summary>
        public int MissileParm3;

        /// <summary>
        /// 弹道参数4
        /// 抛物线-缩放时间（毫秒）
        /// </summary>
        public int MissileParm4;

        /// <summary>
        /// 弹道爆炸特效；配置资源名
        /// </summary>
        public string MissileExplode;

        /// <summary>
        /// 爆炸特效攻击时间
        /// 数组表示多段攻击时间
        /// 单位毫秒
        /// </summary>
        public uint[] EffectExplodeHittime;

        /// <summary>
        /// 调用受击特效；配置资源名
        /// </summary>
        public string EffectHurt;

        /// <summary>
        /// 2dui受击特效
        /// </summary>
        public string EffectHurtui;

        /// <summary>
        /// 调用ui特效；配置资源名
        /// </summary>
        public string EffectUi;

        /// <summary>
        /// 格挡调用ui特效；配置资源名
        /// </summary>
        public string EffectUi2;

        /// <summary>
        /// 棋盘整体特效
        /// </summary>
        public string EffectChessboard;

        /// <summary>
        /// 攻击音效；配置资源名
        /// </summary>
        public uint AttackSound;

        /// <summary>
        /// 受击音效；配置资源名
        /// </summary>
        public uint HurtSound;

        /// <summary>
        /// 震动摄像机
        /// </summary>
        public uint ShakeCamera;

        /// <summary>
        /// 技能释放后，延迟多少时间（毫秒）进行摄像机震动
        /// </summary>
        public uint DelayShake;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.6
    /// Desc    skill_act配置文件访问接口
    /// </summary>
    public partial class SkillActDao:BaseDao<SkillActDao,uint,SkillActConfig>
    {
        public override string GetName()
        {
            return "skill_act";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.6
    /// Desc    skill_act配置文件解码器
    /// </summary>
    public partial class SkillActDecoder : BaseCfgDecoder<SkillActConfig, SkillActCfgData>
    {
        public override string GetName()
        {
            return "skill_act";
        }

        protected override void ProcessRow(SkillActConfig excel)
        {
            GetU32("id", out excel.Id);
            GetString("desc", out excel.Desc);
            GetString("action", out excel.Action);
            GetString("effect_ground", out excel.EffectGround);
            GetU32("ground_type", out excel.GroundType);
            GetArr("attackjudge_delay", StrHelper.ArrSplitLv1, out excel.AttackjudgeDelay, ParseU32);
            GetString("effect_missile", out excel.EffectMissile);
            GetU32("effect_missile_magicdelay", out excel.EffectMissileMagicdelay);
            GetU32("missile_type", out excel.MissileType);
            GetU32("missile_accelerate", out excel.MissileAccelerate);
            GetU32("missile_speed", out excel.MissileSpeed);
            GetArr("missile_parm1", StrHelper.ArrSplitLv1, out excel.MissileParm1, ParseI32);
            GetArr("missile_parm2", StrHelper.ArrSplitLv1, out excel.MissileParm2, ParseI32);
            GetI32("missile_parm3", out excel.MissileParm3);
            GetI32("missile_parm4", out excel.MissileParm4);
            GetString("missile_explode", out excel.MissileExplode);
            GetArr("effect_explode_hittime", StrHelper.ArrSplitLv1, out excel.EffectExplodeHittime, ParseU32);
            GetString("effect_hurt", out excel.EffectHurt);
            GetString("effect_hurtui", out excel.EffectHurtui);
            GetString("effect_ui", out excel.EffectUi);
            GetString("effect_ui2", out excel.EffectUi2);
            GetString("effect_chessboard", out excel.EffectChessboard);
            GetU32("attack_sound", out excel.AttackSound);
            GetU32("hurt_sound", out excel.HurtSound);
            GetU32("shake_camera", out excel.ShakeCamera);
            GetU32("delay_shake", out excel.DelayShake);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
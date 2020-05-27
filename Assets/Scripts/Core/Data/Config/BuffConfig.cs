using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    buff配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class BuffConfig : IConfig<uint>
    {
        /// <summary>
        /// 状态id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 策划备注字段
        /// </summary>
        public string DesignerRemarks;

        /// <summary>
        /// 点击图标显示的文本信息
        /// </summary>
        public string Tid;

        /// <summary>
        /// 状态的持续时间，单位为回合
        /// </summary>
        public uint BuffTime;

        /// <summary>
        /// 状态的事件（例如沉默，还有其他独有性的状态效果）
        /// </summary>
        public uint BuffEvent;

        /// <summary>
        /// 状态的类型，1为增益，2为减益，0为中立
        /// </summary>
        public uint BuffKind;

        /// <summary>
        /// 状态的生效时机是回合开始还是回合结束，0回合开始前，1回合结束后
        /// </summary>
        public uint BuffEffective;

        /// <summary>
        /// 状态是否可叠加，1为不可叠加，0为叠加
        /// </summary>
        public uint IsBestrow;

        /// <summary>
        /// 状态叠加上限次数
        /// </summary>
        public uint BestrowTime;

        /// <summary>
        /// buff的绑点位置
        /// </summary>
        public uint BuffEffectPoint;

        /// <summary>
        /// buff的状态特效
        /// </summary>
        public string BuffEffect;

        /// <summary>
        /// buff的图标状态，主要用于配置一些buff挂在角色血条旁边的小图标。
        /// </summary>
        public string BuffIcon;

        /// <summary>
        /// buff的图标优先级(从大到小排列）
        /// </summary>
        public uint BuffIconPriority;

        /// <summary>
        /// 加攻击固定值
        /// </summary>
        public int AddAtkValue;

        /// <summary>
        /// 加攻击百分比
        /// </summary>
        public int AddAtkPercent;

        /// <summary>
        /// 加防御固定值
        /// </summary>
        public int AddDefValue;

        /// <summary>
        /// 加防御百分比
        /// </summary>
        public int AddDefPercent;

        /// <summary>
        /// 加暴击（百分比）
        /// </summary>
        public int AddCritPercent;

        /// <summary>
        /// 增加怒气固定值（负值为减）
        /// </summary>
        public int AddAngryValue;

        /// <summary>
        /// 增加最大怒气百分比（负值为减）
        /// </summary>
        public int AddAngryPercent;

        /// <summary>
        /// 每多少回合回复一次
        /// </summary>
        public uint PreHpTime;

        /// <summary>
        /// 每回合回复/伤害固定值
        /// </summary>
        public int PreHp;

        /// <summary>
        /// 每回合回复/伤害百分比
        /// </summary>
        public int PreHpPercent;

        /// <summary>
        /// 回复/伤害的最大值（针对百分比回复和伤害）
        /// </summary>
        public uint PerHpChangeMaxForPer;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    buff配置文件访问接口
    /// </summary>
    public partial class BuffDao:BaseDao<BuffDao,uint,BuffConfig>
    {
        public override string GetName()
        {
            return "buff";
        }

        protected override void OnChangeLang(ref BuffConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                BuffConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.Tid, language, ref cfg.Tid);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    buff配置文件解码器
    /// </summary>
    public partial class BuffDecoder : BaseCfgDecoder<BuffConfig, BuffCfgData>
    {
        public override string GetName()
        {
            return "buff";
        }

        protected override void ProcessRow(BuffConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetString("designer_remarks", out excel.DesignerRemarks);
            GetString("$tid", out excel.Tid);
            GetU32("buff_time", out excel.BuffTime);
            GetU32("buff_event", out excel.BuffEvent);
            GetU32("buff_kind", out excel.BuffKind);
            GetU32("buff_effective", out excel.BuffEffective);
            GetU32("is_bestrow", out excel.IsBestrow);
            GetU32("bestrow_time", out excel.BestrowTime);
            GetU32("buff_effect_point", out excel.BuffEffectPoint);
            GetString("buff_effect", out excel.BuffEffect);
            GetString("buff_icon", out excel.BuffIcon);
            GetU32("buff_icon_priority", out excel.BuffIconPriority);
            GetI32("add_atk_value", out excel.AddAtkValue);
            GetI32("add_atk_percent", out excel.AddAtkPercent);
            GetI32("add_def_value", out excel.AddDefValue);
            GetI32("add_def_percent", out excel.AddDefPercent);
            GetI32("add_crit_percent", out excel.AddCritPercent);
            GetI32("add_angry_value", out excel.AddAngryValue);
            GetI32("add_angry_percent", out excel.AddAngryPercent);
            GetU32("pre_hp_time", out excel.PreHpTime);
            GetI32("pre_hp", out excel.PreHp);
            GetI32("pre_hp_percent", out excel.PreHpPercent);
            GetU32("per_hp_change_max_for_per", out excel.PerHpChangeMaxForPer);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
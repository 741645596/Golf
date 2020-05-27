using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    passvityskill配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class PassvityskillConfig : IConfig<uint>
    {
        /// <summary>
        /// 被动技能id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 策划备注字段
        /// </summary>
        public string DesignerRemarks;

        /// <summary>
        /// 被动技名称
        /// </summary>
        public string Tid;

        /// <summary>
        /// 被动技图标
        /// </summary>
        public string IconAddress;

        /// <summary>
        /// 被动技描述
        /// </summary>
        public string InfoTid;

        /// <summary>
        /// 被动技描述内填写的具体值，“|”隔开
        /// </summary>
        public string InfoValue;

        /// <summary>
        /// 技能表现ID，索引到skill_act.csv
        /// </summary>
        public uint ActID;

        /// <summary>
        /// 被动技等级
        /// </summary>
        public uint Lv;

        /// <summary>
        /// 被动事件
        /// </summary>
        public uint Event;

        /// <summary>
        /// 加攻击固定值
        /// </summary>
        public uint AddAtkValue;

        /// <summary>
        /// 加攻击百分比
        /// </summary>
        public uint AddAtkPercent;

        /// <summary>
        /// 加防御固定值
        /// </summary>
        public uint AddDefValue;

        /// <summary>
        /// 加防御百分比
        /// </summary>
        public uint AddDefPercent;

        /// <summary>
        /// 加生命值固定值
        /// </summary>
        public uint AddHpValue;

        /// <summary>
        /// 加生命值百分比
        /// </summary>
        public uint AddHpPercent;

        /// <summary>
        /// 加命中（百分比）
        /// </summary>
        public uint AddHit;

        /// <summary>
        /// 加闪避（百分比）
        /// </summary>
        public uint AddDodge;

        /// <summary>
        /// 加暴击（百分比）
        /// </summary>
        public uint AddCritPercent;

        /// <summary>
        /// 加抗暴击（百分比）
        /// </summary>
        public uint AddUncritPercent;

        /// <summary>
        /// 增加初始怒气固定值
        /// </summary>
        public uint AddAngryValue;

        /// <summary>
        /// 增加最大怒气百分比
        /// </summary>
        public uint AddAngryPercent;

        /// <summary>
        /// 增加受击获得的怒气值
        /// </summary>
        public uint AddHitAngry;

        /// <summary>
        /// 回合结束时增加的怒气值
        /// </summary>
        public uint AddEndAngry;

        /// <summary>
        /// 进场为自身添加的buff,有多个“|”隔开
        /// </summary>
        public string ObiterBuffId;

        /// <summary>
        /// 添加的buff(不同事件下，对象不同）
        /// </summary>
        public string BuffType;

        /// <summary>
        /// 优先级（用于入场时，触发效果的优先级。越高代表越早触发）
        /// </summary>
        public uint Priority;


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
    /// Desc    passvityskill配置文件访问接口
    /// </summary>
    public partial class PassvityskillDao:BaseDao<PassvityskillDao,uint,PassvityskillConfig>
    {
        public override string GetName()
        {
            return "passvityskill";
        }

        protected override void OnChangeLang(ref PassvityskillConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                PassvityskillConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.Tid, language, ref cfg.Tid);
                tDao.TryGetText(cfg.InfoTid, language, ref cfg.InfoTid);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.18
    /// Desc    passvityskill配置文件解码器
    /// </summary>
    public partial class PassvityskillDecoder : BaseCfgDecoder<PassvityskillConfig, PassvityskillCfgData>
    {
        public override string GetName()
        {
            return "passvityskill";
        }

        protected override void ProcessRow(PassvityskillConfig excel)
        {
            GetU32("id", out excel.Id);
            GetString("designer_remarks", out excel.DesignerRemarks);
            GetString("$tid", out excel.Tid);
            GetString("icon_address", out excel.IconAddress);
            GetString("$info_tid", out excel.InfoTid);
            GetString("info_value", out excel.InfoValue);
            GetU32("actID", out excel.ActID);
            GetU32("lv", out excel.Lv);
            GetU32("event", out excel.Event);
            GetU32("add_atk_value", out excel.AddAtkValue);
            GetU32("add_atk_percent", out excel.AddAtkPercent);
            GetU32("add_def_value", out excel.AddDefValue);
            GetU32("add_def_percent", out excel.AddDefPercent);
            GetU32("add_hp_value", out excel.AddHpValue);
            GetU32("add_hp_percent", out excel.AddHpPercent);
            GetU32("add_hit", out excel.AddHit);
            GetU32("add_dodge", out excel.AddDodge);
            GetU32("add_crit_percent", out excel.AddCritPercent);
            GetU32("add_uncrit_percent", out excel.AddUncritPercent);
            GetU32("add_angry_value", out excel.AddAngryValue);
            GetU32("add_angry_percent", out excel.AddAngryPercent);
            GetU32("add_hit_angry", out excel.AddHitAngry);
            GetU32("add_end_angry", out excel.AddEndAngry);
            GetString("obiter_buff_id", out excel.ObiterBuffId);
            GetString("buff_type", out excel.BuffType);
            GetU32("priority", out excel.Priority);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
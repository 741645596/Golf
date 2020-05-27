using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    skill配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SkillConfig : IConfig<uint>
    {
        /// <summary>
        /// 技能id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 策划备注字段
        /// </summary>
        public string DesignerRemarks;

        /// <summary>
        /// 技能名称
        /// </summary>
        public string Tid;

        /// <summary>
        /// 释放技能的立绘
        /// </summary>
        public string Icon2dPath;

        /// <summary>
        /// 释放技能时的音效
        /// </summary>
        public uint SkillVoice;

        /// <summary>
        /// 技能图标
        /// </summary>
        public string IconAddress;

        /// <summary>
        /// 技能描述
        /// </summary>
        public string InfoTid;

        /// <summary>
        /// 技能描述内填写的具体值，“|”隔开
        /// </summary>
        public string InfoValue;

        /// <summary>
        /// 技能表现ID，索引到skill_act.csv
        /// </summary>
        public uint ActId;

        /// <summary>
        /// 技能类型（1普攻；2技能；3天赋；4大招）
        /// </summary>
        public uint Skilltype;

        /// <summary>
        /// 技能的ai类型（0无，1普通被动，2以上具体ai类型）
        /// </summary>
        public uint SkillAiType;

        /// <summary>
        /// 0代表无伤害类型，1代表单体，2代表群体
        /// </summary>
        public uint SkillDamageType;

        /// <summary>
        /// 技能等级
        /// </summary>
        public uint Lv;

        /// <summary>
        /// 技能冷却时间（主要用于AI普攻,单位为回合）
        /// </summary>
        public uint Cd;

        /// <summary>
        /// 技能释放所需的怒气值（主动技消耗怒气，大招消耗充能点）
        /// </summary>
        public uint Cost;

        /// <summary>
        /// 关联到skill_effect表
        /// </summary>
        public uint[] ObjectEffect;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    skill配置文件访问接口
    /// </summary>
    public partial class SkillDao:BaseDao<SkillDao,uint,SkillConfig>
    {
        public override string GetName()
        {
            return "skill";
        }

        protected override void OnChangeLang(ref SkillConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                SkillConfig cfg = cfgs[i];
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
    /// Date    2019.3.4
    /// Desc    skill配置文件解码器
    /// </summary>
    public partial class SkillDecoder : BaseCfgDecoder<SkillConfig, SkillCfgData>
    {
        public override string GetName()
        {
            return "skill";
        }

        protected override void ProcessRow(SkillConfig excel)
        {
            GetU32("id", out excel.Id);
            GetString("designer_remarks", out excel.DesignerRemarks);
            GetString("$tid", out excel.Tid);
            GetString("icon_2d_path", out excel.Icon2dPath);
            GetU32("skill_voice", out excel.SkillVoice);
            GetString("icon_address", out excel.IconAddress);
            GetString("$info_tid", out excel.InfoTid);
            GetString("info_value", out excel.InfoValue);
            GetU32("act_id", out excel.ActId);
            GetU32("skilltype", out excel.Skilltype);
            GetU32("skill_ai_type", out excel.SkillAiType);
            GetU32("skill_damage_type", out excel.SkillDamageType);
            GetU32("lv", out excel.Lv);
            GetU32("cd", out excel.Cd);
            GetU32("cost", out excel.Cost);
            GetArr("object_effect", StrHelper.ArrSplitLv1, out excel.ObjectEffect, ParseU32);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
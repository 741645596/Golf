using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.6
    /// Desc    creature配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class CreatureConfig : IConfig<uint>
    {
        /// <summary>
        /// 怪物id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 怪物形象显示;配置资源地址
        /// </summary>
        public string AvatarPath;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark;

        /// <summary>
        /// 配置怪物名称
        /// </summary>
        public string AvatarName;

        /// <summary>
        /// 怪物形象比例（1:1缩放）;100表示100%
        /// </summary>
        public uint AvatarScale;

        /// <summary>
        /// 怪物头像
        /// </summary>
        public string Image;

        /// <summary>
        /// 怪物光环，配置资源名称
        /// </summary>
        public string Halo;

        /// <summary>
        /// 0小怪1精英2boss
        /// </summary>
        public uint Type;

        /// <summary>
        /// 战斗ui类型
        /// </summary>
        public string UiType;

        /// <summary>
        /// 普攻ID与等级
        /// </summary>
        public uint[] NormalAttack;

        /// <summary>
        /// 主动技ID与等级
        /// </summary>
        public uint[] ActiveSkill;

        /// <summary>
        /// 被动技ID与等级
        /// </summary>
        public uint[] PassiveSkill;

        /// <summary>
        /// 入场动画时间
        /// </summary>
        public uint AppareTime;


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
    /// Desc    creature配置文件访问接口
    /// </summary>
    public partial class CreatureDao:BaseDao<CreatureDao,uint,CreatureConfig>
    {
        public override string GetName()
        {
            return "creature";
        }

        protected override void OnChangeLang(ref CreatureConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                CreatureConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.AvatarName, language, ref cfg.AvatarName);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.6
    /// Desc    creature配置文件解码器
    /// </summary>
    public partial class CreatureDecoder : BaseCfgDecoder<CreatureConfig, CreatureCfgData>
    {
        public override string GetName()
        {
            return "creature";
        }

        protected override void ProcessRow(CreatureConfig excel)
        {
            GetU32("id", out excel.Id);
            GetString("avatar_path", out excel.AvatarPath);
            GetString("remark", out excel.Remark);
            GetString("$avatar_name", out excel.AvatarName);
            GetU32("avatar_scale", out excel.AvatarScale);
            GetString("image", out excel.Image);
            GetString("halo", out excel.Halo);
            GetU32("type", out excel.Type);
            GetString("ui_type", out excel.UiType);
            GetArr("normal_attack", StrHelper.ArrSplitLv1, out excel.NormalAttack, ParseU32);
            GetArr("active_skill", StrHelper.ArrSplitLv1, out excel.ActiveSkill, ParseU32);
            GetArr("passive_skill", StrHelper.ArrSplitLv1, out excel.PassiveSkill, ParseU32);
            GetU32("appare_time", out excel.AppareTime);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
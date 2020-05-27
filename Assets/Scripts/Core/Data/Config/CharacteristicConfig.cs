using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.1
    /// Desc    characteristic配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class CharacteristicConfig : IConfig<uint>
    {
        /// <summary>
        /// 配置属性id
        /// </summary>
        public uint CharacteristicId;

        /// <summary>
        /// 配置属性底框对应图标路径
        /// </summary>
        public string FrameAddress;

        /// <summary>
        /// 配置属性图标路径
        /// </summary>
        public string IconAddress;

        /// <summary>
        /// 配置属性对应珠子图标路径
        /// </summary>
        public string BeadAddress;

        /// <summary>
        /// 配置属性名称TID
        /// </summary>
        public string NameTid;

        /// <summary>
        /// 配置属性描述TID
        /// </summary>
        public string InfoTid;

        /// <summary>
        /// 配置属性克制关系
        /// </summary>
        public int[] Restraint;

        /// <summary>
        /// 配置被克制关系
        /// </summary>
        public int[] Weak;

        /// <summary>
        /// 怒气条充能图片
        /// </summary>
        public string MpColor;

        /// <summary>
        /// 怒气条底材质
        /// </summary>
        public string MpMaterial;


        public uint GetKey()
        {
            return CharacteristicId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.1
    /// Desc    characteristic配置文件访问接口
    /// </summary>
    public partial class CharacteristicDao:BaseDao<CharacteristicDao,uint,CharacteristicConfig>
    {
        public override string GetName()
        {
            return "characteristic";
        }

        protected override void OnChangeLang(ref CharacteristicConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                CharacteristicConfig cfg = cfgs[i];
                tDao.TryGetText(cfg.NameTid, language, ref cfg.NameTid);
                tDao.TryGetText(cfg.InfoTid, language, ref cfg.InfoTid);
            }
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.1
    /// Desc    characteristic配置文件解码器
    /// </summary>
    public partial class CharacteristicDecoder : BaseCfgDecoder<CharacteristicConfig, CharacteristicCfgData>
    {
        public override string GetName()
        {
            return "characteristic";
        }

        protected override void ProcessRow(CharacteristicConfig excel)
        {
            GetU32("#characteristic_id", out excel.CharacteristicId);
            GetString("frame_address", out excel.FrameAddress);
            GetString("icon_address", out excel.IconAddress);
            GetString("bead_address", out excel.BeadAddress);
            GetString("$name_tid", out excel.NameTid);
            GetString("$info_tid", out excel.InfoTid);
            GetArr("restraint", StrHelper.ArrSplitLv1, out excel.Restraint, ParseI32);
            GetArr("weak", StrHelper.ArrSplitLv1, out excel.Weak, ParseI32);
            GetString("mp_color", out excel.MpColor);
            GetString("mp_material", out excel.MpMaterial);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
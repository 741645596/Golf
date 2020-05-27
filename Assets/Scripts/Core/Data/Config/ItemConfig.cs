using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.27
    /// Desc    item配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class ItemConfig : IConfig<uint>
    {
        /// <summary>
        /// 配置道具id
        /// </summary>
        public uint ItemId;

        /// <summary>
        /// 配置图标路径
        /// </summary>
        public string IconAddress;

        /// <summary>
        /// 配置道具名称对应TID
        /// </summary>
        public string NameTid;

        /// <summary>
        /// 配置道具描述对应TID
        /// </summary>
        public string InfoTid;

        /// <summary>
        /// 配置道具类型：
        /// 1~100 为代币（1食物，2铁矿）
        /// 101~2000为普通道具（101英雄升级专用经验道具，102佣兵升级专用经验道具）
        /// </summary>
        public uint ItemType;

        /// <summary>
        /// 配置道具需要使用的参数
        /// </summary>
        public string Parameter;

        /// <summary>
        /// 配置道具品质
        /// </summary>
        public uint Quality;

        /// <summary>
        /// 配置道具堆叠上限
        /// </summary>
        public uint MaxNum;

        /// <summary>
        /// 配置该道具是否显示在道具背包中
        /// </summary>
        public uint ItemBag;

        /// <summary>
        /// 配置是否为代币：
        /// 1代表代币
        /// 2代表普通道具
        /// </summary>
        public uint Type;


        public uint GetKey()
        {
            return ItemId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.27
    /// Desc    item配置文件访问接口
    /// </summary>
    public partial class ItemDao:BaseDao<ItemDao,uint,ItemConfig>
    {
        public override string GetName()
        {
            return "item";
        }

        protected override void OnChangeLang(ref ItemConfig[] cfgs, string language)
        {
            TextDao tDao = TextDao.Inst;
            for (int i = 0; i < cfgs.Length; i++)
            {
                ItemConfig cfg = cfgs[i];
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
    /// Date    2019.2.27
    /// Desc    item配置文件解码器
    /// </summary>
    public partial class ItemDecoder : BaseCfgDecoder<ItemConfig, ItemCfgData>
    {
        public override string GetName()
        {
            return "item";
        }

        protected override void ProcessRow(ItemConfig excel)
        {
            GetU32("#item_id", out excel.ItemId);
            GetString("icon_address", out excel.IconAddress);
            GetString("$name_tid", out excel.NameTid);
            GetString("$info_tid", out excel.InfoTid);
            GetU32("item_type", out excel.ItemType);
            GetString("parameter", out excel.Parameter);
            GetU32("quality", out excel.Quality);
            GetU32("max_num", out excel.MaxNum);
            GetU32("item_bag", out excel.ItemBag);
            GetU32("type", out excel.Type);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
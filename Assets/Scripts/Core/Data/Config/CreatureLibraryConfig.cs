using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.16
    /// Desc    creature_library配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class CreatureLibraryConfig : IConfig<uint>
    {
        /// <summary>
        /// 库ID
        /// </summary>
        public uint Id;

        /// <summary>
        /// 怪物ID
        /// </summary>
        public uint[] CreatureId;

        /// <summary>
        /// 权重；百分制
        /// </summary>
        public uint[] Weight;


        public uint GetKey()
        {
            return Id;
        }

        public uint GetCreatureID(int weight)
        {
            uint id = 0;
            uint total = 0;

            for (int i = 0; i < Weight.Length; ++i)
            {
                total += Weight[i];
                if (total >= weight)
                {
                    id = this.CreatureId[i];
                    break;
                }
            }

            return id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.16
    /// Desc    creature_library配置文件访问接口
    /// </summary>
    public partial class CreatureLibraryDao:BaseDao<CreatureLibraryDao,uint,CreatureLibraryConfig>
    {
        public override string GetName()
        {
            return "creature_library";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.16
    /// Desc    creature_library配置文件解码器
    /// </summary>
    public partial class CreatureLibraryDecoder : BaseCfgDecoder<CreatureLibraryConfig, CreatureLibraryCfgData>
    {
        public override string GetName()
        {
            return "creature_library";
        }

        protected override void ProcessRow(CreatureLibraryConfig excel)
        {
            GetU32("id", out excel.Id);
            GetArr("creature_id", StrHelper.ArrSplitLv1, out excel.CreatureId, ParseU32);
            GetArr("weight", StrHelper.ArrSplitLv1, out excel.Weight, ParseU32);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
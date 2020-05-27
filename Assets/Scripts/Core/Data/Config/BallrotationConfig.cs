using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.21
    /// Desc    ballrotation配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class BallrotationConfig : IConfig<int>
    {
        /// <summary>
        /// Id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 弹跳次数
        /// </summary>
        public int JumpTimes;

        /// <summary>
        /// 旋转参数
        /// </summary>
        public int RotationPara;

        /// <summary>
        /// 前后旋球旋转参数
        /// </summary>
        public int SpinPara;


        public int GetKey()
        {
            return JumpTimes;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.21
    /// Desc    ballrotation配置文件访问接口
    /// </summary>
    public partial class BallrotationDao:BaseDao<BallrotationDao,int,BallrotationConfig>
    {
        public override string GetName()
        {
            return "ballrotation";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.21
    /// Desc    ballrotation配置文件解码器
    /// </summary>
    public partial class BallrotationDecoder : BaseCfgDecoder<BallrotationConfig, BallrotationCfgData>
    {
        public override string GetName()
        {
            return "ballrotation";
        }

        protected override void ProcessRow(BallrotationConfig excel)
        {
            GetU32("id", out excel.Id);
            GetI32("#jump_times", out excel.JumpTimes);
            GetI32("rotation_para", out excel.RotationPara);
            GetI32("spin_para", out excel.SpinPara);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
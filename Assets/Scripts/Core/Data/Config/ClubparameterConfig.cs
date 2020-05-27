using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.13
    /// Desc    clubparameter配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class ClubparameterConfig : IConfig<uint>
    {
        /// <summary>
        /// 球杆类型Id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 球杆类型名称
        /// </summary>
        public int NameTid;

        /// <summary>
        /// 最低打击距离
        /// </summary>
        public int MinDistance;

        /// <summary>
        /// 增强击球距离
        /// </summary>
        public int HitDistanceIncrease;

        /// <summary>
        /// 减弱击球距离
        /// </summary>
        public int HitDistanceDecrease;

        /// <summary>
        /// 指针基础速度
        /// </summary>
        public int NeedleSpeed;

        /// <summary>
        /// 仰角sin值
        /// </summary>
        public int AngleSin;

        /// <summary>
        /// 仰角cos值
        /// </summary>
        public int AngleCos;

        /// <summary>
        /// 曲球最大初速度校正
        /// </summary>
        public int CurvedAddition;

        /// <summary>
        /// 侧旋最大角度
        /// </summary>
        public int SideSpinAngle;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.13
    /// Desc    clubparameter配置文件访问接口
    /// </summary>
    public partial class ClubparameterDao:BaseDao<ClubparameterDao,uint,ClubparameterConfig>
    {
        public override string GetName()
        {
            return "clubparameter";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.5.13
    /// Desc    clubparameter配置文件解码器
    /// </summary>
    public partial class ClubparameterDecoder : BaseCfgDecoder<ClubparameterConfig, ClubparameterCfgData>
    {
        public override string GetName()
        {
            return "clubparameter";
        }

        protected override void ProcessRow(ClubparameterConfig excel)
        {
            GetU32("id", out excel.Id);
            GetI32("name_tid", out excel.NameTid);
            GetI32("min_distance", out excel.MinDistance);
            GetI32("hit_distance_increase", out excel.HitDistanceIncrease);
            GetI32("hit_distance_decrease", out excel.HitDistanceDecrease);
            GetI32("needle_speed", out excel.NeedleSpeed);
            GetI32("angle_sin", out excel.AngleSin);
            GetI32("angle_cos", out excel.AngleCos);
            GetI32("curved_addition", out excel.CurvedAddition);
            GetI32("side_spin_angle", out excel.SideSpinAngle);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
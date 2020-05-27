using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.22
    /// Desc    skill_effect配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class SkillEffectConfig : IConfig<uint>
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
        /// 目标类型，1为自身，2为友军，3为敌军，4为棋盘等
        /// </summary>
        public uint Object;

        /// <summary>
        /// 目标选择策略id，可填多个。根据排序，若是0、1、2代表组合，同时满足所有条件
        /// </summary>
        public uint[] SelectObject;

        /// <summary>
        /// 0代表多个条件排序，或的关系。1代表组合，全满足条件的才是目标。（与的关系）
        /// </summary>
        public uint SelectType;

        /// <summary>
        /// 目标选择个数
        /// </summary>
        public uint Num;

        /// <summary>
        /// 排序 0 随机 1 最大 2 最小
        /// </summary>
        public uint Order;

        /// <summary>
        /// 效果id
        /// </summary>
        public uint FunctionEffect;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.22
    /// Desc    skill_effect配置文件访问接口
    /// </summary>
    public partial class SkillEffectDao:BaseDao<SkillEffectDao,uint,SkillEffectConfig>
    {
        public override string GetName()
        {
            return "skill_effect";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.22
    /// Desc    skill_effect配置文件解码器
    /// </summary>
    public partial class SkillEffectDecoder : BaseCfgDecoder<SkillEffectConfig, SkillEffectCfgData>
    {
        public override string GetName()
        {
            return "skill_effect";
        }

        protected override void ProcessRow(SkillEffectConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetString("designer_remarks", out excel.DesignerRemarks);
            GetU32("object", out excel.Object);
            GetArr("select_object", StrHelper.ArrSplitLv1, out excel.SelectObject, ParseU32);
            GetU32("select_type", out excel.SelectType);
            GetU32("num", out excel.Num);
            GetU32("order", out excel.Order);
            GetU32("function_effect", out excel.FunctionEffect);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
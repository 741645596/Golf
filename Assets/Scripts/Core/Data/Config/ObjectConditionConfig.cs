using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.22
    /// Desc    object_condition配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class ObjectConditionConfig : IConfig<uint>
    {
        /// <summary>
        /// 选敌的编号，该表唯一id
        /// </summary>
        public uint Id;

        /// <summary>
        /// 描述，策划编辑使用
        /// </summary>
        public string DesignerRemarks;

        /// <summary>
        /// 具体选敌策略，0为随机，1为属性,2单位类型（对珠子而已，value的值1普通，2炸弹，3水晶，4冻结,999全部类型）
        /// </summary>
        public uint ConditionType;

        /// <summary>
        /// 判断条件(0随机，1等于，2小于 3大于 4不等于）
        /// </summary>
        public uint ConditionOp;

        /// <summary>
        /// 具体的属性（根据类型目标，代表不同的属性值。如棋盘，代表的是珠子的颜色，如单位，则代表具体单位这些属性的具体值）
        /// </summary>
        public uint ConditionValue;


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
    /// Desc    object_condition配置文件访问接口
    /// </summary>
    public partial class ObjectConditionDao:BaseDao<ObjectConditionDao,uint,ObjectConditionConfig>
    {
        public override string GetName()
        {
            return "object_condition";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.22
    /// Desc    object_condition配置文件解码器
    /// </summary>
    public partial class ObjectConditionDecoder : BaseCfgDecoder<ObjectConditionConfig, ObjectConditionCfgData>
    {
        public override string GetName()
        {
            return "object_condition";
        }

        protected override void ProcessRow(ObjectConditionConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetString("designer_remarks", out excel.DesignerRemarks);
            GetU32("condition_type", out excel.ConditionType);
            GetU32("condition_op", out excel.ConditionOp);
            GetU32("condition_value", out excel.ConditionValue);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
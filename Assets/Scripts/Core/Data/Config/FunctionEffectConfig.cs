using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    function_effect配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class FunctionEffectConfig : IConfig<uint>
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
        /// 效果id，例如1攻击力百分比回血，2技能伤害，3加buff，4棋盘变色，5冻结棋盘，6腐蚀棋盘，7生命百分比回血,9格挡百分比等
        /// </summary>
        public uint EffectId;

        /// <summary>
        /// 根据效果不同，含义不同。例如伤害和回血代表具体伤害值，加buff代表增加的buff的id，变色代表从A颜色变为B颜色
        /// </summary>
        public int Value;

        /// <summary>
        /// 计算技能伤害时使用，代表固定值伤害值|最大生命百分比的伤害系数
        /// </summary>
        public int[] Value2;

        /// <summary>
        /// 产生效果时，调用的表现的id（调用skill_act表的id）
        /// </summary>
        public uint ActId;


        public uint GetKey()
        {
            return Id;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    function_effect配置文件访问接口
    /// </summary>
    public partial class FunctionEffectDao:BaseDao<FunctionEffectDao,uint,FunctionEffectConfig>
    {
        public override string GetName()
        {
            return "function_effect";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.2.20
    /// Desc    function_effect配置文件解码器
    /// </summary>
    public partial class FunctionEffectDecoder : BaseCfgDecoder<FunctionEffectConfig, FunctionEffectCfgData>
    {
        public override string GetName()
        {
            return "function_effect";
        }

        protected override void ProcessRow(FunctionEffectConfig excel)
        {
            GetU32("#id", out excel.Id);
            GetString("designer_remarks", out excel.DesignerRemarks);
            GetU32("effect_id", out excel.EffectId);
            GetI32("value", out excel.Value);
            GetArr("value2", StrHelper.ArrSplitLv1, out excel.Value2, ParseI32);
            GetU32("act_id", out excel.ActId);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
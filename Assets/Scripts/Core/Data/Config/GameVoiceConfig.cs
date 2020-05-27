using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    game_voice配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class GameVoiceConfig : IConfig<uint>
    {
        /// <summary>
        /// 游戏音效的id
        /// </summary>
        public uint VoiceId;

        /// <summary>
        /// 游戏音效的文件名称
        /// </summary>
        public string VoiceName;

        /// <summary>
        /// 音量
        /// </summary>
        public uint Volume;


        public uint GetKey()
        {
            return VoiceId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    game_voice配置文件访问接口
    /// </summary>
    public partial class GameVoiceDao:BaseDao<GameVoiceDao,uint,GameVoiceConfig>
    {
        public override string GetName()
        {
            return "game_voice";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    game_voice配置文件解码器
    /// </summary>
    public partial class GameVoiceDecoder : BaseCfgDecoder<GameVoiceConfig, GameVoiceCfgData>
    {
        public override string GetName()
        {
            return "game_voice";
        }

        protected override void ProcessRow(GameVoiceConfig excel)
        {
            GetU32("#voice_id", out excel.VoiceId);
            GetString("voice_name", out excel.VoiceName);
            GetU32("volume", out excel.Volume);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
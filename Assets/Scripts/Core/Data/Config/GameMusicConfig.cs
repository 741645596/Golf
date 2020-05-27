using System;
using IGG.Core.Helper;

namespace IGG.Core.Data.Config
{
    #region 配置定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    game_music配置文件映射类型
    /// </summary>
    [Serializable]
    public partial class GameMusicConfig : IConfig<uint>
    {
        /// <summary>
        /// 游戏音乐的id
        /// </summary>
        public uint MusicId;

        /// <summary>
        /// 游戏音乐的文件名称
        /// </summary>
        public string MusicName;

        /// <summary>
        /// 音量
        /// </summary>
        public uint Volume;


        public uint GetKey()
        {
            return MusicId;
        }
    }
    #endregion

    #region 配置访问接口定义
    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    game_music配置文件访问接口
    /// </summary>
    public partial class GameMusicDao:BaseDao<GameMusicDao,uint,GameMusicConfig>
    {
        public override string GetName()
        {
            return "game_music";
        }
    }
    #endregion

    #region 配置解码器定义
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG

    /// <summary>
    /// Author  Robot_CfgCodeCreator
    /// Date    2019.3.4
    /// Desc    game_music配置文件解码器
    /// </summary>
    public partial class GameMusicDecoder : BaseCfgDecoder<GameMusicConfig, GameMusicCfgData>
    {
        public override string GetName()
        {
            return "game_music";
        }

        protected override void ProcessRow(GameMusicConfig excel)
        {
            GetU32("#music_id", out excel.MusicId);
            GetString("music_name", out excel.MusicName);
            GetU32("volume", out excel.Volume);
            ProcessRowExt(excel);
        }
    }

#endif
    #endregion
}
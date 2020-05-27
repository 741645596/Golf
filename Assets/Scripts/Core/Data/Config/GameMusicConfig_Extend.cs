using System.Collections.Generic;

namespace IGG.Core.Data.Config
{
    #region 配置访问接口扩展
    public partial class GameMusicDao
    {
    }
    #endregion

    #region 配置定义扩展
    public partial class GameMusicConfig
    {
    }
    #endregion

    #region 配置解码器扩展
#if ServerClient || UNITY_EDITOR || USE_CSV_CONFIG
    public partial class GameMusicDecoder
    {
        private void ProcessRowExt(GameMusicConfig excel)
        {
            //在这里对配置的解析进行扩展
        }

        protected override void AfterProcess(List<GameMusicConfig> datas)
        {
            //整张表处理完成后执行，用于对表的后期处理，如排序
        }

        public override void AllDecodeAfterProcess()
        {
            //全部配置处理完后执行，用于处理有全局关系的值，可以在生成配置期间进行预处理
        }
    }
#endif
    #endregion
}
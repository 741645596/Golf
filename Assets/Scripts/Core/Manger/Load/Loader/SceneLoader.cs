#region Namespace

using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace IGG.Game.Core.Load
{
    /// <summary>
    /// Author  xuzhihui
    /// Date    2018.06.13
    /// Desc    场景加载器
    /// </summary>
    public class SceneLoader : Loader
    {
        private AsyncOperation m_request;

        public SceneLoader()
            : base(LoaderType.Scene)
        {
        }

        public override void Load()
        {
            base.Load();

            LoadSceneMode mode = LoadSceneMode.Single;
            if (Param != null)
            {
                bool additive = (bool) Param;
                if (additive)
                {
                    mode = LoadSceneMode.Additive;
                }
            }

            if (IsAsync)
            {
                m_request = SceneManager.LoadSceneAsync(Path, mode);
            }
            else
            {
                SceneManager.LoadScene(Path, mode);
                OnLoadCompleted(true);
            }
        }

        public override void Update()
        {
            if (State == LoaderState.Loading)
            {
                if (m_request == null)
                {
                    OnLoadCompleted(false);
                }
                else if (m_request.isDone)
                {
                    OnLoadCompleted(true);
                    m_request = null;
                }
                else
                {
                    OnLoadProgress(m_request.progress);
                }
            }
        }
    }
}
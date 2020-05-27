#region Namespace

using System;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif

#endregion

namespace IGG.Game.Core.Load
{
    /// <summary>
    /// Author  xuzhihui
    /// Date    2018.06.13
    /// Desc    普通资源加载器(编辑器专用)
    /// </summary>
    public class AssetLoader : Loader
    {
        private Object m_data;

        public AssetLoader()
            : base(LoaderType.Asset)
        {
        }

        public override void Load()
        {
            base.Load();

#if UNITY_EDITOR
            Type type = Param as Type;
            if (type == null)
            {
                type = typeof(Object);
            }

            if (IsAsync)
            {
                m_data = AssetDatabase.LoadAssetAtPath(Path, type);
            }
            else
            {
                Object data = AssetDatabase.LoadAssetAtPath(Path, type);
                OnLoadCompleted(data);
            }
#else
			if(!IsAsync)
			{
				OnLoadCompleted(null);
			}
#endif
        }

        public override void Update()
        {
            if (State == LoaderState.Loading)
            {
                OnLoadCompleted(m_data);
                m_data = null;
            }
        }
    }
}
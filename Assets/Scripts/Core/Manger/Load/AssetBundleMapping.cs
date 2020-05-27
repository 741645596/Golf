﻿#region Namespace

using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = IGG.Logging.Logger;

#endregion

public class AssetBundleMapping : ScriptableObject
{
    public AssetBundleInfo[] Assetbundles;

    private Dictionary<string, string> m_mapping;

    public void Init()
    {
        m_mapping = new Dictionary<string, string>();

        for (int i = 0; i < Assetbundles.Length; ++i)
        {
            AssetBundleInfo info = Assetbundles[i];
            for (int j = 0; j < info.Files.Length; ++j)
            {
                if (!m_mapping.ContainsKey(info.Files[j]))
                {
                    m_mapping.Add(info.Files[j], info.Name);
                }
            }
        }
    }

    public string GetAssetBundleNameFromAssetPath(string assetPath)
    {
        assetPath = assetPath.ToLower();
        if (assetPath.StartsWith("data/"))
        {
            assetPath = assetPath.Substring(assetPath.IndexOf('/') + 1);
        }

        string abName;
        if (!m_mapping.TryGetValue(assetPath, out abName))
        {
            Logger.LogError(string.Format("No Found Asset: {0}", assetPath));
            return "";
        }

        return abName;
    }

    [Serializable]
    public class AssetBundleInfo
    {
        public string[] Files;
        public string Name;
    }
}
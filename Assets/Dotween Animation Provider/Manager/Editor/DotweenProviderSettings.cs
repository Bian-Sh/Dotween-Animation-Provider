using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DotweenProviderSettings : ScriptableObject
{
    [Header("将预览时需要刷新的组件类型放在这儿")]
    public List<MonoScript> types = new List<MonoScript>();
    static DotweenProviderSettings m_Instance;
    static DotweenProviderSettings Instance
    {
        get
        {
            if (!m_Instance)
            {
                m_Instance = LoadOrCreateMainAsset<DotweenProviderSettings>();
            }
            return m_Instance;
        }
    }

    void Awake() 
    {
        //预设 这两个已知的必须刷新的组件
        types = new List<MonoScript>();
        types.Add(GetScriptAssetsByType(typeof(TMPro.TextMeshProUGUI)));
        types.Add(GetScriptAssetsByType(typeof(UnityEngine.UI.Text)));
    }

    internal static bool VerifyTargetNeedSetDirty(UnityEngine.Object target)
    {
        return Instance.types.Exists(v=>v.GetClass() == target.GetType());
    }

    #region Assistance Function
    static T LoadOrCreateMainAsset<T>() where T : ScriptableObject
    {
        var ms = GetScriptAssetsByType(typeof(T));
        var path = AssetDatabase.GetAssetPath(ms);
        path = path.Substring(0, path.LastIndexOf("/"));
        path = Path.Combine(path, "Data");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var type = ms.GetClass();
        path = $"{path}/{type.Name}.asset";
        var asset = AssetDatabase.LoadMainAssetAtPath(path);
        if (!asset)
        {
            asset = CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
        }
        return asset as T;
    }
    static MonoScript GetScriptAssetsByType(Type _type)
    {
        MonoScript monoScript = default;
        var scriptGUIDs = AssetDatabase.FindAssets($"t:script {_type.Name}");
        foreach (var scriptGUID in scriptGUIDs)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(scriptGUID);
            monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
            if (monoScript && monoScript.GetClass() == _type) break;
        }
        return monoScript;
    }
    #endregion
}

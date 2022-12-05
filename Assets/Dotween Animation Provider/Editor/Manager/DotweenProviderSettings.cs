using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DotweenProviderSettings : ScriptableObject
{
    [Header("将预览时需要刷新的组件类型放在这儿")]
    [Tooltip("在编辑器中预览时，有些组件不会出现效果，那是因为需要实时刷新，把这些组件放到这儿，预览管理器就会对他们的实例进行刷新了")]
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
        //已知 图形学 组件预览时要主动去刷新，先预设到列表中
        types = new List<MonoScript>();
        types.Add(GetScriptAssetsByType(typeof(UnityEngine.UI.Graphic)));
    }

    internal static bool VerifyTargetNeedSetDirty(UnityEngine.Object target)
    {
        Predicate<MonoScript> predicate = v =>
        {
            return v.GetClass().IsAssignableFrom(target.GetType())
                      || v.GetClass() == target.GetType();
        };
        return Instance.types.Exists(predicate);
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

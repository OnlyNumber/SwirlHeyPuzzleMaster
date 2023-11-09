using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Android;

namespace OptimisationTool
{
    public class AssetFinder
    {
        private string[] _allAssets;
        private HashSet<string> _usedAssets;
        private Object[] _resources;

        public AssetFinder()
        {
            _allAssets = AssetDatabase.FindAssets("t:texture t:material t:audioClip t:prefab", new[] { "Assets" });
            _usedAssets = new HashSet<string>();
            _resources = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        }

        public List<string> FindUnusedAssets()
        {
            FindUsedAssetsInResources();
            FindUsedAssetsInScriptableObjects();
            FindUsedAssetsInIcons();

            List<string> unusedAssets = new List<string>();

            foreach (string assetGuid in _allAssets)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

                if (!IsInResourcesFolder(assetPath) && !_usedAssets.Contains(assetPath))
                {
                    unusedAssets.Add(assetPath);
                }
            }

            return unusedAssets;
        }

        private void FindUsedAssetsInResources()
        {
            foreach (var obj in _resources.OfType<GameObject>())
            {
                FindUsedAssetsInGameObjectComponents(obj);
                FindUsedAssetsInRendererMaterials(obj);
            }
        }

        private void FindUsedAssetsInGameObjectComponents(GameObject go)
        {
            var components = go.GetComponentsInChildren<Component>(true);
            foreach (var component in components)
            {
                if (component == null)
                    continue;

                AnalyzeSerializedObject(component);
            }
        }

        private void FindUsedAssetsInRendererMaterials(GameObject go)
        {
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                if (renderer.sharedMaterials == null)
                    continue;

                foreach (var material in renderer.sharedMaterials)
                {
                    if (material == null || material.shader == null)
                        continue;

                    int propertyCount = ShaderUtil.GetPropertyCount(material.shader);
                    for (int i = 0; i < propertyCount; i++)
                    {
                        if (ShaderUtil.GetPropertyType(material.shader, i) != ShaderUtil.ShaderPropertyType.TexEnv)
                            continue;

                        string texturePropertyName = ShaderUtil.GetPropertyName(material.shader, i);
                        Texture texture = material.GetTexture(texturePropertyName);
                        if (texture != null)
                        {
                            AnalyzeSerializedObject(texture);
                        }
                    }
                }
            }
        }

        private void FindUsedAssetsInScriptableObjects()
        {
            var scriptableObjects = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" });
            foreach (var scriptableObjectGuid in scriptableObjects)
            {
                string scriptableObjectPath = AssetDatabase.GUIDToAssetPath(scriptableObjectGuid);
                var scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(scriptableObjectPath);
                if (scriptableObject != null)
                {
                    AnalyzeSerializedObject(scriptableObject);
                }
            }
        }

        private void FindUsedAssetsInIcons()
        {
            _usedAssets.UnionWith(PlayerSettings.GetIcons(UnityEditor.Build.NamedBuildTarget.iOS, IconKind.Any).Select(icon => AssetDatabase.GetAssetPath(icon)));

            var androidIcons = new List<Texture2D>();
            androidIcons.AddRange(PlayerSettings.GetIcons(UnityEditor.Build.NamedBuildTarget.Unknown, IconKind.Any));
            androidIcons.AddRange(PlayerSettings.GetPlatformIcons(UnityEditor.Build.NamedBuildTarget.Android, AndroidPlatformIconKind.Adaptive).SelectMany(i => i.GetTextures()));
            androidIcons.AddRange(PlayerSettings.GetPlatformIcons(UnityEditor.Build.NamedBuildTarget.Android, AndroidPlatformIconKind.Round).SelectMany(i => i.GetTextures()));
            androidIcons.AddRange(PlayerSettings.GetPlatformIcons(UnityEditor.Build.NamedBuildTarget.Android, AndroidPlatformIconKind.Legacy).SelectMany(i => i.GetTextures()));

            _usedAssets.UnionWith(androidIcons.Select(icon => AssetDatabase.GetAssetPath(icon)));
        }

        private void AnalyzeSerializedObject(Object obj)
        {
            var serializedObject = new SerializedObject(obj);
            var iterator = serializedObject.GetIterator();
            while (iterator.NextVisible(true))
            {
                if (iterator.propertyType == SerializedPropertyType.ObjectReference && iterator.objectReferenceValue != null)
                {
                    string assetPath = AssetDatabase.GetAssetPath(iterator.objectReferenceValue);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        _usedAssets.Add(assetPath);
                    }
                }
            }
        }

        private bool IsInResourcesFolder(string assetPath)
        {
            return assetPath.Split('/').Any(segment => segment == "Resources");
        }
    }
}

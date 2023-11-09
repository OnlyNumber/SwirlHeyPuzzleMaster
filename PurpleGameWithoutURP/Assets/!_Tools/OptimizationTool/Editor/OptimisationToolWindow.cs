using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace OptimisationTool
{
    public enum AlgorithmState
    {
        Starting,
        UnusedAssetsHandling,
        FindingDuplicates,
        DuplicatesHandling,
        OptimizationHandling
    }
    public class OptimisationToolWindow : EditorWindow
    {
        #region Values
        private string _mainFolder = "Assets";
        private bool _formatGroupEnabled;
        private bool _pngFormatEnabled = true;
        private bool _jpegFormatEnabled = true;
        private bool _jpgFormatEnabled = true;
        private long _thresholdComputational;
        private float _threshold = 1;
        private static readonly float _minWidth = 800f;
        private static readonly float _minHeight = 400f;

        private static readonly float _maxWidth = 800f;
        private static readonly float _maxHeight = 400f;
        #endregion

        #region Additional Classes
        private FileSearch _fileSearch;
        private DuplicatesFinder _finder;
        private TextureOptimizer _textureOptimizer;
        private AssetFinder _assetFinder;
        #endregion

        #region Other Fields
        private Vector2 _verticalScrollPosition = Vector2.zero;

        private List<string> _filesToOptimize;
        private Dictionary<string, List<string>> _duplicateFilesGroups = new();
        private List<string> _unusedAssets;

        private AlgorithmState _currentState;
        #endregion

        #region Base Methods

        [MenuItem("Window/Optimisation Tool")]
        public static void ShowWindow()
        {
            OptimisationToolWindow window = GetWindow<OptimisationToolWindow>();
            window.titleContent = new GUIContent("Optimisation Tool");
            window.minSize = new Vector2(_minWidth, _minHeight);
            window.maxSize = new Vector2(_maxWidth, _maxHeight);
            window.Show();
        }

        private void Awake()
        {
            _currentState = AlgorithmState.Starting;

            _fileSearch = new FileSearch(_pngFormatEnabled, _jpegFormatEnabled, _jpgFormatEnabled, _mainFolder);
            _finder = new DuplicatesFinder();
            _textureOptimizer = new TextureOptimizer(_thresholdComputational);
        }

        private void OnEnable()
        {
            OpenAllScenesInBuildSettings();
        }


        private void OnGUI()
        {
            ShowMainGUI();

            if (string.IsNullOrEmpty(_mainFolder))
            {
                return;
            }

            if (!_pngFormatEnabled && !_jpegFormatEnabled && !_jpgFormatEnabled)
            {
                return;
            }

            HandleStates();

        }
        #endregion

        #region GUI Methods
        private void ShowMainGUI()
        {
            _formatGroupEnabled = EditorGUILayout.BeginToggleGroup("Settings", _formatGroupEnabled);
            _pngFormatEnabled = EditorGUILayout.Toggle("PNG Format Toggle", _pngFormatEnabled);
            _jpegFormatEnabled = EditorGUILayout.Toggle("JPEG Format Toggle", _jpegFormatEnabled);
            _jpgFormatEnabled = EditorGUILayout.Toggle("JPG Format Toggle", _jpgFormatEnabled);
            _threshold = EditorGUILayout.Slider("Size Threshold in MB", _threshold, 0.5f, 1.5f);

            EditorGUILayout.EndToggleGroup();

            _thresholdComputational = (long)((_threshold + 0.7f) * 1024f * 1024f);

            if (_currentState == AlgorithmState.Starting)
            {
                if (GUILayout.Button("Duplicates"))
                {
                    _currentState = AlgorithmState.FindingDuplicates;
                }
                if (GUILayout.Button("Optimization"))
                {
                    _currentState = AlgorithmState.OptimizationHandling;
                }
            }

            if (_currentState == AlgorithmState.FindingDuplicates)
            {
                if (GUILayout.Button("Unused Assets"))
                {
                    _currentState = AlgorithmState.Starting;
                }
                if (GUILayout.Button("Optimization"))
                {
                    _currentState = AlgorithmState.OptimizationHandling;
                }
            }

            if (_currentState == AlgorithmState.OptimizationHandling)
            {
                if (GUILayout.Button("Unused Assets"))
                {
                    _currentState = AlgorithmState.Starting;
                }
                if (GUILayout.Button("Duplicates"))
                {
                    _currentState = AlgorithmState.FindingDuplicates;
                }
            }
        }

        private void ShowUnusedAssetsSearchGUI()
        {
            GUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Find Unused Assets"))
            {
                if (!Directory.Exists(_mainFolder))
                {
                    EditorUtility.DisplayDialog("Error", $"Current folder '{_mainFolder}' could not be found", "Ok");
                    return;
                }

                _unusedAssets?.Clear();

                HandleUnusedAssets();

                _currentState = AlgorithmState.UnusedAssetsHandling;
            }

            GUILayout.EndHorizontal();
        }

        private void ShowDuplicatesSearchGUI()
        {
            GUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Find Duplicates"))
            {
                if (!Directory.Exists(_mainFolder))
                {
                    EditorUtility.DisplayDialog("Error", $"Current folder '{_mainFolder}' could not be found", "Ok");
                    return;
                }

                _duplicateFilesGroups?.Clear();

                _fileSearch = new FileSearch(_pngFormatEnabled, _jpegFormatEnabled, _jpgFormatEnabled, _mainFolder);

                HandleDuplicates();

                _currentState = AlgorithmState.DuplicatesHandling;
            }

            GUILayout.EndHorizontal();
        }

        private void ShowGraphicsSearchGUI()
        {
            GUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Check Graphics"))
            {
                if (!Directory.Exists(_mainFolder))
                {
                    EditorUtility.DisplayDialog("Error", $"Current folder '{_mainFolder}' could not be found", "Ok");
                    return;
                }

                _filesToOptimize?.Clear();

                _fileSearch = new FileSearch(_pngFormatEnabled, _jpegFormatEnabled, _jpgFormatEnabled, _mainFolder);
                _textureOptimizer = new TextureOptimizer(_thresholdComputational);

                HandleFilesToOptimize();
            }

            GUILayout.EndHorizontal();
        }
        #endregion

        #region Navigation Methods
        private void ShowUnusedAssetsNavigationButtons()
        {
            if (_unusedAssets == null || _unusedAssets.Count <= 0)
            {
                if (_assetFinder.FindUnusedAssets().Count == 0)
                {
                    _currentState = AlgorithmState.Starting;
                }
                return;
            }

            GUILayout.BeginVertical("box");

            if (GUILayout.Button("Back"))
            {
                _currentState = AlgorithmState.Starting;
                GUILayout.EndVertical();
                return;
            }

            if (GUILayout.Button("Delete all Unused Assets.(Don't use this action, if you are not sure)"))
            {
                DeleateAllUnusedAssets();
                _currentState = AlgorithmState.Starting;
                GUILayout.EndVertical();
                return;
            }

            GUILayout.Label("Unused Assets:");
            _verticalScrollPosition = EditorGUILayout.BeginScrollView(_verticalScrollPosition, GUILayout.Height(_maxHeight / 2));
            foreach (var file in _unusedAssets)
            {
                if (!File.Exists(file))
                {
                    HandleUnusedAssets();
                    continue;
                }

                if (GUILayout.Button(Path.GetFileName(file)))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(file).GetInstanceID());
                }
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();

        }

        private void ShowDuplicatesNavigationButtons()
        {
            if (_duplicateFilesGroups == null || _duplicateFilesGroups.Count <= 0)
            {
                if (!_finder.PairExists(_duplicateFilesGroups))
                {
                    _currentState = AlgorithmState.Starting;
                }
                return;
            }

            GUILayout.BeginVertical("box");

            if (GUILayout.Button("Back"))
            {
                _currentState = AlgorithmState.Starting;
                GUILayout.EndVertical();
                return;
            }

            GUILayout.Label("Duplicate Files:");
            _verticalScrollPosition = EditorGUILayout.BeginScrollView(_verticalScrollPosition, GUILayout.Height(_maxHeight / 2));
            foreach (var duplicateGroup in _duplicateFilesGroups)
            {
                GUILayout.Label(duplicateGroup.Key);
                foreach (var file in duplicateGroup.Value)
                {
                    if (!File.Exists(file))
                    {
                        HandleDuplicates();
                        continue;
                    }

                    if (GUILayout.Button(Path.GetFileName(file)))
                    {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(file).GetInstanceID());
                    }
                }
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void ShowGraphicsNavigationButtons()
        {
            if (_filesToOptimize != null && _filesToOptimize.Count > 0)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label("Files to Optimize:");
                _verticalScrollPosition = EditorGUILayout.BeginScrollView(_verticalScrollPosition, GUILayout.Height(_maxHeight / 2));
                foreach (var file in _filesToOptimize)
                {
                    if (!File.Exists(file))
                    {
                        continue;
                    }

                    if (GUILayout.Button(Path.GetFileName(file)))
                    {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(file).GetInstanceID());
                    }
                }
                EditorGUILayout.EndScrollView();
                GUILayout.EndVertical();
                return;
            }
        }
        #endregion

        #region Additional Methods
        private void HandleDuplicates()
        {
            var files = _fileSearch.Search();
            _duplicateFilesGroups = _finder.FindDuplicates(files);
        }

        private void HandleFilesToOptimize()
        {
            var files = _fileSearch.Search();
            _filesToOptimize = _textureOptimizer.GetNotOptimizedFiles(files);
        }

        private void HandleUnusedAssets()
        {
            _assetFinder = new AssetFinder();
            _unusedAssets = _assetFinder.FindUnusedAssets();
        }

        private void HandleStates()
        {
            switch (_currentState)
            {
                case AlgorithmState.Starting:
                    ShowUnusedAssetsSearchGUI();
                    break;
                case AlgorithmState.UnusedAssetsHandling:
                    ShowUnusedAssetsNavigationButtons();
                    break;
                case AlgorithmState.FindingDuplicates:
                    ShowDuplicatesSearchGUI();
                    break;
                case AlgorithmState.DuplicatesHandling:
                    ShowDuplicatesNavigationButtons();
                    break;
                case AlgorithmState.OptimizationHandling:
                    ShowGraphicsSearchGUI();
                    ShowGraphicsNavigationButtons();
                    break;
            }
        }

        private void OpenAllScenesInBuildSettings()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            {
                Scene currentScene = EditorSceneManager.GetSceneAt(i);
                EditorSceneManager.CloseScene(currentScene, true);
            }

            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
        }

        private void DeleateAllUnusedAssets()
        {
            if (_unusedAssets != null && _unusedAssets.Count > 0)
            {
                foreach (var asset in _unusedAssets)
                {
                    AssetDatabase.DeleteAsset(asset);
                }
            }
        }
        #endregion
    }
}

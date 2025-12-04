using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace iMoney.Transactions.EditorTools {
    public class TransactionTestDataWindow : EditorWindow {
        private const string EditorPrefsKey = "iMoney_TransactionTestDataWindow_State";

        [Serializable]
        private class WindowState {
            public int startYear;
            public int endYear;
            public int entriesPerWeek;

            public bool usePersistentPath;
            public string customFolderPath;
            public string fileName;

            public bool[] monthSelected; // length 12
            public bool[] weekSelected;  // length 5
        }

        private int startYear = 2025;
        private int endYear = 2025;

        // Month selection: Jan..Dec
        private readonly bool[] monthSelected = new bool[12] {
        true, true, false, false, false, false,
        false, false, false, false, false, false
    };

        // Weeks of month: 1..5 (last is overflow 29–31)
        private readonly bool[] weekSelected = new bool[5] {
        true, true, true, true, true
    };

        private int entriesPerWeek = 10;

        // Path options
        private bool usePersistentPath = true;
        private string customFolderPath = "";
        private string fileName = "iMoney";

        private bool initialized = false;

        [MenuItem("Tools/iMoney!/Transaction Test Data")]
        public static void ShowWindow() {
            var window = GetWindow<TransactionTestDataWindow>("Transaction Test Data");
            window.minSize = new Vector2(380, 320);
        }

        private void OnEnable() {
            LoadState();

            if (initialized) return;
            initialized = true;

            Vector2 initialSize = new Vector2(420, 755);
            minSize = initialSize;
            maxSize = initialSize;

            EditorApplication.delayCall += () => {
                try { maxSize = new Vector2(10000, 10000); }
                catch { }
            };
        }

        private void OnDisable() {
            SaveState();
        }

        private void OnGUI() {
            DrawYearRange();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Months to Include", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("box");
            DrawMonthToggles();
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Weeks of Month to Include", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("box");
            DrawWeekToggles();
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            entriesPerWeek = EditorGUILayout.IntField("Entries per Week", entriesPerWeek);
            if (entriesPerWeek < 0) entriesPerWeek = 0;

            EditorGUILayout.Space();
            DrawPathSection();

            EditorGUILayout.Space();
            Button_OpenFolder();
            Button_GenerateFile();
        }

        private void Button_OpenFolder() {
            string folderToOpen = usePersistentPath ? Application.persistentDataPath : customFolderPath;
            bool canOpenFolder = !string.IsNullOrWhiteSpace(folderToOpen) && Directory.Exists(folderToOpen);

            using (new EditorGUI.DisabledScope(!canOpenFolder)) {
                if (GUILayout.Button("Open Folder")) {
                    EditorUtility.RevealInFinder(folderToOpen);
                }
            }
        }

        private void Button_GenerateFile() {
            bool canGenerate = HasAnyMonthSelected() && HasAnyWeekSelected() && entriesPerWeek > 0 && HasValidPathInputs();
            using (new EditorGUI.DisabledScope(!canGenerate)) {
                if (GUILayout.Button("Generate JSON File")) {
                    GenerateJsonFile();
                }
            }
        }

        #region UI drawing

        private void DrawYearRange() {
            EditorGUILayout.LabelField("Years", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            startYear = EditorGUILayout.IntField("From Year", startYear);
            endYear = EditorGUILayout.IntField("To Year", endYear);

            if (startYear < 1) startYear = 1;
            if (endYear < 1) endYear = 1;
            if (endYear < startYear) endYear = startYear;

            EditorGUI.indentLevel--;
        }

        private void DrawMonthToggles() {
            string[] monthNames = {
            "January", "February", "March", "April",
            "May", "June", "July", "August",
            "September", "October", "November", "December"
        };

            for (int i = 0; i < 12; i++) {
                monthSelected[i] = EditorGUILayout.ToggleLeft(monthNames[i], monthSelected[i]);
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("All Months")) {
                for (int i = 0; i < 12; i++) monthSelected[i] = true;
            }
            if (GUILayout.Button("None")) {
                for (int i = 0; i < 12; i++) monthSelected[i] = false;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawWeekToggles() {
            for (int i = 0; i < 5; i++) {
                string label = i switch {
                    0 => "Week 1 (days 1-7)",
                    1 => "Week 2 (days 8-14)",
                    2 => "Week 3 (days 15-21)",
                    3 => "Week 4 (days 22-28)",
                    4 => "Overflow (days 29-31)",
                    _ => $"Week {i + 1}",
                };
                weekSelected[i] = EditorGUILayout.ToggleLeft(label, weekSelected[i]);
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("All Weeks")) {
                for (int i = 0; i < 5; i++) weekSelected[i] = true;
            }
            if (GUILayout.Button("None")) {
                for (int i = 0; i < 5; i++) weekSelected[i] = false;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPathSection() {
            EditorGUILayout.LabelField("Output Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            usePersistentPath = EditorGUILayout.ToggleLeft("Use Persistent Path", usePersistentPath);

            fileName = EditorGUILayout.TextField("File Name", fileName);

            if (!usePersistentPath) {
                EditorGUILayout.BeginHorizontal();
                customFolderPath = EditorGUILayout.TextField("Custom Folder", customFolderPath);
                if (GUILayout.Button("Browse", GUILayout.MaxWidth(70f))) {
                    string selected = EditorUtility.OpenFolderPanel("Select Output Folder", customFolderPath, "");
                    if (!string.IsNullOrEmpty(selected)) {
                        customFolderPath = selected;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            string previewPath = GetResolvedPathPreview();
            EditorGUILayout.LabelField("Final Path:", EditorStyles.miniBoldLabel);
            EditorGUILayout.LabelField(previewPath, EditorStyles.wordWrappedMiniLabel);

            EditorGUI.indentLevel--;
        }

        #endregion

        #region Helpers / validation

        private bool HasAnyMonthSelected() {
            for (int i = 0; i < 12; i++) {
                if (monthSelected[i]) return true;
            }
            return false;
        }

        private bool HasAnyWeekSelected() {
            for (int i = 0; i < 5; i++) {
                if (weekSelected[i]) return true;
            }
            return false;
        }

        private bool HasValidPathInputs() {
            if (string.IsNullOrWhiteSpace(fileName)) {
                return false;
            }

            if (!usePersistentPath && string.IsNullOrWhiteSpace(customFolderPath)) {
                return false;
            }

            return true;
        }

        private string GetResolvedPathPreview() {
            string folder = usePersistentPath
                ? Application.persistentDataPath
                : (string.IsNullOrWhiteSpace(customFolderPath) ? "<no folder>" : customFolderPath);

            string file = string.IsNullOrWhiteSpace(fileName) ? "<no file>" : EnsureJsonExtension(fileName);

            if (folder == "<no folder>" || file == "<no file>") {
                return "Invalid path configuration.";
            }

            return Path.Combine(folder, file);
        }

        private static string EnsureJsonExtension(string name) {
            if (name.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) {
                return name;
            }
            return name + ".json";
        }

        #endregion

        #region Generation

        private void GenerateJsonFile() {
            string folder = usePersistentPath
                ? Application.persistentDataPath
                : customFolderPath;

            if (string.IsNullOrWhiteSpace(folder)) {
                Debug.LogError("[TransactionTestDataWindow] Output folder is empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(fileName)) {
                Debug.LogError("[TransactionTestDataWindow] File name is empty.");
                return;
            }

            string file = EnsureJsonExtension(fileName);
            string fullPath = Path.Combine(folder, file);

            string dir = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            var settings = new TransactionGenerationSettings {
                StartYear = startYear,
                EndYear = endYear,
                Months = (bool[])monthSelected.Clone(),
                Weeks = (bool[])weekSelected.Clone(),
                EntriesPerWeek = entriesPerWeek
            };

            TransactionRoot root = TransactionTestDataGenerator.Generate(settings);
            string json = JsonUtility.ToJson(root, true);
            File.WriteAllText(fullPath, json);

            Debug.Log($"[TransactionTestDataWindow] Generated {root.transactions.Count} transactions. " +
                      $"Years {startYear}–{endYear}\nPath: {fullPath}");
            EditorUtility.RevealInFinder(fullPath);
        }

        #endregion

        #region EditorPrefs persistence

        private void SaveState() {
            var state = new WindowState {
                startYear = startYear,
                endYear = endYear,
                entriesPerWeek = entriesPerWeek,
                usePersistentPath = usePersistentPath,
                customFolderPath = customFolderPath,
                fileName = fileName,
                monthSelected = (bool[])monthSelected.Clone(),
                weekSelected = (bool[])weekSelected.Clone()
            };

            string json = JsonUtility.ToJson(state);
            EditorPrefs.SetString(EditorPrefsKey, json);
        }

        private void LoadState() {
            if (!EditorPrefs.HasKey(EditorPrefsKey)) {
                return;
            }

            string json = EditorPrefs.GetString(EditorPrefsKey, string.Empty);
            if (string.IsNullOrEmpty(json)) {
                return;
            }

            try {
                var state = JsonUtility.FromJson<WindowState>(json);
                if (state == null) return;

                startYear = state.startYear;
                endYear = state.endYear;
                entriesPerWeek = state.entriesPerWeek;

                usePersistentPath = state.usePersistentPath;
                customFolderPath = state.customFolderPath ?? "";
                fileName = string.IsNullOrWhiteSpace(state.fileName) ? "iMoney" : state.fileName;

                if (state.monthSelected != null && state.monthSelected.Length == 12) {
                    for (int i = 0; i < 12; i++) {
                        monthSelected[i] = state.monthSelected[i];
                    }
                }

                if (state.weekSelected != null && state.weekSelected.Length == 5) {
                    for (int i = 0; i < 5; i++) {
                        weekSelected[i] = state.weekSelected[i];
                    }
                }
            }
            catch (Exception e) {
                Debug.LogWarning($"[TransactionTestDataWindow] Failed to load EditorPrefs state. Resetting. Exception: {e}");
            }
        }

        #endregion
    }
}

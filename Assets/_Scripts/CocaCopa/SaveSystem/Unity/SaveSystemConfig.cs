using CocaCopa.SaveSystem.API;
using CocaCopa.SaveSystem.Runtime;
using UnityEngine;

namespace CocaCopa.SaveSystem.Unity {
    [CreateAssetMenu(fileName = "SaveSystemConfig", menuName = "CocaCopa/Save System/Config", order = 0)]
    internal sealed class SaveSystemConfig : ScriptableObject {
        [Header("Initialization")]
        [Tooltip(
            "Controls when the save system initializes.\n\n" +
            "[None] \nNo automatic initialization. You must call Initialize() manually.\n\n" +
            "[AfterAssembliesLoaded]\nVery early. Assemblies are loaded but Unity subsystems aren't ready yet.\n\n" +
            "[BeforeSplashScreen]\nEarly initialization before the splash screen shows.\n\n" +
            "[SubsystemRegistration]\nExtremely early; for low-level engine-like services.\n\n" +
            "[BeforeSceneLoad]\nRecommended. Initializes before the first scene loads.\n\n" +
            "[AfterSceneLoad]\nInitializes after the first scene has fully loaded."
        )]
        [SerializeField] private SaveSystemInitPhase initPhase = SaveSystemInitPhase.BeforeSceneLoad;

        [Header("Storage")]
        [Tooltip("If true, uses Application.persistentDataPath as the root directory.")]
        [SerializeField] private bool usePersistentDataPath = true;

        [Header("Serialization")]
        [SerializeField] private bool prettyPrintJson = true;

        [Header("Encryption")]
        [SerializeField] private bool useEncryption = false;

        [Tooltip("High-entropy passphrase to derive the AES key. Change this per project.")]
        [SerializeField] private string passphrase = "CHANGE_ME";

        [Tooltip("Salt as hex (at least 16 hex chars = 8 bytes). Keep constant for this project.")]
        [SerializeField] private string saltHex = "0011223344556677";

        public SaveSystemInitPhase InitPhase => initPhase;

        /// <summary>
        /// Initializes the save system according to this config.
        /// </summary>
        public void Initialize() {
            // Root folder
            string root = usePersistentDataPath
                ? Application.persistentDataPath
                : null;

            // JSON serializer (Unity-side SPI implementation)
            var json = new UnityJsonSerializer(prettyPrint: prettyPrintJson);

            // Optional encryption
            byte[] saltBytes = null;
            string effectivePassphrase = null;

            if (useEncryption) {
                saltBytes = HexUtility.ParseHexToBytes(saltHex, nameof(SaveSystemConfig));
                effectivePassphrase = passphrase;
            }

            var impl = new DefaultSaveStorage(
                jsonSerializer: json,
                rootDirectory: root,
                passphrase: effectivePassphrase,
                salt: saltBytes
            );

            SaveStorage.Initialize(impl);
        }
    }
}

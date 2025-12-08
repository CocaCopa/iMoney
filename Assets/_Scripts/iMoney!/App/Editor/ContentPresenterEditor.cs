using UnityEngine;
using UnityEditor;
using iMoney.App.Spendings.Unity;

namespace iMoney.App.EditorTools {
    [CustomEditor(typeof(ContentPresenter))]
    public class ContentPresenterEditor : Editor {
        private Transform holderTransform;

        private void OnEnable() {
            holderTransform = (target as ContentPresenter).transform.GetChild(0);
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.Space(10);
            Button_ShowContentHolder();
            Button_HideContentHolder();
            EditorGUILayout.Space(10);
            base.OnInspectorGUI();
        }

        private void Button_ShowContentHolder() {
            GUI.enabled = !holderTransform.gameObject.activeSelf;
            if (GUILayout.Button("Show Content")) {
                holderTransform.gameObject.SetActive(true);
            }
            GUI.enabled = true;
        }

        private void Button_HideContentHolder() {
            GUI.enabled = holderTransform.gameObject.activeSelf;
            if (GUILayout.Button("Hide Content")) {
                holderTransform.gameObject.SetActive(false);
            }
            GUI.enabled = true;
        }
    }
}

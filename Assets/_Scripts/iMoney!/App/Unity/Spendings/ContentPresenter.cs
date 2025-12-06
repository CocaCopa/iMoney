using iMoney.App.Spendings.Runtime;
using UnityEngine;

namespace iMoney.App.Spendings.Unity {
    public class ContentPresenter : MonoBehaviour, IContentPresenter {
        [Header("References")]
        [SerializeField] private GameObject spendRowPrefab;
        [SerializeField] private Transform contentHolder;

        private void Awake() {
            DestroyPlaceholders();
        }

        private void DestroyPlaceholders() {
            for (int i = contentHolder.childCount - 1; i >= 0; i--) {
                var child = contentHolder.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        public void AddSpendRow(bool useDarkColor, string title, string amount, string type) {
            var rowObj = Instantiate(spendRowPrefab, contentHolder);
            if (rowObj.TryGetComponent<SpendRow>(out var row)) {
                row.UseDarkColor(useDarkColor);
                row.SetRowType(type == "Spend" ? SpendRow.Type.Spend : SpendRow.Type.Add);
                row.SetData(title, amount + "€");
            }
            else { Debug.LogError("Could not find SpendRow component"); }
        }
    }
}

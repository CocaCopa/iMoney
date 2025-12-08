using iMoney.App.Spendings.Runtime;
using UnityEngine;

namespace iMoney.App.Spendings.Unity {
    public class ContentPresenter : MonoBehaviour, IContentPresenter {
        [Header("References")]
        [SerializeField] private GameObject transactionRowPrefab;
        [SerializeField] private Transform contentHolder;

        [Header("General")]
        [SerializeField] private RowColor firstRowColor = RowColor.Light;

        private RowColor currentRowColor;

        private void Awake() {
            currentRowColor = firstRowColor;
            DestroyPlaceholders();
        }

        private void DestroyPlaceholders() {
            for (int i = contentHolder.childCount - 1; i >= 0; i--) {
                var child = contentHolder.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        public void AddTransactionRow(string title, decimal amount, TransactionType trType) {
            var rowObj = Instantiate(transactionRowPrefab, contentHolder);
            if (rowObj.TryGetComponent<TransactionRowUI>(out var row)) {
                row.UseDarkColor(currentRowColor == RowColor.Dark);
                currentRowColor = currentRowColor == RowColor.Dark ? RowColor.Light : RowColor.Dark;
                row.SetRowType(MapType(trType));
                row.SetData(title, amount);
            }
            else { Debug.LogError($"{nameof(ContentPresenter)} Could not find {nameof(TransactionRowUI)} component"); }
        }

        private TransactionRowUI.Type MapType(TransactionType trType) {
            return trType switch {
                TransactionType.Add => TransactionRowUI.Type.Add,
                TransactionType.Spend => TransactionRowUI.Type.Spend,
                TransactionType.Neutral => TransactionRowUI.Type.Neutral,
                _ => throw new System.ArgumentOutOfRangeException(nameof(trType), trType, null)
            };
        }

        private enum RowColor {
            Dark,
            Light
        };
    }
}

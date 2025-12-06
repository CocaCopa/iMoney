using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace iMoney.App.Spendings.Unity {
    public class SpendRow : MonoBehaviour {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleTxt;
        [SerializeField] private TextMeshProUGUI amountTxt;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Image typeImg;

        [Header("Row Colors")]
        [SerializeField] private Color darkColor = Color.white;
        [SerializeField] private Color lightColor = Color.white;

        [Header("Type Colors")]
        [SerializeField] private Color addColor = Color.white;
        [SerializeField] private Color spendColor = Color.white;

        public enum Type { Add, Spend };

        public void UseDarkColor(bool useDarkColor) {
            backgroundImg.color = useDarkColor ? darkColor : lightColor;
        }

        public void SetRowType(Type type) {
            switch (type) {
                case Type.Add: typeImg.color = addColor; break;
                case Type.Spend: typeImg.color = spendColor; break;
            }
        }

        public void SetData(string title, string amount) {
            titleTxt.text = title;
            amountTxt.text = amount;
        }
    }
}

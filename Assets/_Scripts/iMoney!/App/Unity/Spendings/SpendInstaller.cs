using iMoney.App.Spendings.Runtime;
using UnityEngine;

namespace iMoney.App.Spendings.Unity {
    internal class SpendInstaller : MonoBehaviour {
        [SerializeField] private ContentPresenter dailyPresenter;
        [SerializeField] private ContentPresenter weeklyPresenter;

        private IContentPresenter DailyPresenter => dailyPresenter;
        private IContentPresenter WeeklyPresenter => weeklyPresenter;

        private SpendFlow spendFlow;

        private void Awake() {
            spendFlow = new SpendFlow(DailyPresenter, WeeklyPresenter);
        }

        private void Start() {
            spendFlow.Initialize();
        }
    }
}

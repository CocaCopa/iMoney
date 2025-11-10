using CocaCopa.Core.Animation;
using UnityEngine;

namespace iMoney.BalanceEntry.Unity.Animation {
    internal sealed class CurveEasing : IEasing {
        private readonly AnimationCurve curve;

        public CurveEasing(AnimationCurve curve) {
            this.curve = curve;
        }

        public float Evaluate(float t) => curve.Evaluate(t);
    }
}

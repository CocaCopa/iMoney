using UnityEngine;

public class ModalAnimation : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RectTransform modalRect;
    [SerializeField] private RectTransform vkRect;

    [Header("Animation")]
    [SerializeField] private AnimationCurve visibilityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float visibilitySpeed = 1f;
    [SerializeField] private float delayTime;

    private RectPositions modalPositions;
    private RectPositions vkPositions;

    private float modalAnimPoints;
    private float vkAnimPoints;

    private float animationSpeed;
    private float delayTimer;

    public bool IsAnimating {
        get; private set;
    }

    private void Start() {
        Canvas.ForceUpdateCanvases();
        FindPositions();
        modalRect.anchoredPosition = modalPositions.hidden;
        vkRect.anchoredPosition = vkPositions.hidden;
        delayTimer = delayTime;
        animationSpeed = visibilitySpeed;
        enabled = false;
    }

    private void FindPositions() {
        Canvas.willRenderCanvases -= FindPositions;
        var modalVisible = modalRect.anchoredPosition;
        var vkVisible = vkRect.anchoredPosition;

        var modalWidth = modalRect.rect.width;
        var vkHeight = vkRect.rect.height;
        var modalHidden = modalVisible + Vector2.left * modalWidth;
        var vkHidden = vkVisible + Vector2.down * vkHeight;

        modalPositions = new RectPositions(modalVisible, modalHidden);
        vkPositions = new RectPositions(vkVisible, vkHidden);
    }

    private void Update() {
        PlaySequence();
    }

    public void SetActive(bool active) {
        IsAnimating = true;
        enabled = true;
        delayTimer = delayTime;
        animationSpeed = active ? visibilitySpeed : -visibilitySpeed;
    }

    private void PlaySequence() {
        LerpRectTransform(vkRect, ref vkAnimPoints, vkPositions);

        delayTimer -= Time.deltaTime;
        delayTimer = Mathf.Max(0f, delayTimer);

        if (delayTimer == 0f) {
            LerpRectTransform(modalRect, ref modalAnimPoints, modalPositions);
        }

        if ((modalAnimPoints == 1f && vkAnimPoints == 1f) ||
            (modalAnimPoints == 0f && vkAnimPoints == 0f)) {
            enabled = false;
            IsAnimating = false;
        }
    }

    private void LerpRectTransform(RectTransform rectTransform, ref float animPoints, RectPositions positions) {
        animPoints += animationSpeed * Time.deltaTime;
        animPoints = Mathf.Clamp01(animPoints);
        float vkTime = visibilityCurve.Evaluate(animPoints);
        rectTransform.anchoredPosition = Vector2.LerpUnclamped(positions.hidden, positions.visible, vkTime);
    }

    private class RectPositions {
        public Vector2 visible;
        public Vector2 hidden;
        public RectPositions(Vector2 visible, Vector2 hidden) {
            this.visible = visible;
            this.hidden = hidden;
        }
    }
}

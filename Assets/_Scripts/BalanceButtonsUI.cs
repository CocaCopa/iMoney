using System;
using CocaCopa.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class BalanceButtonsUI : MonoBehaviour {
    [Header("Buttons")]
    [SerializeField] private Button addBtn;
    [SerializeField] private Button spendBtn;

    [Header("Masks")]
    [SerializeField] private RectTransform addMask;
    [SerializeField] private RectTransform spendMask;
    [SerializeField] private float addAlpha;
    [SerializeField] private float spendAlpa;
    [SerializeField] private AnimationCurve visibilityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float visibilitySpeed = 1f;

    public event Action OnAddPressed;
    public event Action OnSpendPressed;

    public bool AddActive {
        get; private set;
    }

    public bool SpendActive {
        get; private set;
    }

    private Image addImage;
    private Image spendImage;

    private float visibilityAnimPoints;
    private float animationSpeed;

    private void Awake() {
        CacheComponents();
        AddListeners();
        animationSpeed = visibilitySpeed;
    }

    private void Start() {
        enabled = false;
    }

    private void CacheComponents() {
        addImage = addMask.GetComponent<Image>();
        spendImage = spendMask.GetComponent<Image>();
    }

    private void AddListeners() {
        addBtn.onClick.AddListener(() => OnAddPressed?.SafeInvoke(nameof(OnAddPressed)));
        spendBtn.onClick.AddListener(() => OnSpendPressed?.SafeInvoke(nameof(OnSpendPressed)));
    }

    public void HideSpendButton() {
        AddActive = true;
        SpendActive = false;
        addImage.raycastTarget = true;
        spendImage.raycastTarget = true;
        enabled = true;
    }

    public void HideActiveButton() {
        AddActive = false;
        SpendActive = true;
        addImage.raycastTarget = true;
        spendImage.raycastTarget = true;
        enabled = true;
    }

    private void Update() {
        if (AddActive) {
            AnimateAlpha(spendImage, addAlpha);
        }
        if (SpendActive) {
            AnimateAlpha(addImage, spendAlpa);
        }

        if (visibilityAnimPoints == 1f || visibilityAnimPoints == 0f) {
            enabled = false;
        }
    }

    private void AnimateAlpha(Image targetImage, float targetAlpha) {
        visibilityAnimPoints += animationSpeed * Time.unscaledDeltaTime;
        visibilityAnimPoints = Mathf.Clamp01(visibilityAnimPoints);
        float time = visibilityCurve.Evaluate(visibilityAnimPoints);
        Color imgColor = targetImage.color;
        imgColor.a = Mathf.Lerp(0f, targetAlpha / 255f, time);
        targetImage.color = imgColor;
    }
}

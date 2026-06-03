using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RKode.UI {
public abstract class UIPanelBase : UIBehaviour { 
    [SerializeField] protected GameObject _panel;
    public GameObject Panel => _panel ?? gameObject;

    [SerializeField] protected Button[] _closeBtns;

    public bool IsVisible => Panel.activeSelf;

    public System.Action onShow;
    public System.Action onHide;

    protected override void Awake() {
        Panel.SetActive(false);
    }

    protected override void OnEnable() {
        foreach(var btn in _closeBtns) {
            btn.onClick.AddListener(Hide);
        }
    }

    protected override void OnDisable() {
        foreach(var btn in _closeBtns) {
            btn.onClick.RemoveListener(Hide);
        }
    }

    public virtual void Show() {
        Panel.SetActive(true);
        onShow?.Invoke();
    }

    public virtual void Hide() {
        Panel.SetActive(false);
        onHide?.Invoke();
    }
}
}
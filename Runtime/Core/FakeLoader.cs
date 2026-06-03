using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RKode.Utils {
public class FakeLoader : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private Slider _loadingBar;

    [Header("Settings")]
    [SerializeField] private float loadingLerpSpeed = 2f;

    [Space] 
    public UnityEvent onLoadingComplete;

    private float _targetProgress = 0f;

    private IEnumerator Start() {
        if(_loadingBar)
            _loadingBar.value = 0f;
        _targetProgress = 0.2f;

        yield return new WaitForSeconds(1.2f);
        _targetProgress = .6f;

        yield return new WaitForSeconds(1.6f);
        _targetProgress = 1f;

        yield return new WaitForSeconds(.5f);
        onLoadingComplete?.Invoke();
    }

    private void Update() {
        if(_loadingBar)
            _loadingBar.value = Mathf.MoveTowards(_loadingBar.value, _targetProgress, Time.deltaTime * loadingLerpSpeed);
    }
}
}
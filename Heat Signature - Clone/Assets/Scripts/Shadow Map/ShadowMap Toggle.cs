using System;
using UnityEngine;

public class ShadowMapToggle : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject _shadowMapCanvas;
    public event Action<bool> OnShadowMapToggle;
    private bool _shadowMapToggled = false;
    void OnEnable()
    {
        _inputReader.EnableSpaceAction();
        _inputReader.OnSpace += ToggleShadowMap;
    }
    void OnDisable()
    {
        _inputReader.DisableSpaceAction();
        _inputReader.OnSpace -= ToggleShadowMap;
    }

    private void ToggleShadowMap()
    {
        _shadowMapToggled = !_shadowMapToggled;
        
        if(_shadowMapCanvas != null)
        {
            _shadowMapCanvas.SetActive(_shadowMapToggled);
        }

        OnShadowMapToggle?.Invoke(_shadowMapToggled);
    }
}

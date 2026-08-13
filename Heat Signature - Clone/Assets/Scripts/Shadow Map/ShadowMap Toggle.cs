using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;

public class ShadowMapToggle : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private GameObject _shadowMapCanvas;
    private bool _shadowMapToggled = false;

    private Coroutine _shadowMapRoutine = null;
    private float _timeInShadowMap = 3f;

    private float _timeVariables = 1f;
    //Possible change cen be to keep the time instead of resetting it and giving the toggle a cooldown.
    void OnEnable()
    {
        //Input Reader
        _inputReader.EnableSpaceAction();
        _inputReader.OnSpace += ToggleShadowMap;

        //Time Manager
        _timeManager.OnBulletTimeChange += ChangeTimeVariable;
    }
    void OnDisable()
    {
        //Input Reader
        _inputReader.DisableSpaceAction();
        _inputReader.OnSpace -= ToggleShadowMap;

        //Time Manager
        _timeManager.OnBulletTimeChange -= ChangeTimeVariable;
    }

    private void ToggleShadowMap()
    {
        _shadowMapToggled = !_shadowMapToggled;
        Debug.Log(_shadowMapToggled);
        
        if(_shadowMapCanvas != null)
        {
            _shadowMapCanvas.SetActive(_shadowMapToggled);
        }

        if(!_shadowMapToggled)
        {
            //Coroutine
            if(_shadowMapRoutine != null)
            {
                StopCoroutine(_shadowMapRoutine);
                _shadowMapRoutine = null;
            }     

            _inputReader.EnableInteractAction();
        }
        else
        {
            //Coroutine
            if(_shadowMapRoutine != null)
            {
                StopCoroutine(_shadowMapRoutine);
            }
            _shadowMapRoutine = StartCoroutine(StopShadowMap());

            _inputReader.DisableInteractAction();
        }
    }

    private void ChangeTimeVariable(float newValue)
    {
        _timeVariables = newValue;
    }
    private IEnumerator StopShadowMap()
    {
        float timeElapsed = 0f;

        while(timeElapsed < _timeInShadowMap)
        {
            timeElapsed += Time.deltaTime * _timeVariables;
            yield return null;
        }

        ToggleShadowMap();
    }
}

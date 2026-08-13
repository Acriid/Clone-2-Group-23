using UnityEngine;
using UnityEngine.InputSystem;

public class GadgetTest : MonoBehaviour
{
    public Slipstream Gun;
    private bool _usingWeapon = false;
    void Update()
    {
        ThrowCheck();
    }

    private void ThrowCheck()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Gun.UseItem();
            _usingWeapon = !_usingWeapon;
            Debug.Log($"Using Slipstream: {_usingWeapon}");
        }        
    }

}

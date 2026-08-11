using UnityEngine;
using UnityEngine.InputSystem;

public class GunTargetTest : MonoBehaviour
{
    public GunWeapon Gun;
    private bool _usingWeapon = true;
    void Start()
    {
        Gun.UseItem();
    }

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
            Debug.Log($"Using weapon: {_usingWeapon}");
        }        
    }

}

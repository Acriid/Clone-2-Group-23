using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTargetTest : MonoBehaviour
{
    public MeleeWeapon Weapon;
    private bool _usingWeapon = false;
    void Update()
    {
        ThrowCheck();
    }

    private void ThrowCheck()
    {
        if(Keyboard.current.mKey.wasPressedThisFrame)
        {
            Weapon.UseItem();
            _usingWeapon = !_usingWeapon;
            Debug.Log($"Using weapon: {_usingWeapon}");
        }        
    }

}

using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTargetTest : MonoBehaviour
{
    public MeleeWeapon Weapon;
    private bool _usingWeapon = false;
    void Start()
    {
        Weapon.ThrowItem();
    }

    void Update()
    {
        ThrowCheck();
    }

    private void ThrowCheck()
    {
        if(Keyboard.current.mKey.wasPressedThisFrame)
        {
            Weapon.ThrowItem();
            _usingWeapon = !_usingWeapon;
            Debug.Log($"Using weapon: {_usingWeapon}");
        }        
    }

}

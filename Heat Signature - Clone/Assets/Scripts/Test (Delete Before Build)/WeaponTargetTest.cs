using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTargetTest : MonoBehaviour
{
    public MeleeWeapon Weapon;
    private bool _usingWeapon = true;
    void Start()
    {
        Weapon.UseItem();
    }

    void Update()
    {
        ThrowCheck();
    }

    private void ThrowCheck()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Weapon.UseItem();
            _usingWeapon = !_usingWeapon;
            Debug.Log($"Using weapon: {_usingWeapon}");
        }        
    }

}

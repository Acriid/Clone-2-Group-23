using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTargetTest : MonoBehaviour
{
    public Item Weapon;
    private bool _usingWeapon = true;
    void Start()
    {
        Weapon.UseItem();
    }

    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Weapon.UseItem();
            Debug.Log($"Using weapon: {_usingWeapon}");
        }
    }

}

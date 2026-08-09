using UnityEngine;
using UnityEngine.InputSystem;

public class GunTargetTest : MonoBehaviour
{
    public InputReader InputReader;
    public GunWeapon Gun;
    private bool _usingWeapon = false;
    void OnEnable()
    {
        InputReader.OnClick += ShootGun;
        InputReader.EnableClickAction();
    }
    void OnDisable()
    {
        InputReader.OnClick -= ShootGun;
        InputReader.DisableClickAction();
    }
    private void ShootGun()
    {
        Gun.UseItem();
        _usingWeapon = !_usingWeapon;
    }

}

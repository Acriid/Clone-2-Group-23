using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Bullet/DefaultBullet")]
public class BulletSO : ScriptableObject
{
    public float BulletSpeed = 0f;
    public float BulletLifeTime = 10f;
    public bool PlayerBullet = true;
}

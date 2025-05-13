using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 3f;

    private float shootTimer;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            shootTimer = 0f;
        }
    }
}

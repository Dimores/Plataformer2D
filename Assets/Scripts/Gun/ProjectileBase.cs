using Orby.Enemy;
using Orby.Managers;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private Vector3 direction;

    public float timeToDestroy = 2f;

    public float side = 1;

    public int DamageAmount { get; set; }

    public Vector3 Direction { get => direction; set => direction = value; }

    private void Awake()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        if (Direction.x != 0 && Direction.y == 0)
            transform.Translate(Direction * Time.deltaTime * side, Space.World);
        else
            transform.Translate(Direction * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.transform.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.EXPLOSION, this.transform.position,
                null);
            enemy.Damage(DamageAmount);
            Destroy(gameObject);
        }
    }
}
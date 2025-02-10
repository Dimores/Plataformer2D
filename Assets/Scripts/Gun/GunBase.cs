using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public ProjectileBase prefabProjectile;

    public Transform positioToShoot;

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
            Shoot();
    }

    public void Shoot()
    {
        var projectile = Instantiate(prefabProjectile);
        projectile.transform.position = positioToShoot.position;
    }
}

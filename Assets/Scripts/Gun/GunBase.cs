using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public PlayerData playerData;
    public ProjectileBase prefabProjectile;

    public Transform positioToShoot;
    public Transform playerSideReference;

    private Coroutine _currentCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(playerData.shootKey.value))
        {
            _currentCoroutine = StartCoroutine(StartShoot());
        }
        else if (Input.GetKeyUp(playerData.shootKey.value))
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
        }
    }

    IEnumerator StartShoot()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(playerData.timeBetweenShoot.value);
        }
    }

    public void Shoot()
    {
        var projectile = Instantiate(prefabProjectile);
        projectile.transform.position = positioToShoot.position;
        projectile.side = playerSideReference.transform.localScale.x;
    }
}

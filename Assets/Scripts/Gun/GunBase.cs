using System.Collections;
using System.Collections.Generic;
using Orby.Player;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    //public PlayerData playerData;
    public ProjectileBase prefabProjectile;

    public Transform positioToShoot;
    public Transform playerSideReference;

    public AudioRandomPlayAudioClips randomShootAudio;

    private Coroutine _currentCoroutine;
    private Player player;

    // Input
    private float horizontal;
    private float vertical;

    private void Awake()
    {
        playerSideReference = GetComponentInParent<Player>().gameObject.transform;
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        ProcessInputs();
        if (Input.GetKeyDown(player.playerData.attackKey) && _currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(StartShoot());
        }
        else if (Input.GetKeyUp(player.playerData.attackKey) && _currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }


    IEnumerator StartShoot()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(player.playerData.attackSpeed);
        }
    }

    public void Shoot()
    {
        if (randomShootAudio != null) randomShootAudio.PlayRandom();

        var projectile = Instantiate(prefabProjectile);
        projectile.transform.position = positioToShoot.position;
        projectile.side = playerSideReference.transform.localScale.x;
        DefineProjectileDirection(projectile);
    }

    private void ProcessInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void DefineProjectileDirection(ProjectileBase projectile)
    {
        if (horizontal > 0 && vertical == 0)
        {
            projectile.Direction = new Vector3(35, 0, 0);
        } else if (horizontal > 0 && vertical > 0)
        {
            projectile.Direction = new Vector3(35, 10, 0);
        }
        // Calcula o ângulo em relação ao eixo X
        float angle = Mathf.Atan2(projectile.Direction.y, projectile.Direction.x) * Mathf.Rad2Deg;

        // Aplica a rotação
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

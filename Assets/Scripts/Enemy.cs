using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] [Range(1, 50)] int unitValue;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 1.5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletVelocity = 20f;

    [Header("Sound&Effect")]
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] [Range(0, 1)] float volumeDeath = 0.5f;
    [SerializeField] [Range(0, 1)] float volumeShoot = 0.5f;
    [SerializeField] [Range(0, 1)] float volumeDamage = 0.5f;
    [SerializeField] GameObject explosion_prefab;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        PlaySound(shootSound, volumeShoot);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletVelocity);
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(damageDealer != null)
        {

            ProcessHit(damageDealer);

        }
    }

   

    private void ProcessHit(DamageDealer damageDealer)
    {
        //  
        health -= damageDealer.GetDamage();
        PlaySound(hitSound, volumeDamage);
        //
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

 private void GenerarExplosion()
    {
        GameObject explosion = Instantiate(explosion_prefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }
     
    private void Die()
    {
        ProcessPoints();
        GenerarExplosion();
        Destroy(gameObject);
        PlaySound(deathSound,volumeDeath);
    }

    private void PlaySound(AudioClip sound,float volume)
    {     
        AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position,volume);
    }

    private void ProcessPoints()
    {
        GameSession sesionActual = FindObjectOfType<GameSession>();
        sesionActual.AddToScore(unitValue);
    }
}

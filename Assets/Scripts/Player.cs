using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    [Header("Configuration")]
    [Range(10, 20)]
    [SerializeField] float moveSpeed = 15;
    [SerializeField] float padding = 0.6f;
    [SerializeField] float paddingTop = 5f;
    
    [Header("LifeAndDeath")]
    [SerializeField] float health = 150;
    [SerializeField] int lives = 1;
    private float startHealth;
    [SerializeField] GameObject playerExplosionPrefab;


    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [Range(0.01f, 0.5f)]
    [SerializeField] float projectileFiringPeriod = 0.25f;
    [SerializeField] Transform shootTransform;
    Coroutine firingCoroutine;


    [Header("Audio")]
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip damageSound;
    float volMax = .5f;
    float volMin = .8f;

    // 
    [Header("Boundaries")]
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    private Level level;


    // Start is called before the first frame update
    void Start()
    {
        setUpMoveBoundaries();
        startHealth = health;
        level = GetComponent<Level>();
    }

    public float getHealth()
    {
        return health;
    }

    public float getHealthPercent()
    {
        float percent = (health/startHealth);
        return percent;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            PlaySound(shootSound,0.4f);
            Vector2 shootPosition = new Vector2(shootTransform.position.x, shootTransform.position.y);
            GameObject laserAux = Instantiate(laserPrefab, shootPosition, Quaternion.identity) as GameObject;
            laserAux.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }


    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime;


        var newXPos = Mathf.Clamp(transform.position.x + deltaX * moveSpeed,xMin,xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY * moveSpeed,yMin,yMax);
        transform.position = new Vector2(newXPos, newYPos);


    }


    private void setUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        // 
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        // 
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingTop;

        //Debug.DrawLine(new Vector3(xMin,yMax,0), new Vector3(xMax,yMax,0));

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        //  
        health -= damageDealer.GetDamage();
        PlaySound(damageSound, 0.4f);
        //
        damageDealer.Hit();
        if (health <= 0)
        {
            CalculateLives();
            
        }
    }

    private void CalculateLives()
    {
        if (lives > 1)
        {
            lives--;
            health = startHealth;
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        GenerateExplosion();
        float vol = UnityEngine.Random.Range(volMin, volMax);
        PlaySound(shootSound, vol);
        Destroy(gameObject);
        level.LoadGameOver();
    }

    private void GenerateExplosion()
    {
        GameObject explosion = Instantiate(playerExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1);
    }

    private void PlaySound(AudioClip sound, float volume)
    {
        AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, volume);
    }


}

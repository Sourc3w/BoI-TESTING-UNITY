using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 movement;
    private Vector2 velocity;
    public float speed = 2f;
    new Rigidbody2D rigidbody;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    public int health = 3;
    Renderer rend;
    Color c;

    void Awake()
    {

    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
        c = rend.material.color;
    }

    void Update()
    {
        float horMov = Input.GetAxisRaw("Horizontal");
        float verMov = Input.GetAxisRaw("Vertical");
        float horShoot = Input.GetAxisRaw("HorizontalShooting");
        float verShoot = Input.GetAxisRaw("VerticalShooting");

        if((horShoot != 0 || verShoot != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(horShoot, verShoot);
            lastFire = Time.time;
        }

        rigidbody.velocity = new Vector2(horMov * speed, verMov * speed);

    }

    void FixedUpdate()
    {

    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2
            (
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed
            );
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if((col.gameObject.tag == "Enemy") && (health > 0))
        {
            health -= 1;
           StartCoroutine("Invulnerable");
        }
        else if(health == 0)
        {
            //Destroy(gameObject);
            Debug.Log("GAME OVER");
        }
    }
    IEnumerator Invulnerable()
    {
        Physics2D.IgnoreLayerCollision(10, 9, true);
        c.a = 0.6f;
        rend.material.color = c;
        yield return new WaitForSeconds(1.5f);
        Physics2D.IgnoreLayerCollision(10, 9, false);
        c.a = 1f;
        rend.material.color = c;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DamageDealt;
    public float ProjVel;

    //Variables associated with the smoke trail and its spawn rate
    public GameObject SmokeTrail;
    public float SmokeTrailF;

    //Variables associated with the ember trail and its spawn rate
    public GameObject EmberTrail;
    public float EmberTrailF;

    public GameObject Explosion;

    // Use this for initialization
    void Start()
    {
        SmokeTrailF = Random.Range(2.0f, 5.0f);
        EmberTrailF = Random.Range(2.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.right * ProjVel) * Time.deltaTime;

        //Checks if Smoke Trail is off Cooldown and if so will Spawn Smoke, otherwise decrements SmokeTrailF each step.
        if (SmokeTrailF <= 0)
        {
            SpawnSmoke();
        }
        else
        {
            SmokeTrailF--;
        }

        if (EmberTrailF <= 0)
        {
            SpawnEmber();
        }
        else
        {
            EmberTrailF--;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Colliding with " + col.gameObject.name);
        GameObject Explos = Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    /*
        void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Hitting with a PC Fireball!");
        Vector2 average = Vector2.zero;

        if (col.gameObject.name.Contains("Enemy"))
        {
            Debug.Log("Hitting " + col.gameObject.name + " with a PC Fireball!");
            col.gameObject.GetComponent<EnemyState>().TakeDamage(DamageDealt);//, new Vector3(HitPoint.x, HitPoint.y, col.gameObject.transform.position.z));
            
        }
    }
    */

    void SpawnSmoke()
    {
        //Making sure the trail can't immediately spawn again afterwards. Also adds
        SmokeTrailF = Random.Range(3.0f, 5.0f);
        EmberTrailF = +1.0F;
        
        //Generate the smoke trail. Particle system will probably be far better to use, but that's something to work on for later.
        GameObject Smoke = Instantiate(SmokeTrail, transform.position, transform.rotation);
        Smoke.transform.position = new Vector3(Smoke.transform.position.x, Smoke.transform.position.y, -1);
    }

    void SpawnEmber()
    {
        EmberTrailF = Random.Range(3.0f, 5.0f);
        SmokeTrailF = +1.0F;
        GameObject Ember = Instantiate(EmberTrail, transform.position, transform.rotation);
        Ember.transform.position = new Vector3(Ember.transform.position.x, Ember.transform.position.y, -1);
    }

    /*
    foreach (ContactPoint2D BlastHit in col.contacts)
    {
        average += BlastHit.point;
    }
    Vector2 HitPoint = average / col.contacts.Length;
    if (col.gameObject.name == "Grunt(Clone)")
    {
        Debug.Log("Hitting " + col.gameObject.name + " with a energy blast!");
        //col.gameObject.GetComponent<EnemyState>().TakeDamage(DamageDealt, new Vector3(HitPoint.x, HitPoint.y, col.gameObject.transform.position.z));
        Destroy(gameObject);
    }
    else if (col.gameObject.name == "Bearer(Clone)")
    {
        Debug.Log("Hitting " + col.gameObject.name + " with a energy blast!");
        //col.gameObject.GetComponent<EnemyState>().TakeDamage(DamageDealt, new Vector3(HitPoint.x, HitPoint.y, col.gameObject.transform.position.z));
        Destroy(gameObject);
    }
    */
}

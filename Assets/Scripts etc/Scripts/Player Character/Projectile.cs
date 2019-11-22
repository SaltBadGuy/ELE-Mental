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
    public float SmokeTrailCDMin;
    public float SmokeTrailCDMax;

    //Variables associated with the ember trail and its spawn rate
    public GameObject EmberTrail;
    public float EmberTrailF;
    public float EmberTrailCDMin;
    public float EmberTrailCDMax;

    public Vector3 GiveDir;

    public GameObject Explosion;

    // Use this for initialization
    void Start()
    {
        SmokeTrailF = Random.Range(SmokeTrailCDMin, SmokeTrailCDMax);
        EmberTrailF = Random.Range(EmberTrailCDMin, EmberTrailCDMax);
        //GiveDir = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z));// * transform.forward;

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
        //Making sure the trail can't immediately spawn again afterwards.
        SmokeTrailF = Random.Range(SmokeTrailCDMin, SmokeTrailCDMax);
        

        //Generate the smoke trail. Particle system will probably be far better to use, but that's something to work on for later.
        GameObject Smoke = Instantiate(SmokeTrail, transform.position, transform.rotation);
        Smoke.transform.position = new Vector3(Smoke.transform.position.x, Smoke.transform.position.y, -1);
    }

    void SpawnEmber()
    {
        //Making sure the trail can't immediately spawn again afterwards.
        EmberTrailF = Random.Range(EmberTrailCDMin, EmberTrailCDMax);
        //Generate the smoke trail. Particle system will probably be far better to use, but that's something to work on for later. Will also provide a random offset to make the particle move slightly.
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

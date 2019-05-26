using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DamageDealt;
    public float ProjVel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.right * ProjVel) * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("HITTING WITH AN ENERGY BLAST TBQH");
        Vector2 average = Vector2.zero;

        if (col.gameObject.name.Contains("Enemy"))
        {
            Debug.Log("Hitting " + col.gameObject.name + " with a energy blast!");
            col.gameObject.GetComponent<EnemyState>().TakeDamage(DamageDealt);//, new Vector3(HitPoint.x, HitPoint.y, col.gameObject.transform.position.z));
            Destroy(gameObject);
        }
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

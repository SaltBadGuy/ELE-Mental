using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DamageDealt;
    public float ProjVel;

    //Spin is used for splinters, simply cosmetic. Spinsign is set to either +1 or -1, effectively randomizing if it'll spin clockwise/counterclockwise
    public bool Spin;
    public int SpinSign;

    //SelfDesB tells us if the projectile has a fixed life. we eventually want all projectiles to disappear after a fixed time, either for gameplay/balance reasons and/or to prevent the fireball escaping the play area and travelling forever.
    //SelfDesB == true: Regular behaviour, the projectile must expire a SelfDesF amount of seconds after spawning.
    //SelfDesB == false; This is for when projectiles have been spawned but aren't to be used yet. Earth projectiles can be held by the player, and we don't want it to expire before the player even throws it. 
    //Once the projectile becomes active, it'll always be told to self-destruct after SelfDesF.

    public bool SelfDesB;
    public bool SelfDesDone;
    public float SelfDesF;

    //Bools used for player earth projectiles, the projectiles do not have hitboxes or expire while not fired yet. If not active, they'll follow a parent object around. Most projectiles are active by default. 
    //The parent information is held so we know who cast the projectile. The spawn parent is to get the exact spawn object it came from (mostly for debugging player earth projectiles.
    public bool Active;
    public int NoOfHits;
    public Transform SpawnParent;
    public Transform Parent;

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

    public GameObject Explosion;

    //Used for Earth projectiles.
    public GameObject Splinter;
    public int SplinterCount;
    public int CanSplinter = 1;


    // Use this for initialization
    void Start()
    {
        int[] SpinSignCandidates = new int[] { -1, 1};
        SpinSign = SpinSignCandidates[Random.Range(0, SpinSignCandidates.Length)];
        SmokeTrailF = Random.Range(SmokeTrailCDMin, SmokeTrailCDMax);
        EmberTrailF = Random.Range(EmberTrailCDMin, EmberTrailCDMax);

    }

    // Update is called once per frame
    void Update()
    {

        if (!SelfDesDone)
        {
            if (SelfDesB)
            {
                SelfDestruct(SelfDesF);
                SelfDesDone = true;
            }
        }

        if (Active)
        {
            SelfDesB = true;

            transform.position += (transform.right * ProjVel) * Time.deltaTime;
            if (Spin)
            {
                transform.GetChild(0).rotation *= Quaternion.Euler(0, 0, 30 * SpinSign);
            }

            GetComponent<BoxCollider2D>().enabled = true;

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
        //For Earth projectiles, this keeps the projectile behind the player while it's prepared but not fired yet.
        else
        {
            transform.position = SpawnParent.position;
            transform.rotation = SpawnParent.rotation;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Colliding with " + col.gameObject.name);
        if (Active)
        {
            GameObject Explos = Instantiate(Explosion, transform.position, transform.rotation);

            //If this hits something that isn't a wall (ie. an enemy), we check if it casts splinters, continues for more hits or simply blows up
            if (col.tag != "Wall")
            {
                if (CanSplinter > 0)
                {
                    SplinterSpawn();
                }

                if (NoOfHits > 0)
                {
                    NoOfHits--;
                }
                if (NoOfHits == 0)
                {
                    Active = false;
                    SelfDestruct(0);
                }
            }
            else
            {
                Active = false;
                SelfDestruct(0);
            }
        }
    }

    void SplinterSpawn()
    {

        float BaseOffSetRot = 30;


        for (int i = -Mathf.FloorToInt(SplinterCount/2); i < Mathf.FloorToInt(SplinterCount/2) + 1; i++)
        {
            
            float OffSetRot;
            OffSetRot = 360 + (BaseOffSetRot * i);
            //If we go over 360 degrees we simply take away 360 again and get the absolute value (effectively, we can go from 30 to 15 to 0 to 345 to 330 by taking away 15 each time).
            if (OffSetRot > 360)
            {
                Mathf.Floor(OffSetRot -= 360);
            }
            Debug.Log("Splinter i is " + i + ", OffSetRot is " + OffSetRot);
            GameObject Proj = Instantiate(Splinter, transform.position, transform.rotation * Quaternion.Euler(0, 0, -OffSetRot));
            Proj.GetComponent<Projectile>().CanSplinter = CanSplinter - 1;
        }
    }

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

    void SelfDestruct(float f)
    {
        //SpawnParent.GetComponent<PrjSpawn>().Child = null;
        Destroy(gameObject, f);
    }
    
}

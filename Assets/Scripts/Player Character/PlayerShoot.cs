using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public Camera mainCamera;

    PlayerState PlayerState;

    // keep track of the mouse position
    private Vector3 mousePosition;
    // keep track of where our mouse is in 2d space
    private Vector3 mouseTarget;

    public bool StartFiring;

    public float DamageDealt;
    public float RateOfFire;
    public float FireCD;
    public float inputX;
    public float inputY;
    public float lookX;
    public float lookY;
    public float GainElement;

    public GameObject FireBlast;
    public Transform ProjSpawn;
    public GameObject ProjSpark;

    public Vector2 look;

    // Use this for initialization
    void Start()
    {
        PlayerState = GetComponent<PlayerState>();
        ProjSpawn = this.gameObject.GetComponentInChildren<Transform>().Find("ProjectileSpawn");

    }

    // Update is called once per frame
    void Update()
    {

        inputX = Input.GetAxis("R_XAxis_1");
        inputY = Input.GetAxis("R_YAxis_1");

        if (inputX != 0 || inputY != 0)
        {
            look = new Vector2(inputX, inputY);
            StartFiring = true;
        }
        else
        {
            StartFiring = false;
        }

        if (FireCD > 0)
        {
            FireCD--;
        }


        if (GetComponent<PlayerState>().Alive)
        {
            if (StartFiring)
            {
                if (PlayerState.Anger < 100)
                {
                    PlayerState.Anger++;
                }
                PlayerState.Boredom = 0;
                if (FireCD <= 0)
                {
                    if (GetComponent<PlayerState>().ElementEquipped == "Fire")
                    {
                        Debug.Log("Firing!");
                        FireCD = RateOfFire;
                        GameObject Projectile = Instantiate(FireBlast, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        Projectile.transform.position = new Vector3(Projectile.transform.position.x, Projectile.transform.position.y, -1);
                        //GameObject WandSparkObj = Instantiate(ProjSpark, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        //WandSparkObj.GetComponent<PartTrack>().TargetObj = ProjSpawn;
                        //Projectile.GetComponent<Bullet>().DamageDealt = DamageDealt;
                        Destroy(Projectile, 5f);
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Water")
                    {
                        Debug.Log("Firing!");
                        FireCD = RateOfFire;
                        GameObject Projectile = Instantiate(FireBlast, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        Projectile.transform.position = new Vector3(Projectile.transform.position.x, Projectile.transform.position.y, -1);
                        GameObject WandSparkObj = Instantiate(ProjSpark, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        WandSparkObj.GetComponent<PartTrack>().TargetObj = ProjSpawn;
                        //Projectile.GetComponent<Bullet>().DamageDealt = DamageDealt;
                        Destroy(Projectile, 5f);
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Earth")
                    {
                        Debug.Log("Firing!");
                        FireCD = RateOfFire;
                        GameObject Projectile = Instantiate(FireBlast, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        Projectile.transform.position = new Vector3(Projectile.transform.position.x, Projectile.transform.position.y, -1);

                        //Creates the WandSpark and tells the WandSpark where it needs to keep the projectile (So that the spark moves with the character
                        GameObject WandSparkObj = Instantiate(ProjSpark, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        WandSparkObj.GetComponent<PartTrack>().TargetObj = ProjSpawn;
                        //Projectile.GetComponent<Bullet>().DamageDealt = DamageDealt;
                        Destroy(Projectile, 5f);
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Air")
                    {
                        Debug.Log("Firing!");
                        FireCD = RateOfFire;
                        GameObject Projectile = Instantiate(FireBlast, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        Projectile.transform.position = new Vector3(Projectile.transform.position.x, Projectile.transform.position.y, -1);
                        GameObject WandSparkObj = Instantiate(ProjSpark, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        WandSparkObj.GetComponent<PartTrack>().TargetObj = ProjSpawn;
                        //Projectile.GetComponent<Bullet>().DamageDealt = DamageDealt;
                        Destroy(Projectile, 5f);
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Spare Code")
                    {
                        Debug.Log("Firing!");
                        FireCD = RateOfFire;
                        var mousePos = Input.mousePosition;
                        mousePos.x -= Screen.width / 2;
                        mousePos.y -= Screen.height / 2;
                        mousePos.z = 0;
                        var heading = mousePos - transform.position;
                        var direction = heading;
                        var origin = transform.position;
                        GameObject Projectile = Instantiate(FireBlast, transform.position, transform.rotation);
                        Projectile.transform.position = new Vector3(Projectile.transform.position.x, Projectile.transform.position.y, -1);
                        Projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, heading);
                        //Projectile.GetComponent<Bullet>().DamageDealt = DamageDealt;
                        //Projectile.GetComponent<Rigidbody2D>().velocity = transform.forward * 100; //heading * 0.075f;
                        Projectile.GetComponent<Rigidbody2D>().AddForce(transform.forward * 20);
                        Destroy(Projectile, 5f);
                    }
                }
            }
            else
            {
                if (PlayerState.Anger > -1)
                {
                    PlayerState.Anger--;
                }
            }
        }        
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}



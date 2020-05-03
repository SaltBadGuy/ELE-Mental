using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public Camera mainCamera;

    public PlayerAnim PCA;
    public PlayerState PS;
    public PlayerMovement PM;

    // keep track of the mouse position
    private Vector3 mousePosition;
    // keep track of where our mouse is in 2d space
    private Vector3 mouseTarget;

    public bool StartFiring;

    public float DamageDealt;
    public float RateOfFire;
    public float FireCD;
    public float InputX;
    public float InputY;
    public float LookX;
    public float LookY;
    public float GainElement;

    public GameObject FireBlast;
    public Transform ProjSpawn;
    public GameObject ProjSpark;


    //Referring to the 2 different trees, VisRot is stuff affected by rotation and so on such as the character body, while StaHit is things like the character's hitbox where we don't want it to rotate necessarily.
    public GameObject VisRot;
    public GameObject StaHit;

    public Vector2 look;

    //Either uses the standard 360 controller set up if true, will use keyboard and mouse if false
    public string FireControl;

    // Use this for initialization
    void Start()
    {
        PS = GetComponent<PlayerState>();
        //ProjSpawn = this.gameObject.GetComponentInChildren<Transform>().Find("ProjectileSpawn");

    }

    // Update is called once per frame
    void Update()
    {

        if (FireControl == "Controller")
        {
            InputX = Input.GetAxis("R_XAxis_1");
            InputY = Input.GetAxis("R_YAxis_1");

            //We start firing if any direction on the right stick is held
            if (InputX != 0 || InputY != 0)
            {
                look = new Vector2(InputX, InputY);
                StartFiring = true;
            }
            else
            {
                StartFiring = false;
            }
        }
        else if (FireControl == "Mouse")
        {
            //Get mouse input
            mousePosition = Input.mousePosition;
            mouseTarget = mainCamera.ScreenToWorldPoint(mousePosition);

            //We also check if we're actually firing (with mouse, we check if left click is being used)
            if (Input.GetMouseButton(0))
            {
                StartFiring = true;
            }
            else
            {
                StartFiring = false;
            }

            //Finally we set the mouse position
            InputX = Input.mousePosition.x;
            InputY = Input.mousePosition.y;
        }
        else if (FireControl == "Keyboard")
        {

        }
        else
        {
            Debug.Log("something weird is being used for firing!");
        }

        if (FireCD > 0)
        {
            FireCD--;
        }


        if (GetComponent<PlayerState>().Alive)
        {
            if (StartFiring)
            {

                //While the character is being told to fire, our character's Anger stat goes up, which affects some of the character's animations. 
                if (PS.AnAnger < 100)
                {
                    PS.AnAnger++;
                }
                //The character's boredom is set to 0 to prevent it from playing certain idle animations while fighting off enemies
                PS.AnBoredom = 0;
                

                //Currently only the fire element is implemented properly
                if (FireCD <= 0)
                {
                    if (GetComponent<PlayerState>().ElementEquipped == "Fire")
                    {
                        FireAttack();
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Water")
                    {
                        //If we don't have full charge yet, the player character just does a normal attack
                        if (PS.Charge < 100)
                        {
                            FireCD = RateOfFire;
                            GameObject Projectile = Instantiate(FireBlast, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                            Projectile.transform.position = new Vector3(Projectile.transform.position.x, Projectile.transform.position.y, -1);
                            GameObject WandSparkObj = Instantiate(ProjSpark, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                            WandSparkObj.GetComponent<PartTrack>().TargetObj = ProjSpawn;
                            Destroy(Projectile, 5f);
                        }
                        //If our character is fully charged, then we perform the charged attack
                        else
                        {
                            Debug.Log("Character has done a big charge shot!");
                        }
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Earth")
                    {
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
                        GameObject WandSparkObj = Instantiate(ProjSpark, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
                        WandSparkObj.GetComponent<PartTrack>().TargetObj = ProjSpawn;
                    }
                    //The character also loses all Charge, resetting its movement speed to normal and meaning the character has to wait before getting a fully charged shot (again).
                    PS.Charge = 0;
                }
            }
            else
            {
                if (PS.AnAnger > -1)
                {
                    PS.AnAnger--;
                }
            }
        }        
    }

    void FireAttack()
    {
        //If we don't have full charge yet, the player character just does a normal attack
        if (PS.Charge < 100)
        {
            FireCD = RateOfFire;
            GameObject Projectile = Instantiate(FireBlast, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
            Projectile.transform.position = new Vector3(Projectile.transform.position.x, Projectile.transform.position.y, -1);
            Destroy(Projectile, 5f);
        }
        //If our character is fully charged, then we perform the charged attack (NOT FINISHED YET, ONLY DIFFERENCE IS SCALE JUST TO CLEARLY SHOW CHARGE SYSTEM WORKING)
        else
        {
            FireCD = RateOfFire;
            GameObject Projectile = Instantiate(FireBlast, ProjSpawn.transform.position, ProjSpawn.transform.rotation);
            Projectile.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            Projectile.transform.position = new Vector3(Projectile.transform.position.x, Projectile.transform.position.y, -1);
            Destroy(Projectile, 5f);
        }
    }

    void FixedUpdate()
    {
        
        if (FireControl == "Controller")
        {
            //Rotates relevant VisRot components (anything that isn't legs) to face where the character is aiming
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
            VisRot.transform.Find("Arms").transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            VisRot.transform.Find("Body").transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            VisRot.transform.Find("Face").transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (FireControl == "Mouse") {
            //Rotates relevant VisRot components (anything that isn't legs) to face where the mouse is pointing (small hack to offset by 90 degrees)
            VisRot.transform.Find("Arms").transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseTarget - transform.position);
            VisRot.transform.Find("Arms").transform.rotation *= Quaternion.Euler(0, 0, 90);
            VisRot.transform.Find("Body").transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseTarget - transform.position);
            VisRot.transform.Find("Body").transform.rotation *= Quaternion.Euler(0, 0, 90);
            VisRot.transform.Find("Face").transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseTarget - transform.position);
            VisRot.transform.Find("Face").transform.rotation *= Quaternion.Euler(0, 0, 90);
        }  
    }
}



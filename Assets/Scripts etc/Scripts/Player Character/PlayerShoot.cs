using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public Camera mainCamera;

    public PlayerAnim PA;
    public PlayerState PS;
    public PlayerMovement PM;

    // keep track of the mouse position
    private Vector3 mousePosition;
    // keep track of where our mouse is in 2d space
    private Vector3 mouseTarget;

    public bool StartFiring;

    public float DamageDealt;

    //Our different rates of fire for each element.
    public float FireROF;
    public float WaterROF;
    public float EarthROF;
    public float LightningROF;

    public float AttCD;
    public float InputX;
    public float InputY;
    public float LookX;
    public float LookY;
    public float GainElement;

    //We have our normal element attack objects, our charged variants and their spawn positions (these are set in the inspector)
    public GameObject NorFireAtt;
    public GameObject NorWaterAtt;
    public GameObject NorEarthAtt;
    public GameObject NorLightningAtt;

    public GameObject ChaFireAtt;
    public GameObject ChaWaterAtt;
    public GameObject ChaEarthAtt;
    public GameObject ChaLightningAtt;

    //All the different spawns. Earth doesn't get one as it's handled separately (In Player State).
    public Transform NorFireAttSpawn;
    public Transform NorWaterAttSpawn;
    public Transform NorLightningAttSpawn;

    public Transform ChaFireAttSpawn;
    public Transform ChaWaterAttSpawn;
    public Transform ChaLightningAttSpawn;

    public int WaterBaseOffSetRot;


    //Referring to the 2 different trees, VisRot is stuff affected by rotation and so on such as the character body, while StaHit is things like the character's hitbox where we don't want it to rotate necessarily.
    public GameObject VisRot;
    public GameObject StaHit;

    public Vector2 look;

    public LayerMask EnemyLMask;

    //Either uses the standard 360 controller set up if true, will use keyboard and mouse if false
    public string FireControl;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        GetAttackDir();

        if (FireControl == "Controller")
        {
            //Rotates relevant VisRot components (anything that isn't legs) to face where the character is aiming
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
            VisRot.transform.Find("Arms").transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            VisRot.transform.Find("Body").transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            VisRot.transform.Find("Face").transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (FireControl == "Mouse")
        {
            //Rotates relevant VisRot components (anything that isn't legs) to face where the mouse is pointing (small hack to offset by 90 degrees)
            VisRot.transform.Find("Arms").transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseTarget - transform.position);
            VisRot.transform.Find("Arms").transform.rotation *= Quaternion.Euler(0, 0, 90);
            VisRot.transform.Find("Body").transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseTarget - transform.position);
            VisRot.transform.Find("Body").transform.rotation *= Quaternion.Euler(0, 0, 90);
            VisRot.transform.Find("Face").transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseTarget - transform.position);
            VisRot.transform.Find("Face").transform.rotation *= Quaternion.Euler(0, 0, 90);
        }

    }

    void LateUpdate()
    {

        if (GetComponent<PlayerState>().Alive)
        {
            
            

            if (AttCD > 0)
            {
                AttCD--;
                PS.ChargeAttMulti = 0;
            }
            else
            {
                if (PS.ElementEquipped == "Earth")
                {
                    EarthAttSpawn();
                }
                PS.ChargeAttMulti = 1;
            }

            if (StartFiring)
            {
                AnimChange();

                //Currently only the fire element is implemented properly
                if (AttCD <= 0)
                {
                    if (GetComponent<PlayerState>().ElementEquipped == "Fire")
                    {
                        FireAttack();
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Water")
                    {
                        WaterAttack();
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Earth")
                    {
                        EarthAttack();
                    }
                    else if (GetComponent<PlayerState>().ElementEquipped == "Air")
                    {
                        LightningAttack();
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

    void GetAttackDir()
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
    }

    void AnimChange()
    {
        //While the character is being told to fire, our character's Anger stat goes up, which affects some of the character's animations. 
        if (PS.AnAnger < 100)
        {
            PS.AnAnger++;
        }
        //The character's boredom is set to 0 to prevent it from playing certain idle animations while fighting off enemies
        PS.AnBoredom = 0;
    }

    void EarthAttSpawn()
    {
        for (int i = 1; i < PS.EarthPrjSpawnArr.Length; i++)
        {
            int PairCheck;
            PairCheck = 2 * (Mathf.FloorToInt(PS.Charge / 100 * 4)) + 1;
            if (i <= PairCheck)
            {
                if (!PS.EarthPrjSpawnArr[i].GetComponent<PrjSpawn>().Taken)
                {
                    GameObject Proj = Instantiate(NorEarthAtt, PS.EarthPrjSpawnArr[i].transform.position, PS.EarthPrjSpawnArr[i].transform.rotation);
                    Proj.transform.position = new Vector3(Proj.transform.position.x, Proj.transform.position.y, -1);
                    PS.EarthPrjSpawnArr[i].GetComponent<PrjSpawn>().Child = Proj;
                    PS.EarthPrjSpawnArr[i].GetComponent<PrjSpawn>().Taken = true;
                    Proj.GetComponent<Projectile>().SpawnParent = PS.EarthPrjSpawnArr[i].transform;
                    Proj.GetComponent<Projectile>().Parent = gameObject.transform;                  
                }
            }
        }
    }

   void FireAttack()
   {
       //If we don't have full charge yet, the player character just does a normal attack
       if (PS.Charge < 100)
       {
           PA.Anim.Play("Normal Fire Attack");
           AttCD = FireROF;
           GameObject Proj = Instantiate(NorFireAtt, NorFireAttSpawn.transform.position, NorFireAttSpawn.transform.rotation);
           Proj.transform.position = new Vector3(Proj.transform.position.x, Proj.transform.position.y, -1);
           Proj.GetComponent<Projectile>().Parent = this.transform;
        }
       //If our character is fully charged, then we perform the charged attack (NOT FINISHED YET, ONLY DIFFERENCE IS SCALE JUST TO CLEARLY SHOW CHARGE SYSTEM WORKING)
       else
       {
           PA.Anim.Play("Charged Fire Attack");
           AttCD = FireROF;
           GameObject Proj = Instantiate(ChaFireAtt, ChaFireAttSpawn.transform.position, ChaFireAttSpawn.transform.rotation);
           Proj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
           Proj.transform.position = new Vector3(Proj.transform.position.x, Proj.transform.position.y, -1);
           Proj.GetComponent<Projectile>().Parent = this.transform;
        }
   }

   void EarthAttack()
   {
        //Releases all earth projectiles currently up.
        for (int i = 1; i < PS.EarthPrjSpawnArr.Length; i++)
        {
            if (PS.EarthPrjSpawnArr[i].GetComponent<PrjSpawn>().Taken)
            {
                PS.EarthPrjSpawnArr[i].GetComponent<PrjSpawn>().Taken = false;
                PS.EarthPrjSpawnArr[i].GetComponent<PrjSpawn>().Child.GetComponent<Projectile>().Active = true;
                PS.EarthPrjSpawnArr[i].GetComponent<PrjSpawn>().Child = null;
                AttCD = EarthROF;
            }   
        }
        
    }

    void WaterAttack()
   {
       //If we don't have full charge yet, the player character just does a normal attack
       if (PS.Charge < 100)
       {

           AttCD = WaterROF;
           float OffSetRot;
           OffSetRot = 360 + (Random.Range(-WaterBaseOffSetRot, WaterBaseOffSetRot));
           //If we go over 360 degrees we simply take away 360 again and get the absolute value (effectively, we can go from 30 to 15 to 0 to 345 to 330 by taking away 15 each time).
           if (OffSetRot > 360)
           {
             Mathf.Floor(OffSetRot -= 360);
           }
           GameObject Proj = Instantiate(NorWaterAtt, NorWaterAttSpawn.transform.position, NorWaterAttSpawn.transform.rotation * Quaternion.Euler(0, 0, OffSetRot));
           Proj.transform.position = new Vector3(Proj.transform.position.x, Proj.transform.position.y, -1);
           Proj.GetComponent<Projectile>().Parent = transform;
        }
       //If our character is fully charged, then we perform the charged attack (NOT FINISHED YET, ONLY DIFFERENCE IS SCALE JUST TO CLEARLY SHOW CHARGE SYSTEM WORKING)
       else
       {
           AttCD = WaterROF;
           GameObject Proj = Instantiate(ChaWaterAtt, ChaWaterAttSpawn.transform.position, ChaWaterAttSpawn.transform.rotation);
           Proj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
           Proj.transform.position = new Vector3(Proj.transform.position.x, Proj.transform.position.y, -1);
           Proj.GetComponent<Projectile>().Parent = transform;
        }
   }

    void LightningAttack()
    {
        if (PS.Charge < 100)
        {
            AttCD = FireROF;
            Debug.DrawRay(new Vector3(NorLightningAttSpawn.position.x, NorLightningAttSpawn.position.y, NorLightningAttSpawn.position.z), NorFireAttSpawn.transform.up, Color.green, 1);
            RaycastHit2D hit = Physics2D.Raycast(NorLightningAttSpawn.position, NorFireAttSpawn.transform.up, EnemyLMask);
        }
        //If our character is fully charged, then we perform the charged attack (NOT FINISHED YET, ONLY DIFFERENCE IS SCALE JUST TO CLEARLY SHOW CHARGE SYSTEM WORKING)
        else
        {
            AttCD = FireROF;
            GameObject Proj = Instantiate(ChaFireAtt, NorFireAttSpawn.transform.position, NorFireAttSpawn.transform.rotation);
            Proj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            Proj.transform.position = new Vector3(Proj.transform.position.x, Proj.transform.position.y, -1);
            Proj.GetComponent<Projectile>().Parent = this.transform;
        }
    }
}



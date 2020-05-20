using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    //Emotions, used for animation and facial changes
    public float AnBoredom;
    public float AnAnger;
    public bool AnHappy;
    public bool AnHurt;
    public int HurtCD;

    //Charge refers to a mechanic where the player character both moves faster and gets to do a special charged-up version of their attack. 
    //ChargeMulti is there for possible mechanics that affect how quickly the player character gains charge.
    public float Charge;
    public float ChargeMulti;
    public float ChargeAttMulti;

    //Misc states. ElementEquipped can be Fire, Water, Earth and Lightning. The different lists make it much easier to get all the relevant pieces for each element (fire stuff is at position 0 of each list etc.)
    public bool Alive;
    public string ElementEquipped;
    public List<string> ElementEquippedList = new List<string> { "Fire", "Earth", "Lightning", "Water" };
    public int EleIndex;

    //Objs and arrays for the player's aura.
    public GameObject AuraCircle;
    public GameObject AuraIcon;
    public GameObject[] AuraIconObjArr;

    public Sprite FireIcon;
    public Sprite EarthIcon;
    public Sprite LightningIcon;
    public Sprite WaterIcon;
    public Sprite[] AuraIconSprArr;

    public int EarthSize;
    public GameObject EarthPivot;
    public GameObject EarthPrjSpawn;
    public GameObject[] EarthPrjSpawnArr;

    public PlayerShoot PSH;
    public GameObject Aura;

    // Use this for initialization
    void Start()
    {
        EleIndex = 0;
        AuraIconSprArr = new Sprite[] { FireIcon, EarthIcon, LightningIcon, WaterIcon };
        AuraIconObjArr = new GameObject[5];
        SetUpEarth();
        SetUpAura();
        
    }

    // Update is called once per frame
    void Update()
    {
        

        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        //AuraUpdate();

        AnimUpdate();

        ChargeUpdate();

        //Cycles through elements as a debug tool, currently only flicks between fire and earth but will eventually correctly cycle through the array/list
        if (Input.GetKeyDown("space"))
        {
            EleIndex = CycleThruList(EleIndex, ElementEquippedList);
            ElementEquipped = ElementEquippedList[EleIndex];
        }

    }


    void SetUpAura()
    {
        for (int i = 0; i < AuraIconObjArr.Length; i++)
        {

            float OffSet = 0.16f;

            int mod;           

            if (i / 2f <= 1f)
            {
                mod = 1;
            }
            else
            {
                mod = -1;
            }

            Debug.Log("i is " + i + ", AuraIconObjArr.Length is " + AuraIconSprArr.Length + ", mod is " + mod);

            //We spawn in the base circle of the aura under i = 0
            if (i == 0)
            {
                AuraIconObjArr[i] = Instantiate(AuraCircle, new Vector3(transform.position.x,
                transform.position.y,
                transform.position.z),
                Aura.transform.rotation,
                Aura.transform);
                AuraIconObjArr[i].name = "AuraCircle";
            }
            //We split up our odd and even icons so they can be placed on the 4 sides of the player
            else if (i % 2 == 0)
            {
                AuraIconObjArr[i] = Instantiate(AuraIcon, new Vector3(transform.position.x + OffSet * mod,
                transform.position.y,
                Aura.transform.position.z - 1),
                Aura.transform.rotation,
                Aura.transform);
                AuraIconObjArr[i].transform.localScale = new Vector3(0.25f, 0.25f, AuraIconObjArr[i].transform.localScale.z);
                AuraIconObjArr[i].name = "AuraIcon " + i;
            }
            else
            {
                AuraIconObjArr[i] = Instantiate(AuraIcon, new Vector3(transform.position.x,
                transform.position.y + OffSet * mod,
                Aura.transform.position.z - 1),
                Aura.transform.rotation,
                Aura.transform);
                AuraIconObjArr[i].transform.localScale = new Vector3(0.25f, 0.25f, AuraIconObjArr[i].transform.localScale.z);
                AuraIconObjArr[i].name = "AuraIcon " + i;
            }          
        }
    }

    void SetUpEarth()
    {
        //We load in the relevant gameobjects for our earth projectile. This is stored in an array to make it easier to iterate through them when doing charge calculations.
        /*
         *The formation is:
         * 1 faces straight forward and is directly behind the opponent
         * 2 and 3 are to either side of 1, offset slightly towards the player and are rotated slightly towards the character's facing. 
         * Repeat this, with every even numbered projectile being above (or + in the y axis) and odd numbers being below (or - in the y axis)
         * 
         * ....prj4
         * ..prj2
         * prj1
         * ..prj3
         * ....prj5
         */

        EarthPrjSpawnArr = new GameObject[EarthSize + 1];
        for (int i = 1; i < EarthSize + 1; i++)
        {
            //If i is odd, we make a modifier negative.
            int PairMod = 1;
            if (i % 2 != 0)
            {
                PairMod = -1;
            }

            //float BaseXUnit = 0.0125f * ((EarthSize - 1))
            //float MaxX = BaseXUnit * (EarthSize - 1);
            //float BaseOffSetX = MaxX / ((EarthSize - 1) * 2);

            float MaxX = 0.2f;
            float BaseOffSetX = 0.025f;
            float BaseOffSetY = 0.125f;
            float BaseOffSetRot = 15;

            //We figure out which pair we're working with (eg. i is 2 or 3, 4 or 5 etc.) Once we do this, we figure out how much we should offset our values by. Since 1 isn't a pair, we just set the offsets to 0.)
            //This gets us 1, 3, 7 or 13... times the offset for our x values. Y goes up linearly.
            int PairCount;
            float OffSetX = 0;
            float OffSetY = 0;
            float OffSetRot = 0;
            PairCount = Mathf.FloorToInt(i / 2);
            if (PairCount > 0)
            {
                OffSetX = BaseOffSetX * (PairCount * PairCount - PairCount + 1);
                OffSetY = (BaseOffSetY * PairCount) * PairMod;
                OffSetRot = 360 - (BaseOffSetRot * PairCount * PairMod);
                //If we go over 360 degrees we simply take away 360 again and get the absolute value (effectively, we can go from 30 to 15 to 0 to 345 to 330 by taking away 15 each time).
                if (OffSetRot > 360)
                {
                    Mathf.Floor(OffSetRot -= 360);
                }
            }

            //This is the regular offset that each pair that isn't pair 1 is affected by.
            Debug.Log("i is " + i + ", PairCount is " + PairCount);

             /*Debug.Log("i is " + i + ", BaseXUnit is " + BaseXUnit + ", MaxX is " + MaxX + ", PairCount is " + PairCount + ", BaseOffSetX is " + BaseOffSetX + 
                 ", EarthPivot's local position is " + EarthPivot.transform.localPosition + ", " +
                 "The vector before mods is " + transform.position.x + EarthPivot.transform.localPosition.x);*/

             //transform.position.x + EarthPivot.transform.localPosition.x - MaxX + InitOffSetX + (OffSetX * PairCount)

             EarthPrjSpawnArr[i] = Instantiate(EarthPrjSpawn, new Vector3(transform.position.x + EarthPivot.transform.localPosition.x - MaxX + OffSetX,
                transform.position.y + EarthPivot.transform.localPosition.y + OffSetY, 
                EarthPivot.transform.position.z), 
                Quaternion.Euler(0, 0, OffSetRot), 
                EarthPivot.transform);

            EarthPrjSpawnArr[i].name = "EarthPrjSpawn " + i; 
        }
        
    }

    public void TakeDamage(float DamageTaken)//, Vector3 Impact)
    {
        //gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Impact * 5);
        //CurHP -= DamageTaken;
        //DrawText(DamageTaken.ToString(), "Damage");
        //Debug.Log(gameObject.name + " took " + DamageTaken + " damage, leaving it at " + CurHP + "HP!");
        AnHurt = true;
        HurtCD = 200;
    }

    void AnimUpdate()
    {
        //Boredom controls some random animations the Player Character might make
        AnBoredom++;

        //Currently unused, attended for animations when player character gets hit, no such animation exists currently
        if (HurtCD > 0)
        {
            HurtCD--;
        }
        else
        {
            AnHurt = false;
        }
    }

    void ChargeUpdate()
    {
        //Our character will naturally gain Charge to a max of 100 (this resets when the character fires)
        if (Charge < 100)
        {
            Charge += (1 * ChargeMulti * ChargeAttMulti);
        }
    }

    //This code needs updated, it's far too messy.
    void AuraUpdate()
    {
        /*
        switch (ElementEquipped){
            case "Fire":

                //We change the colour of the ring. We then change its transparency 
                Aura.transform.Find("Circle").GetComponent<SpriteRenderer>().color = new Color(1,0,0, 192 * Charge);

                Aura.transform.Find("Icon 1").GetComponent<SpriteRenderer>().sprite = FireIcon;
                Aura.transform.Find("Icon 1").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Icon 1").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Icon 1").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Icon 1").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                Aura.transform.Find("Icon 2").GetComponent<SpriteRenderer>().sprite = FireIcon;
                Aura.transform.Find("Icon 2").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Icon 2").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Icon 2").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Icon 2").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                Aura.transform.Find("Icon 3").GetComponent<SpriteRenderer>().sprite = FireIcon;
                Aura.transform.Find("Icon 3").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Icon 3").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Icon 3").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Icon 3").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                Aura.transform.Find("Icon 4").GetComponent<SpriteRenderer>().sprite = FireIcon;
                Aura.transform.Find("Icon 1").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Icon 4").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Icon 4").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Icon 4").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                //The size also scales
                Aura.transform.localScale = new Vector3(0.0125f * Charge, 0.0125f * Charge, 0.0125f * Charge);
                break;
            case "Earth":
                Aura.transform.Find("Circle").GetComponent<SpriteRenderer>().color = new Color(0,1,0,0.5f);
                Aura.transform.Find("Circle").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Circle").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Circle").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Circle").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                Aura.transform.Find("Icon 1").GetComponent<SpriteRenderer>().sprite = EarthIcon;
                Aura.transform.Find("Icon 1").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Icon 1").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Icon 1").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Icon 1").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                Aura.transform.Find("Icon 2").GetComponent<SpriteRenderer>().sprite = EarthIcon;
                Aura.transform.Find("Icon 2").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Icon 2").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Icon 2").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Icon 2").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                Aura.transform.Find("Icon 3").GetComponent<SpriteRenderer>().sprite = EarthIcon;
                Aura.transform.Find("Icon 3").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Icon 3").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Icon 3").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Icon 3").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                Aura.transform.Find("Icon 4").GetComponent<SpriteRenderer>().sprite = EarthIcon;
                Aura.transform.Find("Icon 1").transform.GetComponent<SpriteRenderer>().color = new Color
                    (Aura.transform.Find("Icon 4").GetComponent<SpriteRenderer>().color.r,
                    Aura.transform.Find("Icon 4").GetComponent<SpriteRenderer>().color.g,
                    Aura.transform.Find("Icon 4").GetComponent<SpriteRenderer>().color.b,
                    192 * Charge);

                Aura.transform.localScale = new Vector3(0.0125f * Charge, 0.0125f * Charge, 0.0125f * Charge);

                break;
            case "Water":
                Aura.transform.Find("Circle").GetComponent<SpriteRenderer>().color = new Color(0,0,1,0.5f);
                break;
            case "Lightning":
                Aura.transform.Find("Circle").GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 0.5f);
                break;
        }*/
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {

        }
    }

    int CycleThruList(int i, List<string> IncList) {

        if (i == IncList.Capacity - 1)
        {
            i = 0;
            return i;
        }
        else
        {
            i++;
            return i;
        }
    }
}

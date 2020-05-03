using System.Collections;
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

    //Charge refers to a mechanic where the player character both moves faster and gets to do a special charged-up version of their attack
    public float Charge;

    //Misc states. ElementEquipped can be Fire, Water, Earth and Lightning. The different lists make it much easier to get all the relevant pieces for each element (fire stuff is at position 0 of each list etc.)
    public bool Alive;
    public string ElementEquipped;

    public Sprite FireIcon;
    public Sprite EarthIcon;
    public Sprite LightningIcon;
    public Sprite WaterIcon;

    public List<string> ElementEquippedArr = new List<string> { "Fire", "Earth", "Lightning", "Water" };
    //public List<Sprite> ElementEquippedIconArr = new List<Sprite> { FireIcon, EarthIcon, LightningIcon, WaterIcon };



    public PlayerShoot PS;
    public GameObject Aura;

    // Use this for initialization
    void Start()
    {
        ElementEquipped = "Fire";
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        //Boredom controls some random animations the Player Character might make
        AnBoredom++;

        //More importantly, our character will naturally gain Charge to a max of 100 (this resets when the character fires)
        if (Charge < 100)
        {
            Charge++;
        }

        //Currently unused, attended for animations when player character gets hit, no such animation exists currently
        if(HurtCD > 0)
        {
            HurtCD--;
        }
        else
        {
            AnHurt = false;
        }

        AuraUpdate();

        //Cycles through elements as a debug tool, currently only flicks between fire and earth but will eventually correctly cycle through the array/list
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Tried to cycle thru elements");
            if (ElementEquipped == "Fire")
            {
                ElementEquipped = "Earth";
            }
            else
            {
                ElementEquipped = "Fire";
            }
            CycleThruArray();
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

    void AuraUpdate()
    {
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
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {

        }
    }

    void CycleThruArray() {

    }
}

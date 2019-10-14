using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{

    //Emotions, used for animation and facial changes
    public float Boredom;
    public float Anger;
    public bool Happy;
    public bool Hurt;
    public int HurtCD;

    //Misc states. ElementEquipped can be Fire, Water, Earth and Lightning.
    public bool Alive;
    public string ElementEquipped;

    public PlayerShoot PS;

    // Use this for initialization
    void Start()
    {
        ElementEquipped = "Fire";
        PS = GetComponent<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        Boredom++;
        if(HurtCD > 0)
        {
            HurtCD--;
        }
        else
        {
            Hurt = false;
        }
    }

    public void TakeDamage(float DamageTaken)//, Vector3 Impact)
    {
        //gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Impact * 5);
        //CurHP -= DamageTaken;
        //DrawText(DamageTaken.ToString(), "Damage");
        //Debug.Log(gameObject.name + " took " + DamageTaken + " damage, leaving it at " + CurHP + "HP!");
        Hurt = true;
        HurtCD = 200;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {

        }
    }
}

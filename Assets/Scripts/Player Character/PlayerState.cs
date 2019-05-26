using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public int Lives;
    public float MaxHP;
    public float CurHP;

    //Emotions, used for animation and facial changes
    public float Boredom;
    public float Anger;
    public bool Happy;
    public bool Hurt;

    public bool Alive;
    public string ElementEquipped;

    public PlayerShoot PS;

    // Use this for initialization
    void Start()
    {
        Lives = 3;
        ElementEquipped = "Fire";
        PS = GetComponent<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        Boredom++;
    }

    public void TakeDamage(float DamageTaken)//, Vector3 Impact)
    {
        //gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Impact * 5);
        CurHP -= DamageTaken;
        //DrawText(DamageTaken.ToString(), "Damage");
        Debug.Log(gameObject.name + " took " + DamageTaken + " damage, leaving it at " + CurHP + "HP!");
    }
}

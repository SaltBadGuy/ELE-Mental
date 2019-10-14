using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public float MaxHP;
    public float CurHP;
    public float AttackCD;
    public float RateOfAttack;
    public float DamageDealt;

    public bool StillLiving = true;

    public GameObject PowerDrop;

    // Use this for initialization
    void Start()
    {
        CurHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (StillLiving)
        {
            if (CurHP < 1)
            {
                StillLiving = false;
                Die();
            }
        }
    }

    public void TakeDamage(float DamageTaken)//, Vector3 Impact)
    {
        //gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Impact * 5);
        CurHP -= DamageTaken;
        DrawText(DamageTaken.ToString(), "Damage");
        Debug.Log(gameObject.name + " took " + DamageTaken + " damage, leaving it at " + CurHP + "HP!");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (StillLiving)
        {
            Debug.Log("Colliding");
            if (AttackCD <= 0f)
            {
                Debug.Log("Attacking!");
                if (col.gameObject.name == "PC")
                {
                    Debug.Log("Attacking PC!");
                    col.gameObject.GetComponent<PlayerState>().TakeDamage(DamageDealt);
                }
                AttackCD = RateOfAttack;
            }
        }
    }

    void DrawText(string TextToDraw, string Type)
    {
        /*
        if (Type == "Damage")
        {
            GameObject DrawTextObj = Instantiate(DropTextObj);
            GameObject DrawText = DrawTextObj.transform.Find("DropText").gameObject;
            GameObject DrawTextIcon = DrawTextObj.transform.Find("Icon").gameObject;
            DrawTextIcon.GetComponent<SpriteRenderer>().color = new Color(DrawTextIcon.GetComponent<SpriteRenderer>().color.r, DrawTextIcon.GetComponent<SpriteRenderer>().color.g, DrawTextIcon.GetComponent<SpriteRenderer>().color.b, 0f);

            //DrawTextArr.Add(DrawText);
            DrawText.GetComponent<TextMesh>().text = TextToDraw.ToString();
            DrawText.GetComponent<TextMesh>().color = Color.red;
            DrawTextObj.transform.position = new Vector3(transform.position.x, transform.position.y, -4);
            iTween.FadeTo(DrawText, 0f, 1f);
            Destroy(DrawTextObj, 1f);
        }
        else if (Type == "Scrap")
        {
            GameObject DrawTextObj = Instantiate(DropTextObj);
            GameObject DrawText = DrawTextObj.transform.Find("DropText").gameObject;
            GameObject DrawTextIcon = DrawTextObj.transform.Find("Icon").gameObject;

            //DrawTextArr.Add(DrawTextObj);
            DrawText.GetComponent<TextMesh>().text = TextToDraw.ToString();
            DrawText.GetComponent<TextMesh>().color = Color.green;
            DrawText.transform.position = new Vector3(transform.position.x, transform.position.y, -4);
            DrawTextIcon.transform.position = DrawText.transform.position + (transform.right * 0.2f);
            DrawTextIcon.transform.position = new Vector3(DrawTextIcon.transform.position.x, DrawTextIcon.transform.position.y, -4);
            iTween.FadeTo(DrawText, 0f, 1f);
            iTween.FadeTo(DrawTextObj, 0f, 1f);
            Destroy(DrawTextObj, 1f);
        }
        else if (Type == "Power")
        {
            GameObject DrawTextObj = Instantiate(DropTextObj);
            GameObject DrawText = DrawTextObj.transform.Find("DropText").gameObject;

            //DrawTextArr.Add(DrawText);
            DrawText.GetComponent<TextMesh>().text = TextToDraw.ToString();
            DrawText.GetComponent<TextMesh>().color = Color.blue;
            DrawTextObj.transform.position = new Vector3(transform.position.x, transform.position.y, -4);
            iTween.FadeTo(DrawText, 0f, 1f);
            Destroy(DrawTextObj, 1f);
        }
        */
    }

    void Die()
    {
        ScatterStuff();
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject, 1f);
    }

    void ScatterStuff()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject Drop = Instantiate(PowerDrop, transform.position, transform.rotation);
            Vector3 v = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.up;
            Drop.GetComponent<Rigidbody2D>().velocity = v * 2;
        }
    }
}

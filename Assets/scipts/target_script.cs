using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    [SerializeField]
    private int hp=5;
    public void hit(int dmg)
    {Debug.Log("hit registred!\n");
        hp-=dmg;
    if(hp<=0)
    death();
    }
    private void death()
    {Destroy(gameObject);}
}

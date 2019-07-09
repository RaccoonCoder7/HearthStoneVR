using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardState : MonoBehaviour
{
    public string cardName;
    public int cost;
    public int originHealth;
    public int originAttck;
    public string species;
    public bool canAttack = false;

    private int health;
    private int attack;

    private void Start()
    {
        health = originHealth;
        attack = originAttck;
    }

    public int HP
    {
        get { return health; }
        set
        {
            health = value;
        }
    }

    public int Attack
    {
        get { return attack; }
        set
        {
            attack = value;
        }
    }
}

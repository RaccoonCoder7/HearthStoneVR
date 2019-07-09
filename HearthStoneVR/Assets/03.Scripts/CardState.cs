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
    public int mainAudioClipNumber;

    private int health;
    private int attack;
    private AudioSource audioSource;
    private AudioClip attackClip;

    private void Start()
    {
        health = originHealth;
        attack = originAttck;
        GameObject.Find("GameMgr").GetComponent<GameManager>().ChngeMusic(mainAudioClipNumber, true);
        audioSource = GetComponent<AudioSource>();
        attackClip = Resources.Load("attack") as AudioClip;
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

    public void playAttackSound(){
        audioSource.clip = attackClip;
        audioSource.Play();
    }
}

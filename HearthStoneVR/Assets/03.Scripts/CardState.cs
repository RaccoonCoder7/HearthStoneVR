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
    private Vector3 origin;

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

    public void doAttack(Vector3 targetPos){
        StartCoroutine("playAttackSound");
        StartCoroutine(attackAnim(targetPos));
    }

    IEnumerator playAttackSound(){
        yield return new WaitForSeconds(0.5f);
        audioSource.clip = attackClip;
        audioSource.Play();
    }

    IEnumerator attackAnim(Vector3 targetPos){
        origin = transform.position;
        Hashtable ht = new Hashtable();
        ht.Add("time", 0.5f);
        ht.Add("position", targetPos);
        ht.Add("easetype", iTween.EaseType.easeInBack);
        iTween.MoveTo(gameObject, ht);
        yield return new WaitForSeconds(0.5f);
        Hashtable ht2 = new Hashtable();
        ht2.Add("time", 0.5f);
        ht2.Add("position", origin);
        ht2.Add("easetype", iTween.EaseType.easeOutCubic);
        iTween.MoveTo(gameObject, ht2);
    }
}

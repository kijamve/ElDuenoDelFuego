using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estalactita : MonoBehaviour
{
    public List<GameObject> parts;
    public GameObject player;
    public GameObject arm;
    public float dangerLevelToEnemy = 5.0f;
    public float distanceProjectionFactor;
    public AudioSource audioInAttack;
    public AudioSource audioBreak;
    public EstalactitaColliderDetector coliderDetector;


    public float life = 3.0f;
    private float dangerPlayerRatio = 1.0f;

    FlyCamera playersObj = null;
    bool destroyedByPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        dangerPlayerRatio = PlayerPrefs.GetFloat("dangerPlayerRatio");
        if (dangerPlayerRatio <= 0.1f || dangerPlayerRatio > 4.0f)
        {
            dangerPlayerRatio = 1.0f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!arm) { 
            return; 
        } else if(!playersObj)
        {
            playersObj = player.GetComponent<FlyCamera>();
        }
        float distance = arm.transform.position.x - gameObject.transform.position.x - distanceProjectionFactor;
        bool isForward = distance > 0;
        bool inAttack = false;
        if (playersObj != null)
        {
            inAttack = playersObj.playerBobo.inAttack || playersObj.playerTucusito.inAttack;
        }
        //Debug.Log("Position to Player: P: " + arm.transform.position.x + " - E: " + gameObject.transform.position.x);
        //Debug.Log("Distance to Player: " + distance + " - F: " + isForward + " - A: " + inAttack + " - D: " + destroyedByPlayer + " - L: " + life);
        if (!destroyedByPlayer && inAttack && isForward && distance < 6.0f && arm.transform.position.y > 12.5f)
        {
            audioInAttack.mute = false;
            life -= Time.deltaTime * dangerPlayerRatio;
            if (life < 0)
            {
                audioInAttack.mute = true;
                audioBreak.Play();
                destroyedByPlayer = true;
                foreach (GameObject part in parts)
                {
                    Rigidbody rb = part.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
            }
        }
        else
        {
            audioInAttack.mute = true;
        }
    }
}
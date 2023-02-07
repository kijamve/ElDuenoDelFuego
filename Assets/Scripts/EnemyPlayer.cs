using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class EnemyPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject initFireObject;
    public GameObject psFire;

    public GameObject avisoIdle;
    public GameObject avisoGanador;

    public GameObject estrellas;
    public GameObject candelitaGanadora;
    public GameObject ramita;

    public float dangerLevelToEnemy = 5.0f;

    public Rigidbody body;
    public float forceFireInverse;
    public Transform cameraTransform;

    public float life = 1.0f;

    private float fireCameraEffect;
    private Quaternion cameraRotationDefault;

    public Animator animator;

    public float walkMinDistance;
    public float walkMaxDistance;

    public float fireMinDistance;
    public float fireMaxDistance;
    public float walkVelocity;

    private int lastIdAtackEvent = 1;
    private float nextAttack = 2.0f;

    private bool inAttack = false;
    private bool inWalk = false;
    private bool walking = false;
    private bool dead = false;

    public AudioSource audioPlayerDead;
    public AudioSource audioAttack;
    public AudioSource audioAttackFire;
    public AudioSource audioMove;
    public AudioSource audioDead;
    public AudioSource audioIdle;

    // Start is called before the first frame update
    void Start()
    {
        cameraRotationDefault = cameraTransform.rotation;
    }
    private IEnumerator AtackBlend(bool withFire, float duration)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
        }
        if (withFire)
        {
            Vector3 dir = (player.transform.position - initFireObject.transform.position).normalized;
            body.AddForce(forceFireInverse * -dir, ForceMode.Impulse);
            GameObject fire = Instantiate(psFire, initFireObject.transform.position, Quaternion.identity);
            Rigidbody rgFire = fire.GetComponent<Rigidbody>();
            audioAttackFire.Play();
            rgFire.velocity = 10f * dir;
            Destroy(fire, 8.0f);
            cameraEffectFire(0.5f);
        }
        else
        {
            FlyCamera obj = player.GetComponent<FlyCamera>();
            bool playerDead = obj.dead;
            audioAttack.Play();
            float distance = Math.Abs(player.transform.position.x - gameObject.transform.position.x);
            Debug.Log("Distance Attack Caiman: " + distance);
            if (distance < 40.0f)
            {
                audioPlayerDead.Play();
                obj.addDanger(dangerLevelToEnemy);
            }
        }
        animator.SetBool("isAttack", false);
    }
    void initFire(bool withFire)
    {

        inAttack = true;

        animator.SetFloat("direction", 1.0f);

        lastIdAtackEvent = (lastIdAtackEvent + 1) % 10000;
        animator.SetBool("isAttack", true);
        animator.SetBool("isWalk", false);
        StartCoroutine(AtackBlend(withFire, 0.2f));
    }
    void initWalk(bool isForward)
    {

        inAttack = true;

        body.velocity = new Vector3(walkVelocity * (isForward ? 1 : -1), 0.0f, 0.0f);

        animator.SetBool("isAttack", false);
        animator.SetBool("isWalk", true);
    }
    void cameraEffectFire(float max)
    {
        fireCameraEffect = max;
    }
    private void FixedUpdate()
    {
        if(fireCameraEffect > .01f)
        {
            cameraTransform.rotation = cameraRotationDefault * Quaternion.Euler(
                    Random.Range(-fireCameraEffect, fireCameraEffect),
                    Random.Range(-fireCameraEffect, fireCameraEffect),
                    Random.Range(-fireCameraEffect, fireCameraEffect)
                );
            fireCameraEffect *= 0.9f;
        }
    }
    public void addDanger(float level)
    {
        life -= level;
    }
    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            return;
        }
        float distance = Vector3.Distance(player.transform.position, initFireObject.transform.position);
        bool isForward = (player.transform.position.x - initFireObject.transform.position.x)+1.5f > 0;
        bool playerDead = player.GetComponent<FlyCamera>().dead;

        nextAttack -= Time.deltaTime;

        if (!playerDead && nextAttack <= 0.0f) {
            if (distance > fireMinDistance && distance < fireMaxDistance)
            {
                initFire(true);
            } else if (isForward && distance <= fireMinDistance) {
                initFire(false);
            }
            float minTimeAttack = PlayerPrefs.GetFloat("minTimeAttack");
            float maxTimeAttack = PlayerPrefs.GetFloat("maxTimeAttack");
            if (minTimeAttack < 0.01f)
            {
                minTimeAttack = 2.0f;
            }
            if (minTimeAttack > maxTimeAttack || maxTimeAttack < 0.01f)
            {
                maxTimeAttack = minTimeAttack + 1.5f;
            }
            nextAttack = Random.Range(minTimeAttack, maxTimeAttack);
        }

        if (!isForward || distance > fireMaxDistance) {
            initWalk(isForward);
        } else {
            body.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            animator.SetBool("isWalk", false);
        }
        if (life < 0.01f)
        {
            dead = true;
            estrellas.SetActive(true);
            candelitaGanadora.SetActive(true);
            ramita.SetActive(true);
            avisoGanador.SetActive(true);
            avisoIdle.SetActive(false);
            audioIdle.Stop();
            audioDead.Play();
            animator.SetBool("isDead", true);
            animator.SetBool("isWalk", false);
            animator.SetBool("isAttack", false);
        }
    }

}

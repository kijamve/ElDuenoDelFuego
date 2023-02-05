using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstalactitaColliderDetector : MonoBehaviour
{
    public AudioSource audioDead;
    public AudioSource audioEnemyDead;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlaneA" || other.gameObject.name == "VitalColiderBody")
        {
            Debug.Log("Estalactita Collider: " + other.gameObject.name);
            audioDead.Play();
            Destroy(gameObject.transform.parent.gameObject, 2.5f);
        }
        if (other.gameObject.name == "VitalColiderBody")
        {
            audioEnemyDead.Play();
            Estalactita obj = gameObject.transform.parent.gameObject.GetComponent<Estalactita>();
            EnemyPlayer enemyObj = enemy.GetComponent<EnemyPlayer>();
            enemyObj.addDanger(obj.dangerLevelToEnemy);
        }
    }
}

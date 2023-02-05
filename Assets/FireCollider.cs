using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollider : MonoBehaviour
{
    public AudioSource audioDead;
    public float dangerLevelToEnemy = 5.0f;
    public bool isDanger = true;
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
        if (isDanger && other.gameObject.name == "PlayerBox")
        {
            Debug.Log("Fire Collider: " + other.gameObject.name);
            FlyCamera obj = other.gameObject.GetComponent<FlyCamera>();
            obj.addDanger(dangerLevelToEnemy);
            audioDead.Play();
        }
    }
}

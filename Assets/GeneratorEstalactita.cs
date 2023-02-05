using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorEstalactita : MonoBehaviour
{
    public List<GameObject> allEstalactitas;
    public GameObject player;
    public GameObject enemy;
    public GameObject arm;

    // Start is called before the first frame update
    void Start()
    {
        float init = 0.0f;
        while (init > -90.0f)
        {
            GameObject obj = Instantiate(allEstalactitas[Random.Range(0, allEstalactitas.Count)], new Vector3(init, 8.8f), Quaternion.identity);
            Estalactita o = obj.GetComponent<Estalactita>();
            o.player = player;
            o.arm = arm;
            o.coliderDetector.enemy = enemy;
            init -= Random.Range(10.0f, 15.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

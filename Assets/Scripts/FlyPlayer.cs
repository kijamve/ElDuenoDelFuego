using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FlyPlayer : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject ramita;
    public GameObject flama;
    public GameObject avisoGanador;
    public GameObject avisoSalir;
    public float fixXCameraPosition = 0.0f;
    public float fixYCameraPosition = 0.0f;
    public bool isDead = false;
    GameObject boxParent;
    Quaternion lastRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    Vector3 rotateInput = new Vector3(0.0f, 0.0f, 0.0f);

    private int lastIdEvent = 1;
    private int lastIdAtackEvent = 1;

    public bool inAttack = false;
    public Animator animator;
    bool withRamita = false;
    void Start()
    {
        boxParent = transform.parent.gameObject;
    }

    private IEnumerator Rotate(GameObject objectToRotate, Quaternion endRotation, float duration)
    {
        int currentIdEvent = lastIdEvent;
        Quaternion startRotation = lastRotation;
        for ( float t = 0 ; t < duration ; t+= Time.deltaTime )
        {
            if (lastIdEvent != currentIdEvent)
                break;
            lastRotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            objectToRotate.transform.rotation = boxParent.transform.rotation * lastRotation;
            yield return null;
        }
        if (lastIdEvent == currentIdEvent)
            objectToRotate.transform.rotation = boxParent.transform.rotation * endRotation;

    }
    private IEnumerator AtackBlend(Animator a, string name, float toValue, float duration)
    {
        int currentIdAtackEvent = lastIdAtackEvent;
        float currentPos = a.GetFloat(name);
        float diff = toValue - currentPos;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            if (lastIdAtackEvent != currentIdAtackEvent)
                break;

            a.SetFloat(name, currentPos + diff * (t / duration));

            yield return null;
        }
        if (lastIdAtackEvent == currentIdAtackEvent)
            a.SetFloat(name, toValue);
    }
    private IEnumerator BackToMain()
    {
        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            yield return null;
        }
        SceneManager.LoadScene("MainMenuScene");
    }
    void Update()
    {
        if (isDead)
        {
            return;
        }
        bool isChange = false;

        bool isA = Input.GetKeyDown(KeyCode.A);
        bool isUA = Input.GetKeyUp(KeyCode.A);
        bool isCA = Input.GetKey(KeyCode.A);
        bool isD = Input.GetKeyDown(KeyCode.D);
        bool isUD = Input.GetKeyUp(KeyCode.D);
        bool isCD = Input.GetKeyUp(KeyCode.D);
        mainCamera.transform.position = new Vector3(
            gameObject.transform.position.x + fixXCameraPosition,
            gameObject.transform.position.y * 0.3f + fixYCameraPosition,
            mainCamera.transform.position.z
        );
        if (isA) {
            isChange = true;
            if (isCD)
                rotateInput.z = 0;
            else
                rotateInput.z = 30;
        }
		if (isUA) {
            isChange = true;
            if (!isCD)
                rotateInput.z = 0;
        }
		if(isD) {
            isChange = true;
            if (isCA)
                rotateInput.z = 0;
            else
                rotateInput.z = -15;
        }
        if (isUD)
        {
            isChange = true;
            if (!isCA)
                rotateInput.z = 0;
        }
        if (withRamita)
        {
            ramita.transform.position = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y - 0.5f, 0.0f);
            flama.transform.position = new Vector3(gameObject.transform.position.x - 3.5f, gameObject.transform.position.y - 0.8f, 0.0f);
            if (gameObject.transform.position.x > 20)
            {
                StartCoroutine(BackToMain());
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float distance = Vector3.Distance(ramita.transform.position, gameObject.transform.position);
            Debug.Log("Distancia Ramita: " + distance);
            Debug.Log("Status Ramita: " + ramita.active);
            if (ramita.active && !withRamita)
            {
                if (distance < 10)
                {
                    avisoGanador.SetActive(false);
                    avisoSalir.SetActive(true);
                    withRamita = true;
                }
            } else
            {
                lastIdAtackEvent = (lastIdAtackEvent + 1) % 10000;
                StartCoroutine(AtackBlend(animator, "attack", 1.0f, 0.3f));
                inAttack = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (ramita.active)
            {

            }
            else
            {
                lastIdAtackEvent = (lastIdAtackEvent + 1) % 10000;
                StartCoroutine(AtackBlend(animator, "attack", 0.0f, 0.3f));
                inAttack = false;
            }
        }
        if (isChange)
        {
            lastIdEvent = (lastIdEvent + 1) % 10000;
            Quaternion toRotate = Quaternion.Euler(rotateInput);
            StartCoroutine( Rotate( gameObject, toRotate, 0.5f ) ) ;
        }
    }
}

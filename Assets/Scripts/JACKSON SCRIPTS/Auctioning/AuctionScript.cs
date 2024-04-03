using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AuctionScript : MonoBehaviour
{
    public GameObject botType;

    public float speed;
    public bool isWalking;
   
    public Animator botAnimator;

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public Quaternion lookBack;

    public GameObject spot1;
    public GameObject spot2;
    public GameObject spot3;

    [SerializeField] private GameObject number1;
    [SerializeField] private GameObject number2;
    [SerializeField] private GameObject number3;

    void Start()
    {
        lookBack = Quaternion.LookRotation(Vector3.back);
        isWalking = false;
       number1 = Instantiate(botType, new Vector3((float)9.516, (float)0.905, (float)1.8), Quaternion.LookRotation(Vector3.left) );
       number2 = Instantiate(botType, new Vector3((float)15, (float)0.905, (float)1.8), Quaternion.LookRotation(Vector3.left));
       number3 = Instantiate(botType, new Vector3((float)20, (float)0.905, (float)1.8), Quaternion.LookRotation(Vector3.left));
       number1.GetComponent<Animator>();
       number2.GetComponent<Animator>();
       number3.GetComponent<Animator>();

       StartCoroutine(whileWalking());
       isWalking = false;
    }
    public IEnumerator whileWalking()
    {
        while ((number1.transform.position != slot1.transform.position && number1.transform.rotation != lookBack) ||
               (number2.transform.position != slot2.transform.position && number2.transform.rotation != lookBack)||
               ((number3.transform.position != slot3.transform.position) && (number3.transform.rotation != lookBack )))
        {

            isWalking = true;
            yield return new WaitForSeconds(0.00001f);
            if (number1.transform.position != slot1.transform.position)
            {
                number1.GetComponent<Animator>().SetBool("IsWalking", true);
                number1.transform.position = Vector3.MoveTowards(number1.transform.position, slot1.transform.position, speed);
                spot1.transform.LookAt(number1.transform);
            }
            else
            {
                number1.GetComponent<Animator>().SetBool("IsWalking", false);
                number1.transform.rotation = Quaternion.Lerp(number1.transform.rotation,lookBack, speed);
            }
            if (number2.transform.position != slot2.transform.position)
            {
                number2.GetComponent<Animator>().SetBool("IsWalking", true);
                number2.transform.position = Vector3.MoveTowards(number2.transform.position, slot2.transform.position, speed);
                spot2.transform.LookAt(number2.transform);
            }
            else
            {
                number2.GetComponent<Animator>().SetBool("IsWalking", false);
                number2.transform.rotation = Quaternion.Lerp(number2.transform.rotation,lookBack, speed);
            }
            if (number3.transform.position != slot3.transform.position)
            {
                number3.GetComponent<Animator>().SetBool("IsWalking", true);
                number3.transform.position = Vector3.MoveTowards(number3.transform.position, slot3.transform.position, speed);
                spot3.transform.LookAt(number3.transform);
            }
            else
            {
                number3.GetComponent<Animator>().SetBool("IsWalking", false);
                number3.transform.rotation = Quaternion.Lerp(number3.transform.rotation,lookBack, speed);
            }
        }

        StartCoroutine(finishTurning());


    }

    private IEnumerator finishTurning()
    {
        while (number3.transform.rotation != lookBack)
        {
            yield return new WaitForSeconds(0.00001f);
            number3.GetComponent<Animator>().SetBool("IsWalking", false);
            number3.transform.rotation = Quaternion.Lerp(number3.transform.rotation,lookBack, speed);
        }
    }

    private void robotSpawner()
    {

    }

    void Update()
    {






    }
}

using System.Collections;
using System.Collections.Generic;
using SaveLoad;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using static Unity.Mathematics.math;
using Random = UnityEngine.Random;

public class AuctionScript : MonoBehaviour
{
    public float speed;

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    
    public GameObject end1;
    public GameObject end2;
    public GameObject end3;

    public GameObject back2;
    
    public Quaternion lookBack;

    public GameObject spot1;
    public GameObject spot2;
    public GameObject spot3;

    public TextMeshProUGUI money;

    [SerializeField] private GameObject number1;
    [SerializeField] private GameObject number2;
    [SerializeField] private GameObject number3;
    
    [SerializeField] private Animator popup1;
    [SerializeField] private Animator popup2;
    [SerializeField] private Animator popup3;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsBackWalking = Animator.StringToHash("IsBackWalking");
    private static readonly int Type1 = Animator.StringToHash("Type1");
    private static readonly int Type2 = Animator.StringToHash("Type2");
    private static readonly int Type3 = Animator.StringToHash("Type3");

    void Start()
    {
        money.text = "$" + CurrentData.Instance.uiData.saveMoney;
        RobotManager manager = FindObjectOfType<RobotManager>();
       float random1 = Random.Range(0f, 100f);
       float random2 = Random.Range(0f, 100f); 
       float random3 = Random.Range(0f, 100f);
       number1 = Instantiate(Robot(random1), new Vector3((float)9.516, (float)0.905, (float)1.8), Quaternion.LookRotation(Vector3.left) );
       number2 = Instantiate(Robot(random2), new Vector3((float)15, (float)0.905, (float)1.8), Quaternion.LookRotation(Vector3.left));
       number3 = Instantiate(Robot(random3), new Vector3((float)20, (float)0.905, (float)1.8), Quaternion.LookRotation(Vector3.left));
       
       RunSpotLightColorsAndPopupAnimVar();
       
       lookBack = Quaternion.LookRotation(Vector3.back);
       UpdatePopupData(popup1, manager.auctionRobots[0]);
       UpdatePopupData(popup2, manager.auctionRobots[1]);
       UpdatePopupData(popup3, manager.auctionRobots[2]);
       StartCoroutine(Stage1Walk(number1));
       StartCoroutine(Stage1Walk(number2));
       StartCoroutine(Stage1Walk(number3));
       
       Debug.Log("Initiated Walking");
    }
    

    private void RunSpotLightColorsAndPopupAnimVar()
    {
        RobotManager manager = FindObjectOfType<RobotManager>();
        for (int i = 0; i < 3; i++)
       {
           if (i == 0)
           {
               if (manager.auctionRobots[i].robotType == "basic")
               {
                   spot1.gameObject.GetComponent<Light>().color = Color.grey;
                   popup1.SetInteger(Type1, 0);
               }
               if (manager.auctionRobots[i].robotType == "silver")
               {
                   spot1.gameObject.GetComponent<Light>().color = (Color.white);
                   popup1.SetInteger(Type1, 1);
               }
               if (manager.auctionRobots[i].robotType == "gold")
               {
                   spot1.gameObject.GetComponent<Light>().color = (Color.yellow);
                   popup1.SetInteger(Type1, 2);
               }
               if (manager.auctionRobots[i].robotType == "diamond")
               {
                   spot1.gameObject.GetComponent<Light>().color = (Color.blue);
                   popup1.SetInteger(Type1, 2);
               }
               if (manager.auctionRobots[i].robotType == "ultra")
               {
                   spot1.gameObject.GetComponent<Light>().color = Color.red;
                   popup1.SetInteger(Type1, 3);
               }
               Debug.Log(i + " " + spot1.GetComponent<Light>().color);
           } else if (i == 1)
           {
               if (manager.auctionRobots[i].robotType == "basic")
               {
                   spot2.gameObject.GetComponent<Light>().color = (Color.grey);
                   popup2.SetInteger(Type2, 0);
               }
               if (manager.auctionRobots[i].robotType == "silver")
               {
                   spot2.gameObject.GetComponent<Light>().color = (Color.white);
                   popup2.SetInteger(Type2, 1);
               }
               if (manager.auctionRobots[i].robotType == "gold")
               {
                   spot2.gameObject.GetComponent<Light>().color = (Color.yellow);
                   popup2.SetInteger(Type2, 2);
               }
               if (manager.auctionRobots[i].robotType == "diamond")
               {
                   spot2.gameObject.GetComponent<Light>().color = (Color.blue);
                   popup2.SetInteger(Type2, 2);
               }
               if (manager.auctionRobots[i].robotType == "ultra")
               {
                   spot2.gameObject.GetComponent<Light>().color = (Color.red);
                   popup2.SetInteger(Type2, 3);
               }
               Debug.Log(i + " " + spot2.GetComponent<Light>().color);
           } else if (i == 2)
           {
               if (manager.auctionRobots[i].robotType == "basic")
               {
                   spot3.gameObject.GetComponent<Light>().color = (Color.grey);
                   popup3.SetInteger(Type3, 0);
               }
               if (manager.auctionRobots[i].robotType == "silver")
               {
                   spot3.gameObject.GetComponent<Light>().color = (Color.white);
                   popup3.SetInteger(Type3, 1);
               }
               if (manager.auctionRobots[i].robotType == "gold")
               {
                   spot3.gameObject.GetComponent<Light>().color = (Color.yellow);
                   popup3.SetInteger(Type3, 2);
               }
               if (manager.auctionRobots[i].robotType == "diamond")
               {
                   spot3.gameObject.GetComponent<Light>().color = (Color.blue);
                   popup3.SetInteger(Type3, 2);
               }
               if (manager.auctionRobots[i].robotType == "ultra")
               {
                   spot3.gameObject.GetComponent<Light>().color = (Color.red);
                   popup3.SetInteger(Type3, 3);
               }
               Debug.Log(i + " " + spot3.GetComponent<Light>().color);
           }
           else
           {
               Debug.Log("No Light");
           }
           
       }
    }

    #region Robot Movement
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Stage1Walk(GameObject robot)
    {
        GameObject thisSlot = null;
        GameObject thisLight = null;
        if (robot == number1)
        {
            thisSlot = slot1;
            thisLight = spot1;
        }
        else if (robot == number2)
        {
            thisSlot = slot2;
            thisLight = spot2;
        } else if (robot == number3)
        {
            thisSlot = slot3;
            thisLight = spot3;
        }
        while (thisSlot != null && robot.transform.position != thisSlot.transform.position)
        {
            
            yield return new WaitForSeconds(0.00001f);
            if (robot.transform.position != thisSlot.transform.position)
            {
                robot.GetComponent<Animator>().SetBool(IsWalking, true);
                robot.transform.position = Vector3.MoveTowards(robot.transform.position, thisSlot.transform.position, speed);
                thisLight.transform.LookAt(robot.transform);
            }
        }
        robot.GetComponent<Animator>().SetBool(IsWalking, false);
        if (robot == number3)
        {
            Debug.Log("3rd end walking");
            StartCoroutine(Stage2Rotate());
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Stage2Rotate()
    {
        while (number1.transform.rotation != lookBack)
        {
            yield return new WaitForSeconds(0.00001f);
            number1.GetComponent<Animator>().SetBool(IsWalking, false);
            number1.transform.rotation = Quaternion.RotateTowards(number1.transform.rotation,lookBack, speed*25);
            number2.GetComponent<Animator>().SetBool(IsWalking, false);
            number2.transform.rotation = Quaternion.RotateTowards(number2.transform.rotation,lookBack, speed*25);
            number3.GetComponent<Animator>().SetBool(IsWalking, false);
            number3.transform.rotation = Quaternion.RotateTowards(number3.transform.rotation,lookBack, speed*25);
        } 
        Debug.Log("Finished Rotating");
        StartCoroutine(Stage3Forward());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Stage3Forward()
    {
        Debug.Log("Move Forward");
        while (number1.transform.position != end1.transform.position ||
               number2.transform.position != end2.transform.position ||
               number3.transform.position != end3.transform.position)
        {
            yield return new WaitForSeconds(0.00001f);
            if (number1.transform.position != end1.transform.position)
            {
                number1.GetComponent<Animator>().SetBool(IsWalking, true);
                number1.transform.position = Vector3.MoveTowards(number1.transform.position, end1.transform.position, speed);
                spot1.transform.LookAt(number1.transform);
            }
            if (number2.transform.position != end2.transform.position)
            {
                number2.GetComponent<Animator>().SetBool(IsWalking, true);
                number2.transform.position = Vector3.MoveTowards(number2.transform.position, end2.transform.position, speed);
                spot2.transform.LookAt(number2.transform);
            }
            if (number3.transform.position != end3.transform.position)
            {
                number3.GetComponent<Animator>().SetBool(IsWalking, true);
                number3.transform.position = Vector3.MoveTowards(number3.transform.position, end3.transform.position, speed);
                spot3.transform.LookAt(number3.transform);
            }
        }
        number2.GetComponent<Animator>().SetBool(IsWalking, false);
        StartCoroutine(Stage4RotateLeft());
        StartCoroutine(Stage4RotateRight());
        StartCoroutine(Stage4StepBack());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Stage4StepBack()
    {
        
        while (number2.transform.position != back2.transform.position)
        {
            yield return new WaitForSeconds(0.00001f);
            number2.GetComponent<Animator>().SetBool(IsBackWalking, true);
            number2.transform.position = Vector3.MoveTowards(number2.transform.position,back2.transform.position, speed/5);
        }
        number2.GetComponent<Animator>().SetBool(IsBackWalking, false);
        yield return new WaitForSeconds(.5f);
        popup2.SetBool("Popup", true);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Stage4RotateLeft()
    {
        GameObject o = FindObjectOfType<Camera>().gameObject;
        GameObject empty = new GameObject
        {
            transform =
            {
                position = number1.transform.position
            }
        };
        empty.transform.LookAt(o.transform);
        Quaternion angle = empty.transform.rotation;
        Destroy(empty);
        while (number1.transform.rotation != angle)
        {
            yield return new WaitForSeconds(0.00001f);
            number1.GetComponent<Animator>().SetBool(IsWalking, false);
            number1.transform.rotation = Quaternion.RotateTowards(number1.transform.rotation,angle, speed*25);
        }
        popup1.SetBool("Popup", true);
    }
    private IEnumerator Stage4RotateRight()
    {
        GameObject o = FindObjectOfType<Camera>().gameObject;
        GameObject empty = new GameObject
        {
            transform =
            {
                position = number3.transform.position
            }
        };
        empty.transform.LookAt(o.transform);
        Quaternion angle = empty.transform.rotation;
        Destroy(empty);
        while (number3.transform.rotation != angle)
        {
            yield return new WaitForSeconds(0.00001f);
            number3.GetComponent<Animator>().SetBool(IsWalking, false);
            number3.transform.rotation = Quaternion.RotateTowards(number3.transform.rotation,angle, speed*25);
        } 
        yield return new WaitForSeconds(1f);
        popup3.SetBool("Popup", true);
    }

    #endregion
    public void UpdatePopupData(Animator currentAnim, RobotAttributes currentBot)
    {
        Transform option = currentAnim.transform.Find("FORE");
        TextMeshProUGUI typeText = option.Find("TYPE")?.GetComponent<TextMeshProUGUI>();
        if (typeText != null)
            typeText.text = currentBot.robotType;
        TextMeshProUGUI sizeText = option.Find("SIZE")?.GetComponent<TextMeshProUGUI>();
        if (sizeText != null)
            sizeText.text = currentBot.size;
        TextMeshProUGUI cpsText = option.Find("CPS")?.GetComponent<TextMeshProUGUI>();
        if (cpsText != null)
            cpsText.text = (round(currentBot.cps*10))/10  + " CPS";
        TextMeshProUGUI priceText = option.Find("PRICE")?.GetComponent<TextMeshProUGUI>();
        if (priceText != null)
            priceText.text = "$" + (currentBot.price);
        Image image = option.GetComponent<Image>();
        Debug.Log(image);
        if (image != null)
            image.color = currentBot.robotType == "basic" ? Color.grey :
                currentBot.robotType == "silver" ? Color.white :
                currentBot.robotType == "gold" ? Color.yellow :
                currentBot.robotType == "diamond" ? Color.blue :
                currentBot.robotType == "ultra" ? Color.red :
                Color.black;
    }

    public bool IsPopupActive()
    {
        return (popup1.GetBool("Popup") && popup2.GetBool("Popup") && popup3.GetBool("Popup"));
    }
    
    #region Robot Creation
    private GameObject Robot(float random)
    {
        RobotManager manager = FindObjectOfType<RobotManager>();
        if (random >= 0 & random < 70)
        {
            RobotAttributes thisAttribute = BasicRobot();
            manager.auctionRobots.Add(thisAttribute);
            if (thisAttribute.size == "Normal")
            {
                return manager.basicNormalACPrefab;
            } if (thisAttribute.size == "Scrawny")
            {
                return manager.basicScrawnyACPrefab;
            }
        }
        else if (random >= 70 & random < 85)
        {
            RobotAttributes thisAttribute = SilverRobot();
            manager.auctionRobots.Add(thisAttribute);
            if (thisAttribute.size == "Normal")
            {
                return manager.silverNormalACPrefab;
            } if (thisAttribute.size == "Tall")
            {
                return manager.silverTallACPrefab;
            }
        }
        else if (random >= 85 & random < 92.5f)
        {
            RobotAttributes thisAttribute = GoldRobot();
            manager.auctionRobots.Add(thisAttribute);
            if (thisAttribute.size == "Built")
            {
                return manager.goldBuiltACPrefab;
            } if (thisAttribute.size == "Tall")
            {
                return manager.goldTallACPrefab;
            }
        }
        else if (random >= 92.5f & random < 98)
        {
            RobotAttributes thisAttribute = DiamondRobot();
            manager.auctionRobots.Add(thisAttribute);
            if (thisAttribute.size == "Built")
            {
                return manager.diamondBuiltACPrefab;
            } if (thisAttribute.size == "Massive")
            {
                return manager.diamondMassiveACPrefab;
            }
        }
        else if (random >= 98 & random <= 100)
        {
            RobotAttributes thisAttribute = UltraBot();
            manager.auctionRobots.Add(thisAttribute);
            if (thisAttribute.size == "Massive")
            {
                return manager.ultraMassiveACPrefab;
            } if (thisAttribute.size == "Tank")
            {
                return manager.ultraTankACPrefab;
            }
        }

        return null;
    }
    
    

    private RobotAttributes BasicRobot()
    {
        RobotAttributes newRobotAttributes = new RobotAttributes();
        newRobotAttributes.cps = Random.Range(1f, 10f);
        newRobotAttributes.price = (int)(newRobotAttributes.cps * Random.Range(100, 200));
        newRobotAttributes.quickSellPrice = newRobotAttributes.price/10;
        newRobotAttributes.robotType = "basic";
        newRobotAttributes.size = newRobotAttributes.cps <= 5 ? "Scrawny" : "Normal";
        newRobotAttributes.ID = Random.Range(0, 999999);
        return newRobotAttributes;
    }

    private RobotAttributes SilverRobot()
    {
        RobotAttributes newRobotAttributes = new RobotAttributes();
        newRobotAttributes.cps = Random.Range(10f, 35f);
        newRobotAttributes.price = (int)(newRobotAttributes.cps * Random.Range(100, 200));
        newRobotAttributes.quickSellPrice = newRobotAttributes.price/10;
        newRobotAttributes.robotType = "silver";
        newRobotAttributes.size = newRobotAttributes.cps <= 22.5f ? "Normal" : "Tall";
        newRobotAttributes.ID = Random.Range(0, 999999);
        return newRobotAttributes;
    }

    private RobotAttributes GoldRobot()
    {
        RobotAttributes newRobotAttributes = new RobotAttributes();
        newRobotAttributes.cps = Random.Range(35f, 75f);
        newRobotAttributes.price = (int)(newRobotAttributes.cps * Random.Range(200, 300));
        newRobotAttributes.quickSellPrice = newRobotAttributes.price/10;
        newRobotAttributes.robotType = "gold";
        newRobotAttributes.size = newRobotAttributes.cps <= 55 ? "Tall" : "Built";
        newRobotAttributes.ID = Random.Range(0, 999999);
        return newRobotAttributes;
    }

    private RobotAttributes DiamondRobot()
    {
        RobotAttributes newRobotAttributes = new RobotAttributes();
        newRobotAttributes.cps = Random.Range(75f, 175f);
        newRobotAttributes.price = (int)(newRobotAttributes.cps * Random.Range(300, 400));
        newRobotAttributes.quickSellPrice = newRobotAttributes.price/10;
        newRobotAttributes.robotType = "diamond";
        newRobotAttributes.size = newRobotAttributes.cps <= 125 ? "Built" : "Massive";
        newRobotAttributes.ID = Random.Range(0, 999999);
        return newRobotAttributes;
    }

    private RobotAttributes UltraBot()
    {
        RobotAttributes newRobotAttributes = new RobotAttributes();
        newRobotAttributes.cps = Random.Range(175f, 500f);
        newRobotAttributes.price = (int)(newRobotAttributes.cps * Random.Range(500, 1999));
        newRobotAttributes.quickSellPrice = newRobotAttributes.price/10;
        newRobotAttributes.robotType = "ultra";
        newRobotAttributes.size = newRobotAttributes.cps <= 350 ? "Massive" : "Tank";
        newRobotAttributes.ID = Random.Range(0, 999999);
        return newRobotAttributes;
    }
    

    #endregion
}

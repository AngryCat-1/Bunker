using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GuideController : MonoBehaviour
{
    public GameObject instructionPanel;
    public GameObject backgroundPanel;
    public Text instructionText;
    public EventTrigger[] eventTriggers;
    public GameObject[] arms;
    public int triggerIndexG;

    public Transform setTr;
    private Transform saveTr;

    public LayerMask lm;

    public Item Lom;

    
  
  

    private string key;



    private void Start()
    {

    }

    public void ShowInstruction(int triggerIndex, string instruction, int time_scale, string keY)
    {
       

        foreach (EventTrigger trigger in eventTriggers)
        {
            trigger.gameObject.SetActive(false);
        }


        instructionText.text = instruction;


        instructionPanel.SetActive(true);


        eventTriggers[triggerIndex].transform.SetAsLastSibling();

        eventTriggers[triggerIndex].gameObject.SetActive(true);

        arms[triggerIndex].SetActive(true);

        saveTr = eventTriggers[triggerIndex].transform.parent;
        eventTriggers[triggerIndex].transform.SetParent(setTr);

        triggerIndexG = triggerIndex;

        //  GetComponent<PlayerMovement>().enabled = false;

        key = keY;

        Time.timeScale = time_scale;

        eventTriggers[4].gameObject.SetActive(false);
    }

    void HideInstruction()
    {
        if (arms[triggerIndexG]!=null)
        {
            print("HideInstruction");
            Time.timeScale = 1;
            instructionPanel.SetActive(false);

            eventTriggers[triggerIndexG].transform.SetParent(saveTr);


            arms[triggerIndexG].SetActive(false);

            print("ttt");

            foreach (EventTrigger trigger in eventTriggers)
            {
                trigger.gameObject.SetActive(true);
            }
            CheckKey();
            //  GetComponent<PlayerMovement>().enabled = true;
            eventTriggers[4].gameObject.SetActive(true);

            
        }

    }

    public void OnButtonClick()
    {
        HideInstruction();
    }

   

    void CheckKey()
    {
        if (key == "aSItwirdInstr")
        {
            key = "";
            GetComponent<WeaponManager>().SetIndex(0);
            GetComponent<WeaponManager>().UpdateWeapons();
            ShowInstruction(7, "Чтобы сломать дверь - нажмите эту кнопку", 1, "");
        }
        if (key == "act_1_patrolBring_AfterEndZombieBoss")
        {
            key = "";
            FindObjectOfType<MissionController>().CustomCreateMissionOnStep("Взять патроны в доме", "houseBring_bullets");
        }
    }
}

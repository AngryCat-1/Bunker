using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM;

public class InteractionSystemv2 : MonoBehaviour
{
    public Item NeedItem;
    public string key;
    public bool needInv = true;
    public WeaponManager wm;
    public MissionController mc;
    public Inventory iv;

    bool isComplete;

   


    public void Act()
    {
        print("isv2");
        if (key == "house_first_act_move" && !isComplete)
        {
            mc.CompleteMissionById("house_find_1");
            FindObjectOfType<GuideController>().ShowInstruction(8, "Осторожно! В доме есть зомби. Просто наведитесь на зомби и оружие будет стрелять само", 4, "");
            FindObjectOfType<MissionController>().CustomCreateMissionOnStep("Убить всех зомби в доме", "CustomCreateMission_Act1_needToDieZombies");
            isComplete = true;

        }
        if (key == "engine1_first_act_move" && iv.items.Contains(NeedItem) && !isComplete)
        {
            mc.CompleteMissionById("motor_give11");

            for (int i = 0; i < iv.items.Count; i++)
            {
                if (iv.items[i] == NeedItem)
                {
                    iv.items[i] = null;
                    wm.UpdateWeapons();
                    iv.UpdateCells();
                   
                }
            }

            isComplete = true;
        }
        if (key == "Act1_end" && !isComplete)
        {
            FindObjectOfType<VariablesManager>().act_index = 2;
            Time.timeScale = 0;
            FindObjectOfType<VariablesManager>().act1.EndCanvas.SetActive(true);
        }
        if (key == "house_door_to_battery" && !isComplete && iv.items.Contains(NeedItem))
        {
            Destroy(GameObject.Find("house_door_to_battery_obj_a"));

            for (int i = 0; i < iv.items.Count; i++)
            {
                if (iv.items[i] == NeedItem)
                {
                    iv.items[i] = null;
                    wm.UpdateWeapons();
                    iv.UpdateCells();

                }
            }

            isComplete = true;

        }
      
    }
}

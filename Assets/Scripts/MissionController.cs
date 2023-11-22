using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Text;
using System;
using HUDIndicator;

public class MissionController : MonoBehaviour
{
    public List<Step> steps;
    public Text txt;
    public int step_index;
    public bool needToDisplay = true;
    bool isStart;

    private VM.VariablesManager vm;
    private GuideController gc;
    private PlayerMovement pm;


    private void Start()
    {
        vm = FindObjectOfType<VM.VariablesManager>();
        gc = FindObjectOfType<GuideController>();
        pm = FindObjectOfType<PlayerMovement>();
    }

    public void CompleteMissionById(string id)
    {
       
        foreach (Mission mission in steps[step_index].mises)
        {
            if (mission.id == id)
            {
                mission.Complete = true;
            }
        }

    }

    public static T[] AddToArray<T>(T[] array, T item)
    {
        if (array == null)
        {
            // Если входной массив пустой, создаем новый массив из одного элемента
            return new T[] { item };
        }
        else
        {
            // Создаем новый массив, увеличивая его размер на 1 и добавляя элемент в конец
            T[] newArray = new T[array.Length + 1];
            Array.Copy(array, newArray, array.Length);
            newArray[array.Length] = item;
            return newArray;
        }
    }

    public bool AreAllMissionsCompleted()
    {
        Step currentStep = steps[step_index];

        foreach (Mission mission in currentStep.mises)
        {
            if (!mission.Complete)
            {
                return false; // Если хотя бы одна обычная миссия в текущем шаге не выполнена, вернуть false
            }
        }

        return true; // Все обычные миссии в текущем шаге выполнены
    }


    public void DisplayMissions()
    {
        if (step_index >= 0 && step_index < steps.Count && needToDisplay)
        {
            Step currentStep = steps[step_index];
            StringBuilder missionText = new StringBuilder();

            // Отобразить Main_mises с более крупным шрифтом
            missionText.AppendFormat("<size=50>{0}</size>\n\n", currentStep.Main_mises.textl);

            // Отобразить остальные миссии
            foreach (Mission mission in currentStep.mises)
            {
                string missionColor = mission.Complete ? "cyan" : "white"; // Зеленый цвет для выполненных миссий
                missionText.AppendFormat("<color={0}>{1}</color>\n", missionColor, mission.textl);
            }

            // Установить текст в Text компонент
            txt.text = missionText.ToString();
        }
        else
        {
            txt.text = "";
        }
    }


    private void Update()
    {
        CheckStep();
        DisplayMissions();

        if (AreAllMissionsCompleted())
        {
            
            StartCoroutine(step_end());
            isStart = true;
        }

        if (Array.TrueForAll(vm.act1.needToDieZombies, element => element == null) && !vm.act1.needToDieZombiesBoolVarIsComplete)
        {
            vm.act1.needToDieZombiesBoolVarIsComplete = true;
            CompleteMissionById("CustomCreateMission_Act1_needToDieZombies");
            CustomCreateMissionOnStep("Убейте зомби-босса ломом", "zombieBossKill");
            vm.act1.ZombieBoss.SetActive(true);
            vm.act1.isNeedRendererBullets = true;
            gc.ShowInstruction(8, "Ой, у вас закончились патроны. Убейте Зомби босса ломом и возьмите патроны", 1, "act_1_patrolBring_AfterEndZombieBoss");
            pm.patrols = 0;
        }
        if (vm.act1.ZombieBoss == null)
        {
            CompleteMissionById("zombieBossKill");
        }
        
    }

    public void CustomCreateMissionOnStep(string text, string id)
    {
        var newMisConfig = new Mission();
        newMisConfig.id = id;
        newMisConfig.textl = text;
        steps[step_index].mises.Add(newMisConfig);
    }


    IEnumerator step_end()
    {
        if (!isStart)
        {
            steps[step_index].Main_mises.Complete = true;
            yield return new WaitForSeconds(2);
            step_index++;
            yield return new WaitForSeconds(1);
            isStart = false;
            CheckStep();
        }
       
    }

    


    void CheckStep()
    {
        if (steps[step_index].key == "LineToHouse")
        {
            if (GameObject.Find("SM_Prop_CarBattery_01_item")!=null)
            {

                GameObject.Find("SM_Bld_House_02_motor").GetComponent<IndicatorOffScreen>().visible = true;
                GameObject.Find("SM_Bld_House_02_motor").GetComponent<IndicatorOnScreen>().visible = true;

            }
          
        }
        if (steps[step_index].key == "EngineWalk")
        {
            GameObject.Find("SM_Bld_House_02_motor").GetComponent<IndicatorOffScreen>().visible = false;
            GameObject.Find("SM_Bld_House_02_motor").GetComponent<IndicatorOnScreen>().visible = false;
            GameObject.Find("CarSM_Veh_NewsVan_01").GetComponent<IndicatorOffScreen>().visible = false;
            GameObject.Find("CarSM_Veh_NewsVan_01").GetComponent<IndicatorOnScreen>().visible = false;
            FindObjectOfType<VM.VariablesManager>().act1.canSitCar = true;
        }
    }
}


[System.Serializable]
public class Step
{
    public Mission Main_mises;
    public List<Mission> mises;
    public string key;
}

[System.Serializable] 
public class Mission
{
    public string textl;
    public string id;
    public bool Complete;
    
}
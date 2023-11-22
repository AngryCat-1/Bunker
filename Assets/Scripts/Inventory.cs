using HUDIndicator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public EventTrigger et_button_bring;

    public LayerMask collisionLayer;
    public LayerMask enemyLayer;
    public float raycastDistance = 8f;
    public string itm_et;
    public GameObject bring_gn;

    public Cell[] cells;
    public WeaponManager wm;
    public PlayerMovement pm;

    public bool isAutomat;

    private Camera mainCamera;
    private PlayerHealth playerHealth;
    private VM.VariablesManager variablesManager;
    private GuideController guideController;

    private void Start()
    {
        mainCamera = Camera.main;
        playerHealth = GetComponent<PlayerHealth>();
        variablesManager = FindObjectOfType<VM.VariablesManager>();
        guideController = GetComponent<GuideController>();
        StartCoroutine(ienShoot());
    }

    private void Update()
    {
        if (items.Count <= 3 && playerHealth.currentHealth > 0)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance, collisionLayer))
            {
                GameObject hitObject = hit.collider.gameObject;
                et_button_bring.gameObject.SetActive(true);
                itm_et = hitObject.name;
                bring_gn = hitObject;

                if (!variablesManager.act1.isFirstLomBring)
                {
                    guideController.ShowInstruction(5, "Эта кнопка нужна для подбора предметов. Нажмите ее и возьмите лом", 1, "aSItwirdInstr");
                    variablesManager.act1.isFirstLomBring = true;
                }
            }
            else
            {
                et_button_bring.gameObject.SetActive(false);
                itm_et = "";
            }
        }
    }
  

    public IEnumerator ienShoot()
    {
        while (true)
        {
            if (playerHealth.currentHealth > 0)
            {
                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                Ray ray = mainCamera.ScreenPointToRay(screenCenter);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, enemyLayer) && items[wm.weapon_index] != null && items[wm.weapon_index].isWeapon)
                {
                    if (!items[wm.weapon_index].ws.isMelleeWeapon)
                    {
                        print("shot");
                        pm.ShootBullets();
                        isAutomat = true;
                    }

                    if (items[wm.weapon_index].ws.isMelleeWeapon && Physics.Raycast(ray, out hit, 2, enemyLayer))
                    {
                        print("shotMelle");
                        pm.ShootBullets();
                        isAutomat = true;
                    }
                }
                else if (items[wm.weapon_index] != null && items[wm.weapon_index].isWeapon)
                {
                    wm.shot.SetActive(true);
                    isAutomat = false;
                }
            }
            yield return new WaitForSeconds(pm.cooldown);
        }
    }

    public void Brings()
    {
        var g = Resources.Load(itm_et) as GameObject;
        var v = g.GetComponent<Item>();
        int ind = -1;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = v;
                ind = i;

                FindObjectOfType<AudioManager>().bringItem.Play();

                if (v.isWriter)
                {
                    wm.WriterObj.SetActive(true);
                    wm.WriterObj.GetComponentInChildren<Text>().text = v.wr.text;
                }

                Destroy(bring_gn);
                break;
            }
        }

        UpdateCells();
        wm.UpdateWeapons();

        if (v.isWriter)
        {
            wm.WriterObj.SetActive(true);
            wm.WriterObj.GetComponentInChildren<Text>().text = v.wr.text;
        }

        if (g.name == "SM_Wep_Revolver_01_item")
        {
            FindObjectOfType<MissionController>().CompleteMissionById("revolver_find_1");
        }
        if (g.name == "Res_item_writer_id1_item")
        {
            FindObjectOfType<MissionController>().CompleteMissionById("plane_give_1");
        }
        if (g.name == "SM_Prop_CarBattery_01_item")
        {
            FindObjectOfType<MissionController>().CompleteMissionById("motor_find_1");
            GameObject.Find("CarSM_Veh_NewsVan_01").GetComponent<IndicatorOffScreen>().visible = true;
            GameObject.Find("CarSM_Veh_NewsVan_01").GetComponent<IndicatorOnScreen>().visible = true;
        }
        if (g.name == "SM_Wep_Crowbar_01_item")
        {
            FindObjectOfType<MissionController>().CompleteMissionById("lom_bring_1");
        }
    }

    public void UpdateCells()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                cells[i].graf.gameObject.SetActive(true);
                cells[i].graf.sprite = Sprite.Create(items[i].img, new Rect(0, 0, items[i].img.width, items[i].img.height), Vector2.zero);
            }
            else
            {
                cells[i].graf.gameObject.SetActive(false);
            }
        }
    }
}

[System.Serializable]
public class Cell
{
    public Image graf;
}

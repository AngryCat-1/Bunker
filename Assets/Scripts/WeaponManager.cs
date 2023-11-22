using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponManager : MonoBehaviour
{
    public int weapon_index;
    public Inventory inv;
    public GameObject shot;
    public Transform put_away_obj;
    public GameObject put_away_but;

    public List<GameObject> can_active_items;
    public List<GameObject> physics_obj;

    public PlayerMovement pm;
    public GameObject WriterObj;

    private void Start()
    {
        UpdateWeapons();
    }

    // Устанавливает текущий индекс оружия
    public void SetIndex(int index)
    {
        weapon_index = index;
        UpdateWeapons();
        TryActivateWriter();
    }

    // Обновляет состояние оружия
    public void UpdateWeapons()
    {
        foreach (var item in inv.items)
        {
            if (item != null && item.find_object_my != "")
            {
                TrySetActiveItem(item.find_object_my, false);
                shot.SetActive(false);
            }
        }

        TrySetActiveItem(inv.items[weapon_index]?.find_object_my, true);
    }

    private void TrySetActiveItem(string itemName, bool setActive)
    {
        var itemToActivate = can_active_items.FirstOrDefault(go => go.name == itemName);
        if (itemToActivate != null)
        {
            itemToActivate.SetActive(setActive);
        }
    }

    private void TryActivateWriter()
    {
        if (inv.items[weapon_index]?.isWriter == true)
        {
            WriterObj.SetActive(true);
            WriterObj.GetComponentInChildren<Text>().text = inv.items[weapon_index].wr.text;
            FindObjectOfType<AudioManager>().writerItem.Play();
        }
    }

    private void Update()
    {
        UpdateUIAndMovement();
    }

    private void UpdateUIAndMovement()
    {
        bool hasSelectedItem = inv.items[weapon_index] != null;

        put_away_but.SetActive(hasSelectedItem);
        shot.SetActive(hasSelectedItem && inv.items[weapon_index].isWeapon);

        if (hasSelectedItem)
        {
            pm.cooldown = inv.items[weapon_index].ws.cooldown;
            pm.speed_bullet = inv.items[weapon_index].ws.speed_bul;
            pm.damage = inv.items[weapon_index].ws.damage;
            pm.maxpatrols = inv.items[weapon_index].ws.maxpatrols;
        }
    }

    public void PutAway()
    {
        var gb = Instantiate(physics_obj.FirstOrDefault(go => go.name == inv.items[weapon_index]?.gb_item), put_away_obj.position, put_away_obj.rotation);
        gb.name = gb.name[..^7];
        TrySetActiveItem(inv.items[weapon_index]?.find_object_my, false);
        inv.items[weapon_index] = null;
        inv.cells[weapon_index].graf.sprite = null;
        shot.SetActive(false);
        inv.UpdateCells();
    }
}

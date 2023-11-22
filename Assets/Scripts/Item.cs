using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Texture2D img;
    public string find_object_my = "";
    public string gb_item;


    public bool isWeapon;
    public WeaponSettings ws;

    public bool isWriter;
    public Writer wr;

    


}

[System.Serializable]
public class WeaponSettings
{
    public GameObject BulletEfect;
    public float speed_bul;
    public float cooldown = 0.2f;
    public float damage = 0.2f;
    public int maxpatrols = 30;
    public bool isMelleeWeapon;

}
[System.Serializable]
public class Writer
{
    public string text;
}

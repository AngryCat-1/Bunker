using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    public GameObject[] need_destroy_obj;
    public string event_str;

    public void DestroyNeed_des_obj()
    {
        StartCoroutine(FindObjneeds());
    }


    public IEnumerator FindObjneeds()
    {
        if (event_str == "1PlayCastSceneBagap")
        {
            var gl = FindObjectOfType<PlayerMovement>();
            var g = gl.GetComponent<Animator>();

            for (int i = 0; i < need_destroy_obj.Length; i++)
            {
                need_destroy_obj[i].transform.Rotate(0, 80, 0);
            }




            gl.enabled = false;
            g.enabled = true;
            yield return new WaitForSeconds(1);
            g.Play("1PlayCastSceneBagap");
            yield return new WaitForSeconds(6);
            gl.enabled = true;
            g.enabled = false;
            FindObjectOfType<MissionController>().CompleteMissionById("door_open_car_first");

            gl.speed = 4.8f;


        }




        Destroy(gameObject);
    }

  
}

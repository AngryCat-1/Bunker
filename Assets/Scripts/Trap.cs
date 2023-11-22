using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
   public bool isTrapped;
   public void TrapFV()
   {
        if (!isTrapped)
        {
            FindObjectOfType<PlayerHealth>().TakeDamage(90);
            GetComponent<Animator>().SetBool("IsTrap", true);
            isTrapped = true;
            FindObjectOfType<AudioManager>().trap.Play();
        }
       
   }
}

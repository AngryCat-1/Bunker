using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroyTriggerCheck : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "NewInteractionSystemTrigger" && GetComponent<RCC_CarControllerV3>().canControl)
        {
            other.GetComponent<InteractionSystemv2>().Act();

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayerTransition : MonoBehaviour
{

	public GameObject[] PlayerGBCarChange;
	public GameObject[] CarGBCarChange;
	public GameObject Car;
	public RCC_Camera rcccm;
	bool isInCar;
	public void GoToCar()
	{

		for (int i = 0; i < PlayerGBCarChange.Length; i++)
		{
			PlayerGBCarChange[i].SetActive(false);
		}
		for (int i = 0; i < CarGBCarChange.Length; i++)
		{
			CarGBCarChange[i].SetActive(true);
		}

		Car.GetComponent<RCC_CarControllerV3>().enabled = true;
		Car.GetComponent<RCC_CarControllerV3>().canControl = true;
		if (rcccm!=null)
        {
			rcccm.gameObject.SetActive(true);
		}


		isInCar = true;




	}

    private void FixedUpdate()
    {
        if (isInCar ==  true)
        {
			Car.GetComponent<RCC_CarControllerV3>().idleEngineSoundVolume = 0.5f;
			Car.GetComponent<RCC_CarControllerV3>().maxEngineSoundVolume = 1;
		}
		else
        {
			Car.GetComponent<RCC_CarControllerV3>().idleEngineSoundVolume = 0;
			Car.GetComponent<RCC_CarControllerV3>().maxEngineSoundVolume = 0;
		}
    }
    public void InsideToCar()
	{
		for (int i = 0; i < PlayerGBCarChange.Length; i++)
		{
			PlayerGBCarChange[i].SetActive(true);
		}
		for (int i = 0; i < CarGBCarChange.Length; i++)
		{
			CarGBCarChange[i].SetActive(false);
		}

		PlayerGBCarChange[0].transform.position = Car.transform.Find("PlayerLeavePoint").position;

		Car.GetComponent<RCC_CarControllerV3>().enabled = false;
		Car.GetComponent<RCC_CarControllerV3>().canControl = false;

		rcccm = FindObjectOfType<RCC_Camera>();
		rcccm.gameObject.SetActive(false);


		isInCar = false;


	}
}

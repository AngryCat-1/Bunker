using UnityEngine;

namespace VM
{
    public class VariablesManager : MonoBehaviour
    {
        public int act_index = 1;

        public Act_1 act1;
        public Act_2 act2;
        public Act_3 act3;


        private void Update()
        {
            if (act1.EndBullets == null && act1.endbul_bool == false)
            {               
                FindObjectOfType<MissionController>().CompleteMissionById("houseBring_bullets");
                act1.endbul_bool = true;
            }
        }
    }
    [System.Serializable]
    public class Act_1
    {
        public bool isFirstLomBring;
        public bool isFirstLomOpenDoor;
        public bool canSitCar;
        public bool needToDieZombiesBoolVarIsComplete;
        public bool isNeedRendererBullets;
        public bool endbul_bool;
        public GameObject[] needToDieZombies;
        public GameObject ZombieBoss;
        public GameObject EndCanvas;
        public GameObject EndBullets;
    }

    [System.Serializable]
    public class Act_2
    {

    }

    [System.Serializable]
    public class Act_3
    {

    }
}


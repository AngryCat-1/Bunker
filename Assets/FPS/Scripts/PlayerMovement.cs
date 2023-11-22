using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VM;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] public Transform EffectSpawnPoint;
	[SerializeField] public GameObject BulletEfect;
	public float speed_bullet = 6;
	public float damage = 1f;
	public float cooldown = 0.2f;
 
	public GameObject hitParticle;
	public GameObject bloodParticle;
	public TMP_Text PatrolsText;
	public float speed = 5.5f;
	float shootTimer = 0;

	private Rigidbody rb;
	private AudioSource shootSound;

	public WeaponManager wm;

	public DestroyTrigger dt;
	public GameObject CarTransitionToPlayer;

	public Vector3 moveX;
	public Vector3 moveY;

	public int patrols;
	public int maxpatrols;

	private float particle_cooldown;
	private float aud_cooldown;
	public GameObject GuideObj;
	//public bool canSitCar;


	void Start () {
		shootSound = GameObject.Find("ShotSound").GetComponent<AudioSource> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	void FixedUpdate () {
		if (!GuideObj.activeSelf)
        {
			moveX = JoystickLeft.positionX * speed * transform.right;
			moveY = JoystickLeft.positionY * speed * transform.forward;
			rb.MovePosition(transform.position + moveX * Time.fixedDeltaTime + moveY * Time.fixedDeltaTime);
			if (moveX.magnitude >= 0.1f || moveY.magnitude >= 0.1f)
			{
				if (!FindObjectOfType<AudioManager>().walk.isPlaying && GetComponent<PlayerHealth>().currentHealth > 0)
				{
					FindObjectOfType<AudioManager>().walk.Play();
				}

			}
			else
			{
				FindObjectOfType<AudioManager>().walk.Stop();
			}
		}
		
		

		//transform.position = transform.position + moveX * Time.fixedDeltaTime + moveY  * Time.fixedDeltaTime;
	}

	void Update()
	{
		aud_cooldown -= Time.deltaTime;
		particle_cooldown -= Time.deltaTime;
		PatrolsText.text = patrols + "/" + maxpatrols;
		PatrolsText.gameObject.SetActive(GetComponent<Inventory>().items[GetComponent<WeaponManager>().weapon_index] != null && GetComponent<Inventory>().items[GetComponent<WeaponManager>().weapon_index].isWeapon && !GetComponent<Inventory>().items[GetComponent<WeaponManager>().weapon_index].ws.isMelleeWeapon && FindObjectOfType<VariablesManager>().act1.isNeedRendererBullets);

		

		transform.rotation = Quaternion.Euler(JoystickRight.rotY, JoystickRight.rotX, 0);

		shootTimer += Time.deltaTime;
		if (JoystickRight.shot && !wm.inv.isAutomat)
		{
			if (shootTimer >= cooldown)
			{
				shootTimer = 0;
				
				ShootBullets();
			}
		}

		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
		{
			if (hit.transform.gameObject.layer == LayerMask.NameToLayer("RCC") && FindObjectOfType<VM.VariablesManager>().act1.canSitCar)
            {
				CarTransitionToPlayer.SetActive(true);
            }
			else
            {
				CarTransitionToPlayer.SetActive(false);
			}
		}
	}

	public void ShootBullets() {
		if (wm.inv.items[wm.weapon_index] != null && !wm.inv.items[wm.weapon_index].ws.isMelleeWeapon && (patrols > 0 || !FindObjectOfType<VariablesManager>().act1.isNeedRendererBullets))
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
			{
				GameObject particle = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(particle, 0.5f);
				GameObject d = Instantiate(BulletEfect, EffectSpawnPoint.position, Quaternion.identity);
				d.transform.LookAt(hit.point);
				d.GetComponent<FireEffectController>().SpeedSize = speed_bullet;

				if (hit.transform.gameObject.GetComponent<ZombieAI>() != null)
				{
					if (particle_cooldown <= 0)
                    {
						GameObject blood = Instantiate(bloodParticle, hit.point, Quaternion.LookRotation(hit.normal));
						Destroy(blood, 0.5f);
						particle_cooldown = 0.035f;
						
					}
					

					hit.transform.gameObject.GetComponent<ZombieHealth>().TakeDamage(damage);
				}

				shootSound.Play();
				if (FindObjectOfType<VariablesManager>().act1.isNeedRendererBullets)
				{
					patrols--;
				}

			}
		}
		else if (wm.inv.items[wm.weapon_index] != null && wm.inv.items[wm.weapon_index].ws.isMelleeWeapon)
		{

			StartCoroutine(PlayOneShotUpd(FindObjectOfType<AudioManager>().MelleShot));
			
			wm.can_active_items.FirstOrDefault(go => go.name == wm.inv.items[wm.weapon_index].find_object_my).GetComponent<Animator>().Play("Shoot");
			if (dt != null)
			{
				dt.DestroyNeed_des_obj();
			}

			RaycastHit hit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2))
			{
				
				if (hit.transform.gameObject.GetComponent<ZombieAI>() != null)
                {
					if (particle_cooldown <= 0)
					{
						GameObject blood = Instantiate(bloodParticle, hit.point, Quaternion.LookRotation(hit.normal));
						Destroy(blood, 0.5f);
						particle_cooldown = 0.2f;

						
					}

					hit.transform.gameObject.GetComponent<ZombieHealth>().TakeDamage(damage);
				}
				
			}

			
		}
	}

	IEnumerator PlayOneShotUpd(AudioSource aud)
    {
		if (aud_cooldown <= 0)
		{
			aud_cooldown = 0.4f;
			aud.PlayOneShot(aud.clip);
			yield return null;
			//particle_cooldown = 0.5f;

			
		}

	}

	public void Jump() {
		if(JoystickRight.jump) {
			JoystickRight.jump = false;
			GetComponent<Rigidbody> ().AddForce(new Vector3(0, 300, 0));	
		}
	}

	void OnTriggerEnter(Collider col) {
		JoystickRight.jump = true;
	}

    private void OnTriggerStay(Collider other)
    {
		if (other.gameObject.GetComponent<DestroyTrigger>() != null)
		{
			dt = other.gameObject.GetComponent<DestroyTrigger>();

		}
		if (other.gameObject.GetComponent<Trap>() != null)
		{
			other.gameObject.GetComponent<Trap>().TrapFV();

		}
		if (other.gameObject.GetComponent<Patrol>() != null)
		{
			patrols += other.gameObject.GetComponent<Patrol>().patrols;
			Destroy(other.gameObject);
			if (patrols>maxpatrols)
            {
				patrols = maxpatrols;

			}

		}

		if (other.tag == "NewInteractionSystemTrigger")
		{
			other.GetComponent<InteractionSystemv2>().Act();
			print("collision with is");

		}
		
	}

    private void OnTriggerExit(Collider other)
    {
		if (dt != null && other.gameObject == dt.gameObject)
		{
			dt = null;

		}

	}

	

}

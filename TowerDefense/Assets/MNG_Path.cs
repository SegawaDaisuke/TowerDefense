using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNG_Path : MonoBehaviour {

	public Transform mob;

	public Transform wpParent;
	private Transform castleDoor;
	private Transform chest;

	private MNG_Castle mngCastle;

	public float speed = 10f;
	public float atkCooldown = 1f;
	public bool gizmos = true;

	private Queue<Transform> wpQueue;
	private Transform nextWp;
	private bool attackingCastle = false;
	public bool gotChest = false;


	void Start () {
		mngCastle = this.gameObject.GetComponent<MNG_Castle>();
		castleDoor = mngCastle.door;
		chest = mngCastle.chest;

		wpQueue = new Queue<Transform>();
		for (int i = 0; i < wpParent.childCount; i++) {
			wpQueue.Enqueue(wpParent.GetChild(i));
		}

		wpQueue.Enqueue(castleDoor);
		wpQueue.Enqueue(chest);

		for (int i = wpParent.childCount-1; i >= 0; i--) 
		{
			wpQueue.Enqueue(wpParent.GetChild(i));
		}
	}

	// Update is called once per frame
	void Update () {
		atkCooldown -= Time.deltaTime;

		if (attackingCastle){
			if (atkCooldown <= 0) {
				mngCastle.dealDamage(5);
				atkCooldown = 1f;

				if (mngCastle.destroyed) {
					attackingCastle = false;
					nextWp = wpQueue.Dequeue();
				}
			}
		}else{
			if (nextWp == null || Vector3.Distance(nextWp.transform.position, mob.transform.position) <= 0.5){
				// We arrive at our next wp

				if (nextWp == castleDoor && !mngCastle.destroyed && !attackingCastle){
					attackingCastle = true;
				}else{ // We don't want to attack the castle door
					if (wpQueue.Count > 0){
						
						if (nextWp == chest) {
							gotChest = true;
						}

						nextWp = wpQueue.Dequeue();

					} else{
						//We arrive back at the starting point
						Destroy(this.gameObject);
					}
				}
			}

			float step = speed * Time.deltaTime;
			mob.transform.position = Vector3.MoveTowards(mob.transform.position, nextWp.transform.position, step);

			if (gotChest) {
				chest.position = mob.position;
			}
		}
	}


	private void OnDrawGizmos()
	{
		if (gizmos)
		{
			Gizmos.color = Color.red;
			for (int i = 0; i < wpParent.childCount; i++)
			{
				Gizmos.DrawWireSphere(wpParent.GetChild(i).transform.position, 0.2f);
				if (i > 0)
				{
					Gizmos.DrawLine(wpParent.GetChild(i).position, wpParent.GetChild(i - 1).position);
				}
			}
		}
	}
}
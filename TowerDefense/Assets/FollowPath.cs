using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

	public Transform wpParent;
	public Transform castleDoor;
	public Transform[] chest;

	public CastleDoorManager castleDoorMNG;

	public float speed = 10f;
	public float atkCooldown = 1f;
	public bool gizmos = true;

	private Queue<Transform> wpQueue;
	private Transform nextWp;
	private bool attackingCastle = false;

	// Use this for initialization
	void Start () {
		castleDoorMNG = GameObject.FindObjectOfType<CastleDoorManager>();

		wpQueue = new Queue<Transform>();
		for (int i = 0; i < wpParent.childCount; i++) {
			wpQueue.Enqueue(wpParent.GetChild(i));
		}

		wpQueue.Enqueue(castleDoor);

		for (int i = 0; i < chest.Length; i++) {
			wpQueue.Enqueue(chest[i]);
		}

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
				castleDoorMNG.dealDamage(5);
				atkCooldown = 1f;

				if (castleDoorMNG.destroyed) {
					attackingCastle = false;
					nextWp = wpQueue.Dequeue();
				}
			}
		}else{
			if (nextWp == null || Vector3.Distance(nextWp.transform.position, this.transform.position) <= 0.5){
				// We arrive at our next wp

				if (nextWp == castleDoor && !castleDoorMNG.destroyed && !attackingCastle){
					attackingCastle = true;
				}else{ // We don't want to attack the castle door
					if (wpQueue.Count > 0){
						nextWp = wpQueue.Dequeue();
					}
					else{
						Destroy(this.gameObject);
					}
				}
			}

			float step = speed * Time.deltaTime;
			this.transform.position = Vector3.MoveTowards(transform.position, nextWp.transform.position, step);
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
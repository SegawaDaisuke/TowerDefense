using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNG_Castle : MonoBehaviour {

	public Transform castle;
	public Transform door;
	public Transform chest;
    public int maxHP = 100;
    public int currentHP;
    public bool destroyed = false;

	// Use this for initialization
	void Start () {
        currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void dealDamage(int damage) {
        currentHP = Mathf.Max(0, currentHP - damage);

        if (currentHP == 0) { destroyDoor(); }
    }

    public void destroyDoor() {
        destroyed = true;
        door.gameObject.SetActive(false);
    }
}

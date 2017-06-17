using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDoorManager : MonoBehaviour {

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

        foreach (Transform child in this.transform) {
            if (child.name == "Door") {
                // TODO Particle effect
                child.gameObject.SetActive(false);
            }
        }
    }
}

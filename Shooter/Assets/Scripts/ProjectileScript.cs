using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public float projectileSpeed = 20;
    private float projectileDamage = 10;
    public PlayerControllerScript player;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += Vector3.forward * projectileSpeed;
	}

    public void SetDamage(float amount)
    {
        projectileDamage = amount;
        Debug.Log(amount);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.TakeDamage(projectileDamage);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNecromancerScript : MonoBehaviour {

    public Slider enemyFireRatetSlider;
    private float enemyFireRate = 0.2f;
    private float secondsSinceLastFire = 0;

    public Dropdown enemyDifficulty;
    public float enemyDamage = 10;

    public float scoreBonus = 100;

    public Slider enemyHealtBar;
    public float enemyMaxHealth;
    public float enemyCurrentHealth;

    private GameObject player;
    private Animator animator;
    private PlayerControllerScript playerController;

    public Transform projectileSpawn;
    public GameObject projectile;

    bool attacking = false;
	// Use this for initialization
	void Start ()
    {
        enemyHealtBar.maxValue = enemyMaxHealth;
        enemyCurrentHealth = enemyMaxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerControllerScript>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateHealthBar();
        transform.LookAt(player.transform);

        secondsSinceLastFire += Time.deltaTime;
        if(attacking && secondsSinceLastFire > enemyFireRate)
        {
            animator.SetTrigger("Attack1Trigger");
            GameObject currentProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
            ProjectileScript projScript = currentProjectile.GetComponent<ProjectileScript>();
            projScript.SetDamage(enemyDamage);
            Destroy(currentProjectile, 5);
        }

	}

    void UpdateHealthBar()
    {
        enemyHealtBar.value = enemyCurrentHealth;
    }
    public void SetEnemyFireRate()
    {
        enemyFireRate = enemyFireRatetSlider.value;
        Text sliderText = enemyFireRatetSlider.GetComponentInChildren<Text>();
        sliderText.text = "Enemy Fire Rate: " + Math.Round(enemyFireRatetSlider.value, 2);
    }

    public void Damage(int damageAmount)
    {
        enemyCurrentHealth -= damageAmount;
        if(enemyCurrentHealth <= 0)
        {
            playerController.AddScore(scoreBonus);
            gameObject.SetActive(false);
        }
    }
    public void SetEnemyDifficulty()
    {
        switch (enemyDifficulty.value)
        {
            case 0:
                enemyDamage = 10;
                break;
            case 1:
                enemyDamage = 20;
                break;
            case 2:
                enemyDamage = 100;
                break;
            default:
                enemyDamage = 10;
                break;
        }
        Debug.Log(enemyDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            Debug.Log("Player in Range");
            attacking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
         {
            attacking = false;
            Debug.Log("Stopped attacking");
        }

    }
}
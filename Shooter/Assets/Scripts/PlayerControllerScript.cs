using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    public GUIControllerScript GUIController;

    public Slider playerSpeedSlider;
    private float playerSpeed = 10;

    public int gunDamage = 20;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;

    public float maxHealth = 100;
    public float currentHealth = 100;

    public int magazineSize = 30;
    public int totalAmmoLeft = 90;
    public int loadedAmmo = 30;
    private int totalAmmo;
    public Text ammoStatusText;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;

    private float playerScore = 0;

    public Slider playerJumpHeightSlider;
    private float playerJumpHeight = 4;
    Vector3 sidewardMovement;
    Vector3 forwardMovement;
    public GameObject mainCamera;
    public float lookingSpeed = 2f;
    float lookVertical;
    float lookHorizontal;
    private Rigidbody rb;
    // Use this for initialization
    void Start ()
    {
        laserLine = GetComponentInChildren<LineRenderer>();
        gunAudio = GetComponentInChildren<AudioSource>();
        fpsCam = GetComponentInChildren<Camera>();

        totalAmmo = totalAmmoLeft + loadedAmmo;
        SetSpeed();
        SetJumpHeight();
        SetAmmoStatusText();
        rb = GetComponentInChildren<Rigidbody>();
       // mainCamera = GetComponentInChildren<Camera>();
	}

    private void Update()
    {
        if(currentHealth <= 0)
        {
            GUIController.MainMenu();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        ManageInput();
        MovePlayer();
        LookAround();
    }

    public void AddScore(float amount)
    {
        playerScore += amount;
    }

    public float GetScore()
    {
        return playerScore;
    }

    private void MovePlayer()
    {
        Vector3 movement = (sidewardMovement + forwardMovement).normalized * playerSpeed;
        movement = movement.normalized * playerSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + movement);
       // transform.position = rb.transform.position;
    }

    private void LookAround()
    {
        if(lookHorizontal != 0)
        {
            rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, 1, 0) * lookHorizontal * lookingSpeed);
        }
        if(lookVertical != 0)
        {
            mainCamera.transform.Rotate(-lookVertical * lookingSpeed, 0, 0, Space.Self);
        }
    }

    private void ManageInput()
    {
        sidewardMovement = transform.right * Input.GetAxisRaw("Horizontal");
        forwardMovement = transform.forward * Input.GetAxisRaw("Vertical");

        lookHorizontal = Input.GetAxis("Mouse X");
        lookVertical = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            ShootGun();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadAmmo();
        }

    }

    public void ReloadAmmo()
    {
        if(loadedAmmo < magazineSize && totalAmmoLeft > 0)
        {
            if(totalAmmo > magazineSize)
            {
                totalAmmoLeft -= magazineSize - loadedAmmo;
                loadedAmmo = magazineSize;
            }
            else if(totalAmmo <= magazineSize)
            {
                loadedAmmo += totalAmmoLeft;
                totalAmmoLeft = 0;
            }
            else
            {
                loadedAmmo = totalAmmoLeft;
                totalAmmoLeft = 0;
            }
        }
        SetAmmoStatusText();
    }

    public void SetSpeed()
    {
        playerSpeed = playerSpeedSlider.value;
        Text sliderText = playerSpeedSlider.GetComponentInChildren<Text>();
        sliderText.text = "Player Speed: " + playerSpeedSlider.value;
    }

    public void SetJumpHeight()
    {
        playerJumpHeight = playerJumpHeightSlider.value;
        Text sliderText = playerJumpHeightSlider.GetComponentInChildren<Text>();
        sliderText.text = "Player Jump Height: " + playerJumpHeightSlider.value;
    }

    public void SetAmmoStatusText()
    {
        ammoStatusText.text = loadedAmmo + " / " + totalAmmoLeft;
    }

    void ShootGun()
    {
        if(loadedAmmo > 0)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);

                EnemyNecromancerScript health = hit.collider.GetComponent<EnemyNecromancerScript>();

                if (health != null)
                {
                    health.Damage(gunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }

        }
        else
        {
            ReloadAmmo();
        }
        SetAmmoStatusText();
        totalAmmo = totalAmmoLeft + loadedAmmo;
    }


    private IEnumerator ShotEffect()
    {
        loadedAmmo--;
        gunAudio.Play();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }

}

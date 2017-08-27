using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetalDoorScript : MonoBehaviour
{

    public Text enterPrompt;

    public float openingSpeed = 5;

    private bool hasCollided = false;

    private bool isOpening = false;

    private Vector3 originalPosition;

    private float size;
    private float distanceToOrigPos = 0;
    // Use this for initialization
	void Start ()
    {
        originalPosition = transform.position;
        size = GetComponent<Collider>().bounds.size.z;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("Use") > 0 && hasCollided)
        {
            isOpening = true;
        }

        if(hasCollided && isOpening && distanceToOrigPos <= size)
        {
            transform.Translate(openingSpeed * Time.deltaTime, 0, 0);

            distanceToOrigPos = Vector3.Distance(originalPosition, transform.position);
        }
        else if(distanceToOrigPos > size)
        {
            enterPrompt.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !hasCollided)
        {
            enterPrompt.gameObject.SetActive(true);

            hasCollided = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        enterPrompt.gameObject.SetActive(false);

        hasCollided = false;

        Debug.Log("Collision Exit");
    }
}

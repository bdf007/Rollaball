using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    private TextMeshProUGUI countText;
    private GameObject winTextObject;
    private GameObject ruleTextObject;


    private Rigidbody rb;
    private int count;
    private  float movementX;
    private  float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // get the count text component as a chid of the canvas
        countText = GameObject.Find("CountText").GetComponent<TextMeshProUGUI>();
        // get the object WinText as a child of the canvas
        winTextObject = GameObject.Find("WinText");
        // get the object RuleText as a child of the canvas
        ruleTextObject = GameObject.Find("RuleText");
        ruleTextObject.SetActive(true);
        count = 0;

        SetCount();
        winTextObject.SetActive(false);
        StartCoroutine(PutTheRuleAndWait());
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCount()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.SetActive(true);
            // start the coroutine to wait 5 seconds before going to the next scene in the build
            StartCoroutine(WaitAndLoadNextScene());
        }
    }

    // coroutine to wait 5 seconds before going to the next scene in the build
    
    IEnumerator WaitAndLoadNextScene()
    { 
    
        yield return new WaitForSeconds(2);
        // get the current scene index
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        // get the number of scene in the build
        int numberOfSceneInBuild = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        if (currentSceneIndex < numberOfSceneInBuild - 1)
        {
            // load the next scene in the build
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            // load the first scene in the build
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    IEnumerator PutTheRuleAndWait()
    {

        yield return new WaitForSeconds(2);
        // deactivate ruleText
        ruleTextObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if game object has tag "PickUp" and is active (not picked up) then disable it
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCount();
        }
    }
}

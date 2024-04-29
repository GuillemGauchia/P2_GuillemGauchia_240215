using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayMachine : MonoBehaviour
{
    public float horizontalBoundary = 22;
    public float movementSpeed = 14f;
    public GameObject hayBalePrefab; // Reference to the Hay Bale prefab.
    public Transform haySpawnpoint; // The point from which the hay will to be shot.
    public float shootInterval = 1.0f; // The smallest amount of time between shots
    private float shootTimer; // A timer that to keep track whether the machine can shoot or not
    public Transform modelParent; // Parent transform for the model

    // Model prefabs
    public GameObject blueModelPrefab;
    public GameObject yellowModelPrefab;
    public GameObject redModelPrefab;

    void Start()
    {
        LoadModel();
        StartCoroutine(IncreaseMovementSpeed());
        StartCoroutine(DecreaseShootInterval());
    }

    private void LoadModel()
    {
        Destroy(modelParent.GetChild(0).gameObject); // Destroy previous model
        switch (GameSettings.hayMachineColor) // Load selected model
        {
            case HayMachineColor.Blue:
                Instantiate(blueModelPrefab, modelParent);
                break;
            case HayMachineColor.Yellow:
                Instantiate(yellowModelPrefab, modelParent);
                break;
            case HayMachineColor.Red:
                Instantiate(redModelPrefab, modelParent);
                break;
        }
    }

    private void UpdateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput < 0 && transform.position.x > -horizontalBoundary)
        {
            transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
        }
        else if (horizontalInput > 0 && transform.position.x < horizontalBoundary)
        {
            transform.Translate(transform.right * movementSpeed * Time.deltaTime);
        }
    }

    private void ShootHay()
    {
        Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.identity);
        SoundManager.Instance.PlayShootClip();
    }

    private void UpdateShooting()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0 && Input.GetKey(KeyCode.Space))
        {
            shootTimer = shootInterval;
            ShootHay();
        }
    }

    IEnumerator IncreaseMovementSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
            if (movementSpeed < 30.0f) // Increase speed only if it's less than 25 units
            {
                movementSpeed += 1.0f;
            }
        }
    }

    IEnumerator DecreaseShootInterval()
    {
        int decreaseCount = 0;
        while (decreaseCount < 4) 
        {
            yield return new WaitForSeconds(10.0f); 
            shootInterval -= 0.1f; 
            shootInterval = Mathf.Max(shootInterval, 0.1f); 
            decreaseCount++; 
        }
    }

    void Update()
    {
        UpdateMovement();
        UpdateShooting();
    }
}

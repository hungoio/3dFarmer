using UnityEngine;
using ithappy.Animals_FREE;
using Ursaanimation.CubicFarmAnimals;

public class FarmAnimalAI : MonoBehaviour
{
    [Header("Cài đặt")]
    public AnimalData data;
    public float wanderRadius = 3f;
    public float waitTime = 3f;
    public float maxWalkTime = 4f;
    private float walkTimer = 0f;

    private Vector3 homePosition;
    private bool hasHome = false;

    private float produceTimer;
    private float waitTimer;
    private bool isWalking = false;
    private Vector3 targetPosition;
    private CreatureMover mover;
    private AnimationController animControl;

    [Header("Trạng thái thu hoạch")]
    public bool isReadyToHarvest = false;

    void Start()
    {
        mover = GetComponent<CreatureMover>();
        animControl = GetComponent<AnimationController>();
        if (!hasHome) SetHome(transform.position);
        PickNewTarget();
    }

    public void SetHome(Vector3 position)
    {
        homePosition = position;
        hasHome = true;
    }

    void Update()
    {
        if (mover == null || isReadyToHarvest) return;

        HandleMovement();
        HandleProduction();
    }

    void HandleProduction()
    {
        if (data != null)
        {
            produceTimer += Time.deltaTime;
            if (produceTimer >= data.produceTime)
            {
                PrepareForHarvest();
            }
        }
    }

    void PrepareForHarvest()
    {
        isReadyToHarvest = true;
        isWalking = false;
        if (animControl != null)
        {
            animControl.animator.Play(animControl.standtositAnimation);
        }
        Debug.Log(data.animalName + " đang ngồi chờ thu hoạch.");
    }

    public void OnHarvested()
    {
        isReadyToHarvest = false;
        produceTimer = 0;

        if (animControl != null)
        {
            animControl.animator.Play(animControl.sittostandAnimation);
        }

        if (data.productPrefab != null)
        {
            Instantiate(data.productPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        PickNewTarget();
    }

    void HandleMovement()
    {
        if (isWalking)
        {
            walkTimer += Time.deltaTime;
            float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                              new Vector3(targetPosition.x, 0, targetPosition.z));

            if (distance > 0.5f && walkTimer < maxWalkTime)
            {
                mover.SetInput(new Vector2(0, 1), targetPosition, false, false);
            }
            else
            {
                StopWalking();
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;
            mover.SetInput(Vector2.zero, transform.position + transform.forward, false, false);
            if (waitTimer <= 0) PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        targetPosition = homePosition + new Vector3(randomPoint.x, 0, randomPoint.y);
        isWalking = true;
        walkTimer = 0f;
    }

    void StopWalking()
    {
        isWalking = false;
        waitTimer = waitTime;
    }
}
using UnityEngine;
using ithappy.Animals_FREE;

public class FarmAnimalAI : MonoBehaviour
{
    [Header("Cài đặt")]
    public AnimalData data;
    public float wanderRadius = 3f; // Giảm bán kính lại để phù hợp diện tích chuồng
    public float waitTime = 3f;

    // 👇 THÊM BIẾN NÀY ĐỂ NHỚ VỊ TRÍ CHUỒNG
    private Vector3 homePosition;
    private bool hasHome = false;

    private float produceTimer;
    private float waitTimer;
    private bool isWalking = false;
    private Vector3 targetPosition;
    private CreatureMover mover;

    void Start()
    {
        mover = GetComponent<CreatureMover>();

        // Nếu lúc sinh ra chưa được giao nhà, thì lấy vị trí hiện tại làm nhà tạm
        if (!hasHome) SetHome(transform.position);

        PickNewTarget();
    }

    // 👇 HÀM MỚI: Để Chuồng gà gọi hàm này ngay khi Instantiate con gà
    public void SetHome(Vector3 position)
    {
        homePosition = position;
        hasHome = true;
    }

    void Update()
    {
        if (mover == null) return;
        HandleMovement();

        if (data != null)
        {
            produceTimer += Time.deltaTime;
            if (produceTimer >= data.produceTime)
            {
                Produce();
                produceTimer = 0;
            }
        }
    }

    void HandleMovement()
    {
        if (isWalking)
        {
            float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                              new Vector3(targetPosition.x, 0, targetPosition.z));
            if (distance > 0.5f)
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

            if (waitTimer <= 0)
            {
                PickNewTarget();
            }
        }
    }

    void PickNewTarget()
    {
        // 👇 THAY ĐỔI QUAN TRỌNG: Lấy vị trí ngẫu nhiên quanh homePosition (Chuồng)
        // chứ không phải quanh transform.position (Con gà)
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        targetPosition = homePosition + new Vector3(randomPoint.x, 0, randomPoint.y);

        isWalking = true;
    }

    void StopWalking()
    {
        isWalking = false;
        waitTimer = waitTime;
    }

    void Produce()
    {
        if (data.productPrefab != null)
        {
            Instantiate(data.productPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }
}
using ithappy.Animals_FREE;
using UnityEngine;
using Ursaanimation.CubicFarmAnimals;

public class FarmAnimalAI : MonoBehaviour
{
    [Header("Cài đặt")]
    public AnimalData data;
    public float wanderRadius = 3f; // Giảm bán kính lại để phù hợp diện tích chuồng
    public float waitTime = 3f;
    public float maxWalkTime = 4f; // Đi quá 4 giây là bỏ cuộc
    private float walkTimer = 0f;  // Bộ đếm thời gian đang đi

    // 👇 THÊM BIẾN NÀY ĐỂ NHỚ VỊ TRÍ CHUỒNG
    private Vector3 homePosition;
    private bool hasHome = false;

    private float produceTimer;
    private float waitTimer;
    private bool isWalking = false;
    private Vector3 targetPosition;
    private CreatureMover mover;
    private AnimationController animControl;
    public bool isReadyToHarvest = false;

    void Start()
    {
        mover = GetComponent<CreatureMover>();
        animControl = GetComponent<AnimationController>(); // Lấy script điều khiển anim

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
        // QUAN TRỌNG: Nếu đang chờ thu hoạch thì dừng mọi hành động di chuyển
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
                // KIỂM TRA: Nếu là Cừu (tên trong data) thì mới ngồi chờ
                if (data.animalName == "Sheep" || data.animalName == "Cừu")
                {
                    PrepareForHarvest();
                }
                else
                {
                    // Nếu là Gà: Đẻ trứng xong reset thời gian và đi tiếp luôn (Code cũ của bạn)
                    Produce();
                    produceTimer = 0;
                }
            }
        }
    }

    void PrepareForHarvest()
    {
        isReadyToHarvest = true;
        isWalking = false;

        // Chạy animation ngồi xuống (stand_to_sit)
        if (animControl != null)
        {
            animControl.animator.Play(animControl.standtositAnimation);
        }
    }

    // Hàm này sẽ được gọi khi bạn dùng Kéo quét qua con cừu
    public void OnHarvested()
    {
        isReadyToHarvest = false; // Tắt trạng thái chờ
        produceTimer = 0; // Reset thời gian

        // Đứng dậy đi tiếp
        if (animControl != null)
            animControl.animator.Play(animControl.sittostandAnimation);

        // CHỈ tạo ra vật phẩm, KHÔNG xóa bản thân con cừu
        if (data.productPrefab != null)
        {
            Instantiate(data.productPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        PickNewTarget();
    }
    void Produce()
    {
        if (data.productPrefab != null)
        {
            Instantiate(data.productPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    void HandleMovement()
    {
        if (isWalking)
        {
            // 👇 THÊM: Tăng thời gian đã đi bộ
            walkTimer += Time.deltaTime;

            float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                              new Vector3(targetPosition.x, 0, targetPosition.z));

            // 👇 SỬA LẠI ĐIỀU KIỆN: Chỉ đi tiếp nếu chưa tới nơi VÀ chưa quá thời gian
            if (distance > 0.5f && walkTimer < maxWalkTime)
            {
                mover.SetInput(new Vector2(0, 1), targetPosition, false, false);
            }
            else
            {
                // Nếu đã tới đích, HOẶC bị kẹt quá 4 giây -> Dừng lại
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
        walkTimer = 0f; // 👇 THÊM DÒNG NÀY: Reset đồng hồ khi bắt đầu đi mục tiêu mới
    }

    void StopWalking()
    {
        isWalking = false;
        waitTimer = waitTime;
    }
}
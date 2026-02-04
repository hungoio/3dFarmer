using UnityEngine;
using ithappy.Animals_FREE; // Sử dụng thư viện của asset bạn gửi

public class FarmAnimalAI : MonoBehaviour
{
    [Header("Cài đặt")]
    public AnimalData data; // Kéo file ChickenData vào đây
    public float wanderRadius = 5f; // Bán kính đi dạo
    public float waitTime = 3f; // Thời gian đứng nghỉ

    // Bộ đếm thời gian
    private float produceTimer;
    private float waitTimer;

    // Trạng thái
    private bool isWalking = false;
    private Vector3 targetPosition;

    // Tham chiếu đến script di chuyển của asset
    private CreatureMover mover;

    void Start()
    {
        // Tự động tìm cái chân (CreatureMover)
        mover = GetComponent<CreatureMover>();

        // Bắt đầu chọn điểm đi đầu tiên
        PickNewTarget();
    }

    void Update()
    {
        if (mover == null) return;

        // 1. XỬ LÝ DI CHUYỂN
        HandleMovement();

        // 2. XỬ LÝ ĐẺ TRỨNG
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
            // Tính khoảng cách đến đích
            float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                              new Vector3(targetPosition.x, 0, targetPosition.z));

            if (distance > 0.5f)
            {
                // Vẫn chưa đến nơi -> Ra lệnh đi tiếp
                // Tham số: (Axis di chuyển, Điểm nhìn, Chạy?, Nhảy?)
                // Axis (0, 1) nghĩa là luôn đi thẳng về phía trước mặt
                mover.SetInput(new Vector2(0, 1), targetPosition, false, false);
            }
            else
            {
                // Đã đến nơi -> Dừng lại
                StopWalking();
            }
        }
        else
        {
            // Đang đứng nghỉ
            waitTimer -= Time.deltaTime;

            // Ra lệnh đứng im (Axis 0,0)
            mover.SetInput(Vector2.zero, transform.position + transform.forward, false, false);

            if (waitTimer <= 0)
            {
                PickNewTarget();
            }
        }
    }

    void PickNewTarget()
    {
        // Chọn điểm ngẫu nhiên xung quanh vị trí hiện tại
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        targetPosition = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);

        isWalking = true;
    }

    void StopWalking()
    {
        isWalking = false;
        waitTimer = waitTime; // Đặt lại đồng hồ nghỉ
    }

    void Produce()
    {
        if (data.productPrefab != null)
        {
            // Sinh ra trứng/sữa, nâng cao y=0.5 để không chìm xuống đất
            Instantiate(data.productPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            Debug.Log(data.animalName + " đã đẻ trứng!");
        }
    }
}
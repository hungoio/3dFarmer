using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 1.5f;

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // Điểm bắn ray (cao hơn mặt đất)
        Vector3 origin = transform.position + Vector3.up * 1f;

        // Hướng ray: chéo xuống phía trước
        Vector3 direction = (transform.forward + Vector3.down).normalized;

        // Debug để nhìn thấy ray
        Debug.DrawRay(origin, direction * interactDistance, Color.red, 1f);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, interactDistance))
        {
            FarmTile tile = hit.collider.GetComponent<FarmTile>();
            if (tile == null) return;

            if (tile.CanHarvest())
                tile.Harvest();
            else
                tile.PlantSeed();
        }
    }
}

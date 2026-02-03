using UnityEngine;
using UnityEngine.InputSystem;

public class PlantingSystem : MonoBehaviour
{
    public GameObject plantPrefab;
    private bool canPlant = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FarmLand"))
        {
            canPlant = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FarmLand"))
        {
            canPlant = false;
        }
    }

    void Update()
    {
        if (canPlant && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Plant();
        }
    }

    void Plant()
    {
        Instantiate(
            plantPrefab,
            transform.position + Vector3.forward,
            Quaternion.identity
        );
    }
}

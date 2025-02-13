using UnityEngine;
using UnityEngine.UI;

public class PickupSystem : MonoBehaviour
{
    public Transform handPosition;
    public RectTransform touchField;
    public Button dropButton;
    public float throwForce = 5f;

    private GameObject heldItem;

    void Start()
    {
        dropButton.gameObject.SetActive(false);
        dropButton.onClick.AddListener(DropItem);
    }

    void Update()
    {
        DetectTouch();
    }

    void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, 4f))
                {
                    if (hit.collider.CompareTag("Pickup") && heldItem == null)
                    {
                        PickupItem(hit.collider.gameObject);
                        return;
                    }
                }
            }
        }
    }

    void PickupItem(GameObject item)
    {
        heldItem = item;
        heldItem.transform.SetParent(handPosition);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        dropButton.gameObject.SetActive(true);
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            heldItem.transform.SetParent(null);
            rb.isKinematic = false;
            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
            heldItem = null;
            dropButton.gameObject.SetActive(false);
        }
    }
}

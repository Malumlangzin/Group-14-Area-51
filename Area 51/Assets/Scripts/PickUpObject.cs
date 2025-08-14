using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;

    // Unity Message
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform holdPoint)
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }

    public void Drop()
    {
        rb.useGravity = true;

        rb.isKinematic = false;
        transform.SetParent(null);
    }

    public void Throw(Vector3 impulse)
    {
        Drop();

        rb.isKinematic = false;
       
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        rb.MovePosition(targetPosition);
    }
}
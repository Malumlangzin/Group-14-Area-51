using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollection : MonoBehaviour
{
    public GameObject Modulator_Prefab;
    public GameObject InventoryContent;

    private void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Modulator")
        {
            Destroy(obj);
            Instantiate(Modulator_Prefab, Vector3.zero, Quaternion.identity, InventoryContent.transform);
        }
    }
}

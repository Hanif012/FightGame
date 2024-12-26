using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D other){
        other.transform.SetParent(null);
    }
}

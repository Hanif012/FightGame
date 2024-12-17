using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private PlatformDestination platformDestination;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;

    private int targetDestinationIndex;
    private Transform previousDestination;
    private Transform targetDestination;
    private float timeToDestination;
    private float elapsedTime;
    private bool isWaiting;
    void Start()
    {
        TargetNextDestination();
    }

    
    void Update()
    {   
        if (isWaiting) return;

        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeToDestination;
        transform.position = Vector3.Lerp(previousDestination.position, targetDestination.position, elapsedPercentage);
        
        if(elapsedPercentage >= 1){
            StartCoroutine(WaitAtDestination());
        }
    }

    private void TargetNextDestination(){
        previousDestination = platformDestination.GetDestination(targetDestinationIndex);
        targetDestinationIndex = platformDestination.GetNextDestinationIndex(targetDestinationIndex);
        targetDestination = platformDestination.GetDestination(targetDestinationIndex);

        elapsedTime = 0;

        float distanceToDestination = Vector3.Distance(previousDestination.position, targetDestination.position);
        timeToDestination = distanceToDestination / speed;
    }
    private IEnumerator WaitAtDestination(){
        isWaiting = true; 
        yield return new WaitForSeconds(waitTime); 
        isWaiting = false; 
        TargetNextDestination();
    }
}

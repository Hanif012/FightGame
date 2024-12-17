using UnityEngine;

public class PlatformDestination : MonoBehaviour
{
    
    public Transform GetDestination(int destinationIndex){
        return transform.GetChild(destinationIndex);
    }

    public int GetNextDestinationIndex(int currentDestinationIndex){
        int nextDestinationIndex = currentDestinationIndex + 1;

        if(nextDestinationIndex == transform.childCount){
            nextDestinationIndex = 0;
        }

        return nextDestinationIndex;
    }
}

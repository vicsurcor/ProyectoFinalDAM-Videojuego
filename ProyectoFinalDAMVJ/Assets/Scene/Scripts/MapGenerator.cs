using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public PlatformMap platformMap;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        GameObject currentPlatform = Instantiate(platformMap.normalPlatforms[0], Vector3.zero, Quaternion.identity);
        int branchesPlaced = 0;
        bool mergePlaced = false;
        bool changePlaced = false;

        for (int i = 1; i < platformMap.mapLength; i++)
        {
            Transform exitAttachPoint = currentPlatform.transform.Find("ExitAttachPoint");
            if (exitAttachPoint != null)
            {
                GameObject nextPlatform;
                if (i > 1 && i < platformMap.mapLength - 1 && !changePlaced && branchesPlaced < platformMap.numberOfBranches) 
                {
                    nextPlatform = Instantiate(platformMap.changePlatforms[Random.Range(0, platformMap.changePlatforms.Length)], exitAttachPoint.position, Quaternion.identity);
                    branchesPlaced++;
                    changePlaced = true;
                }
                else if (i < platformMap.mapLength - 1 && changePlaced && !mergePlaced)
                {
                    nextPlatform = Instantiate(platformMap.mergePlatforms[Random.Range(0, platformMap.mergePlatforms.Length)], exitAttachPoint.position, Quaternion.identity);
                    mergePlaced = true;
                }
                else
                {
                    nextPlatform = Instantiate(platformMap.normalPlatforms[Random.Range(0, platformMap.normalPlatforms.Length)], exitAttachPoint.position, Quaternion.identity);
                }

                // Attach the next platform to the previous platform's exit attach point
                Transform entryAttachPoint = nextPlatform.transform.Find("EntryAttachPoint");
                if (entryAttachPoint != null)
                {
                    nextPlatform.transform.position = exitAttachPoint.position;
                }

                currentPlatform = nextPlatform;

                // Handle additional exit attach points for branching
                if (i < platformMap.mapLength - 1) // Only handle additional exit points if not the last platform
                {
                    for (int j = 1; j <= branchesPlaced; j++)
                    {
                        Transform additionalExitAttachPoint = currentPlatform.transform.Find("ExitAttachPoint" + j);
                        if (additionalExitAttachPoint != null)
                        {
                            GameObject branchPlatform = Instantiate(platformMap.normalPlatforms[Random.Range(0, platformMap.normalPlatforms.Length)], additionalExitAttachPoint.position, Quaternion.identity);
                            branchPlatform.transform.position = additionalExitAttachPoint.position;
                        }
                    }
                }

                // Check for higher or lower exit attach points
                foreach (Transform attachPoint in currentPlatform.transform)
                {
                    if (attachPoint.name.StartsWith("ExitAttachPoint"))
                    {
                        if (attachPoint.position.y > currentPlatform.transform.position.y || attachPoint.position.y < currentPlatform.transform.position.y)
                        {
                            GameObject verticalPlatform = Instantiate(platformMap.normalPlatforms[Random.Range(0, platformMap.normalPlatforms.Length)], attachPoint.position, Quaternion.identity);
                            verticalPlatform.transform.position = attachPoint.position;
                        }
                    }
                }
            }
        }
    }
}


using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public PlatformMap platformMap;
    private int branchesMade = 0; 
    private int branchesMerged = 0;
    private int platPlaced = 0;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        Transform currentPoint = transform;

        for (int i = 0; i < platformMap.mapLength; i++)
        {
            currentPoint = PlaceNextPlatform(currentPoint);
            platPlaced++;
        }
    }

    private Transform PlaceNextPlatform(Transform currentPoint)
    {
        // Select a random platform from the normalPlatforms array
        GameObject platformPrefab = platformMap.normalPlatforms[Random.Range(0, platformMap.normalPlatforms.Length)];
        
        if (platformMap.numberOfBranches > 0 && branchesMade < platformMap.numberOfBranches && platPlaced > 1)
        {
            platformPrefab = platformMap.changePlatforms[Random.Range(0, platformMap.changePlatforms.Length)];
            branchesMade++;
        }
        else if (branchesMade > 0 && branchesMerged < branchesMade && platPlaced < platformMap.mapLength)
        {
            platformPrefab = platformMap.mergePlatforms[Random.Range(0, platformMap.mergePlatforms.Length)];
            branchesMerged++;
        }
        
        // Instantiate the platform at the current point's position
        GameObject newPlatform = Instantiate(platformPrefab, currentPoint.position, Quaternion.identity);

        // Assuming each platform has an ExitAttachPoint transform
        Transform exitAttachPoint = newPlatform.transform.Find("ExitAttachPoint");
        

        return exitAttachPoint;
    }
}

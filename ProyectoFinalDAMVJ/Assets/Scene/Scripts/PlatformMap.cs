using UnityEngine;

[CreateAssetMenu(fileName = "NewPlatformMap", menuName = "Platform Map")]
public class PlatformMap : ScriptableObject
{
    public GameObject[] normalPlatforms;
    public GameObject[] mergePlatforms;
    public GameObject[] changePlatforms;
    public int numberOfBranches;
    public int mapLength;
}

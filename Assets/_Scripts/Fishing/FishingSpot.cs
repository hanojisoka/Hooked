using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class FishingSpot : MonoBehaviour
{

    public float SquareSize = 10f;
    [SerializeField] private float lineSize = 10f;
    [SerializeField] private bool flipLine;
    [SerializeField] private GameObject fishingSpot;

    [Button]
    public void RandomizeSpawnPosition()
    {
        Vector3 randPos;
        if (flipLine)
            randPos = new Vector3(Random.Range(transform.position.x + lineSize, transform.position.x - lineSize), transform.position.y, transform.position.z);
        else
            randPos = new Vector3(transform.position.x, transform.position.y, Random.Range(transform.position.z + lineSize, transform.position.z - lineSize));
        fishingSpot.transform.position = randPos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(fishingSpot.transform.position, new Vector3(SquareSize, 0f, SquareSize));
        Gizmos.color = Color.cyan;
        if (flipLine)
            Gizmos.DrawLine(transform.position + Vector3.left * lineSize, transform.position + Vector3.right * lineSize);
        else
            Gizmos.DrawLine(transform.position + Vector3.forward * lineSize, transform.position + Vector3.back * lineSize);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationGround : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Array objek yang akan di-spawn
    public int spawnCount = 5; // Jumlah objek yang akan di-spawn
    public Vector2 spawnAreaMin; // Batas minimum area spawn (x, y)
    public Vector2 spawnAreaMax; // Batas maksimum area spawn (x, y)

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (objectsToSpawn.Length == 0)
        {
            Debug.LogWarning("Pastikan objek diatur!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(randomObject, randomPosition, Quaternion.identity);
        }
    }
}

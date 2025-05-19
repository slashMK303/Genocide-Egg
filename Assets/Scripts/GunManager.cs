using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] GameObject gunPrefab;

    Transform player;
    List<Vector2> gunPositions = new List<Vector2>();
    int spawnedGuns = 0;
    int maxGuns = 6; // Batas maksimum senjata yang bisa ditambahkan

    private void Start()
    {
        player = GameObject.Find("Player").transform;

        gunPositions.Add(new Vector2(-1.1f, 0.2f));
        gunPositions.Add(new Vector2(1.1f, 0.2f));

        gunPositions.Add(new Vector2(-1f, -0.5f));
        gunPositions.Add(new Vector2(1f, -0.5f));

        gunPositions.Add(new Vector2(-0.8f, 1f));
        gunPositions.Add(new Vector2(0.8f, 1f));

        AddGun();
        AddGun();
    }

    public void BuyGun()
    {
        AddGun();
    }


    void AddGun()
    {
        if (spawnedGuns >= maxGuns) return; // Cegah penambahan senjata jika sudah mencapai batas

        var pos = gunPositions[spawnedGuns];

        var newGun = Instantiate(gunPrefab, pos, Quaternion.identity);

        newGun.GetComponent<Gun>().SetOffset(pos);
        spawnedGuns++;
    }

    public int GetWeaponCount()
    {
        return spawnedGuns;
    }
}
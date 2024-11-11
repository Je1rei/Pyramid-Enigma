using UnityEngine;

public class TreasureFactory : MonoBehaviour
{
    public Treasure Create(Treasure prefab)
    {
        return Instantiate(prefab);
    }
}

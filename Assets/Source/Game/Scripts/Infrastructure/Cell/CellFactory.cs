    using UnityEngine;

    namespace Source.Game.Scripts
    {
        public class CellFactory : MonoBehaviour
        {
            public Cell Create(Cell prefab, Vector3Int position, Grid grid)
            {
                Cell cell = Instantiate(prefab);
                cell.Init(position, grid);

                return cell;
            }
        }
    }
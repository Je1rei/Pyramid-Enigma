using UnityEngine;

public class CompositeRootMainMenu : MonoBehaviour
{
    private void Awake()
    {
        // сюда прописать зависимости
    }

    private void RegisterServices()
    {
        ServiceLocator.Initialize();

        // ServiceLocator.Locator.Register...
    }

    private void Init() 
    {
        //Init всех сущностей
    }
}
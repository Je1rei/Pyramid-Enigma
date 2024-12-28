using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

    public static ServiceLocator Current { get; private set; }

    public ServiceLocator()
    {
        Current = this;
    }
        
    public TService Get<TService>() where TService : IService
    {
        if (_services.TryGetValue(typeof(TService), out IService service))
        {
            return (TService)service;
        }

        throw new Exception($"Service of type {typeof(TService)} not found");
    }

    public void Register<TService>(TService service) where TService : IService
    {
        if (_services.ContainsKey(typeof(IService)) == false)
        {
            _services[typeof(TService)] = service;
        }
        else
        {
            Debug.LogError($"Service {typeof(TService).Name} is already registered");
        }
    }
}
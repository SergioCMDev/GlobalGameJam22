using System;
using System.Collections.Generic;

public class ServiceLocator
{
    public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());
    private static ServiceLocator _instance;
    private readonly Dictionary<Type, object> _services;
    private readonly Dictionary<Type, object> _models;
    private ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
        _models = new Dictionary<Type, object>();
    }
    
    public void RegisterService<T>(T service)
    {
        var type = typeof(T);
        // Assert.IsFalse(_services.ContainsKey(type), 
        //     $"Service {type} already registered");
        if (_services.ContainsKey(type))
        {
            return;
        }
        _services.Add(type, service);
    }

    public T GetService<T>()
    {
        var type = typeof(T);
        if (!_services.TryGetValue(type, out var service))
        {
            throw new Exception($"Service {type} not found");
        }

        return (T) service;
    }

    /// <summary>
    /// Returns the script object inserted between '< >' 
    /// </summary>
    public T GetModel<T>()
    {
        var type = typeof(T);
        if (!_models.TryGetValue(type, out var model))
        {
            throw new Exception($"Model {type} not found");
        }

        return (T) model;
    }

    /// <summary>
    /// Saves the script object inserted as parameter
    /// </summary>
    public void RegisterModel<T>(T model)
    {
        var type = typeof(T);
        // Assert.IsFalse(_services.ContainsKey(type), 
        //     $"Service {type} already registered");
        if (_models.ContainsKey(type))
        {
            return;
        }
        _models.Add(type, model);
    }

    public void UnregisterService<T>()
    {
        var type = typeof(T);
        // Assert.IsFalse(_services.ContainsKey(type), 
        //     $"Service {type} already registered");
        if (!_services.ContainsKey(type))
        {
            return;
        }
        _services.Remove(type);
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static T GetRandomElement<T>(this List<T> list)
    {
        return list.ElementAt(Random.Range(0, list.Count));
    }
    
    public static IEnumerable<Type> GetAllChildrenOfClassOrNull<T>() where T : class
    {
        return Assembly.GetAssembly(typeof(T)).GetTypes().Where(t => t.IsSubclassOf(typeof(T)))
                .Where(ty => !ty.IsAbstract);
    }

    public static ReadOnlyCollection<T> GetReadonlyCollection<T>
        (this IEnumerable<T> originalCollection, Func<T, bool> filter = null) where T : class
    {
        filter ??= x => true;
        return originalCollection.Where(filter).ToList().AsReadOnly();
    }

    public static T GetRandom<T>([NotNull] this IEnumerable<T> collectionToCheck)
    {
        var toCheck = collectionToCheck as T[] ?? collectionToCheck.ToArray();
        return toCheck.ElementAt(Random.Range(0, toCheck.Length));
    }
    
    public static T GetRandomNonDuplicate<T>([NotNull] this IEnumerable<T> collectionToCheck,
        [NotNull] IEnumerable<T> duplicates, Func<T, bool> filter = null)
    {
        if (collectionToCheck == null) throw new ArgumentNullException(nameof(collectionToCheck));
        if (duplicates == null) throw new ArgumentNullException(nameof(duplicates));
        
        filter ??= x => true;
        var nonDuplicates =
            collectionToCheck.Where(t => !duplicates.Contains(t)).Where(filter).ToArray();

        return nonDuplicates.GetRandom();
    }
    
    public static T[] LoadScriptableObjects<T>() where T : ScriptableObject
    {
        var assets = Resources.LoadAll<T>("ScriptableObjects");
        return InstantiateAssets(assets);
    }
    
    private static T[] InstantiateAssets<T>(T[] array) where T : ScriptableObject
    {
        var instantiatedArray = new T[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            var newInstance = array[i].Clone();
            instantiatedArray[i] = newInstance;
        }

        return instantiatedArray;
    }
    
    private static T Clone<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null)
        {
            Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
            return (T)ScriptableObject.CreateInstance(typeof(T));
        }
 
        T instance = Object.Instantiate(scriptableObject);
        instance.name = scriptableObject.name; // remove (Clone) from name
        return instance;
    }
}
using System;
using System.Collections;
using System.Configuration.Assemblies;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enemies;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    public static EnemyLoader Current;
    
    public static readonly Dictionary<string, Type> AllEnemyTypesByName = new Dictionary<string, Type>();

    private static readonly Dictionary<string, EnemySpritePack> EnemySpritePacks =
        new Dictionary<string, EnemySpritePack>(); 
    
    private void Awake()
    {
        if (Current)
        {
            Destroy(gameObject);
            return;
        }
        
        Current = this;
    }
    
    void Start()
    {
        // Get the enemies
        var enemiesEnumerable =
            Extensions.GetAllChildrenOfClassOrNull<Enemy>();

        foreach (var type in enemiesEnumerable)
        {
            // Remove the 'Enemy' from the end of file name
            var enemyName = StripEnemyTypeName(type.Name);
            AllEnemyTypesByName.Add(enemyName, type);

            EnemySpritePacks.Add(enemyName, CreateSpritePack(enemyName));
        }
    }
    
    public EnemySpritePack GetEnemySpritePack(string enemyTypeName)
    {
        var strippedName = StripEnemyTypeName(enemyTypeName);
        if (EnemySpritePacks.TryGetValue(strippedName, out var enemySpritePack))
        {
            return enemySpritePack;
        }
        
        throw new Exception("No EnemySpritePack for " + strippedName + " found!");
    }

    private static string StripEnemyTypeName(string typeName)
    {
        return typeName.Remove(typeName.Length - 5, 5);
    }

    private EnemySpritePack CreateSpritePack(string enemyName)
    {
        var lowerCaseEnemyName = enemyName.ToLower();
        var idleSprites = Resources.LoadAll<Sprite>("Sprites/Enemies/" + lowerCaseEnemyName + "_idle");
        
        if (idleSprites.Length == 0)
            throw new Exception("No idles sprites for " + enemyName + " found!");

        return new EnemySpritePack(idleSprites);
    }
}

public class EnemySpritePack
{
    public readonly Sprite[] idleSprites;

    public EnemySpritePack(Sprite[] idleSprites)
    {
        this.idleSprites = idleSprites;
    }
}

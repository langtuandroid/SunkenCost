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

    public static readonly List<EnemyAsset> EnemyAssets = new List<EnemyAsset>();

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
        // Get the Enemy assets
        var enemyAssets = Extensions.LoadScriptableObjects<EnemyAsset>();
        
        // Get the Enemy classes by type
        var enemyTypes =
            Extensions.GetAllChildrenOfClassOrNull<Enemy>();

        foreach (var type in enemyTypes)
        {
            // Remove the 'Enemy' from the end of file name
            var enemyName = StripEnemyTypeName(type.Name);
            
            var correspondingAsset = enemyAssets.FirstOrDefault(asset => asset.name == enemyName);
            if (correspondingAsset is null) throw new Exception("No asset found for " + enemyName);

            correspondingAsset.Class = type;
            correspondingAsset.SpritePack = CreateSpritePack(enemyName);
            
            EnemyAssets.Add(correspondingAsset);
        }
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

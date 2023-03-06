using System.Collections;
using System.Configuration.Assemblies;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enemies;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static readonly List<string> EnemiesList = new List<string>();
    void Start()
    {
        // Get the enemies
        var enemiesEnumerable =
            Extensions.GetAllChildrenOfClassOrNull<Enemy>();

        foreach (var type in enemiesEnumerable)
        {
            // Remove the 'Enemy' from the end of file name
            var enemyName = type.FullName.Remove(type.FullName.Length - 5, 5);
            EnemiesList.Add(enemyName);
        }
    }
}

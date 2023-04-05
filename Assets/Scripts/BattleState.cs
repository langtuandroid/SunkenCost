using BattleScreen;
using BattleScreen.ActionsAndResponses;
using BattleScreen.Events;
using UnityEngine;

public class BattleState : MonoBehaviour
{
    public static BattleState current;

    [field: SerializeField] public BattleEvents BattleEvents { get; }
    [field: SerializeField] public EnemyController EnemyController { get; }
    
    [field: SerializeField] public DamageHandler DamageHandler { get; }
    
    private void Awake()
    {
        if (current)
        {
            Destroy(current.gameObject);
        }

        current = this;
    }
}
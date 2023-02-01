using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NextBattleButton : MonoBehaviour
{
    
    public void Click()
    {
        
        MainManager.Current.LoadNextBattle();
    }
}

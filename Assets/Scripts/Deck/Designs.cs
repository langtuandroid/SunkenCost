using Deck;
using UnityEngine;

#region Damage

#region Melee
public class Swordsman : CommonDesign
{
    protected override void Init()
    {
        Title = "Stab";
        Category = DesignCategory.Melee;
        Color = Color.cyan;
        AddStat(St.Damage, 6);
    }

    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 3);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Damage, 3);
   }
}

public class Ambush : UncommonDesign
{
    protected override void Init()
    {
        Title = "Ambush";
        Category = DesignCategory.Ambush;
        Color = new Color(0.2f, 0.35f, 0.38f);
        AddStat(St.Damage, 3);
        AddStat(St.DamageFlatModifier, 3);
    }

    protected override void FirstLevelUp()
    {
        ModifyStat(St.DamageFlatModifier, 2);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.DamageFlatModifier, 5);
    }
}

public class LoneWolf : RareDesign
{
    protected override void Init()
    {
        Title = "Lone Wolf";
        Category = DesignCategory.LoneWolf;
        Color = new Color(0.2f, 0.35f, 0.38f);
        AddStat(St.Damage, 60);
        AddStat(St.DamageFlatModifier, -10);
    }

    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 10);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Damage, 10);
    }
}

public class GrandFinalist : UncommonDesign
{
    protected override void Init()
    {
        Title = "Finalise";
        Category = DesignCategory.GrandFinalist;
        Color = new Color(0.2f, 0.35f, 0.38f);
        AddStat(St.Damage, 10);
        AddStat(St.DamagePlayer, -1);
    }

    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 2);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Damage, 3);
    }
}

#endregion

#region Ranged
public class Slinger : CommonDesign
{
    protected override void Init()
    {
        Title = "Hurl";
        Category = DesignCategory.Ranged;
        Color = Color.red;
        AddStat(St.Damage, 4);
        AddStat(St.MinRange, 1);
        AddStat(St.MaxRange, 1);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Damage, 2);
    }
}

public class Archer : CommonDesign
{
    protected override void Init()
    {
        Title = "Pelt";
        Category = DesignCategory.Ranged;
        Color = Color.red;
        AddStat(St.Damage, 3);
        AddStat(St.MinRange, 1);
        AddStat(St.MaxRange, 2);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Damage, 2);
    }
}

public class Marksman : UncommonDesign
{
    protected override void Init()
    {
        Title = "Snipe";
        Category = DesignCategory.Ranged;
        Color = Color.red;
        AddStat(St.Damage, 6);
        AddStat(St.MinRange, 3);
        AddStat(St.MaxRange, 3);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 2);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Damage, 3);
    }
}

public class AirRaid : UncommonDesign
{
    protected override void Init()
    {
        Title = "Raid";
        Category = DesignCategory.AirRaid;
        Color = Color.red;
        AddStat(St.Damage, 2);
        AddStat(St.MinRange, 1);
        AddStat(St.MaxRange, 2);
        AddStat(St.StatMultiplier, 2);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 2);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.StatMultiplier, 1);
    }
}

#endregion

#region Area
public class Stomp : CommonDesign
{
    protected override void Init()
    {
        Title = "Stomp";
        Category = DesignCategory.Area;
        Color = Color.yellow;
        AddStat(St.Damage, 1);
        AddStat(St.MaxRange, 1);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.UsesPerTurn, 1);
    }
}

public class Splatter : UncommonDesign
{
    protected override void Init()
    {
        Title = "Bomb";
        Category = DesignCategory.Area;
        Color = Color.yellow;
        AddStat(St.UsesPerTurn, 1);
        AddStat(St.Damage, 7);
        AddStat(St.MaxRange, 2);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 3);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.UsesPerTurn, 5);
    }
}
#endregion


#endregion

#region Boost

public class Boost : CommonDesign
{
    protected override void Init()
    {
        Title = "Rally";
        Category = DesignCategory.Boost;
        Color = Color.magenta;
        AddStat(St.Boost, 2);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Boost, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Boost, 2);
    }
}

public class StrikeZone : UncommonDesign
{
    protected override void Init()
    {
        Title = "Focus";
        Category = DesignCategory.StrikeZone;
        Color = new Color(0f, 0.59f, 0.71f);
        AddStat(St.Boost, 2);
    }
    
    protected override void FirstLevelUp()
    {
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Boost, 1);
    }
}

#endregion

#region Movement Modifiers

public class Impede : CommonDesign
{
    protected override void Init()
    {
        Title = "Impede";
        Category = DesignCategory.Block;
        Color = new Color(0.54f, 0.54f, 0.67f);
        AddStat(St.UsesPerTurn, 1);
        AddStat(St.MinRange, 0);
        AddStat(St.MaxRange, 0);
        AddStat(St.Block, 2);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Block, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.UsesPerTurn, 1);
    }
}

public class Reverse : RareDesign
{
    protected override void Init()
    {
        Title = "Reverse";
        Category = DesignCategory.Reverse;
        Color = new Color(0f, 0.72f, 0.55f);
        AddStat(St.MovementBoost, 0);
        AddStat(St.UsesPerTurn, 1);
    }

    protected override void FirstLevelUp()
    {
        ModifyStat(St.MovementBoost, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.UsesPerTurn, 1);
    }
}

public class Hop : UncommonDesign
{
    protected override void Init()
    {
        Title = "Hop";
        Category = DesignCategory.Hop;
        Color = Color.green;
        AddStat(St.Hop, 1);
    }
    
    protected override void FirstLevelUp()
    {
    }

    protected override void SecondLevelUp()
    {
    }
}


#endregion

#region Debuffs

public class Poison : CommonDesign
{
    protected override void Init()
    {
        Title = "Poison";
        Category = DesignCategory.Poison;
        Color = new Color(0.41f, 0.67f, 0f);
        AddStat(St.Poison, 3);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Poison, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Poison, 2);
    }
}

public class Cauterize : RareDesign
{
    protected override void Init()
    {
        Title = "Cauterize";
        Category = DesignCategory.Cauterize;
        Color = new Color(0.41f, 0.67f, 0f);
        AddStat(St.StatMultiplier, 1);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.StatMultiplier, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.StatMultiplier, 1);
    }
}

#endregion

public class Infirmary : UncommonDesign
{
    protected override void Init()
    {
        Title = "Rest";
        Category = DesignCategory.Infirmary;
        Color = new Color(0.67f, 0.2f, 0.27f);
        AddStat(St.HealPlayer, 1);
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.HealPlayer, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.HealPlayer, 1);
    }
}

public class Research : UncommonDesign
{
    protected override void Init()
    {
        Title = "Research";
        Category = DesignCategory.Research;
        Color = new Color(0.61f, 0.35f, 0.67f);
        AddStat(St.IntRequirement, 10);
        AddStat(St.Gold, 1);
    }
    
    protected override void FirstLevelUp()
    {
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Gold, 1);
    }
}
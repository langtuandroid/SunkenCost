using UnityEngine;

#region Damage

#region Melee
public class Swordsman : Design
{
    protected override void Init()
    {
        Title = "Swordsman";
        Category = DesignCategory.Melee;
        Color = Color.cyan;
        AddStat(St.Rarity, 1);
        AddStat(St.Damage, 6);
        
        base.Init();
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

public class LoneWolf : Design
{
    protected override void Init()
    {
        Title = "Lone Wolf";
        Category = DesignCategory.LoneWolf;
        Color = new Color(0.2f, 0.35f, 0.38f);
        AddStat(St.Rarity, 3);
        AddStat(St.Damage, 30);
        AddStat(St.DamageFlatModifier, -3);
        base.Init();
    }

    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 5);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Damage, 5);
    }
}
#endregion

#region Ranged
public class Slinger : Design
{
    protected override void Init()
    {
        Title = "Slinger";
        Category = DesignCategory.Ranged;
        Color = Color.red;
        AddStat(St.Rarity, 1);
        AddStat(St.Damage, 4);
        AddStat(St.MinRange, 1);
        AddStat(St.MaxRange, 1);

        base.Init();
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

public class Archer : Design
{
    protected override void Init()
    {
        Title = "Archer";
        Category = DesignCategory.Ranged;
        Color = Color.red;
        AddStat(St.Rarity, 1);
        AddStat(St.Damage, 3);
        AddStat(St.MinRange, 1);
        AddStat(St.MaxRange, 2);

        base.Init();
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

public class Marksman : Design
{
    protected override void Init()
    {
        Title = "Marksman";
        Category = DesignCategory.Ranged;
        Color = Color.red;
        AddStat(St.Rarity, 2);
        AddStat(St.Damage, 6);
        AddStat(St.MinRange, 3);
        AddStat(St.MaxRange, 3);

        base.Init();
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

#region Area
public class Stomp : Design
{
    protected override void Init()
    {
        Title = "Mortar";
        Category = DesignCategory.Area;
        Color = Color.yellow;
        AddStat(St.Rarity, 1);
        AddStat(St.Damage, 1);
        AddStat(St.MaxRange, 1);

        base.Init();
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

public class Splatter : Design
{
    protected override void Init()
    {
        Title = "Bomber";
        Category = DesignCategory.Area;
        Color = Color.yellow;
        AddStat(St.Rarity, 2);
        AddStat(St.UsesPerTurn, 1);
        AddStat(St.Damage, 5);
        AddStat(St.MaxRange, 2);

        base.Init();
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Damage, 2);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.UsesPerTurn, 1);
    }
}
#endregion


#endregion

#region Boost

public class Boost : Design
{
    protected override void Init()
    {
        Title = "Flag Bearer";
        Category = DesignCategory.Boost;
        Color = Color.magenta;
        AddStat(St.Rarity, 1);
        AddStat(St.Boost, 2);

        base.Init();
    }
    
    protected override void FirstLevelUp()
    {
        ModifyStat(St.Boost, 1);
    }

    protected override void SecondLevelUp()
    {
        ModifyStat(St.Boost, 1);
    }
}

public class StrikeZone : Design
{
    protected override void Init()
    {
        Title = "Strike Zone";
        Category = DesignCategory.StrikeZone;
        Color = new Color(0f, 0.59f, 0.71f);
        AddStat(St.Rarity, 2);
        AddStat(St.Boost, 2);

        base.Init();
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

public class Impede : Design
{
    protected override void Init()
    {
        Title = "Impede";
        Category = DesignCategory.Block;
        Color = new Color(0.54f, 0.54f, 0.67f);
        AddStat(St.Rarity, 1);
        AddStat(St.UsesPerTurn, 1);
        AddStat(St.MinRange, 0);
        AddStat(St.MaxRange, 0);
        AddStat(St.Block, 2);
        base.Init();
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

public class Reverse : Design
{
    protected override void Init()
    {
        Title = "Reverse";
        Category = DesignCategory.Reverse;
        Color = new Color(0f, 0.72f, 0.55f);
        AddStat(St.Rarity, 3);
        AddStat(St.MovementBoost, 0);
        AddStat(St.UsesPerTurn, 1);

        base.Init();
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

public class Hop : Design
{
    protected override void Init()
    {
        Title = "Hop";
        Category = DesignCategory.Hop;
        Color = Color.green;
        AddStat(St.Rarity, 2);
        AddStat(St.Hop, 1);

        base.Init();
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

public class Poison : Design
{
    protected override void Init()
    {
        Title = "Poison";
        Category = DesignCategory.Poison;
        Color = new Color(0.41f, 0.67f, 0f);
        AddStat(St.Rarity, 1);
        AddStat(St.Poison, 3);

        base.Init();
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

#endregion

public class Infirmary : Design
{
    protected override void Init()
    {
        Title = "Infirmary";
        Category = DesignCategory.Infirmary;
        Color = new Color(0.67f, 0.2f, 0.27f);
        AddStat(St.Rarity, 2);
        AddStat(St.HealPlayer, 1);
        base.Init();
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
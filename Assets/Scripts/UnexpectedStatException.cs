using System;
using Designs;

public class UnexpectedStatException : Exception
{
    private StatType _statType; 
    
    public override string Message => _statType.ToString();
    
    public UnexpectedStatException(StatType statType)
    { 
        _statType = statType;
    }
}
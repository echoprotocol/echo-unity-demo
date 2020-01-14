using System;


public class BattleContainer : IDisposable
{
    private readonly ulong buttleId;


    public ulong Id => buttleId;

    public BattleContainer(uint buttleId)
    {
        this.buttleId = buttleId;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
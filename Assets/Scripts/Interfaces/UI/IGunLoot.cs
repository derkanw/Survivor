using System;

public interface IGunLoot
{
    public event Action LootSpawned;
    public event Action LevelEnd;
    public void SetArsenalSize(int size);
    public void SpawnLoot();
}
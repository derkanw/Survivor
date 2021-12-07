using System;

public interface IGunLoot
{
    public event Action LootSpawned;
    public void SetArsenalSize(int size);
    public void SpawnLoot(string sceneName);
}
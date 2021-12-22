using System;

public interface IGameOverModel
{
    public event Action LevelEnd;

    public void Activate();
    public void ChangeScene();

}
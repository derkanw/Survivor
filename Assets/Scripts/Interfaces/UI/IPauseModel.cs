using System;

public interface IPauseModel : IModel
{
    public event Action LevelEnd;
    public void ChangeScene();
}
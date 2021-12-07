using System;

public interface IPauseModel
{
    public event Action Resume;
    public event Action SaveProgress;

    public void OnPause();
    public void OnGoToMenu();
}
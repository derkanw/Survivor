using System;

public interface IButtonModel
{
    public event Action GoToMenu;
    public event Action Pause;
    public event Action Resume;
    public event Action LooksStats;

    public void DisableButtons();
}
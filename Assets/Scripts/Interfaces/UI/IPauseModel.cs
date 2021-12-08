using System;

public interface IPauseModel : IModel
{
    public event Action SaveProgress;
}
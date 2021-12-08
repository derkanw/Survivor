using System;

public interface IModel
{
    public event Action Resume;
    public void ViewModel();
}
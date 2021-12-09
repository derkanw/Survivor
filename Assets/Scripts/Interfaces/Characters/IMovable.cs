using UnityEngine;

public interface IMovable
{
    public void MoveTo(Vector3 position);
    public void Stay();
}
public interface IStat
{
    // TODO : doesn't work correctly
    public float Value { get; }
    public void Init();
    public void Modify(int level);
}
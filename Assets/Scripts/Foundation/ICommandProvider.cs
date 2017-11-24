namespace Foundation
{
    public interface ICommandProvider
    {
        bool InputFromHuman { get; }
        bool Moving { get; }
        bool Attacking { get; }
        float XMovement { get; }
        float ZMovement { get; }
    }
}
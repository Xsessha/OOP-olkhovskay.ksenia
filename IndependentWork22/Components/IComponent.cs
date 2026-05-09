namespace IndependentWork22.Components
{
    public interface IComponent
    {
        decimal GetAmount();
        void Display(string indent = "");
    }
}
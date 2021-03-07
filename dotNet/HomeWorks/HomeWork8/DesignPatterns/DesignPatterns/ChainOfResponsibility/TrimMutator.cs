namespace DesignPatterns.ChainOfResponsibility
{
    public class TrimMutator : BaseMutator
    {
        public override string Mutate(string str)
        {
            str = str.Trim();
            
            return base.Mutate(str);
        }
    }
}
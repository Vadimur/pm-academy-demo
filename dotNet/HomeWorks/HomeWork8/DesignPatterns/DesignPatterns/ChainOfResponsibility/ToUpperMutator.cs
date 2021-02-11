namespace DesignPatterns.ChainOfResponsibility
{
    public class ToUpperMutator : BaseMutator
    {
        public override string Mutate(string str)
        {
            str = str.ToUpper();
            
            return base.Mutate(str);
        }
    }
}
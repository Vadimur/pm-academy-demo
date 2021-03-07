using System.Text.RegularExpressions;

namespace DesignPatterns.ChainOfResponsibility
{
    public class RemoveNumbersMutator : BaseMutator
    {
        public override string Mutate(string str)
        {
            str = Regex.Replace(str, @"[\d]", string.Empty);
            
            return base.Mutate(str);
        }
    }
}
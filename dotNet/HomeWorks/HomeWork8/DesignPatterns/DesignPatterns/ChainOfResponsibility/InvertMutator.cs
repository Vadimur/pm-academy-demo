using System;

namespace DesignPatterns.ChainOfResponsibility
{
    public class InvertMutator : BaseMutator
    {
        public override string Mutate(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            str = new string(charArray);
            
            return base.Mutate(str);
        }
    }
}
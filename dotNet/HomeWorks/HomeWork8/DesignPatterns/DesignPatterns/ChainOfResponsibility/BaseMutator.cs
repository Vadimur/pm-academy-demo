namespace DesignPatterns.ChainOfResponsibility
{
    public class BaseMutator : IStringMutator
    {
        private IStringMutator _nextHandler;
        
        public IStringMutator SetNext(IStringMutator next)
        {
            _nextHandler = next;
            return next;
        }
        
        public virtual string Mutate(string str)
        {
            return _nextHandler != null ? _nextHandler.Mutate(str) : str;
        }
    }
}
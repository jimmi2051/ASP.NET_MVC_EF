namespace SuperMarketMini.Servies.Validation
{
    public interface IValidationDictionary
    {
        void Clear();
        void AddError(string key, string errorMessage);
        bool IsValid { get; }
    }
}

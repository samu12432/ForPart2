namespace ForParts.IRepository.Supply
{
    public interface ISupplyExisting
    {
        Task<List<string>> GetExistingCodesAsync(List<string> codes);
    }
}

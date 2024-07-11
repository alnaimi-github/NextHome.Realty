namespace NextHome.Realty.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        IVillaNumberRepository VillaNumber { get; }
        IVillaImageRepository VillaImage { get; }

        Task SaveAsync();
    }
}

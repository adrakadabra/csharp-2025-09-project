namespace OrderPickingService.Services.Repositories.Abstractions;

public interface IStorageServiceClient
{
    Task AssemblyAsync(Guid orderNumber, string article, CancellationToken cancellationToken = default);
}
using OrderPickingService.Services.Repositories.Abstractions;

namespace OrderPickingService.Infrastructure.ExternalServices.Storage;

internal sealed class StorageServiceClient(
    HttpClient httpClient) : IStorageServiceClient
{
    public async Task AssemblyAsync(Guid orderNumber, string article, CancellationToken cancellationToken = default)
    {
        var url = $"/reservationitems/pickForAssembly?orderNumber={orderNumber}&article={article}";
        
        var response = await httpClient.PutAsync(url, null, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Storage service returned {response.StatusCode}: {error}");
        }
    }
}
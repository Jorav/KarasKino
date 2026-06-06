using System.Net.Http.Headers;
using System.Text.Json;

namespace KarasKino.Infrastructure.Movies;

public class TmdbClient(HttpClient httpClient, IOptions<TmdbConfiguration> config)
{
  private readonly TmdbConfiguration _config = config.Value;

  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  public async Task<TmdbFindResponse?> FindByImdbId(string imdbId, CancellationToken ct = default)
  {
    httpClient.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Bearer", _config.AccessToken);

    var response = await httpClient.GetAsync(
      $"{_config.BaseUrl}/find/{imdbId}?external_source=imdb_id", ct);

    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
      throw new InvalidOperationException("TMDB authentication failed. Check the access token configuration.");

    if (!response.IsSuccessStatusCode)
      return null;

    var json = await response.Content.ReadAsStringAsync(ct);
    return JsonSerializer.Deserialize<TmdbFindResponse>(json, JsonOptions);
  }
}

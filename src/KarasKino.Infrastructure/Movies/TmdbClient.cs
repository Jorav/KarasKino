using System.Net.Http.Headers;
using System.Text.Json;
using KarasKino.Infrastructure.Movies.Model;

namespace KarasKino.Infrastructure.Movies;

public class TmdbClient(HttpClient httpClient, IOptions<TmdbConfiguration> config)
{
  private readonly TmdbConfiguration _config = config.Value;

  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  private void SetAuthHeader()
  {
    httpClient.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Bearer", _config.AccessToken);
  }

  public async Task<TmdbFindResponse?> FindByImdbId(string imdbId, CancellationToken ct = default)
  {
    SetAuthHeader();
    var response = await httpClient.GetAsync(
      $"{_config.BaseUrl}/find/{imdbId}?external_source=imdb_id", ct);

    if (!response.IsSuccessStatusCode)
      return null;

    var json = await response.Content.ReadAsStringAsync(ct);
    return JsonSerializer.Deserialize<TmdbFindResponse>(json, JsonOptions);
  }

  public async Task<TmdbMovieDetailsResponse?> GetMovieDetails(int tmdbId, CancellationToken ct = default)
  {
    SetAuthHeader();
    var response = await httpClient.GetAsync(
      $"{_config.BaseUrl}/movie/{tmdbId}?append_to_response=credits", ct);

    if (!response.IsSuccessStatusCode)
      return null;

    var json = await response.Content.ReadAsStringAsync(ct);
    return JsonSerializer.Deserialize<TmdbMovieDetailsResponse>(json, JsonOptions);
  }
}

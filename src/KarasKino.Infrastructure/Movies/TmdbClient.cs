using System.Net.Http.Headers;
using System.Text.Json;
using KarasKino.Infrastructure.Movies.Model;
using Microsoft.Extensions.Options;

namespace KarasKino.Infrastructure.Movies;

// Primary constructor now accepts the framework factory
public class TmdbClient(IHttpClientFactory httpClientFactory, IOptions<TmdbConfiguration> config)
{
  private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Tmdb");
  private readonly TmdbConfiguration _config = config.Value;

  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  private void SetAuthHeader()
  {
    _httpClient.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Bearer", _config.AccessToken);
  }

  public async Task<TmdbFindResponse?> FindByImdbId(string imdbId, CancellationToken ct = default)
  {
    SetAuthHeader();
    var response = await _httpClient.GetAsync(
      $"{_config.BaseUrl}/find/{imdbId}?external_source=imdb_id", ct);

    if (!response.IsSuccessStatusCode)
      return null;

    var json = await response.Content.ReadAsStringAsync(ct);
    return JsonSerializer.Deserialize<TmdbFindResponse>(json, JsonOptions);
  }

  public async Task<TmdbMovieDetailsResponse?> GetMovieDetails(int tmdbId, CancellationToken ct = default)
  {
    SetAuthHeader();
    var response = await _httpClient.GetAsync(
      $"{_config.BaseUrl}/movie/{tmdbId}?append_to_response=credits", ct);

    if (!response.IsSuccessStatusCode)
      return null;

    var json = await response.Content.ReadAsStringAsync(ct);
    return JsonSerializer.Deserialize<TmdbMovieDetailsResponse>(json, JsonOptions);
  }
}

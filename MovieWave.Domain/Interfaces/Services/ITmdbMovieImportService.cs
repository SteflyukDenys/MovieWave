namespace MovieWave.Domain.Interfaces.Services;

public interface ITmdbMovieImportService
{
	Task ImportDataAsync();
}
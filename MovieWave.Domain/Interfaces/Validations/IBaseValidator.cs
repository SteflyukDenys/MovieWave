using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Validations;

public interface IBaseValidator<T> where T : class
{
	BaseResult ValidateOnNull(T model);
}
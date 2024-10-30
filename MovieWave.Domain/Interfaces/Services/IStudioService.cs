using MovieWave.Domain.Dto.Studio;
using MovieWave.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieWave.Domain.Interfaces.Services;

public interface IStudioService
{
	Task<CollectionResult<StudioDto>> GetAllAsync();
	Task<BaseResult<StudioDto>> GetByIdAsync(long id);
	Task<BaseResult<StudioDto>> CreateAsync(CreateStudioDto dto);
	Task<BaseResult<StudioDto>> UpdateAsync(UpdateStudioDto dto);
	Task<BaseResult<StudioDto>> DeleteAsync(long id);
}
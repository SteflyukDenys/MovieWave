using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.Tag;
using MovieWave.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieWave.Domain.Interfaces.Services;

public interface ITagService
{
	Task<CollectionResult<TagDto>> GetAllAsync();

	Task<BaseResult<TagDto>> GetByIdAsync(Guid id);

	Task<BaseResult<TagDto>> CreateAsync(CreateTagDto dto, FileDto seoImage);

	Task<BaseResult<TagDto>> UpdateAsync(UpdateTagDto dto, FileDto seoImage);

	Task<BaseResult<TagDto>> DeleteAsync(Guid id);
}
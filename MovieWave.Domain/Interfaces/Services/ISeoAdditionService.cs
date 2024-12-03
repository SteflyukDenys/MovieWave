using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieWave.Domain.Interfaces.Services;

public interface ISeoAdditionService
{
	//Task<BaseResult<string>> UploadSeoMetaImageAsync(string fileDto);

	Task<BaseResult> DeleteSeoMetaImageAsync(string imagePath);
}
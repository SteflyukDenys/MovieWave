using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Studio;

public record CreateStudioDto(string Name, string? LogoPath, string? Description, SeoAdditionDto SeoAddition);

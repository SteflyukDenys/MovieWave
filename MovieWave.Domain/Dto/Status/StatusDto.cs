using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto.Status;

public record StatusDto(long Id, string StatusType, string? Description);
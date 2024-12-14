﻿namespace MovieWave.Domain.Dto.Voice;

public class VoiceDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public string Locale { get; set; }

    public string? IconPath { get; set; }
}
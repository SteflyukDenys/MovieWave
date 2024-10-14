using Bogus;
using MovieWave.DAL.Seeders.DataGenerators;
using MovieWave.Domain.Entity;
using System;
using System.Collections.Generic;

public static class TagDataGenerator
{
	public static List<Tag> GenerateTags(int parentCount, int childCountPerParent)
	{
		var parentTags = new Faker<Tag>()
			.RuleFor(t => t.Id, f => Guid.NewGuid())
			.RuleFor(t => t.Name, f => f.Lorem.Word())
			.RuleFor(t => t.Description, f => f.Lorem.Sentence())
			.RuleFor(t => t.IsGenre, f => f.Random.Bool())
			.RuleFor(t => t.SeoAddition, _ => SeoAdditionDataGenerator.GenerateSeoAddition())
			.Generate(parentCount);

		var allTags = new List<Tag>(parentTags);

		foreach (var parent in parentTags)
		{
			var childTags = new Faker<Tag>()
				.RuleFor(t => t.Id, f => Guid.NewGuid())
				.RuleFor(t => t.Name, f => f.Lorem.Word())
				.RuleFor(t => t.Description, f => f.Lorem.Sentence())
				.RuleFor(t => t.IsGenre, f => f.Random.Bool())
				.RuleFor(t => t.ParentId, _ => parent.Id)
				.RuleFor(t => t.SeoAddition, _ => SeoAdditionDataGenerator.GenerateSeoAddition())
				.Generate(childCountPerParent);

			allTags.AddRange(childTags);
		}

		return allTags;
	}
}
﻿using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class SubscriptionPlan
{
	public SubscriptionLevel Name { get; set; } // enum
	public string? Description { get; set; }
	public decimal PricePerMonth { get; set; }
	public int MaxDevices { get; set; }
	public VideoQuality VideoQuality { get; set; } // enum

	// One-to-Many
	public ICollection<UserSubscription> UserSubscriptions { get; set; }
}
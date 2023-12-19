﻿using Microsoft.Extensions.Logging;
using Piranha.AttributeBuilder;
using Piranha.Models;
using PiranhaCMS.Validators.Helpers;
using PiranhaCMS.Validators.Models;
using PiranhaCMS.Validators.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PiranhaCMS.Validators.Services;

public class SiteValidatorService : ISiteValidatorService
{
	private IEnumerable<Type> siteTypes;
	private IDictionary<string, IEnumerable<PageValidatorModel>> siteValidatorCollection = new Dictionary<string, IEnumerable<PageValidatorModel>>();

	public void Initialize(Assembly modelsAssembly)
	{
		var types = modelsAssembly.ExportedTypes;

		siteTypes = types.Where(x =>
			x.GetTypeInfo().GetCustomAttributes().Any(y => y is SiteTypeAttribute));

		siteValidatorCollection = ValidatorHelpers.GetPageTypeValidators(siteTypes);
	}

	public void Validate(SiteContentBase model, ILogger logger)
	{
		if (model == null) return;

		try
		{
			if (!siteValidatorCollection.Any() ||
				!siteValidatorCollection.ContainsKey(model.TypeId)) return;

			foreach (var region in siteValidatorCollection[model.TypeId])
			{
				ValidatorHelpers.ValidateRegion(model, model.TypeId, region, siteValidatorCollection);
			}
		}
		catch (ValidationException)
		{
			throw;
		}
		catch (Exception e)
		{
			logger.LogError(e, $"Unhandled exception occured while validating: {e.Message}");
			throw new ValidationException($"Unhandled exception occured while validating: {e.Message}");
		}
	}
}

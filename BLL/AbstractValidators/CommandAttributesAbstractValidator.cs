using BLL.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper.Internal;
using FluentValidation;
using BLL.Attributes.Interfaces;

namespace BLL.AbstractValidators
{
	public abstract class CommandAttributesAbstractValidator<TRequest, TResponse> :
		CommandAbstractValidator<TRequest, TResponse>
		where TRequest : ICommand<TResponse>
	{
		protected CommandAttributesAbstractValidator()
		{
			PropertyInfo[] props = typeof(TRequest).GetProperties();

			IEnumerable<PropertyInfo> propsWithAttr = props.Where(x =>
				x.GetCustomAttributes().Any(y => y is IExValidationAttribute || y is ValidationAttribute));

			Type entityType = typeof(TRequest);
			foreach (PropertyInfo info in propsWithAttr)
			{
				List<ValidationAttribute> attributes = info.GetCustomAttributes().Where(x =>
						x.GetType().GetTypeInheritance().Any(i => i == typeof(ValidationAttribute))).ToList()
					.ConvertAll(x => (ValidationAttribute)x);

				Type propertyType = info.PropertyType;
				if (propertyType == typeof(DateTime))
				{
					AddRules<DateTime>(entityType, info, attributes);
				}
				else if (propertyType == typeof(bool))
				{
					AddRules<bool>(entityType, info, attributes);
				}
				else if (propertyType == typeof(Guid))
				{
					AddRules<Guid>(entityType, info, attributes);
				}
				else if (propertyType == typeof(int))
				{
					AddRules<int>(entityType, info, attributes);
				}
				else if (propertyType == typeof(short))
				{
					AddRules<short>(entityType, info, attributes);
				}
				else if (propertyType == typeof(string))
				{
					AddRules<string>(entityType, info, attributes);
				}
				else if (propertyType == typeof(string[]))
				{
					AddRules<string[]>(entityType, info, attributes);
				}
				else if (propertyType == typeof(decimal))
				{
					AddRules<decimal>(entityType, info, attributes);
				}
				else if (propertyType == typeof(DateTime?))
				{
					AddRules<DateTime?>(entityType, info, attributes);
				}
				else if (propertyType == typeof(bool?))
				{
					AddRules<bool?>(entityType, info, attributes);
				}
				else if (propertyType == typeof(Guid?))
				{
					AddRules<Guid?>(entityType, info, attributes);
				}
				else if (propertyType == typeof(int?))
				{
					AddRules<int?>(entityType, info, attributes);
				}
				else if (propertyType == typeof(short?))
				{
					AddRules<short?>(entityType, info, attributes);
				}
				else if (propertyType == typeof(string))
				{
					AddRules<string>(entityType, info, attributes);
				}
				else if (propertyType == typeof(string[]))
				{
					AddRules<string[]>(entityType, info, attributes);
				}
				else if (propertyType == typeof(decimal?))
				{
					AddRules<decimal?>(entityType, info, attributes);
				}
				else
				{
					throw new NotImplementedException(
						$"Validator can't validate property '{info.Name}' of class '{typeof(TRequest)}' " +
						$"because can't process '{propertyType.Name}' type");
				}
			}
		}

		private void AddRules<T>(Type entityType, PropertyInfo info, List<ValidationAttribute> attributes)
		{
			ParameterExpression arg = Expression.Parameter(entityType, "x");
			MemberExpression property = Expression.Property(arg, info.Name);
			Expression<Func<TRequest, T>> exp = Expression.Lambda<Func<TRequest, T>>(property, arg);

			foreach (ValidationAttribute attribute in attributes)
			{
				if (string.IsNullOrEmpty(attribute.ErrorMessage))
				{
					attribute.ErrorMessage = "Unknown";
				}

				RuleFor(exp).Must((command, _) => attribute.IsValid(info.GetValue(command)))
					.WithErrorCode(attribute.ErrorMessage).OverridePropertyName(info.Name);
			}
		}
	}
}

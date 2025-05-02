using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using Core.Utilities.Messages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {

            //defensive coding
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
                throw new System.Exception(AspectMessages.WrongValidationType);

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];

            foreach (var argument in invocation.Arguments)
            {
                if (argument is IEnumerable<object> entities && argument.GetType().IsGenericType)
                {
                    foreach (var entity in entities)
                    {
                        if (entity.GetType() == entityType)
                        {
                            ValidationTool.Validate(validator, entity);
                        }
                    }
                }
                else if (argument != null && argument.GetType() == entityType)
                {
                    ValidationTool.Validate(validator, argument);
                }
            }
        }

    }
}

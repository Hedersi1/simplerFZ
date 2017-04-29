using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.MVC.Business.Util
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CompareDifferentAttribute : ValidationAttribute
    {
        private string OtherProperty { get; set; }

        public CompareDifferentAttribute(string otherProperty)
        {
            this.OtherProperty = otherProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherValue = validationContext.ObjectType.GetProperty(OtherProperty).GetValue(validationContext.ObjectInstance);

            if (value.Equals(otherValue))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.CustomValidators
{
    public class DateValidation : ValidationAttribute
    {
        public string StartDateVar { get; set; }
        public string EndDateVar { get; set; }


        private DateTime StartDate;
        private DateTime EndDate;

        public DateValidation(string _StartDateVar, string _EndaDateVar)
        {
            StartDateVar = _StartDateVar;
            EndDateVar = _EndaDateVar;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(StartDateVar);
            if (property == null)
                throw new ArgumentException("Property with this name not found");

            StartDate = (DateTime)property.GetValue(validationContext.ObjectInstance);

            property = validationContext.ObjectType.GetProperty(EndDateVar);
            if (property == null)
                throw new ArgumentException("Property with this name not found");

            EndDate = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (StartDate < DateTime.Now)
            {
                return new ValidationResult($"A data inicial deve ser maior que a data atual");
            }
            else if (StartDate >= DateTime.Now.AddDays(7))
            {
                return new ValidationResult($"A data inicial não deve ultrapassar: {DateTime.Now.AddDays(7)}");
            }
            else if (EndDate <= StartDate)
            {
                return new ValidationResult("A data de fim deve ser maior que data inicial");
            }
            else if (EndDate > DateTime.Now.AddDays(7))
            {
                return new ValidationResult($"A data de fim não deve ultrapassar: {StartDate.AddDays(7)}");
            }

            return ValidationResult.Success;
        }
    }
}
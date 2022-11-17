namespace ci_automation_apitester.Objects
{
    public class CustomDataAnnotations
    {
        public class RequiredKey : ValidationAttribute
        {
            public int responseCode;
            private string _errorMsg400;
            private string _errorMsg500;

            public RequiredKey(int newResponseCode)
                //: base("Value cannot be null. (Parameter '{0}')")
            {
                responseCode = newResponseCode;
            }
            public bool AllowEmptyStrings { get; set; }
            public int ResponseCode { get { return responseCode; } }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }

                return ValidationResult.Success;
            }
        }
        public class Required500 : ValidationAttribute
        {
            public int responseCode;

            public Required500(int newResponseCode)
                : base("Value cannot be null. (Parameter '{0}')")
            {
                responseCode = newResponseCode;
            }
            public bool AllowEmptyStrings { get; set; }
            public int ResponseCode { get { return responseCode; } }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }

                return ValidationResult.Success;
            }
        }
        public class BlankValues : ValidationAttribute
        {
            public int responseCode;

            public BlankValues(int newResponseCode)
                : base("Value cannot be null. (Parameter '{0}')")
            {
                responseCode = newResponseCode;
            }
            public bool AllowEmptyStrings { get; set; }
            public int ResponseCode { get { return responseCode; } }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }

                return ValidationResult.Success;
            }
        }
        public class ExcludeChar : ValidationAttribute
        {
            public int responseCode;
            private readonly string _chars;

            public ExcludeChar(int newResponseCode, string chars)
                : base("{0} contains invalid character.")
            {
                responseCode = newResponseCode;
                _chars = chars;
            }
            public int ResponseCode { get { return responseCode; } }

            protected new ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    for (int i = 0; i < _chars.Length; i++)
                    {
                        var valueAsString = value.ToString();
                        if (valueAsString.Contains(_chars[i]))
                        {
                            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                            return new ValidationResult(errorMessage);
                        }
                    }
                }
                return ValidationResult.Success;
            }
        }
    }
}

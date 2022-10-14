using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary1.Model
{
    public class FieldValidation
    {
        public int FieldValidationID { get; set; }
        public string FieldName { get; set; }
        public string RegEx { get; set; }
        public string FormName { get; set; }
        public Nullable<bool> isRequired { get; set; }
        public string DisplayName { get; set; }
        public string FieldValidationType { get; set; }
        public string RequiredMessage { get; set; }
        public string RegexMessage { get; set; }
    }

    public class ModelValidateErrorMessage
    {
        public string Type { get; set; }
        public string FieldName { get; set; }
        public int ReasonCode { get; set; }
        public string Message { get; set; }
    }
}

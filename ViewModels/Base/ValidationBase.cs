using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using BookLibrary1.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using BookLibrary1.Model;

namespace BookLibrary1.ViewModels.Base
{
    public class ValidationBase : ObservableValidator, INotifyScrollToProperty, INotifyDataErrorInfo
    {
        #region Properties
        // Dictionary to collect errors of model
        public Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();
        //public Dictionary<string, List<string>> _dbvalidationErrors = new Dictionary<string, List<string>>();

        // INotifyDataErrorInfo - Event
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // Scroll To Event Property
        public event ScrollToPropertyHandler ScrollToProperty;

        // Lock for async work
        private readonly object _lock = new object();
        public bool HasErrors => _errors.Any(propErrors => propErrors.Value != null && propErrors.Value.Count > 0);
        public bool HasFoucsErrors => _validationErrors.Any(propErrors => propErrors.Value != null && propErrors.Value.Count > 0);
        // Return first invalid property name
        public string GetFirstInvalidPropertyName
        {
            get
            {
                if (!HasErrors)
                {
                    return string.Empty;
                }

                return _errors.Select(x => x.Key).FirstOrDefault();
            }
        }

        #endregion

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (_errors.ContainsKey(propertyName) && (_errors[propertyName] != null) && _errors[propertyName].Count > 0)
                {
                    return _errors[propertyName].ToList();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return _errors.SelectMany(err => err.Value.ToList());
            }
        }

        public void OnErrorsChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                ScrollToProperty(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public void ValidatePropertyOnFoucsOut(object value, [CallerMemberName] string propertyName = null)
        {
            lock (_lock)
            {
                ValidationContext validationContext = new ValidationContext(this, null)
                {
                    MemberName = propertyName
                };
                List<ValidationResult> validationResults = new List<ValidationResult>();
                Validator.TryValidateProperty(value, validationContext, validationResults);
                //_validationErrors.Clear();
                if (_validationErrors.ContainsKey(propertyName))
                {
                    _validationErrors.Remove(propertyName);
                }

                HandleValidation(validationResults);
            }
        }
        public bool ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            lock (_lock)
            {
                bool isValid = false;
                try
                {
                    ValidationContext validationContext = new ValidationContext(this, null)
                    {
                        MemberName = propertyName
                    };
                    List<ValidationResult> validationResults = new List<ValidationResult>();
                    isValid = Validator.TryValidateProperty(value, validationContext, validationResults);

                    //clear previous _errors from tested property  
                    if (_errors.ContainsKey(propertyName))
                    {
                        _errors.Remove(propertyName);
                    }

                    OnErrorsChanged(propertyName);
                    HandleValidationResults(validationResults);
                }
                catch (TimeoutException tmx)
                {
                    LogError.TrackError(tmx, $"ValidateProperty {propertyName}");
                }
                catch (Exception ex)
                {
                    LogError.TrackError(ex, $"ValidateProperty {propertyName}");
                }

                return isValid;
            }
        }

        public void ValidateListOfProperties(List<KeyValuePair<string, string>> list)
        {
            foreach (KeyValuePair<string, string> obj in list)
            {
                ValidateProperty(obj.Value, obj.Key);
            }
        }
        public void Validate()
        {
            try
            {
                lock (_lock)
                {
                    ValidationContext validationContext = new ValidationContext(this, null);
                    List<ValidationResult> validationResults = new List<ValidationResult>();
                    Validator.TryValidateObject(this, validationContext, validationResults, true);

                    //clear all previous _errors  
                    List<string> propNames = _errors.Keys.ToList();
                    _errors.Clear();
                    //propNames.ForEach(pn => OnErrorsChanged(pn));
                    foreach (string propertyName in propNames)
                    {
                        OnErrorsChanged(propertyName);
                    }
                    HandleValidationResults(validationResults);
                }
            }
            catch (TimeoutException tmx)
            {
                LogError.TrackError(tmx, "ValidationBase.Validate()");
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "ValidationBase.Validate()");
            }
        }
        public void RemoveErrors()
        {
            lock (_lock)
            {
                //clear all previous _errors  
                List<string> propNames = _errors.Keys.ToList();
                _errors.Clear();
                //propNames.ForEach(pn => OnErrorsChanged(pn));
                foreach (string propertyName in propNames)
                {
                    OnErrorsChanged(propertyName);
                }
                // HandleValidationResults(validationResults);
            }
        }
        public void RemoveError(string propName)
        {
            if (_errors.ContainsKey(propName))
            {
                _errors.Remove(propName);
            }

            OnErrorsChanged(propName);
        }
        public void RefreshErrors()
        {

            //add _errors to dictionary and inform binding engine about _errors  
            foreach (KeyValuePair<string, List<string>> prop in _errors)
            {
                OnErrorsChanged(prop.Key);
            }
        }
        private void HandleValidation(List<ValidationResult> validationResults)
        {
            //Group validation results by property names  
            IEnumerable<IGrouping<string, ValidationResult>> resultsByPropNames = from res in validationResults
                                                                                  from mname in res.MemberNames
                                                                                  group res by mname into g
                                                                                  select g;

            //add _errors to dictionary and inform binding engine about _errors  
            foreach (IGrouping<string, ValidationResult> prop in resultsByPropNames)
            {
                List<string> messages = prop.Select(r => r.ErrorMessage).ToList();
                _validationErrors.Add(prop.Key, messages);
            }
        }
        private void HandleValidationResults(List<ValidationResult> validationResults)
        {
            //Group validation results by property names  
            IEnumerable<IGrouping<string, ValidationResult>> resultsByPropNames = from res in validationResults
                                                                                  from mname in res.MemberNames
                                                                                  group res by mname into g
                                                                                  select g;

            //add _errors to dictionary and inform binding engine about _errors  
            foreach (IGrouping<string, ValidationResult> prop in resultsByPropNames)
            {
                List<string> messages = prop.Select(r => r.ErrorMessage).ToList();
                _errors.TryAdd(prop.Key, messages);
                OnErrorsChanged(prop.Key);
            }
        }

        #region Db validation

        public ModelValidateErrorMessage ValidateModelProperty<V>(FieldValidation ValidationRule, V Property, bool IsPartnerFilter = false)
        {
            try
            {

                try
                {
                    if (Property != null)
                    {
                        string PropertyValue = string.Empty;
                        PropertyValue = Convert.ToString(Property);
                        bool RegExSuccess = true;
                        bool RequiredSuccess = true;
                        if (ValidationRule.RegEx != null && !string.IsNullOrWhiteSpace(PropertyValue))
                        {
                            RegExSuccess = Regex.Match(PropertyValue, ValidationRule.RegEx).Success;
                        }

                        if (ValidationRule.isRequired.Value)
                        {
                            RequiredSuccess = !string.IsNullOrEmpty(PropertyValue) && !string.IsNullOrWhiteSpace(PropertyValue);
                        }

                        // RequiredSuccess = !string.IsNullOrEmpty(PropertyValue);
                        if (!RegExSuccess || !RequiredSuccess)
                        {
                            //ReasonCodesForWebAPI ReasonCode = 0;
                            if (!RequiredSuccess)
                            {
                                //ReasonCode = (IsPartnerFilter == true) ? FiNext.Core.ReasonCodesForWebAPI.MissingPartnerSpecificField : FiNext.Core.ReasonCodesForWebAPI.MissingField;
                                return new ModelValidateErrorMessage() { Type = "", Message = ValidationRule.RequiredMessage, FieldName = ValidationRule.FieldName };
                            }
                            else
                            {
                                //ReasonCode = (IsPartnerFilter == true) ? FiNext.Core.ReasonCodesForWebAPI.InvalidPartnerSpecifiedField : FiNext.Core.ReasonCodesForWebAPI.InvalidField;
                                return new ModelValidateErrorMessage() { Type = "", Message = ValidationRule.RegexMessage, FieldName = ValidationRule.FieldName };
                            }
                        }

                    }

                }

                catch (Exception Ex)
                {
                    LogError.TrackError(Ex, "ValidationBase");
                    throw Ex;
                }
            }
            catch (Exception Ex)
            {
                LogError.TrackError(Ex, "ValidationBase");
                //new ExceptionHandler(ex);
            }
            return null;
        }
        public void ValidatePropertyFromDatabase(List<FieldValidation> rules, object value, [CallerMemberName] string propertyName = null)
        {
            if (rules != null)
            {
                FieldValidation rule = rules.Where(x => x.FieldName == propertyName).FirstOrDefault();
                if (rule != null)
                {
                    //object v = new object();
                    if (_errors.ContainsKey(rule.FieldName))
                    {
                        _errors.Remove(rule.FieldName);
                    }

                    OnErrorsChanged(rule.FieldName);

                    ModelValidateErrorMessage error = ValidateModelProperty<object>(rule, value);
                    if (error != null)
                    {
                        _errors.TryAdd(rule.FieldName, new List<string> { error.Message });
                        OnErrorsChanged(rule.FieldName);

                    }
                }
            }

        }
        public void ValidateByFormName(List<FieldValidation> rules, object value)
        {
            //object v = new object();
            List<ModelValidateErrorMessage> errors = ValidateAllModel<object>(rules, value);
            foreach (ModelValidateErrorMessage i in errors)
            {
                if (_errors.ContainsKey(i.FieldName))
                {
                    _errors.Remove(i.FieldName);
                }

                _errors.TryAdd(i.FieldName, new List<string> { i.Message });
                OnErrorsChanged(i.FieldName);

            }
        }
        public List<ModelValidateErrorMessage> ValidateAllModel<V>(List<FieldValidation> ValidationRules, V ObjectContainingData, bool IsPartnerFilter = false)
        {
            List<ModelValidateErrorMessage> lstErrorMsgs = new List<ModelValidateErrorMessage>();
            try
            {
                if (ObjectContainingData != null)
                {
                    foreach (FieldValidation PropertyInfo in ValidationRules)
                    {
                        System.Reflection.PropertyInfo Property = null;
                        if (ObjectContainingData.GetType().GetProperty(PropertyInfo.FieldName) != null)
                        {
                            Property = ObjectContainingData.GetType().GetProperty(PropertyInfo.FieldName);
                        }

                        try
                        {
                            if (Property != null)
                            {
                                string PropertyValue = string.Empty;

                                if (!Property.PropertyType.Namespace.StartsWith("System.Collections"))
                                {
                                    PropertyValue = Property.GetValue(ObjectContainingData) != null
                                                    ? Convert.ToString(Property.GetValue(ObjectContainingData, null))
                                                    : string.Empty;

                                    //if (Property.Name == "DateofBirth" || Property.Name == "HireDate")
                                    //{
                                    //    if (!string.IsNullOrWhiteSpace(PropertyValue))
                                    //    {
                                    //        DateTime date = Convert.ToDateTime(PropertyValue);
                                    //        PropertyValue = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    //    }
                                    //}
                                }
                                bool RegExSuccess = true;
                                bool RequiredSuccess = true;

                                if (PropertyInfo.RegEx != null && !string.IsNullOrWhiteSpace(PropertyValue))
                                {
                                    RegExSuccess = Regex.Match(PropertyValue, PropertyInfo.RegEx).Success;
                                }

                                if (PropertyInfo.isRequired.Value)
                                {
                                    RequiredSuccess = !string.IsNullOrEmpty(PropertyValue) && !string.IsNullOrWhiteSpace(PropertyValue);
                                }

                                if (!RegExSuccess || !RequiredSuccess)
                                {
                                    //ReasonCodesForWebAPI ReasonCode = 0;
                                    if (!RequiredSuccess)
                                    {
                                        //ReasonCode = (IsPartnerFilter == true) ? FiNext.Core.ReasonCodesForWebAPI.MissingPartnerSpecificField : FiNext.Core.ReasonCodesForWebAPI.MissingField;
                                        lstErrorMsgs.Add(new ModelValidateErrorMessage() { Type = "", Message = PropertyInfo.RequiredMessage, FieldName = Property.Name });
                                    }
                                    else
                                    {
                                        //ReasonCode = (IsPartnerFilter == true) ? FiNext.Core.ReasonCodesForWebAPI.InvalidPartnerSpecifiedField : FiNext.Core.ReasonCodesForWebAPI.InvalidField;
                                        lstErrorMsgs.Add(new ModelValidateErrorMessage() { Type = "", Message = PropertyInfo.RegexMessage, FieldName = Property.Name });
                                    }
                                }

                            }

                        }

                        catch (Exception Ex)
                        {
                            LogError.TrackError(Ex, "ValidationBase");
                            throw Ex;

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                LogError.TrackError(Ex, "ValidationBase");
                //new ExceptionHandler(ex);
            }
            return lstErrorMsgs;
        }
        #endregion
        #region Invoke Scroll To Property
        /// <summary>
        /// Invoke scroll to property event
        /// </summary>
        /// <param name="PropertyName">PropertyName</param>
        protected void InvokeScrollToProperty(string PropertyName)
        {
            ScrollToProperty?.Invoke(PropertyName);
        }

        public void ScrollToControlProperty(string PropertyName)
        {
            InvokeScrollToProperty(PropertyName);
        }
        #endregion
    }
}

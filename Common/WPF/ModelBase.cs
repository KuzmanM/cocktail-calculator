using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Common.WPF
{
    /// <summary>
    /// Implements base functionality for WPF view model
    /// </summary>
    public class ModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void RisePropertyChanged([CallerMemberName] string name = "")
        {
            OnPropertyChanged(name);
        }

        #endregion

        #region Validation errors management

        /// <summary>
        /// Contains current errors
        /// </summary>
        private Dictionary<string, List<string>> _errors;

        /// <summary>
        /// Gets all item validation errors
        /// </summary>
        public virtual string ValidationErrors
        {
            get
            {
                if (_errors == null || !_errors.Any())
                    return null;

                string result = string.Join(Environment.NewLine, _errors.SelectMany(kv => kv.Value));
                return result;
            }
        }

        /// <summary>
        /// Add error to the item error state
        /// </summary>
        /// <param name="error">Error text</param>
        /// <param name="propertyName">Property name or empty string for the item global error. The default value is the name of the caller property/method.</param>
        /// <param name="notify">True to rise ErrorsChanged and PropertyChanged events</param>
        public void AddError(string error, [CallerMemberName] string propertyName = "", bool notify = true)
        {
            if (_errors == null)
                _errors = new Dictionary<string, List<string>>();

            List<string> propertyErrorsList;
            if (!_errors.TryGetValue(propertyName, out propertyErrorsList))
            {
                propertyErrorsList = new List<string>();
                _errors.Add(propertyName, propertyErrorsList);
            }

            if (!propertyErrorsList.Contains(error))
            {
                propertyErrorsList.Add(error);

                if (notify)
                    RiseErrorsChanged(propertyName);
            }
        }

        /// <summary>
        /// Remove error from the item error state
        /// </summary>
        /// <param name="error">Error text</param>
        /// <param name="propertyName">Property name or empty string for the item global error. The default value is the name of the caller property/method.</param>
        /// <param name="notify">True to rise ErrorsChanged and PropertyChanged events</param>
        /// <remarks>If the propertyName is empty string and list of global errors not exists. Error will be deleted from any existing errors list</remarks>
        public void RemoveError(string error, [CallerMemberName]  string propertyName = "", bool notify = true)
        {
            if (_errors == null)
                return;

            bool removed = false;
            List<string> propertyErrorsList;
            if (_errors.TryGetValue(propertyName, out propertyErrorsList))
            {
                // Remove the error from the specific property or global error list
                removed = propertyErrorsList.Remove(error);
                if (!propertyErrorsList.Any())
                    _errors.Remove(propertyName);
            }
            else if (string.IsNullOrEmpty(propertyName))
            {
                // Remove the error from any error list
                foreach (KeyValuePair<string, List<string>> kv in _errors)
                {
                    removed |= kv.Value.Remove(error);
                }

                // Remove the empty error lists
                if (removed)
                {
                    // store keys of the empty lists
                    List<string> keys = new List<string>();
                    foreach (KeyValuePair<string, List<string>> kv in _errors)
                        if (!kv.Value.Any())
                            keys.Add(kv.Key);

                    // remove the empty lists
                    foreach (string key in keys)
                        _errors.Remove(key);
                }
            }

            if (notify && removed)
                RiseErrorsChanged(propertyName);
        }

        /// <summary>
        /// Clear all errors for particular property
        /// </summary>
        /// <param name="propertyName">Property name or empty string for the item global error.</param>
        /// <param name="notify">True to rise ErrorsChanged and PropertyChanged events</param>
        public void CleanErrors(string propertyName, bool notify)
        {
            if (_errors == null)
                return;

            bool removed = _errors.Remove(propertyName);

            if (notify && removed)
                RiseErrorsChanged(propertyName);
        }

        /// <summary>
        /// Clear all errors
        /// </summary>
        /// <param name="notify">True to rise ErrorsChanged and PropertyChanged events</param>
        public void CleanErrors(bool notify)
        {
            if (_errors == null)
                return;

            bool removed = _errors.Any();
            _errors.Clear();

            if (notify && removed)
                RiseErrorsChanged("");
        }

        /// <summary>
        /// Rise ErrorsChanged and PropertyChanged events
        /// </summary>
        /// <param name="propertyName">Property name or empty string for the item global error. The default value is the name of the caller property/method.</param>
        public void RiseErrorsChanged([CallerMemberName]  string propertyName = "")
        {
            RisePropertyChanged(nameof(ValidationErrors));
            RisePropertyChanged(nameof(HasErrors));
            OnErrorsChanged(propertyName);
        }

        #endregion

        #region INotifyDataErrorInfo

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors
        /// true if the entity currently has validation errors; otherwise, false.
        /// </summary>
        public virtual bool HasErrors
        {
            get
            {
                if (_errors == null)
                    return false;
                else
                    return _errors.Any();
            }
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or System.String.Empty, to retrieve item-level errors.</param>
        /// <returns>The validation errors for the property or item.</returns>
        public virtual IEnumerable GetErrors(string propertyName)
        {
            List<string> result = null;
            if (_errors != null)
            {
                if (!string.IsNullOrEmpty(propertyName))
                {
                    // Errors for particular property
                    _errors.TryGetValue(propertyName, out result);
                }
                else if (_errors.Count == 1)
                {
                    // Single property errors for the entire item (spare efforts of the GC)
                    result = _errors.Single().Value;
                }
                else if (_errors.Any())
                {
                    // Multyple properties errors for the entire item
                    result = new List<string>();
                    foreach (KeyValuePair<string, List<string>> kv in _errors)
                    {
                        if (kv.Value != null)
                            result.AddRange(kv.Value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        /// <summary>
        /// Rise ErrorsChanged - event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnErrorsChanged([CallerMemberName]  string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion
    }
}

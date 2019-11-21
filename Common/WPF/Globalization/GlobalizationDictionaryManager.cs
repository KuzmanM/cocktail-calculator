using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Common.WPF.Globalization
{
    /// <summary>
    /// Globalization culture manager
    /// </summary>
    public class GlobalizationDictionaryManager
    {
        #region Initialization

        /// <summary>
        /// URI pattern of internal dictionaries.
        /// </summary>
        private const string _localUriStringFormat = @"Resources\StringDictionary.{0}.xaml";

        /// <summary>
        /// URI pattern of external dictionaries.
        /// </summary>
        private const string _externalUriStringFormat = @"pack://siteoforigin:,,,/StringDictionary.{0}.xaml";
        
        /// <summary>
        /// URI of default dictionary.
        /// That dictionary is expected to have been explicitly added to target views xaml. 
        /// </summary>
        private const string _defaultDicUri = @"Resources\StringDictionary.en.xaml";

        /// <summary>
        /// Name of optional file containing culture name that will override the current culture.
        /// </summary>
        private const string _forceCultureFileName = "Culture.txt";

        #endregion

        /// <summary>
        /// Apply culture to the target
        /// </summary>
        /// <param name="target">Target for globalization</param>
        /// <param name="culture">Culture</param>
        public void SetCulture(FrameworkElement target, CultureInfo culture)
        {
            // full culture name
            ResourceDictionary newDic = FindResourceDictionary(_localUriStringFormat, culture.Name, UriKind.Relative);
            if (newDic == null)
                newDic = FindResourceDictionary(_externalUriStringFormat, culture.Name, UriKind.Absolute);

            // local language only
            if (newDic == null)
                newDic = FindResourceDictionary(_localUriStringFormat, culture.TwoLetterISOLanguageName, UriKind.Relative);
            if (newDic == null)
                newDic = FindResourceDictionary(_localUriStringFormat, culture.ThreeLetterISOLanguageName, UriKind.Relative);
            if (newDic == null)
                newDic = FindResourceDictionary(_localUriStringFormat, culture.ThreeLetterWindowsLanguageName, UriKind.Relative);

            // externel language only
            if (newDic == null)
                newDic = FindResourceDictionary(_externalUriStringFormat, culture.TwoLetterISOLanguageName, UriKind.Absolute);
            if (newDic == null)
                newDic = FindResourceDictionary(_externalUriStringFormat, culture.ThreeLetterISOLanguageName, UriKind.Absolute);
            if (newDic == null)
                newDic = FindResourceDictionary(_externalUriStringFormat, culture.ThreeLetterWindowsLanguageName, UriKind.Absolute);

            if (newDic != null && newDic.Source.OriginalString != _defaultDicUri)
            {
                // Remove default dictionary if exists
                ResourceDictionary defaultDic = target.Resources.MergedDictionaries
                    .Where(rd => rd.Source.OriginalString == _defaultDicUri)
                    .SingleOrDefault();

                if (defaultDic != null)
                    target.Resources.MergedDictionaries.Remove(defaultDic);

                // Add the new dictionary
                target.Resources.MergedDictionaries.Add(newDic);
            }
        }

        /// <summary>
        /// Apply culture to the target
        /// </summary>
        /// <param name="target">Target for globalization</param>
        /// <param name="culture">Culture</param>
        public void SetCulture(FrameworkElement target, string culture)
        {
            CultureInfo cultureinfo = Parse(culture, false);
            if (cultureinfo == null)
                cultureinfo = GetDefaultCulture();
            SetCulture(target, cultureinfo);
        }

        /// <summary>
        /// Apply default culture to the target
        /// </summary>
        /// <param name="target">Target for globalization</param>
        public void SetCulture(FrameworkElement target)
        {
            CultureInfo culture = GetDefaultCulture();
            SetCulture(target, culture);
        }

        /// <summary>
        /// Get default culture
        /// </summary>
        /// <param name="allowUnknown">Set true to allow unknown cultures</param>
        /// <returns>Default culture</returns>
        private CultureInfo GetDefaultCulture(bool allowUnknown = false)
        {
            CultureInfo result = CultureInfo.CurrentCulture;

            string cultureStr = GetForceCultureName();
            CultureInfo appConfigCulture = Parse(cultureStr, allowUnknown);
            if (appConfigCulture != null)
                result = appConfigCulture;

            return result;
        }

        /// <summary>
        /// Get culture name which to override the current culture
        /// </summary>
        /// <returns>Culture name</returns>
        private string GetForceCultureName()
        {
            string cultureName = null;

            if(System.IO.File.Exists(_forceCultureFileName))
            {
                try
                {
                    cultureName = System.IO.File.ReadAllText(_forceCultureFileName)
                        .Trim()
                        .Replace(" ", string.Empty);
                }
                catch (Exception)
                { }
            }

            return cultureName;
        }

        /// <summary>
        /// Converts string representation of a CultureInfo to CultureInfo object
        /// </summary>
        /// <param name="cultureName">String representation of a CultureInfo</param>
        /// <param name="allowUnknown">Set true to allow unknown cultures</param>
        /// <returns>null or CultureInfo object</returns>
        private CultureInfo Parse(string cultureName, bool allowUnknown)
        {
            CultureInfo result = null;
            
            if (!string.IsNullOrEmpty(cultureName))
            {
                try
                {
                    CultureInfo culture = new CultureInfo(cultureName);
                    if (!culture.EnglishName.Contains("Unknown") || allowUnknown)
                        result = culture;
                }
                catch (Exception)
                { }
            }

            return result;
        }

        /// <summary>
        /// Find resource dictionary
        /// </summary>
        /// <param name="uriStringFormat">Dictionary URI source patern</param>
        /// <param name="culture">The string that will be applied to the uriStringFormat</param>
        /// <param name="uriKind">Kinds of URI</param>
        /// <returns>null or the found dictionary</returns>
        private ResourceDictionary FindResourceDictionary(string uriStringFormat, string culture, UriKind uriKind)
        {
            ResourceDictionary dict = new ResourceDictionary();
            try
            {
                string uriString = string.Format(uriStringFormat, culture);
                Uri source = new Uri(uriString, uriKind);
                dict.Source = source;
            }
            catch (Exception)
            {
                return null;
            }

            return dict;
        }
    }
}

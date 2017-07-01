using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Models;

namespace API.BusinessLogic
{
    public static class APIHelpers
    {
        public static void UpdateApplicationInfo(List<ApplicationInfoRequest> appInfoList)
        {
            appInfoList.ForEach(appInfo =>
            {
                // Clean up some different names of the same Publishers
                if (!string.IsNullOrWhiteSpace(appInfo.Publisher) && appInfo.Publisher.IndexOf("microsoft", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    appInfo.Publisher = "Microsoft Corporation";
                }
                if (!string.IsNullOrWhiteSpace(appInfo.Publisher) && appInfo.Publisher.IndexOf("intel", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    appInfo.Publisher = "Intel Corporation";
                }
                if (!string.IsNullOrWhiteSpace(appInfo.Publisher) && appInfo.Publisher.IndexOf("oracle", StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    appInfo.Publisher = "Oracle Corporation";
                }

                // Deserialize date
                appInfo.InstallDateParsed = GetInstallDateTime(appInfo.InstallDate);
            });
        }

        private static DateTime GetInstallDateTime(string installDateStr)
        {
            Regex dateRegex = new Regex(Constants.DATE_FORMAT_REGEX, RegexOptions.Singleline);
            if (!string.IsNullOrWhiteSpace(installDateStr) && dateRegex.IsMatch(installDateStr))
            {
                return DateTime.ParseExact(installDateStr, Constants.DATE_FORMAT, CultureInfo.InvariantCulture);
            }
            return DateTime.MinValue;
        }
    }
}

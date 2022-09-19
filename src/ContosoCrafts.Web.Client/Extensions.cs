using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace ContosoCrafts.Web.Client
{
    /// <summary>
    /// Extensions class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <param name="navManager">The nav manager.</param>
        /// <returns></returns>
        public static Dictionary<string, StringValues> GetQueryString(this NavigationManager navManager)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);
            return QueryHelpers.ParseQuery(uri.Query);

        }
        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <param name="navManager">The nav manager.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetQueryString(this NavigationManager navManager, string key)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var queryValue);
            return queryValue.ToString();
        }
    }
}
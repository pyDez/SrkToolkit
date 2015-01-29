﻿
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Caching;
    using System.Web.Mvc;

    /// <summary>
    /// Extension methods for the <see cref="Controller"/> class.
    /// </summary>
    public static class SrkControllerExtensions
    {
        internal const string NavigationLineKey = "SrkNavigationLine";

        /// <summary>
        /// Gets the <see cref="NavigationLine" /> associated to the request.
        /// </summary>
        /// <param name="ctrl">The control.</param>
        /// <returns></returns>
        public static SrkToolkit.Web.NavigationLine NavigationLine(this Controller ctrl)
        {
            if (ctrl.HttpContext == null)
                throw new ArgumentNullException("HttpContext is not set", "ctrl");

            var line = ctrl.HttpContext.Items[NavigationLineKey] as NavigationLine;
            if (line == null)
            {
                line = new NavigationLine();
                ctrl.HttpContext.Items[NavigationLineKey] = line;
            }

            return line;
        }

        /// <summary>
        /// Gets an item from the HTTP cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl">The controller.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="buildData">The build data.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Obsolete("Under devleopment")]
        public static T GetFromCache<T>(this Controller ctrl, TimeSpan duration, CacheItemPriority priority, Func<T> buildData, string id)
            where T : class
        {
            var cache = ctrl.HttpContext.Cache;
            var key = GetCacheKey<T>(id);
            var value = cache[key] as T;
            if (value == null)
            {
                value = buildData();
                cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, duration, priority, null);
            }

            return value;
        }

        /// <summary>
        /// Gets an item from the HTTP cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl">The controller.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="buildData">The build data.</param>
        /// <returns></returns>
        [Obsolete("Under devleopment")]
        public static T GetFromCache<T>(this Controller ctrl, TimeSpan duration, CacheItemPriority priority, Func<T> buildData)
            where T : class
        {
            var cache = ctrl.HttpContext.Cache;
            var key = GetCacheKey<T>(null);
            var value = cache[key] as T;
            if (value == null)
            {
                value = buildData();
                cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, duration, priority, null);
            }

            return value;
        }

        /// <summary>
        /// Clears an item from the HTTP cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl">The controller.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Obsolete("Under devleopment")]
        public static T ClearFromCache<T>(this Controller ctrl, string id)
            where T : class
        {
            var cache = ctrl.HttpContext.Cache;
            var key = GetCacheKey<T>(id);
            var value = cache[key] as T;
            if (value != null)
            {
                cache.Remove(key);
                return value;
            }

            return null;
        }

        /// <summary>
        /// Clears an item from the HTTP cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl">The control.</param>
        /// <returns></returns>
        [Obsolete("Under devleopment")]
        public static T ClearFromCache<T>(this Controller ctrl)
            where T : class
        {
            var cache = ctrl.HttpContext.Cache;
            var key = GetCacheKey<T>(null);
            var value = cache[key] as T;
            if (value != null)
            {
                cache.Remove(key);
                return value;
            }

            return null;
        }

        /// <summary>
        /// Gets a action result that will redirect the user to the specified local path. Fallbacks to a second path. Then fallbacks to /Home/Index
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="localUrl">The local URL.</param>
        /// <param name="fallbackLocalUrl">The fallback local URL.</param>
        /// <returns></returns>
        public static ActionResult RedirectToLocal(this Controller controller, string localUrl, string fallbackLocalUrl = null)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            if (localUrl != null && controller.Url.IsLocalUrl(localUrl))
            {
                return controller.Redirect(localUrl);
            }
            else if (fallbackLocalUrl != null && controller.Url.IsLocalUrl(fallbackLocalUrl))
            {
                return controller.Redirect(fallbackLocalUrl);
            }
            else
            {
                return controller.RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Gets the first valid local URL from the method arguments.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="url1">The url1.</param>
        /// <param name="tryReferer">if set to <c>true</c> [try referer].</param>
        /// <param name="url2">The url2.</param>
        /// <returns></returns>
        /// <remarks>
        /// Sometimes you want to redirect the user to the referer and have a fallback.
        /// return this.RedirectToLocal(this.GetAnyLocalUrl(null, true, "fallback url"));
        /// You may also have a returnUrl as action parameter and do
        /// return this.RedirectToLocal(this.GetAnyLocalUrl(returnUrl, true, "fallback url"));
        /// </remarks>
        public static string GetAnyLocalUrl(this Controller controller, string url1, bool tryReferer, string url2 = null)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            if (url1 != null && controller.Url.IsLocalUrl(url1))
            {
                return url1;
            }

            if (tryReferer && controller.Request != null && controller.Request.UrlReferrer != null)
            {
                string referer = controller.Request.UrlReferrer.PathAndQuery;
                if (controller.Url.IsLocalUrl(referer))
                    return referer;
            }

            if (url2 != null && controller.Url.IsLocalUrl(url2))
            {
                return url2;
            }

            return null;
        }

        private static string GetCacheKey<T>(string id) where T : class
        {
            var type = typeof(T);
            var key = "SrkControllerExtensionsCache-" + type.GUID + "-" + type.FullName + "-" + (id ?? "");
            return key;
        }
    }
}

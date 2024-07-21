using Microsoft.AspNetCore.Routing;

namespace MvcRazorPages.Shared.Data
{
    /// <summary>
    /// A class that stores data for redirections to a Razor page. 
    /// </summary>
    public class RedirectToPageData
    {
        #region Fields

        /// <summary>
        /// Backing field for property <see cref="Page"/>.
        /// </summary>
        private string _page = "";

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public RedirectToPageData()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="pageName">The name of the page to redirect to.</param>
        /// <param name="routeValues">The route values to send to the page.</param>
        /// <param name="urlFragment">An optional URL fragment for targeting regions within a page (#ID).</param>
        /// <param name="area">An optional area that will get merged with the route values.</param>
        /// <exception cref="ArgumentException"></exception>
        public RedirectToPageData(string pageName, RouteValueDictionary? routeValues = null, string? urlFragment = null, string? area = null)
        {
            #region Checks

            if (string.IsNullOrEmpty(pageName))
            {
                throw new ArgumentException($"The value of parameter '{nameof(pageName)}' can't be empty.", nameof(pageName));
            }

            #endregion

            Page = pageName;
            RouteValues = routeValues;
            UrlFragment = urlFragment;

            if (!string.IsNullOrEmpty(area))
            {
                if (RouteValues != null)
                {
                    RouteValues.TryAdd("Area", area);
                }
                else
                {
                    RouteValues = new RouteValueDictionary(new { Area = area });
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the page to redirect to.
        /// </summary>
        public string Page
        {
            get
            {
                return _page;
            }

            set
            {
                #region Checks

                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException($"The value of property '{nameof(Page)}' can't be empty.", nameof(Page));
                }

                #endregion

                _page = value;
            }
        }

        /// <summary>
        /// The route values to send to the page.
        /// </summary>
        public RouteValueDictionary? RouteValues { get; set; }

        /// <summary>
        /// An optional URL fragment for targeting regions within a page (#ID).
        /// </summary>
        public string? UrlFragment { get; set; }

        #endregion
    }
}

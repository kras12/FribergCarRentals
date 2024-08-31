using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Shared.Mvc.Components
{
    /// <summary>
    /// A class that handles image slideshows.
    /// </summary>
    public class ImageSlideShow : ViewComponent
    {
        #region Methods

        /// <summary>
        /// The invoke method.
        /// </summary>
        /// <param name="images">The images to include in the slide show.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IViewComponentResult"/>.</returns>
        public IViewComponentResult Invoke(List<SlideShowImageViewModel> images)
        {
            #region Checks

            if (images == null || images.Count == 0)
            {
                throw new ArgumentException("The image collection can't be empty.", nameof(images));
            }

            #endregion

            if (images is null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            return View(new ListViewModel<SlideShowImageViewModel>(images));
        }

        #endregion
    }
}

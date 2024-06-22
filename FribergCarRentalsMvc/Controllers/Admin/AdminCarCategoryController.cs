using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.ViewModels.Other;
using MvcRazorPages.Shared.ViewModels.CarCategory;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace FribergCarRentals.Controllers.Admin
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class AdminCarCategoryController : ViewControllerBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car category that was created.
        /// </summary>
        public const string CreatedCarCategoryIdTempDataKey = "AdminCreatedCarCategoryId";

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Admin/Cars/Categories";

        /// <summary>
        /// The key for the ID of the car category that was deleted. 
        /// </summary>
        public const string DeletedCarCategoryIdTempDataKey = "AdminDeletedCarCategoryId";

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempDataKey = "AdminCarCategoryEditPageSubTitle";

        /// <summary>
        /// The key for the deleted car category redirect data stored in temp storage.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCarCategoryRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        // The injected Auto Mapper.
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        public AdminCarCategoryController(ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper) : base(authorizationService, signInManager)
        {
            _carCategoryRepository = carCategoryRepository;
            _mapper = mapper;
        }

        #endregion

        #region Actions


        // GET: AdminCarCategoryController/Create
        public async Task<IActionResult> Create()
        {
            if (!(await IsAdminLoggedIn()))
            {
                return RedirectToLogin(nameof(Create));
            }

            return View();
        }

        // POST: AdminCarCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarCategoryViewModel createCarCategoryViewModel)
        {
            if (!(await IsAdminLoggedIn()))
            {
                return RedirectToLogin(nameof(Create));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = _mapper.Map<CarCategoryEntity>(createCarCategoryViewModel);
                await _carCategoryRepository.AddAsync(category);
                TempDataHelper.Set(TempData, CreatedCarCategoryIdTempDataKey, category.CarCategoryId);
                return RedirectToAction(nameof(Details), new { id = category.CarCategoryId });
            }

            return View();
        }

        // POST: AdminCarCategoryController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(await IsAdminLoggedIn()))
            {
                return RedirectToLogin(nameof(Delete), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                await _carCategoryRepository.DeleteAsync(id);
                TempDataHelper.Set(TempData, DeletedCarCategoryIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToActionData? data))
                {
                    return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                }
                else
                {
                    return RedirectToAction(nameof(List));
                }
            }

            throw new Exception($"Failed to delete the car category with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCarCategoryController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!(await IsAdminLoggedIn()))
            {
                return RedirectToLogin(nameof(Details), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = await _carCategoryRepository.GetByIdAsync(id);

                if (category is not null)
                {
                    CarCategoryViewModel viewModel = new CarCategoryViewModel(category);

                    if (TempDataHelper.TryGet(TempData, CreatedCarCategoryIdTempDataKey, out int categoryId))
                    {
                        viewModel.Messages.Add(UserMesssageHelper.CreateCarCategoryCreationSuccessMessage(categoryId));
                    }

                    return View(viewModel);
                }
            }

            throw new Exception($"Failed to show the car category with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCarCategoryController/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!(await IsAdminLoggedIn()))
            {
                return RedirectToLogin(nameof(Edit), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = await _carCategoryRepository.GetByIdAsync(id);

                if (category is not null)
                {
                    EditCarCategoryViewModel viewModel = new EditCarCategoryViewModel(category);
                    TempDataHelper.Set(TempData, PageSubTitleTempDataKey, viewModel.PageSubTitle!);
                    return View(viewModel);
                }
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: AdminCarCategoryController/Edit/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCarCategoryViewModel editCarCategoryViewModel)
        {
            if (!(await IsAdminLoggedIn()))
            {
                return RedirectToLogin(nameof(Edit), id);
            }

            if (id <= 0 || id != editCarCategoryViewModel.CarCategoryId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {editCarCategoryViewModel.CarCategoryId}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = _mapper.Map<CarCategoryEntity>(editCarCategoryViewModel);
                await _carCategoryRepository.UpdateAsync(category);
                EditCarCategoryViewModel viewModel = new EditCarCategoryViewModel(category);
                viewModel.Messages.Add(UserMesssageHelper.CreateCarCategoryUpdateSuccessMessage(category.CarCategoryId));
                return View(viewModel);
            }

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempDataKey, out string? pageSubTitle))
            {
                editCarCategoryViewModel.PageSubTitle = pageSubTitle;
                TempDataHelper.Set(TempData, PageSubTitleTempDataKey, editCarCategoryViewModel.PageSubTitle!); // The user can fail again.
            }

            return View(editCarCategoryViewModel);
        }

        // GET: AdminCarCategoryController/List
        public async Task<IActionResult> List()
        {
            if (!(await IsAdminLoggedIn()))
            {
                return RedirectToLogin(nameof(List));
            }

            ListViewModel<CarCategoryViewModel> viewModel = new ((await _carCategoryRepository.GetCategoryStatistics()).Select(x => new CarCategoryViewModel(x)));
            SaveRedirectBackInstructionsForDeleteCarCategoryAction(nameof(List));

            if (TempDataHelper.TryGet(TempData, DeletedCarCategoryIdTempDataKey, out int deletedCarCategoryId))
            {
                viewModel.Messages.Add(UserMesssageHelper.CreateCarCategoryDeletionSuccessMessage(deletedCarCategoryId));
            }

            return View(viewModel);
        }
        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the car category.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, AdminController.RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminCarCategoryController>(), routeValues: routeValues));

            return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
        }

        /// <summary>
        /// Saves data for redirecting back to an action after a car category has been deleted. 
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        private void SaveRedirectBackInstructionsForDeleteCarCategoryAction(string redirectToAction)
        {
            TempDataHelper.Set(TempData, RedirectToPageAfterDeleteTempDataKey, new RedirectToActionData(
                    redirectToAction, ControllerHelper.GetControllerName<AdminCarCategoryController>()));
        }

        #endregion
    }
}

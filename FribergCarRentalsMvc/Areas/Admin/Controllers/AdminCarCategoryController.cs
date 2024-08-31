using FribergCarRentals.Shared.Mvc.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared.Mvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentals.Shared.Models.ViewModels.Message;

namespace FribergCarRentals.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for car categories.
    /// </summary>
    [Route($"{Area}/{CurrentControllerRoutePart}/[action]")]
    [Area(Area)]
    public class AdminCarCategoryController : AdminControllerBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car category that was created.
        /// </summary>
        public const string CreatedCarCategoryIdTempDataKey = "AdminCreatedCarCategoryId";

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Cars/Categories";

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
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Create), ControllerHelper.GetControllerName<AdminCarCategoryController>(), area: Area));
            }

            return View();
        }

        // POST: AdminCarCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarCategoryViewModel createCarCategoryViewModel)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Create), ControllerHelper.GetControllerName<AdminCarCategoryController>(), area: Area));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = _mapper.Map<CarCategoryEntity>(createCarCategoryViewModel);
                await _carCategoryRepository.AddAsync(category);
                TempDataHelper.Set(TempData, CreatedCarCategoryIdTempDataKey, category.CarCategoryId);
                return RedirectToActionInArea(nameof(Details), new RouteValueDictionary(new { id = category.CarCategoryId }));
            }

            return View();
        }

        // POST: AdminCarCategoryController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Delete), ControllerHelper.GetControllerName<AdminCarCategoryController>(), new RouteValueDictionary(new { id }), area: Area));
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
                    return RedirectToActionInArea(nameof(List));
                }
            }

            throw new Exception($"Failed to delete the car category with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCarCategoryController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Details), ControllerHelper.GetControllerName<AdminCarCategoryController>(), new RouteValueDictionary(new { id }), area: Area));
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
                    CarCategoryViewModel viewModel = _mapper.Map<CarCategoryViewModel>(category);

                    if (TempDataHelper.TryGet(TempData, CreatedCarCategoryIdTempDataKey, out int categoryId))
                    {
                        viewModel.Messages.Add(MessageViewModelHelper.CreateCarCategoryCreationSuccessMessage(categoryId));
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
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Edit), ControllerHelper.GetControllerName<AdminCarCategoryController>(), new RouteValueDictionary(new { id }), area: Area));
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
                    EditCarCategoryViewModel viewModel = _mapper.Map<EditCarCategoryViewModel>(category);
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
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Edit), ControllerHelper.GetControllerName<AdminCarCategoryController>(), new RouteValueDictionary(new { id }), area: Area));
            }

            if (id <= 0 || id != editCarCategoryViewModel.CarCategoryId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {editCarCategoryViewModel.CarCategoryId}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = _mapper.Map<CarCategoryEntity>(editCarCategoryViewModel);
                await _carCategoryRepository.UpdateAsync(category);
                EditCarCategoryViewModel viewModel = _mapper.Map<EditCarCategoryViewModel>(category);
                viewModel.Messages.Add(MessageViewModelHelper.CreateCarCategoryUpdateSuccessMessage(category.CarCategoryId));
                return View(viewModel);
            }

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempDataKey, out string? pageSubTitle))
            {
                editCarCategoryViewModel.SetPageSubTitle(pageSubTitle);
                TempDataHelper.Set(TempData, PageSubTitleTempDataKey, editCarCategoryViewModel.PageSubTitle!); // The user can fail again.
            }

            return View(editCarCategoryViewModel);
        }

        // GET: AdminCarCategoryController/List
        public async Task<IActionResult> List()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminCarCategoryController>(), area: Area));
            }

            ListViewModel<CarCategoryViewModel> viewModel = new(_mapper.Map<List<CarCategoryViewModel>>(await _carCategoryRepository.GetCategoryStatistics()));
			TempDataHelper.Set(TempData, RedirectToPageAfterDeleteTempDataKey, 
                new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminCarCategoryController>(), area: Area));

            if (TempDataHelper.TryGet(TempData, DeletedCarCategoryIdTempDataKey, out int deletedCarCategoryId))
            {
                viewModel.Messages.Add(MessageViewModelHelper.CreateCarCategoryDeletionSuccessMessage(deletedCarCategoryId));
            }

            return View(viewModel);
        }

        #endregion
    }
}

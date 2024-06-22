using FribergCarRentals.DataAccess.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.DataTransferObjects
{
    /// <summary>
    /// A DTO class storing car category statistics.
    /// </summary>
    public  class CarCategoryStatisticsDto
    {

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carCategoryEntity">The car category entity.</param>
        /// <param name="carCount">The number of cars using this category.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CarCategoryStatisticsDto(CarCategoryEntity carCategoryEntity, int carCount)
        {
            #region Checks

            if (carCategoryEntity is null)
            {
                throw new ArgumentNullException(nameof(carCategoryEntity), $"The value of parameter '{nameof(carCategoryEntity)}' can't be null.");
            }

            if (carCategoryEntity.CarCategoryId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(CarCategoryEntity.CarCategoryId), $"The value of property '{nameof(CarCategoryEntity.CarCategoryId)}' can't be negative.");
            }

            if (carCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carCount), $"The value of parameter '{nameof(carCount)}' can't be negative.");
            }

            #endregion

            CarCategoryEntity = carCategoryEntity;
            CarCount = carCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car category entity.
        /// </summary>
        public CarCategoryEntity CarCategoryEntity { get; }

        /// <summary>
        /// The number of cars using this category.
        /// </summary>
        public int CarCount { get; } = 0;

        #endregion
    }
}

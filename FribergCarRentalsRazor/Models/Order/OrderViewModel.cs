using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models.Customer;
using FribergCarRentals.Models.Other;

namespace FribergCarRentals.Models.Orders
{
    /// <summary>
    ///  A view model class that handles data relating to an order. 
    /// </summary>
    public class OrderViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carOrder">The car order to model.</param>
        /// <param name="isNewOrder">True if the order was just created.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OrderViewModel(CarOrderEntity carOrder, bool isNewOrder = false)
        {
            #region Checks

            if (carOrder.Customer is null)
            {
                throw new ArgumentNullException("The customer of the order can't be null");
            }

            if (carOrder.CarBookings is null)
            {
                throw new ArgumentNullException("The car booking of the order can't be null");
            }

            #endregion

            CarOrderId = carOrder.CarOrderId;
            OrderDateUtc = carOrder.OrderDateUtc;
            Customer = new CustomerViewModel(carOrder.Customer);
            Payments = carOrder.Payments;
            OrderStatus = carOrder.OrderStatus!;
            CarBooking = carOrder.CarBookings.Count > 0 ? new CarBookingViewModel(carOrder.CarBookings.First())
                : throw new InvalidOperationException("Could not find a car booking");
            IsNewOrder = isNewOrder;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if the order can be marked as completed.
        /// </summary>
        [DisplayName("Can be completed")]
        [BindNever]
        public bool CanBeCompleted
        {
            get
            {
                return OrderStatus.StatusType == DataAccess.Types.OrderStatus.Created;
            }
        }

        /// <summary>
        /// The car booking tied to the order.
        /// </summary>
        [BindNever]
        public CarBookingViewModel CarBooking { get; }

        /// <summary>
        /// The ID of the order.
        /// </summary>
        [DisplayName("Order ID")]
        [BindNever]
        public int CarOrderId { get; }

        /// <summary>
        /// The customer that placed the order.
        /// </summary>
        [BindNever]
        public CustomerViewModel Customer { get; }

        /// <summary>
        /// Returns true if the order can be cancelled.
        /// </summary>
        /// <remarks>A cancellable order is an order having the 
        /// status 'Create'" and where the booking have a 
        /// car pickup calender date that is ahead 
        /// of the current calender day.</remarks>
        [DisplayName("Is Cancelable")]
        [BindNever]
        public bool IsCancelable
        {
            get
            {
                return OrderStatus.StatusType == DataAccess.Types.OrderStatus.Created && CarBooking.CarPickupDate.Date > DateTime.UtcNow.Date;
            }
        }

        /// <summary>
        /// Returns true if the order was just created.
        /// </summary>
        [DisplayName("New Order")]
        [BindNever]
        public bool IsNewOrder { get; private set; }

        /// <summary>
        /// The order date in local time.
        /// </summary>
        [DisplayName("Order Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DefaultDateFormatString)]
        [BindNever]
        public DateTime OrderDateLocal
        {
            get
            {
                return OrderDateUtc.ToLocalTime();
            }
        }

        /// <summary>
        /// The order date in UTC time.
        /// </summary>
        [DisplayName("Order Date")]
        [BindNever]
        public DateTime OrderDateUtc { get; }

        /// <summary>
        /// The order status name.
        /// </summary>
        [DisplayName("Order Status")]
        [BindNever]
        public string OrderStatusName
        {
            get
            {
                return OrderStatus.StatusName;
            }
        }

        /// <summary>
        /// The total sum of the order.
        /// </summary>
        [DisplayName("Order Sum")]
        [DisplayFormat(DataFormatString = DefaultPriceOutputFormatString)]
        [BindNever]
        public decimal OrderSum
        {
            get
            {
                return CarBooking?.RentalCostPerDay * ((CarBooking?.CarReturnDate - CarBooking?.CarPickupDate)?.Days + 1) ?? 0;
            }
        }

        /// <summary>
        /// The total sum of all payments made by the customer.
        /// </summary>
        [DisplayName("Paid")]
        [BindNever]
        public decimal Paid
        {
            get
            {
                return Payments.Sum(x => x.Amount);
            }
        }

        /// <summary>
        /// A collection of payments tied to the order.
        /// </summary>
        [DisplayName("Payments")]
        [BindNever]
        public List<PaymentEntity> Payments { get; } = new();

        /// <summary>
        /// The order status.
        /// </summary>
        [BindNever]
        private OrderStatusEntity OrderStatus { get; }

        #endregion
    }
}

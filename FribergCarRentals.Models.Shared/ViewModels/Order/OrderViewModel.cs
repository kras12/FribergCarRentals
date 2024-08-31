using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.ViewModels.Customer;
using FribergCarRentals.Shared.ViewModels.Other;

namespace FribergCarRentals.Shared.ViewModels.Order
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
        public OrderViewModel()
        {
            
        }        

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carOrderId">The ID of the order.</param>
        /// <param name="orderDateUtc">The order date in UTC time.</param>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="customer">The customer that placed the order.</param>
        /// <param name="carBooking">The car booking tied to the order.</param>
        /// <param name="payments">A collection of payments tied to the order.</param>
        /// <param name="isNewOrder">True if the order was just created.</param>
        public OrderViewModel(int carOrderId, DateTime orderDateUtc, OrderStatusViewModel orderStatus, CustomerViewModel customer, 
            CarBookingViewModel carBooking, List<PaymentViewModel> payments, bool isNewOrder)
        {
            CarOrderId = carOrderId;
            OrderDateUtc = orderDateUtc;
            OrderStatus = orderStatus;
            Customer = customer;
            CarBooking = carBooking;
            Payments = payments;
            IsNewOrder = isNewOrder;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if the order can be marked as completed.
        /// </summary>
        [DisplayName("Can be completed")]
        public bool CanBeCompleted
        {
            get
            {
                return OrderStatus.StatusType == FribergCarRentals.Data.Types.OrderStatus.Created;
            }
        }

        /// <summary>
        /// The car booking tied to the order.
        /// </summary>
        public CarBookingViewModel CarBooking { get; set; }

        /// <summary>
        /// The ID of the order.
        /// </summary>
        [DisplayName("Order ID")]
        public int CarOrderId { get; set; }

        /// <summary>
        /// The customer that placed the order.
        /// </summary>
        public CustomerViewModel Customer { get; set; }

        /// <summary>
        /// Returns true if the order can be cancelled.
        /// </summary>
        /// <remarks>A cancellable order is an order having the 
        /// status 'Create'" and where the booking have a 
        /// car pickup calender date that is ahead 
        /// of the current calender day.</remarks>
        [DisplayName("Is Cancelable")]
        public bool IsCancelable
        {
            get
            {
                return OrderStatus.StatusType == FribergCarRentals.Data.Types.OrderStatus.Created && CarBooking.CarPickupDate.Date > DateTime.UtcNow.Date;
            }
        }

        /// <summary>
        /// Returns true if the order was just created.
        /// </summary>
        [DisplayName("New Order")]
        public bool IsNewOrder { get; set; }

        /// <summary>
        /// The order date in local time.
        /// </summary>
        [DisplayName("Order Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DefaultDateFormatString)]
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
        public DateTime OrderDateUtc { get; set; }

        /// <summary>
        /// The order status name.
        /// </summary>
        [DisplayName("Order Status")]
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
        [DisplayFormat(DataFormatString = ValidationRules.DefaultPriceOutputFormatString)]
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
        public List<PaymentViewModel> Payments { get; set; } = new();

        /// <summary>
        /// The order status.
        /// </summary>
        public OrderStatusViewModel OrderStatus { get; set; }

        #endregion
    }
}

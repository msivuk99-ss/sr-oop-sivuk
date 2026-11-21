using System;

namespace IndependentWork19
{
    public class OutOfStockException : Exception
    {
        public string ProductSku { get; } = string.Empty;
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        public OutOfStockException()
        {
        }

        public OutOfStockException(string message)
            : base(message)
        {
        }

        public OutOfStockException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public OutOfStockException(
            string message,
            string productSku,
            int requestedQuantity,
            int availableQuantity)
            : base(message)
        {
            ProductSku = productSku;
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }
}
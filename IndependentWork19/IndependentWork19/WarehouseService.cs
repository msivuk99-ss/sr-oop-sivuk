using System;

namespace IndependentWork19
{
    public class WarehouseService
    {
        public void ReserveProduct(string sku, int requestedQuantity, int availableQuantity)
        {
            if (requestedQuantity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requestedQuantity),
                    "Кількість товару для резервування має бути більшою за нуль.");
            }

            if (requestedQuantity > availableQuantity)
            {
                throw new OutOfStockException(
                    $"Недостатньо товару '{sku}' на складі.",
                    sku,
                    requestedQuantity,
                    availableQuantity);
            }

            Console.WriteLine($"Товар '{sku}' у кількості {requestedQuantity} од. успішно зарезервовано.");
        }
    }
}
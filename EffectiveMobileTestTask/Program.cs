using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DeliveryService
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Необходимы параметры: _cityDistrict _firstDeliveryDateTime _deliveryLog _deliveryOrder");
                return;
            }

            string cityDistrict = args[0];
            if (!DateTime.TryParseExact(args[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime firstDeliveryDateTime))
            {
                Console.WriteLine($"Некорректная дата: {args[1]}");
                return;
            }

            string deliveryLogPath = args[2];
            string deliveryOrderPath = args[3];

            List<Order> orders = OrderLoader.LoadOrders("orders.txt");
            List<Order> filteredOrders = OrderFilter.FilterOrders(orders, cityDistrict, firstDeliveryDateTime);
            Logger.LogDelivery(deliveryLogPath, filteredOrders);
            FileSaver.SaveFilteredOrders(deliveryOrderPath, filteredOrders);
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public double Weight { get; set; }
        public string District { get; set; }
        public DateTime DeliveryTime { get; set; }
    }

    public static class OrderLoader
    {
        public static List<Order> LoadOrders(string filePath)
        {
            var orders = new List<Order>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue; // Пропуск пустых строк

                var parts = line.Split(';');
                if (parts.Length != 4)
                {
                    Console.WriteLine($"Неверный формат строки (ожидалось 4 части): {line}");
                    continue;
                }

                // Парсинг данных заказа
                if (int.TryParse(parts[0], out int orderId) &&
                    double.TryParse(parts[1].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double weight) &&  // Учитываем запятую
                    DateTime.TryParseExact(parts[3].Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deliveryTime))
                {
                    orders.Add(new Order
                    {
                        OrderId = orderId,
                        Weight = weight,
                        District = parts[2].Trim(),
                        DeliveryTime = deliveryTime
                    });
                }
                else
                {
                    Console.WriteLine($"Ошибка при парсинге заказа: {line}");
                }
            }

            return orders;
        }
    }

    public static class OrderFilter
    {
        public static List<Order> FilterOrders(List<Order> orders, string cityDistrict, DateTime firstDeliveryDateTime)
        {
            DateTime endTime = firstDeliveryDateTime.AddMinutes(30);
            return orders.Where(o => o.District.Equals(cityDistrict, StringComparison.OrdinalIgnoreCase) &&
                                     o.DeliveryTime >= firstDeliveryDateTime &&
                                     o.DeliveryTime <= endTime).ToList();
        }
    }

    public static class Logger
    {
        public static void LogDelivery(string logFilePath, List<Order> filteredOrders)
        {
            using StreamWriter writer = new StreamWriter(logFilePath, true);
            foreach (var order in filteredOrders)
            {
                writer.WriteLine($"{DateTime.Now}: Заказ {order.OrderId} в районе {order.District} отгружен в {order.DeliveryTime}");
            }
        }
    }

    public static class FileSaver
    {
        public static void SaveFilteredOrders(string outputFilePath, List<Order> filteredOrders)
        {
            using StreamWriter writer = new StreamWriter(outputFilePath);
            foreach (var order in filteredOrders)
            {
                writer.WriteLine($"{order.OrderId};{order.Weight.ToString(CultureInfo.InvariantCulture).Replace('.', ',')};{order.District};{order.DeliveryTime:yyyy-MM-dd HH:mm:ss}");
            }
        }
    }
}

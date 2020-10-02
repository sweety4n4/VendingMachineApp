using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


/// <summary>
///  VendorMachineProject expects an input inventory code: inv from the user. If the code entered is valid it will process the order, else it would ask user to re-enter the code.
///  Also, if user wishes to continue shopping need to enter "yes".
///  .Net Version Used: Visual Studio 2019 for MacOS, Application Kind: .Net Core 3.1, Type: Console Application
///  Design Pattern : Dependency Injection, type: constructor injection
/// </summary>

namespace VendingMachineProject
{ 
    public interface IVendingMachine
    {
        void DisplayInventory(List<VendingMachine> list);
        bool IsValidInventory(string code);
        void ProcessOrder(List<VendingMachine> inventoryList);
        void ContinueOrder(string OrderStatus, List<VendingMachine> list);
       
    }

    
    public class VendorMachineOperations
    {
        private IVendingMachine _vendingMachine;
        public VendorMachineOperations(IVendingMachine VendingMachine)
        {
            this._vendingMachine = VendingMachine;
        }

        public void DisplayInventory(List<VendingMachine> list)
        {
            this._vendingMachine.DisplayInventory(list);
        }
        public void ProcessOrder(List<VendingMachine> list)
        {
            this._vendingMachine.ProcessOrder(list);
        }
        public void ContinueOrder(string OrderStatus, List<VendingMachine> list)
        {
            this._vendingMachine.ContinueOrder(OrderStatus, list);
        }
        public bool IsValidInventory(string code)
        {
           return this._vendingMachine.IsValidInventory(code);
        }
    }
    public class MainClass 
    {
        public static List<VendingMachine> vendingMachingList;


        public static void Main(string[] args)
        {

            vendingMachingList = new List<VendingMachine>()
         {
             new VendingMachine()
             {
                 ItemNumber = 1, Name = "Coke", Quantity = 10, Price = 1.25

             },
               new VendingMachine()
             {
                  ItemNumber = 2, Name = "M&M", Quantity = 15, Price = 1.89

             },
               new VendingMachine()
             {
                  ItemNumber = 3, Name = "Water", Quantity = 5, Price = 0.89

             },
               new VendingMachine()
             {
                  ItemNumber = 4, Name = "Snikers", Quantity = 7, Price = 2.05

             },
         };

            VendorMachineOperations vendingMachine = new VendorMachineOperations(new VendingMachine());


            Console.WriteLine("Please select inventory code");

            string inventoryCode = Console.ReadLine();


            if (!string.IsNullOrEmpty(inventoryCode) && vendingMachine.IsValidInventory(inventoryCode))
            {

                vendingMachine.ProcessOrder(vendingMachingList);
            }
            else
            {
                Console.WriteLine("Invalid entry, please try again later!");

            }

        }

       
    }

    public class VendingMachine : IVendingMachine
    {
        private bool _isOrderProcessed;

        public int ItemNumber { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public VendingMachine()
        {
            this._isOrderProcessed = false;
        }

        public bool IsValidInventory(string code)
        {
            return code.Trim().ToUpper() == "INV";
        }

        // Displays the list of items in the inventory
        public void DisplayInventory(List<VendingMachine> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Name} ({item.Quantity}): {item.Price}");
            }

        }
        // Process the order depending on the item(s) selected
        public void ProcessOrder(List<VendingMachine> inventoryList)
        {
            DisplayInventory(inventoryList);

            Console.Write($"Please enter cash, item and quantity to purchase \n <Amount> <Item> <Quantity>");
            Console.ReadKey(true);
            this._isOrderProcessed = false;

            try
            {

                bool isValidAmount = double.TryParse(Console.ReadLine(), out double amount);
                bool isValidItemNumber = int.TryParse(Console.ReadLine(), out int itemNumber);
                bool isValidQuantity = int.TryParse(Console.ReadLine(), out int quantity);


                if (isValidAmount && isValidItemNumber && isValidQuantity)
                {
                    VendingMachine result = inventoryList.FirstOrDefault(o => o.ItemNumber == itemNumber && o.Quantity >= quantity);


                    if (result != null)
                    {

                        this._isOrderProcessed = amount == quantity * result.Price ? true : false;

                    }

                    if (this._isOrderProcessed == true)
                    {
                        // Updating quantity for persistence (Note: this value changes back to original if the application is re-run. For permanent persistence database is needed)
                        result.Quantity = result.Quantity - quantity;

                        Console.WriteLine($"Your order process successfully, Please collect your item(s). Have a good day!");
                        Console.WriteLine($"Would you like to purchase more items?");
                        string wouldLikeToOrder = Console.ReadLine();

                        ContinueOrder(wouldLikeToOrder, inventoryList);

                    }
                    else
                    {
                        Console.WriteLine("Sorry your order cannot be processed at this time due to following reasons: \n Invalid item or Insufficient amount or exact change not entered, \n please try again");
                        Console.WriteLine("Do you want to re-enter your selection?");
                        string IsOrder = Console.ReadLine();

                        ContinueOrder(IsOrder, inventoryList);
                    }
                }
                else
                {
                    Console.WriteLine("Please enter valid amount, item and quantity");
                    ProcessOrder(inventoryList);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sorry for the inconvience an unexpected error due to {ex.Message}");
            }

        }

      
        public void ContinueOrder(string OrderStatus, List<VendingMachine> list)
        {

            if (OrderStatus.ToUpper() == "YES")
            {
                ProcessOrder(list);
            }
            else
            {
                Console.WriteLine("Thanks for shopping with us, Have a good day!");
            }

        }

    }

}









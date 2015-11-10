using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using ALOHASystem.Models;
using System.Data;

namespace ALOHASystem.Controllers
{
    /*Welcome to the HomeController, inside this file is the code that runs the project. Each method pertains to a 
     * certain page and evokes a certain function for that page. Call back methods are used to help seperate data
     * movement, and allows for a more readable design.
     * */
    public class HomeController : Controller
    {
        //creates instance of database. public so that all methods can access it.
        public alohaDataConnection ac = new alohaDataConnection();
        public static OrderCartViewModel ovm = new OrderCartViewModel();
        //This is the main view after login. Essentially this is the home or default screen of the program
        [Authorize]
        public ActionResult ManagerView()
        {
            return View();
        }
        /*This handles the initial order view propogating the page with customer information and item information
         * required for each order. (Quantities of items are handled in the ALOHASystem.js file using jQuery
         * */
        public ActionResult OrderView()
        {
            ovm = new OrderCartViewModel();
            OrderViewModel info = new OrderViewModel();
            DataSet CustomerIDs = new DataSet();
            DataSet ItemInfo = new DataSet();
            CustomerIDs = ac.getCustomerInfo();
            ItemInfo = ac.getItemInfo();

            foreach(DataTable customer in CustomerIDs.Tables)
            {
                foreach (DataRow row in customer.Rows)
                {
                    info.CustomerID.Add(Convert.ToInt32(row["ID"]));
                }
            }

            foreach (DataTable item in ItemInfo.Tables)
            {
                foreach (DataRow row in item.Rows)
                {
                    info.lstItemOrderViewModel.Add(new ItemOrderViewModel()
                        {
                            Name = Convert.ToString(row["itemName"]),
                            Price = Convert.ToDecimal(row["Price"]),
                            Quantity = Convert.ToInt32(row["Quantity"]),
                            ProductID = Convert.ToInt32(row["ID"])
                        });
                }
            }
            return View(info);
        }
        /*This creates a partial view on the Order page that is a table that acts as the "cart" of the order. A checkout
         *button appears with the table and it allows the user to see what is going to be ordered.
         **/
        public PartialViewResult GetOrderInfoForm()
        {
            DataSet itemInfo = new DataSet();
            
            int Customer = Convert.ToInt32(Request.Form["lstCustomerID"]);
            string Product = Convert.ToString(Request.Form["lstItem"]);
            int Quantity = Convert.ToInt32(Request.Form["Quantity"]);
            
            itemInfo = ac.findItemInfo(Product);

            foreach (DataTable item in itemInfo.Tables)
            {

                foreach (DataRow row in item.Rows)
                {
                    ovm.lstOrder.Add(new OrderInfo()
                        {
                            CustomerID = Customer,
                            ProductID = Convert.ToInt32(row["ID"]),
                            ProductQuantity = (Quantity),
                            Name = (Product),
                            Price = (Convert.ToDecimal(row["Price"])),
                            Quantity = (Convert.ToInt32(row["Quantity"]))
                        });
                }
            }
            decimal pri = ovm.lstOrder.Find(x => x.Name == Product).Price;
            ovm.total = (pri * Quantity)+ovm.total;
            return PartialView("_CartView", ovm);
        }
        /*This creates a partial view on the CustomerFindOrderView. This allows for two seperate buttons to be pressed.
         * One checks for orders based on customerID and the other checks for orders based on orderID.
         * */
        public PartialViewResult OrderSearch(string button)
        {
            //Searching by customer ID
            if (button == "Search by Customer ID")
            {
                ovm = new OrderCartViewModel();
                DataSet orderInfo = new DataSet();
                int customerID = Convert.ToInt32(Request.Form["CustomerID"]);
                orderInfo = ac.findOrderInfobyCustomer(customerID);

                foreach (DataTable item in orderInfo.Tables)
                {

                    foreach (DataRow row in item.Rows)
                    {
                        ovm.lstOrderedInfo.Add(new FindOrderInfo()
                        {
                            orderID = Convert.ToInt32(row["orderID"]),
                            itemName = Convert.ToString(row["itemName"]),
                            itemPrice = Convert.ToDecimal(row["itemPrice"]),
                            itemQuantity = Convert.ToInt32(row["itemQuantity"]),
                            date = Convert.ToString(row["date"]),
                            status = Convert.ToString(row["orderStatus"])
                        });
                    }
                }

                return PartialView("_searchOrder", ovm);
            }
            //search by order ID
            else if (button == "Search by Order ID")
            {
                ovm = new OrderCartViewModel();
                DataSet orderInfo = new DataSet();
                int orderID = Convert.ToInt32(Request.Form["OrderID"]);
                orderInfo = ac.findOrderInfobyOrder(orderID);

                foreach (DataTable item in orderInfo.Tables)
                {

                    foreach (DataRow row in item.Rows)
                    {
                        ovm.lstOrderedInfo.Add(new FindOrderInfo()
                        {
                            orderID = Convert.ToInt32(row["orderID"]),
                            itemName = Convert.ToString(row["itemName"]),
                            itemPrice = Convert.ToDecimal(row["itemPrice"]),
                            itemQuantity = Convert.ToInt32(row["itemQuantity"]),
                            date = Convert.ToString(row["date"]),
                            status = Convert.ToString(row["orderStatus"])
                        });
                    }
                }
                return PartialView("_searchOrder", ovm);
            }
            else
                return PartialView("_searchOrder");
        }
        //Actual operations performed on checkout, and saved into database
        public ActionResult Checkout(FormCollection frm)
        {
            int customerID = ovm.lstOrder.Find(x => x.CustomerID == x.CustomerID).CustomerID;
            int orderID = Convert.ToInt32(ac.insertOrderInfo(customerID, "pending", ovm.total));
            foreach(var item in ovm.lstOrder)
            {
                int newquant = ac.insertOrderedItemInfo(item.Name, item.Price, item.ProductQuantity, orderID, item.Quantity);
                ac.updateItemQuantity(item.Name, newquant);
            }

            return RedirectToAction("CustomerFindOrderView", "Home");
        }
        //Simple form is displayed. Model allows for storage for the post back of the form.
        public ActionResult CustomerAddView()
        {
            NewCustomerModel model = new NewCustomerModel();
            return View(model);
        }
        /*This creates a table of all the customers in the database. Their information is ordered by ID number 
         * and allows for an easy view of current customer data.
         * */
        public ActionResult FindCustomerView()
        {
            LstCustomerInfo lci = new LstCustomerInfo();
            DataSet custInfo = new DataSet();
            custInfo = ac.getCustomerInfo();

            foreach (DataTable item in custInfo.Tables)
            {

                foreach (DataRow row in item.Rows)
                {
                    lci.Fname.Add(Convert.ToString(row["Fname"]));
                    lci.Lname.Add(Convert.ToString(row["Lname"]));
                    lci.Phone.Add(Convert.ToString(row["Phone"]));
                    lci.Email.Add(Convert.ToString(row["Email"]));
                    lci.Address.Add(Convert.ToString(row["Address"]));
                    lci.ID.Add(Convert.ToInt32(row["ID"]));

                }
            }

            return View(lci);
        }
        /*This is the post back from the new customer form, this handles the information being submitted into
         * the database
         * */
        public ActionResult GetNewCustomerForm(FormCollection frm)
        {
            string fName = Request.Form["FName"];
            string lName = Request.Form["LName"];
            string phone = Request.Form["Phone"];
            string email = Request.Form["Email"];
            string address = Request.Form["Address"];

            ac.insertCustomerInfo(fName, lName,phone,email,address);
            return RedirectToAction("FindCustomerView", "Home");
        }
        /*This is the initial page to update a customer. The text box accepts the customer ID to update and uses
         * AJAX to post back and create a partial view.
         * */
        public ActionResult CustomerUpdateView()
        {
            UpdateCustomerModel uc = new UpdateCustomerModel();
            DataSet CustomerIDs = new DataSet();
            CustomerIDs = ac.getCustomerInfo();
            foreach (DataTable customer in CustomerIDs.Tables)
            {
                foreach (DataRow row in customer.Rows)
                {
                    uc.CustomerID.Add(Convert.ToInt32(row["ID"]));
                }
            }
            return View(uc);
        }
        /* This is the partial view mentioned above. Given a customer ID, the database will return a form propogated
         * with the current information. Any change made to this form on submit will change the data in the database
         * */
        public PartialViewResult customerUpdateForm(FormCollection frm)
        {
            UpdateCustomerModel cu = new UpdateCustomerModel();
            DataSet CustomerInfo = new DataSet();
            int customerID = Convert.ToInt32(Request.Form["lstCustomerID"]);
            CustomerInfo = ac.findCustomerInfo(customerID);

            foreach (DataTable item in CustomerInfo.Tables)
            {
                foreach (DataRow row in item.Rows)
                {
                    cu.lstCustomer.Add(new findCustomerInfo()
                    {
                        first = Convert.ToString(row["Fname"]),
                        last = Convert.ToString(row["Lname"]),
                        phone = Convert.ToString(row["Phone"]),
                        email = Convert.ToString(row["Email"]),
                        address = Convert.ToString(row["Address"]),
                        id = Convert.ToInt32(row["ID"])
                    });
                }
            }
            return PartialView("_updateCustomer", cu);
        }
        /*After the form mentioned above is submitted this is the method that handles the update in the database
         * */
        public ActionResult customerUpdate(FormCollection frm)
        {
            string f = Convert.ToString(Request.Form["first"]);
            string l = Convert.ToString(Request.Form["last"]);
            string p = Convert.ToString(Request.Form["phone"]);
            string e = Convert.ToString(Request.Form["email"]);
            string a = Convert.ToString(Request.Form["address"]);
            int i = Convert.ToInt32(Request.Form["customerID"]);

            ac.updateCustomer(f, l, p, e, a, i);

            return RedirectToAction("CustomerUpdateView", "Home");
        }
        /*This simply sets up the page. No model passing or computation is required
         * */
        public ActionResult CustomerFindOrderView()
        {
            return View();
        }
        /*This creates a simple view of what is in the inventory database. There is also a button created in the last
         * column of the table to easily remove an item from the database.
         * */
        public ActionResult InventoryView()
        {
            NewInventoryViewModel ivm = new NewInventoryViewModel();
            DataSet ItemInfo = new DataSet();
            ItemInfo = ac.getItemInfo();
            foreach (DataTable item in ItemInfo.Tables)
            {
                
                foreach (DataRow row in item.Rows)
                {
                    ivm.name.Add(Convert.ToString(row["itemName"]));
                    ivm.price.Add(Convert.ToDecimal(row["Price"]));
                    ivm.quantity.Add(Convert.ToInt32(row["Quantity"]));
                    ivm.id.Add(Convert.ToInt32(row["ID"]));
                    
                }
            }
            return View(ivm);
        }
        /* Simple form created to input new item information
         * */
        public ActionResult InventoryAddView()
        {
            return View();
        }
        /*Form post back that actually adds the item into the database. The database will automatically asign the 
         * item an ID
         * */
        public ActionResult GetNewItemForm(FormCollection frm)
        {
            string iname = Request.Form["IName"];
            string iprice = Request.Form["IPrice"];
            decimal quantity = Convert.ToDecimal(Request.Form["Quantity"]);

            ac.insertItemInfo(iname, iprice, quantity);
            return RedirectToAction("InventoryView","Home");
        }
        /*Much like the Customer Update Form option this takes an item name from a drop down and creates a propogated
         * form that can be edited. THIS WILL UPDATE WHATEVER ITEMS ARE CHANGED.
         * */
        public PartialViewResult itemUpdateForm(FormCollection frm)
        {
            NewInventoryViewModel ivm = new NewInventoryViewModel();
            DataSet ItemInfo = new DataSet();
            string itemName = Request.Form["lstItemName"];
            
            ItemInfo = ac.findItemInfo(itemName);
            foreach (DataTable item in ItemInfo.Tables)
            {
                foreach (DataRow row in item.Rows)
                {
                    ivm.lstItem.Add(new FindItemInfo()
                    {
                        Price = Convert.ToDecimal(row["Price"]),
                        Quantity = Convert.ToInt32(row["Quantity"]),
                        Name = Convert.ToString(row["itemName"]),
                        ProductID = Convert.ToInt32(row["ID"])
                    });
                }
            }
            return PartialView("_updateItem", ivm);
        }
        /*Actual updating being handled from the above post back*/
        public ActionResult itemUpdate(FormCollection frm)
        {
            string itemName = Convert.ToString(Request.Form["name"]);
            decimal price = Convert.ToDecimal(Request.Form["price"]);
            int quantity = Convert.ToInt32(Request.Form["quantity"]);
            int id = Convert.ToInt32(Request.Form["productID"]);

            ac.updateItem(itemName, price, quantity, id);

            return RedirectToAction("InventoryView", "Home");
        }
        /*Removes an item from the inventory view page and from the table.*/
        public ActionResult RemoveItem(string submitbtn)
        {
            ac.removeItem(submitbtn);

            return RedirectToAction("InventoryView", "Home");
        }
        /*Removes a customer base on their ID */
        public ActionResult RemoveCustomer(string submitbtn)
        {
            ac.removeCustomer(submitbtn);

            return RedirectToAction("FindCustomerView", "Home");
        }
        /*View that handles the initial page setup to update an item. This is what propogates the drop down menu.*/
        public ActionResult InventoryUpdateView()
        {
            NewInventoryViewModel ivm = new NewInventoryViewModel();
            DataSet ItemInfo = new DataSet();
            ItemInfo = ac.getItemInfo();
            foreach (DataTable item in ItemInfo.Tables)
            {

                foreach (DataRow row in item.Rows)
                {
                    ivm.name.Add(Convert.ToString(row["itemName"]));
                    ivm.price.Add(Convert.ToDecimal(row["Price"]));
                    ivm.quantity.Add(Convert.ToInt32(row["Quantity"]));
                    ivm.id.Add(Convert.ToInt32(row["ID"]));

                }
            }
            return View(ivm);
        }
        /*Takes information from the database and does the computation OUTSIDE the database. Saves on Storage.
         * */
        public ActionResult FinanceView()
        {
            finances fi = new finances();
            decimal totalProfit = ac.profit();
            fi.net = totalProfit;
            fi.amazon = Decimal.Multiply(totalProfit, .2M);
            fi.gross = Decimal.Multiply(totalProfit, .8M);
            return View(fi);
        }
    }
}
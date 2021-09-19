using DAL;

namespace WebCase.Areas.Basic.ViewModels
{
    public class CustomersVM : Customers
    {

        public CustomersVM()
        {

        }

        public int MyProperty { get; set; }


        enum MyEnum
        {
            aa = 0,
            bb = 1,
            cc = 3


        }
    }
}
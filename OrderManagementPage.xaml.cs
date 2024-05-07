using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    public partial class OrderManagementPage : ContentPage
    {
        public OrderManagementPage(IDatabaseManager db)
        {
            InitializeComponent();
            BindingContext = new OrderViewModel(db);
        }
    }

}

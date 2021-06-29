using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XafDcPrefetch.Module.BusinessObjects;

namespace XafDcPrefetch.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PrefetchController : ViewController
    {
        public PrefetchController()
        {
            InitializeComponent();
            SimpleAction Prefetch = new SimpleAction(this, "p", PredefinedCategory.View);
            Prefetch.Caption = "Prefetch";
            Prefetch.Execute += Prefetch_Execute;


            SimpleAction WithoutPrefetch = new SimpleAction(this, "wp", PredefinedCategory.View);
            WithoutPrefetch.Caption = "Without Prefetch";
            WithoutPrefetch.Execute += WithoutPrefetch_Execute;


            SimpleAction LinqQuery = new SimpleAction(this, "lq", PredefinedCategory.View);
            LinqQuery.Caption = "LinqQuery";
            LinqQuery.Execute += LinqQuery_Execute; 

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void LinqQuery_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var LinqOs = this.Application.CreateObjectSpace();

            var LinqOrders = from Order in LinqOs.GetObjectsQuery<IOrder>().Where(c=>c.Number=="1")                             
                             select new { Order.Number, Order.Items }
                             ;
            
            foreach (var item in LinqOrders)
            {
                //Debug.WriteLine(item.Order.Number);
                foreach (IOrderItem orderItem in item.Items)
                {
                    Debug.WriteLine(orderItem.Product);
                   
                }
            }
        }

        private void WithoutPrefetch_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            XPObjectSpace os = (XPObjectSpace)this.Application.CreateObjectSpace();
            XPCollection<IOrder> Orders = new XPCollection<IOrder>(os.Session);
            //os.Session.PreFetch(os.Session.GetClassInfo<IPerson>(), people, nameof(IPerson.PhoneNumbers));

            foreach (var order in Orders)
            {
                //Debug.WriteLine(order.Number);
                foreach (var item in order.Items)
                {
                    Debug.WriteLine(item.Product);
                }
            }

        }

        private void Prefetch_Execute(object sender, SimpleActionExecuteEventArgs e)
        {



            XPObjectSpace os = (XPObjectSpace)this.Application.CreateObjectSpace();
            XPCollection<IOrder> Orders = new XPCollection<IOrder>(os.Session);
            XPCollection<IOrderItem> OrderItems = new XPCollection<IOrderItem>(os.Session);
            XPMemberInfo ItemsMemberInfo = os.Session.GetClassInfo(typeof(IOrder)).GetMember(nameof(IOrder.Items));
            
            
            //1
            os.Session.PreFetch(Orders, nameof(IOrder.Items));

            //2
            //os.Session.PreFetch(Orders, ItemsMemberInfo, OrderItems);


            foreach (var order in Orders)
            {
                Debug.WriteLine(order.Number);
                foreach (var item in order.Items)
                {
                    Debug.WriteLine(item.Product);
                }
            }

            //XPCollection<Person> people = new XPCollection<Person>(session);
            //XPCollection<PhoneNumber> phoneNumbers = new XPCollection<PhoneNumber>(session);
            //XPMemberInfo phoneNumbersInfo = session.GetClassInfo(typeof(Person)).GetMember(nameof(PhoneNumbers));
            //session.PreFetch(people, phoneNumbersInfo, phoneNumbers);


            


        }

     

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}

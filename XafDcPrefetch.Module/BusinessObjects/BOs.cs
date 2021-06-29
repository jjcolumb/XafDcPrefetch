using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafDcPrefetch.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DomainComponent]
    public interface IPerson
    {
        string LastName { get; set; }
        string FirstName { get; set; }
        string FullName { get; set; }
        //TODO association fail in newer version of xaf so you need to use the link action to associate a phone numbers
        IList<IPhone> PhoneNumbers { get; }

    }

    [DefaultClassOptions]
    [DomainComponent]
    public interface IPhone
    {
        string Number { get; set; }

    }
    [DefaultClassOptions]
    [DomainComponent]
    public interface IOrder
    {
        string Number { get; set; }
        IList<IOrderItem> Items { get; }
    }

    [DomainComponent]
    public interface IOrderItem
    {
        string Product { get; set; }
        IOrder Order { get; set; }
    }
}

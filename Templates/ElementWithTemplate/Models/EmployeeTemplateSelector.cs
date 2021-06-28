using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElementWithTemplate.Models
{
    //想让不同类型的内容具有不同的表现方式，可通过ContentTemplate所定义的ContentTemplateSelector属性，此属性的类型是DataTemplateSelector
    //具有一个虚方法，名为SelectTemplate。第一个参数是要被ContentControl显示的对象，SelectTemplate方法的任务是返回一个DataTemplate对象用来显示内容
    class EmployeeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Employee emp = item as Employee;
            FrameworkElement el = container as FrameworkElement;
            return (DataTemplate)el.FindResource(emp.LeftHanded ? "templateLeft" : "templateRight");    //通过LeftHanded属性，在两个模板之间进行选择
        }
    }
}

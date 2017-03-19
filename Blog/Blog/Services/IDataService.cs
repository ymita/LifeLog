using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services
{
    interface IDataService
    {
        Dictionary<DateTime, int> getYearMonthList(DataContext db);
    }
}

using ExamManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamManagement.Helper
{
    public static class Interfaces
    {
        public interface IGetWebPageContent
        {
            IDictionary<int, ContentModel> GetWebPageContent(int articleIndex);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tresa.Services.Interfaces
{
    public interface IExportService
    {
        Task ExportAsync(string filePath);
    }
}

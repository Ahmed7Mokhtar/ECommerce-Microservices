using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.ServiceContracts
{
    public interface IValidationService
    {
        Task ValidateAsync<T>(T dto);
    }
}

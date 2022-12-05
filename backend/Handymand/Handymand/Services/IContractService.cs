using Handymand.Models;
using Handymand.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Services
{
    public interface IContractService
    {
        List<ContractDTO> AllAvailableContracts();

        List<ContractDTO> AllAvailableContractsForHomePage();

        ContractDTO UpdateContract(ContractDTO contract);

        ContractDTO CreateContract(ContractDTO contract);
        ContractDTO ConvertToDTO(Contract contract);

        Contract ConvertFromDTOforCreate(ContractDTO contract);

        bool DeleteContract(ContractDTO contract);
    }
}

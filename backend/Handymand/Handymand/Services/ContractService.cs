using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Repository.DatabaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Services
{
    public class ContractService : IContractService
    {
        private IContractRepository _contractRepository;
        private IUserRepository _userRepository;
        private IContractsSkillsRepository _contractsSkillsRepository;

        public ContractService(IContractRepository contractRepository, IUserRepository userRepository, IContractsSkillsRepository contractsSkillsRepository)
        {
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _contractsSkillsRepository = contractsSkillsRepository;
        }

        public ContractDTO ConvertToDTO(Contract contract)
        {
            var DTO = new ContractDTO();

            DTO.Id = contract.Id;
            DTO.CreationUserId = contract.CreationUserId;
            var creationUser = _userRepository.FindById(contract.CreationUserId);
            if (creationUser != null)
            {
                DTO.CreationUserFullName = creationUser.LastName + " " + creationUser.FirstName;
            }

            if (contract.RefferedUserId != null && contract.RefferedUserId != 0)
            {
                DTO.RefferedUserId = contract.RefferedUserId;

                var refferedUser = _userRepository.FindById((int)contract.RefferedUserId);

                DTO.RefferedUserFullName = refferedUser.LastName + " " + refferedUser.FirstName;

            }
            else
            {
                DTO.ExpirationDate = contract.DateCreated.AddMonths(1);
            }

            DTO.CreationDate = contract.DateCreated;

            DTO.PaymentAmountPerHour = contract.PaymentAmountPerHour;


            return DTO;


        }

        public Contract ConvertFromDTOforCreate(ContractDTO contract)
        {
            var result = new Contract();

            result.CreationUserId = contract.CreationUserId;
            result.RefferedUserId = contract.RefferedUserId;
            result.PaymentAmountPerHour = contract.PaymentAmountPerHour;
            result.DateCreated = DateTime.Now;


            return result;
        }

        public List<ContractDTO> AllAvailableContracts()
        {
            var query = _contractRepository.GetAllWithInclude();

            var result = query.Where(i => i.RefferedUserId == null || i.RefferedUserId == 0)
                .Select(i => new ContractDTO()
                {
                    Id = i.Id,
                    CreationUserFullName = i.CreationUser.LastName + " " + i.CreationUser.FirstName,
                    CreationDate = i.DateCreated,
                    ExpirationDate = i.DateCreated.AddMonths(1),
                    PaymentAmountPerHour = i.PaymentAmountPerHour,
                }).ToList();

            return result;
        }

        public List<ContractDTO> AllAvailableContractsForHomePage()
        {
            var query = _contractRepository.GetAllWithInclude();

            var result = query.Where(i => i.RefferedUserId == 0)
                .Select(i => new ContractDTO()
                {
                    Id = i.Id,
                    CreationUserFullName = i.CreationUser.LastName + " " + i.CreationUser.FirstName,
                    CreationDate = i.DateCreated,
                    ExpirationDate = i.DateCreated.AddMonths(1),
                    PaymentAmountPerHour = i.PaymentAmountPerHour,
                }).Take(6).ToList();

            return result;
        }

        public ContractDTO UpdateContract(ContractDTO contract)
        {
            var forupdate = _contractRepository.GetById(contract.Id);

            if(forupdate != null)
            {
                forupdate.RefferedUserId = contract.RefferedUserId;
                forupdate.PaymentAmountPerHour = contract.PaymentAmountPerHour;

                _contractRepository.Update(forupdate);
                _contractRepository.Save();

                return ConvertToDTO(forupdate);
            }
            else
            {

                throw new Exception("There is no contract with id : " + contract.Id);
            }
        }

        public ContractDTO CreateContract(ContractDTO contract)
        {
            var forCreate = ConvertFromDTOforCreate(contract);

            _contractRepository.Create(forCreate);

            _contractRepository.Save();

            return ConvertToDTO(forCreate);
        }

        public bool DeleteContract(ContractDTO contract)
        {
            var forDelete = _contractRepository.FindById(contract.Id);

            if(forDelete != null)
            {
                _contractRepository.Delete(forDelete);
                _contractRepository.Save();
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}

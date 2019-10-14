using Autofac;
using Matty.Framework;
using Matty.Framework.Enums;
using SmartHome.Core.Data.Repository;
using SmartHome.Core.Entities.DictionaryEntity;
using SmartHome.Core.Infrastructure.Exceptions;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class DictionaryService : CrudServiceBase<object, Dictionary>, IDictionaryService
    {
        private readonly SyntheticDictionaryService _syntheticDictionary;
        private readonly IDictionaryRepository _dictionaryRepository;

        public DictionaryService(ILifetimeScope container, SyntheticDictionaryService syntheticDictionary, IDictionaryRepository dictionaryRepository) : base(container)
        {
            _syntheticDictionary = syntheticDictionary;
            _dictionaryRepository = dictionaryRepository;
        }

        public async Task<ServiceResult<List<string>>> GetDictionaryNames()
        {
            var response = new ServiceResult<List<string>>(Principal);

            try
            {
                var names = await _dictionaryRepository.GetNames();
                response.Data = names.ToList();
                    
                response.Data.AddRange(_syntheticDictionary.GetNames());

                return response;
            }
            catch (Exception ex)
            {
                response.AddAlert(ex.Message, MessageType.Exception);
                throw;
            }
        }

        public async Task<ServiceResult<Dictionary>> GetDictionaryByName(string dictionaryName)
        {
            var response = new ServiceResult<Dictionary>(Principal);

            try
            {
                // Synthetic dictionaries are collected from other entities
                // They are read-only
                if (_syntheticDictionary.HasDictionary(dictionaryName))
                {
                    var dict = _syntheticDictionary.GetDictionary(dictionaryName);
                    response.Data = dict;
                    return response;
                }

                response.Data = await _dictionaryRepository.GetFiltered(x => x.Name == dictionaryName);

                return response;
            }
            catch (Exception ex)
            {
                response.AddAlert(ex.Message, MessageType.Exception);
                throw;
            }
        }

        public async Task<ServiceResult<Dictionary>> AddNewEntry(string dictionaryName, DictionaryValue entry)
        {
            var response = new ServiceResult<Dictionary>(Principal);

            try
            {
                var dict = await _dictionaryRepository.GetFiltered(x => x.Name == dictionaryName);
                if (dict == null) throw new SmartHomeEntityNotFoundException(nameof(dict));

                dict.Values.Add(entry);
                response.Data = await GenericRepository.UpdateAsync(dict);
                response.AddSuccessMessage("Successfully added new entry");

                return response;
            }
            catch (Exception ex)
            {
                response.AddAlert(ex.Message, MessageType.Exception);
                throw;
            }
        }

        public async Task<ServiceResult<Dictionary>> UpdateEntry(string dictionaryName, int entryId, DictionaryValue entry)
        {
            var response = new ServiceResult<Dictionary>(Principal);

            try
            {
                var dict = await _dictionaryRepository.GetFiltered(x => x.Name == dictionaryName);
                if (dict is null) throw new SmartHomeEntityNotFoundException(nameof(dict));

                var dictEntryToUpdate = dict.Values.SingleOrDefault(x => x.Id == entryId);
                if (dictEntryToUpdate is null) throw new SmartHomeEntityNotFoundException(nameof(dictEntryToUpdate));

                dictEntryToUpdate.InternalValue = entry.InternalValue;
                dictEntryToUpdate.DisplayValue = entry.DisplayValue;
                dictEntryToUpdate.IsActive = entry.IsActive;

                response.Data = await GenericRepository.UpdateAsync(dict);
                response.AddSuccessMessage("Successfully updated");

                return response;
            }
            catch (Exception ex)
            {
                response.AddAlert(ex.Message, MessageType.Exception);
                throw;
            }
        }

        public async Task<ServiceResult<Dictionary>> DeleteEntry(string dictionaryName, int entryId)
        {
            var response = new ServiceResult<Dictionary>(Principal);

            try
            {
                var dict = await _dictionaryRepository.GetFiltered(x => x.Name == dictionaryName);
                if (dict == null) throw new SmartHomeEntityNotFoundException(nameof(dict));

                var dictEntryToRemove = dict.Values.SingleOrDefault(x => x.Id == entryId);
                if (dictEntryToRemove == null) throw new SmartHomeEntityNotFoundException(nameof(dictEntryToRemove));

                // 'soft' delete
                dictEntryToRemove.IsActive = false;

                response.Data = await GenericRepository.UpdateAsync(dict);
                response.AddSuccessMessage("Successfully deleted entry");

                return response;
            }
            catch (Exception ex)
            {
                response.AddAlert(ex.Message, MessageType.Exception);
                throw;
            }
        }
    }
}

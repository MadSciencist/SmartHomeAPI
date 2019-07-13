using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Domain.DictionaryEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Core.Infrastructure;

namespace SmartHome.Core.Services
{
    public class DictionaryService : ServiceBase<object, Dictionary>, IDictionaryService
    {
        private readonly SyntheticDictionaryService _syntheticDictionary;

        public DictionaryService(ILifetimeScope container, SyntheticDictionaryService syntheticDictionary  ) : base(container)
        {
            _syntheticDictionary = syntheticDictionary;
        }

        public async Task<ServiceResult<List<string>>> GetDictionaryNames()
        {
            var response = new ServiceResult<List<string>>(Principal);

            try
            {
                response.Data = await GenericRepository.AsQueryableNoTrack()
                    .Distinct()
                    .Select(x => x.Name)
                    .ToListAsync();
                response.Data.AddRange(_syntheticDictionary.GetNames());

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
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

                response.Data = await GenericRepository.AsQueryableNoTrack()
                    .Include(x => x.Values)
                    .FirstOrDefaultAsync(x => x.Name == dictionaryName);

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }

        public async Task<ServiceResult<Dictionary>> AddNewEntry(string dictionaryName, DictionaryValue entry)
        {
            var response = new ServiceResult<Dictionary>(Principal);

            try
            {
                var dict = await GenericRepository.AsQueryable()
                    .Include(x => x.Values)
                    .FirstOrDefaultAsync(x => x.Name == dictionaryName);
                if (dict == null) throw new SmartHomeEntityNotFoundException(nameof(dict));

                dict.Values.Add(entry);
                response.Data = await GenericRepository.UpdateAsync(dict);
                response.Alerts.Add(new Alert("Successfully added new entry", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }

        public async Task<ServiceResult<Dictionary>> UpdateEntry(string dictionaryName, int entryId, DictionaryValue entry)
        {
            var response = new ServiceResult<Dictionary>(Principal);

            try
            {
                var dict = await GenericRepository.AsQueryable()
                    .Include(x => x.Values)
                    .FirstOrDefaultAsync(x => x.Name == dictionaryName);
                if (dict == null) throw new SmartHomeEntityNotFoundException(nameof(dict));

                var dictEntryToUpdate = dict.Values.SingleOrDefault(x => x.Id == entryId);
                if (dictEntryToUpdate == null) throw new SmartHomeEntityNotFoundException(nameof(dictEntryToUpdate));

                // TODO reflection?
                dictEntryToUpdate.InternalValue = entry.InternalValue;
                dictEntryToUpdate.DisplayValue = entry.DisplayValue;
                dictEntryToUpdate.IsActive = entry.IsActive;

                response.Data = await GenericRepository.UpdateAsync(dict);
                response.Alerts.Add(new Alert("Successfully updated", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }

        public async Task<ServiceResult<Dictionary>> DeleteEntry(string dictionaryName, int entryId)
        {
            var response = new ServiceResult<Dictionary>(Principal);

            try
            {
                var dict = await GenericRepository.AsQueryable()
                    .Include(x => x.Values)
                    .SingleOrDefaultAsync(x => x.Name == dictionaryName);
                if (dict == null) throw new SmartHomeEntityNotFoundException(nameof(dict));

                var dictEntryToRemove = dict.Values.SingleOrDefault(x => x.Id == entryId);
                if (dictEntryToRemove == null) throw new SmartHomeEntityNotFoundException(nameof(dictEntryToRemove));

                // 'soft' delete
                dictEntryToRemove.IsActive = false;

                response.Data = await GenericRepository.UpdateAsync(dict);
                response.Alerts.Add(new Alert("Successfully deleted entry", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }
    }
}

using FamilyTree.Core;
using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyTree.BL.Services
{
    public class BLPerson : BLCommon<Person>, IPersonService
    {
        private readonly DbSet<Person> _dbSet;
        private readonly DataContext _context;

        public List<Person> lstPerson = new List<Person>();

        public BLPerson(IPersonRepository dbContext, DataContext context) : base(dbContext)
        {
            _dbSet = context.Set<Person>();
            _context = context;
        }

        public override Response ValidationBeforePreSave(Person person)
        {
            response.IsError = false;
            response.Message = string.Empty;

            if (person == null)
            {
                response.IsError = true;
                response.Message = "Person details are required.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(person.FirstName))
            {
                response.IsError = true;
                response.Message = "First name is required.";
                return response;
            }

            if (person.PersonImageBase64 != null)
            {
                var trimmedImage = person.PersonImageBase64.Trim();

                if (trimmedImage.Length == 0)
                {
                    person.PersonImage = null;
                    person.PersonImageBase64 = null;
                }
                else if (!TryDecodeBase64Image(trimmedImage, out var imageBytes))
                {
                    response.IsError = true;
                    response.Message = "Invalid Base64 image format.";
                    return response;
                }
                else
                {
                    person.PersonImage = imageBytes;
                    person.PersonImageBase64 = trimmedImage;
                }
            }
            else if (EntryType == Core.enmEntryType.E)
            {
                var existing = _context.Person.AsNoTracking().FirstOrDefault(p => p.PersonId == person.PersonId);
                if (existing != null)
                {
                    person.PersonImage = existing.PersonImage;
                }
            }

            return response;
        }

        public override Response GetAll(string[]? includeFields = null, string[]? excludeFields = null)
        {
            var responseResult = base.GetAll(includeFields, excludeFields);
            if (responseResult.DataModel is IEnumerable<object> collection)
            {
                foreach (var item in collection)
                {
                    if (item is Person person)
                    {
                        PopulatePersonImage(person);
                    }
                }
            }

            return responseResult;
        }

        public override Response GetById(int id)
        {
            var responseResult = base.GetById(id);
            if (!responseResult.IsError && responseResult.DataModel is Person person)
            {
                PopulatePersonImage(person);
            }

            return responseResult;
        }

        public Response GetWholeTree()
        {
            lstPerson = _dbSet.ToList();

            foreach (var person in lstPerson)
            {
                PopulatePersonImage(person);
            }

            response.DataModel = BuildFamilyTree();

            return response;
        }

        public List<Person> BuildFamilyTree(Person currentPerson = null)
        {
            HashSet<int> visitedPerson = new HashSet<int>();
            if (currentPerson == null)
            {
                return lstPerson
                    .Where(p => p.MotherId == 0 && p.FatherId == 0)
                    .Select(p => BuildFamilyTreeNode(p, visitedPerson))
                    .Where(p => p != null)
                    .ToList();
            }
            else
            {
                var node = BuildFamilyTreeNode(currentPerson, visitedPerson);
                return node != null ? new List<Person> { node } : new List<Person>();
            }
        }

        private Person BuildFamilyTreeNode(Person currentPerson, HashSet<int> visitedPerson)
        {
            if (currentPerson == null || visitedPerson.Contains(currentPerson.PersonId) /*|| visitedPerson.Contains(currentPerson.SpouseId)*/)
                return null;
            visitedPerson.Add(currentPerson.PersonId);
            //Person spouse = lstPerson.FirstOrDefault(p => p.PersonId == currentPerson.SpouseId);
            Person objPerson = new Person
            {
                PersonId = currentPerson.PersonId,
                FirstName = currentPerson.FirstName,
                LastName = currentPerson.LastName,
                BirthDate = currentPerson.BirthDate,
                //DateOfDeath = currentPerson.DateOfDeath,
                Description = currentPerson.Description,
                MaritalStatus = currentPerson.MaritalStatus,
                Address = currentPerson.Address,
                Occupation = currentPerson.Occupation,
                Qualification = currentPerson.Qualification,
                PersonImageBase64 = currentPerson.PersonImage != null ? Convert.ToBase64String(currentPerson.PersonImage) : currentPerson.PersonImageBase64,
                //Mother = lstPerson.FirstOrDefault(p => p.PersonId == currentPerson.MotherId),
                //Father = lstPerson.FirstOrDefault(p => p.PersonId == currentPerson.FatherId),
                //Spouse = spouse != null && !visitedPerson.Contains(currentPerson.SpouseId) ? spouse : null,
                Children = lstPerson
                .Where(p => p.FatherId == currentPerson.PersonId)
                .Select(child => BuildFamilyTreeNode(child, visitedPerson))
                .Where(child => child != null)
                .ToList()
            };
            visitedPerson.Remove(currentPerson.PersonId);
            return objPerson;
        }

        public Response UploadImage(int personId, string? imageBase64)
        {
            response.IsError = false;
            response.Message = string.Empty;

            var person = _context.Person.FirstOrDefault(p => p.PersonId == personId);
            if (person == null)
            {
                response.IsError = true;
                response.Message = "Person not found.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(imageBase64))
            {
                person.PersonImage = null;
                _context.SaveChanges();
                response.Message = "Image removed successfully.";
                return response;
            }

            if (!TryDecodeBase64Image(imageBase64.Trim(), out var decodedImage))
            {
                response.IsError = true;
                response.Message = "Invalid Base64 image format.";
                return response;
            }

            person.PersonImage = decodedImage;
            _context.SaveChanges();
            response.Message = "Image uploaded successfully.";

            return response;
        }

        private static void PopulatePersonImage(Person person)
        {
            if (person == null)
            {
                return;
            }

            if (person.PersonImage != null && person.PersonImage.Length > 0)
            {
                person.PersonImageBase64 = Convert.ToBase64String(person.PersonImage);
            }
            else
            {
                person.PersonImageBase64 = null;
            }
        }

        private static bool TryDecodeBase64Image(string base64, out byte[] imageBytes)
        {
            try
            {
                imageBytes = Convert.FromBase64String(base64);
                return true;
            }
            catch (FormatException)
            {
                imageBytes = Array.Empty<byte>();
                return false;
            }
        }
    }
}

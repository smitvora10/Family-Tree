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
        private readonly IPersonRepository _personRepository;

        private List<Person> lstPerson = new List<Person>();

        public BLPerson(IPersonRepository dbContext, DataContext context) : base(dbContext)
        {
            _personRepository = dbContext;
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
                string trimmedImage = person.PersonImageBase64.Trim();

                if (trimmedImage.Length == 0)
                {
                    person.PersonImage = null;
                    person.PersonImageBase64 = null;
                }
                else if (!TryDecodeBase64Image(trimmedImage, out byte[] imageBytes))
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
                Person? existing = _context.Person.AsNoTracking().FirstOrDefault(p => p.PersonId == person.PersonId);
                if (existing != null)
                {
                    person.PersonImage = existing.PersonImage;
                }
            }

            return response;
        }





        public Response GetWholeTree()
        {
            lstPerson = _dbSet.ToList();

            // Optimization: Do not populate images here. Images should be fetched lazily.
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
                Person? node = BuildFamilyTreeNode(currentPerson, visitedPerson);
                return node != null ? new List<Person> { node } : new List<Person>();
            }
        }

        private Person BuildFamilyTreeNode(Person currentPerson, HashSet<int> visitedPerson)
        {
            if (currentPerson == null || visitedPerson.Contains(currentPerson.PersonId))
                return null;
            visitedPerson.Add(currentPerson.PersonId);

            Person objPerson = new Person
            {
                PersonId = currentPerson.PersonId,
                FirstName = currentPerson.FirstName,
                LastName = currentPerson.LastName,
                BirthDate = currentPerson.BirthDate,
                Description = currentPerson.Description,
                MaritalStatus = currentPerson.MaritalStatus,
                Address = currentPerson.Address,
                Occupation = currentPerson.Occupation,
                Qualification = currentPerson.Qualification,
                // Optimization: Sending base64 image in the tree as requested.
                PersonImageBase64 = (currentPerson.PersonImage != null && currentPerson.PersonImage.Length > 0)
                                    ? Convert.ToBase64String(currentPerson.PersonImage)
                                    : null,
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

            Person? person = _context.Person.FirstOrDefault(p => p.PersonId == personId);
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

            if (!TryDecodeBase64Image(imageBase64.Trim(), out byte[] decodedImage))
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

        public Response GetPersonImage(int personId)
        {
            response = new Response();
            // Optimization: Only fetch the PersonImage column, not the entire record
            byte[]? personImage = _context.Person
                .AsNoTracking()
                .Where(p => p.PersonId == personId)
                .Select(p => p.PersonImage)
                .FirstOrDefault();

            if (personImage != null && personImage.Length > 0)
            {
                response.DataModel = new { ImageBase64 = Convert.ToBase64String(personImage) };
            }
            else
            {
                // Return null or empty if not found, but don't error out as it might just be no image
                response.DataModel = new { ImageBase64 = (string?)null };
            }

            return response;
        }

        public Response GetPersonDDL(CommonSearchModel model)
        {
            response.DataModel = _personRepository.GetPersonDDL(model);
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

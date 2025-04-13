using FamilyTree.Data;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Common;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.BL.Services
{
    public class BLPerson : BLCommon<Person>, IPersonService
    {
        private readonly IPersonRepository _dbContext;
        private readonly IPersonService _personService;
        private readonly DbSet<Person> _dbSet;
        private readonly DataContext _context;

        public List<Person> lstPerson = new List<Person>();
        public BLPerson(IPersonRepository dbContext, DataContext context) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = context.Set<Person>();
        }


        public override Response ValidationBeforePreSave(Person person)
        {

            //bool isDuplicate = _dbSet.HasDuplicate(
            //    person,
            //    rt => rt.PersonCode,
            //    rt => rt.PersonDescription);

            //if (isDuplicate)
            //{
            //    objResponse.IsError = true;
            //    objResponse.MessageCode = MessageCode.E002.ToString().Replace("~{handler}~", "Relation Type");
            //}
            return response;

        }

        public Response GetWholeTree()
        {
            lstPerson = _dbSet.ToList();

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
                    .ToList();
            }
            else
            {
                return new List<Person> { BuildFamilyTreeNode(currentPerson, visitedPerson) };
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
                //Mother = lstPerson.FirstOrDefault(p => p.PersonId == currentPerson.MotherId),
                //Father = lstPerson.FirstOrDefault(p => p.PersonId == currentPerson.FatherId),
                //Spouse = spouse != null && !visitedPerson.Contains(currentPerson.SpouseId) ? spouse : null,
                Children = lstPerson
                .Where(p => p.FatherId == currentPerson.PersonId)
                .Select(child => BuildFamilyTreeNode(child, visitedPerson))
                .ToList()
            };
            visitedPerson.Remove(currentPerson.PersonId);
            return objPerson;
        }


    }
}

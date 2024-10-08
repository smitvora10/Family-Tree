﻿using FamilyTree.Data;
using FamilyTree.Data.Common;
using FamilyTree.DB.Interfaces;
using FamilyTree.Models.Master;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FamilyTree.BL.Services
{
    public class DBPerson : DBCommon<Person>, IPersonRepository
    {
        private readonly DbSet<Person> _dbSet;
        private readonly DataContext _context;
        public DBPerson(DataContext context) : base(context)
        {
            _dbSet = context.Set<Person>();
        }
        public DataTable GetTree()
        {
            throw new NotImplementedException();
        }
    }



}

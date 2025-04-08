﻿using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.DataBase;
using System;
using System.Threading.Tasks;

namespace PastBeam.Infrastructure.Library.Repositories
{
    public class UserCourseRepository : IUserCourseRepository
    {
        private readonly ApplicationDbContext _context;

        public UserCourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteUserCoursesByUserAsync(int userId)
        {
            var userCourses = await _context.UserCourses
                .Where(uc => uc.UserId == userId)
                .ToListAsync();

            _context.UserCourses.RemoveRange(userCourses);
            await _context.SaveChangesAsync();
        }
    }
}
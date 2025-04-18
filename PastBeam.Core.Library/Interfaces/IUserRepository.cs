﻿using PastBeam.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IUserRepository
    {
        // User related methods
        Task<User?> GetByIdAsync(int userId);

        Task<IEnumerable<User>> GetAllAsync();

        Task DeleteAsync(int userId);


        // Folder related methods
        Task<IEnumerable<Folder>> GetUserFoldersAsync(int userId);

        Task<Folder?> DeleteFolderAsync(int folderId);

        Task CreateFolderAsync(Folder folder);

        Folder? GetFolderById(int folderId);

        Task SuspendUserAsync(int userId, bool isSuspended);
      
        Task<User?> GetUserByIdAsync(int userId);

        Task<bool> UpdateUserProfileAsync(User user);

        Task DeleteUserAsync(int userId);
    }
}
